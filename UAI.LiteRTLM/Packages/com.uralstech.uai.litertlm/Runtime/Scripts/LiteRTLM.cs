// Copyright 2026 URAV ADVANCED LEARNING SYSTEMS PRIVATE LIMITED
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using Uralstech.UAI.LiteRTLM.Native;

#nullable enable
namespace Uralstech.UAI.LiteRTLM
{
    public static class LiteRTLM
    {
        public readonly struct Version
        {
            public readonly int Major;
            public readonly int Minor;
            public readonly int Patch;

            public Version(int major, int minor, int patch)
            {
                Major = major;
                Minor = minor;
                Patch = patch;
            }

            /// <summary>Is the current version ABI compatabile with <paramref name="version"/>?</summary>
            public bool IsABICompatible(Version version)
            {
                return Major == version.Major
                    && Minor >= version.Minor;
            }
        }
        
        /// <summary>The LiteRT-LM library this C# package was written for.</summary>
        public static readonly Version TargetVersion = new(
            EngineH.LiteRTLMTargetVersionMajor,
            EngineH.LiteRTLMTargetVersionMinor,
            EngineH.LiteRTLMTargetVersionPatch
        );
        
        /// <summary>Sets the minimum log level for the LiteRT LM library.</summary>
        /// <param name="level">The minimum severity to log.</param>
        public static void SetMinLogLevel(LogSeverity level) =>
            LiteRTLMException.Check(EngineH.litert_lm_set_min_log_level(level));

        /// <summary>Returns the version of the native library.</summary>
        /// <remarks>
        /// You likely won't ever have to use this unless you're using custom
        /// native binaries, in which case you could compare it to <see cref="TargetVersion"/>.
        /// </remarks>
        public static Version GetAPIVersion()
        {
            LiteRTLMException.Check(EngineH.litert_lm_get_api_version(
                out int major, out int minor, out int patch));
            return new Version(major, minor, patch);
        }
    }

    /// <summary>A base class for anything that strictly owns a <see cref="LiteRTHandle"/>.</summary>
    /// <typeparam name="T"></typeparam>
    public abstract class LiteRTLMWrapper<T> : IDisposable
        where T : LiteRTHandle
    {
        private T? _handle;
        private readonly object _lock = new();

        /// <summary>ALWAYS call during construction.</summary>
        protected void SetHandle(T handle)
        {
            lock (_lock)
            {
                if (_handle != null)
                    throw new InvalidOperationException($"A handle for {GetType().Name} is already set.");
            
                _handle = handle;
            }
        }
        
        /// <summary>Gets the native handle.</summary>
        /// <remarks>
        /// don't use this unless you <i><b>really</b></i> know what you're doing.
        /// even then, you really don't need to use this. probably.
        /// </remarks>
        public T DangerousGetHandle()
        {
            ThrowIfInvalid(out T handle);
            return handle;
        }

        protected void LockedExecute(Func<T, StatusCode> action)
        {
            lock (_lock)
            {
                ThrowIfInvalid(out T handle);
                LiteRTLMException.Check(action(handle));
            }
        }
        
        protected TReturn LockedExecute<TReturn>(Func<T, (StatusCode, TReturn)> action)
        {
            lock (_lock)
            {
                ThrowIfInvalid(out T handle);
                (StatusCode code, TReturn result) = action(handle);
                return LiteRTLMException.Check(code, result);
            }
        }
        
        private void ThrowIfInvalid(out T handle)
        {
            if (_handle is null || _handle.IsInvalid || _handle.IsClosed)
                throw new ObjectDisposedException(GetType().Name);
            handle = _handle;
        }
        
        public void Dispose()
        {
            lock (_lock)
            {
                _handle?.Dispose();
                _handle = null;
            }
            
            GC.SuppressFinalize(this);
        }

        ~LiteRTLMWrapper()
        {
            if (_handle is { IsInvalid: false, IsClosed: false })
                PackageLogger.LogWarning($"An instance of {GetType().Name} was garbage collected. " +
                                         $"Undisposed LiteRT-LM objects can use up significant system resources, " +
                                         $"dispose them as soon as you're done using them.");
        }
    }
}