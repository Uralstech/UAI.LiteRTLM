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
using System.Collections.Generic;
using Uralstech.UAI.LiteRTLM.Native;

#nullable enable
namespace Uralstech.UAI.LiteRTLM
{
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
            lock (_lock)
            {
                ThrowIfInvalid(out T handle);
                return handle;
            }
        }

        protected internal void LockedExecute(Func<T, StatusCode> action)
        {
            lock (_lock)
            {
                ThrowIfInvalid(out T handle);
                LiteRTLMException.Check(action(handle));
            }
        }
        
        protected internal TReturn LockedExecute<TReturn>(Func<T, (StatusCode, TReturn)> action)
        {
            lock (_lock)
            {
                ThrowIfInvalid(out T handle);
                (StatusCode code, TReturn result) = action(handle);
                return LiteRTLMException.Check(code, result);
            }
        }

        protected static void DisposeOnCatch<THandle>(THandle handle, Action<THandle> action)
            where THandle : LiteRTHandle
        {
            try
            {
                action(handle);
            }
            catch
            {
                handle.Dispose();
                throw;
            }
        }

        protected static void ExecuteWithHandles<TWrapper, THandle>(IReadOnlyList<TWrapper> wrappers, Action<IntPtr[]> func)
            where TWrapper : LiteRTLMWrapper<THandle> where THandle : LiteRTHandle
        {
            if (wrappers is null)
                throw new ArgumentNullException(nameof(wrappers), "Input array is null.");
            
            int length = wrappers.Count;
            IntPtr[] handlePtrs = new IntPtr[length];
            THandle?[] incrementedHandles = new THandle?[length];

            try
            {
                for (int i = 0; i < length; i++)
                {
                    THandle handle = wrappers[i]?.DangerousGetHandle()
                                     ?? throw new ArgumentNullException(nameof(wrappers), $"Input at index {i} is null.");
                    
                    bool incremented = false;
                    handle.DangerousAddRef(ref incremented);
                    if (!incremented)
                        throw new InvalidOperationException($"Could not increment reference to handle at input index {i}.");
                    
                    incrementedHandles[i] = handle;
                    handlePtrs[i] = handle.DangerousGetHandle();
                }
                
                func(handlePtrs);
                GC.KeepAlive(wrappers);
            }
            finally
            {
                foreach (THandle? handle in incrementedHandles)
                    handle?.DangerousRelease();
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