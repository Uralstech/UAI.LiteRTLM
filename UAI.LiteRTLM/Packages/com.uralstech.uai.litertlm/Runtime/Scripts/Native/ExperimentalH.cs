// Copyright 2026 URAV ADVANCED LEARNING SYSTEMS PRIVATE LIMITED
// Copyright 2026 The ODML Authors.
//
// Modified by URAV ADVANCED LEARNING SYSTEMS PRIVATE LIMITED:
// Ported to C# P/Invoke from LiteRT-LM.
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

#nullable enable
namespace Uralstech.UAI.LiteRTLM.Native
{
    /// <summary>
    /// WARNING: The APIs declared in this class are EXPERIMENTAL and subject to
    /// change or removal without notice. API stability and backward compatibility
    /// are not guaranteed.
    /// </summary>
    public static class ExperimentalH
    {
        /// <summary>
        /// Updates whether to enable Metal residency set on GPU for the given engine at runtime.
        /// <para>
        /// To configure this setting during initialization, use
        /// <c>litert_lm_engine_settings_set_gpu_enable_metal_residency_set</c> instead.
        /// </para>
        /// <para>
        /// When enabled on Apple platforms (macOS and iOS with Metal GPU backend), this
        /// uses Apple's <c>MTLResidencySet</c> API to ensure model weights and allocations
        /// remain resident in GPU memory, preventing memory swapping and reducing
        /// allocation overhead.
        /// </para>
        /// <para>
        /// This setting is only supported on Apple platforms (macOS / iOS) with the GPU
        /// backend. On other platforms (e.g. Linux, Android, Windows) or non-GPU
        /// backends, this setting has no effect and is safely ignored.
        /// </para>
        /// </summary>
        /// <param name="engine">The engine to update.</param>
        /// <param name="enableMetalResidencySet">Whether to enable Metal residency set.</param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or another <see cref="StatusCode"/> on failure.
        /// <see cref="StatusCode.InvalidArgument"/> if <paramref name="engine"/> is <see cref="IntPtr.Zero"/>.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode
            litert_lm_experimental_engine_update_gpu_enable_metal_residency_set(
                IntPtr engine, [MarshalAs(UnmanagedType.I1)] bool enableMetalResidencySet);

        /// <summary>
        /// Checks whether the LiteRT-LM runtime binary was built with the debugger
        /// tracing backend enabled (<c>LITERT_LM_DEBUGGER_ENABLED=1</c>).
        /// </summary>
        /// <param name="outEnabled">
        /// On success, receives <see langword="true"/> if the debugger is enabled at
        /// compile-time, <see langword="false"/> otherwise; not written on failure.
        /// </param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or another <see cref="StatusCode"/> on failure.
        /// <see cref="StatusCode.InvalidArgument"/> if <paramref name="outEnabled"/> is <see langword="null"/>.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_experimental_is_debugger_enabled(
            [MarshalAs(UnmanagedType.I1)] out bool outEnabled);

        /// <summary>
        /// Gets debug info for the session. The caller is responsible for deleting the
        /// returned object using <see cref="litert_lm_experimental_session_debug_info_delete"/>.
        /// <para>
        /// If debugging is unsupported or disabled for the session, this succeeds and
        /// <paramref name="outDebugInfo"/> is set to <see cref="IntPtr.Zero"/>.
        /// </para>
        /// </summary>
        /// <param name="session">The session to query.</param>
        /// <param name="outDebugInfo">
        /// On success, receives the session debug info, or <see cref="IntPtr.Zero"/> if debugging
        /// is unsupported or disabled; set to <see cref="IntPtr.Zero"/> on failure.
        /// </param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or another <see cref="StatusCode"/> on failure.
        /// <see cref="StatusCode.InvalidArgument"/> if <paramref name="session"/> is
        /// <see cref="IntPtr.Zero"/> or <paramref name="outDebugInfo"/> is <see langword="null"/>.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_experimental_session_get_debug_info(
            IntPtr session, out IntPtr outDebugInfo);

        /// <summary>
        /// Gets session debug info for the conversation's underlying session. The caller is
        /// responsible for deleting the returned object using
        /// <see cref="litert_lm_experimental_session_debug_info_delete"/>.
        /// <para>
        /// If debugging is unsupported or disabled for the session, this succeeds and
        /// <paramref name="outDebugInfo"/> is set to <see cref="IntPtr.Zero"/>.
        /// </para>
        /// </summary>
        /// <param name="conversation">The conversation to query.</param>
        /// <param name="outDebugInfo">
        /// On success, receives the session debug info, or <see cref="IntPtr.Zero"/> if debugging
        /// is unsupported or disabled; set to <see cref="IntPtr.Zero"/> on failure.
        /// </param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or another <see cref="StatusCode"/> on failure.
        /// <see cref="StatusCode.InvalidArgument"/> if <paramref name="conversation"/> is
        /// <see cref="IntPtr.Zero"/> or <paramref name="outDebugInfo"/> is <see langword="null"/>.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_experimental_conversation_get_session_debug_info(
            IntPtr conversation,
            out IntPtr outDebugInfo);

        /// <summary>
        /// Destroys a <c>LiteRtLmSessionDebugInfo</c> object.
        /// </summary>
        /// <param name="debugInfo">The session debug info object to destroy.</param>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern void litert_lm_experimental_session_debug_info_delete(
            IntPtr debugInfo);

        /// <summary>
        /// Gets the relative debug capture directory from a <c>LiteRtLmSessionDebugInfo</c>
        /// object (e.g. "litert_lm_debugger/0"), containing intermediate activation
        /// Safetensors dumps and token generation trace logs. The returned string is
        /// owned by the <paramref name="debugInfo"/> object and is valid only for its lifetime.
        /// </summary>
        /// <param name="debugInfo">The session debug info object.</param>
        /// <param name="outCaptureDir">
        /// On success, receives the relative capture directory string; set to
        /// <see cref="IntPtr.Zero"/> on failure.
        /// </param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or another <see cref="StatusCode"/> on failure.
        /// <see cref="StatusCode.InvalidArgument"/> if <paramref name="debugInfo"/> is
        /// <see cref="IntPtr.Zero"/> or <paramref name="outCaptureDir"/> is <see langword="null"/>.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_experimental_session_debug_info_get_capture_dir(
            IntPtr debugInfo, out IntPtr outCaptureDir);
    }
}