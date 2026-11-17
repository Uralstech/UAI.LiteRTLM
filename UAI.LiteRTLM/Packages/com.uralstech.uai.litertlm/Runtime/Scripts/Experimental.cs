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
using System.Runtime.InteropServices;
using Uralstech.UAI.LiteRTLM.Native;

#nullable enable
namespace Uralstech.UAI.LiteRTLM
{
    /// <summary>
    /// WARNING: This class is EXPERIMENTAL and subject to change or removal without notice.
    /// API stability and backward compatibility are not guaranteed.
    /// </summary>
    public sealed class SessionDebugInfo : LiteRTLMWrapper<SessionDebugInfoHandle>
    {
        internal SessionDebugInfo(SessionDebugInfoHandle handle)
        {
            SetHandle(handle);
        }

        /// <summary>
        /// Returns the relative debug capture directory (e.g. "litert_lm_debugger/0"), containing
        /// intermediate activation Safetensors dumps and token generation trace logs.
        /// </summary>
        public string? GetCaptureDir() =>
            LockedExecute(handle => (ExperimentalH.litert_lm_experimental_session_debug_info_get_capture_dir(handle, out IntPtr dirPtr),
                dirPtr != IntPtr.Zero ? Marshal.PtrToStringUTF8(dirPtr) : null));
    }

    /// <summary>
    /// WARNING: The methods in this class are EXPERIMENTAL and subject to change or removal without notice.
    /// API stability and backward compatibility are not guaranteed.
    /// </summary>
    public static class Experimental
    {
        /// <summary>Updates whether to enable Metal residency set on GPU for the given engine at runtime (Apple platforms only).</summary>
        /// <param name="engine">The engine to update.</param>
        /// <param name="enableMetalResidencySet">Whether to enable Metal residency set.</param>
        public static void UpdateGPUEnableMetalResidencySet(this Engine engine, bool enableMetalResidencySet)
        {
            if (engine is null)
                throw new ArgumentNullException(nameof(engine));

            engine.LockedExecute(handle =>
                ExperimentalH.litert_lm_experimental_engine_update_gpu_enable_metal_residency_set(handle, enableMetalResidencySet));
        }

        /// <summary>Checks whether the LiteRT-LM runtime binary was built with the debugger tracing backend enabled.</summary>
        public static bool IsDebuggerEnabled()
        {
            LiteRTLMException.Check(ExperimentalH.litert_lm_experimental_is_debugger_enabled(out bool enabled));
            return enabled;
        }

        /// <summary>Returns debug info for the session, or <see langword="null"/> if debugging is unsupported or disabled.</summary>
        /// <param name="session">The session to query.</param>
        public static SessionDebugInfo? GetDebugInfo(this Session session)
        {
            if (session is null)
                throw new ArgumentNullException(nameof(session));

            SessionDebugInfoHandle debugInfoHandle = session.LockedExecute(handle =>
                (ExperimentalH.litert_lm_experimental_session_get_debug_info(handle, out SessionDebugInfoHandle outDebugInfo), outDebugInfo));
            if (!debugInfoHandle.IsInvalid)
                return new SessionDebugInfo(debugInfoHandle);

            debugInfoHandle.Dispose();
            return null;
        }

        /// <summary>
        /// Returns session debug info for the conversation's underlying session, or <see langword="null"/>
        /// if debugging is unsupported or disabled.
        /// </summary>
        /// <param name="conversation">The conversation to query.</param>
        public static SessionDebugInfo? GetSessionDebugInfo(this Conversation conversation)
        {
            if (conversation is null)
                throw new ArgumentNullException(nameof(conversation));

            SessionDebugInfoHandle debugInfoHandle = conversation.LockedExecute(handle =>
                (ExperimentalH.litert_lm_experimental_conversation_get_session_debug_info(handle, out SessionDebugInfoHandle outDebugInfo), outDebugInfo));
            if (!debugInfoHandle.IsInvalid)
                return new SessionDebugInfo(debugInfoHandle);

            debugInfoHandle.Dispose();
            return null;
        }
    }
}