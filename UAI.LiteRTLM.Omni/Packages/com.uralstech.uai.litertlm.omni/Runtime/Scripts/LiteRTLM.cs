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

using Uralstech.UAI.LiteRTLM.Omni.Native;

#nullable enable
namespace Uralstech.UAI.LiteRTLM.Omni
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
}