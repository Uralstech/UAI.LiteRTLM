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
namespace Uralstech.UAI.LiteRTLM.Omni.Native
{
    /// <summary>
    /// Error handling model and conventions for the LiteRT LM native API.
    /// </summary>
    /// <remarks>
    /// The LiteRT LM C API uses thread-local error reporting modeled after standard
    /// POSIX <c>errno</c> conventions:
    /// <list type="number">
    /// <item>
    /// <term>Thread Locality</term>
    /// <description>
    /// Errors are stored in thread-local storage. Each thread maintains its own
    /// independent error message. Concurrent calls across different threads do
    /// not overwrite or interfere with each other's error state.
    /// </description>
    /// </item>
    /// <item>
    /// <term>Error Setting on Failure Only</term>
    /// <description>
    /// The error state is updated ONLY when an API function fails. Functions
    /// that succeed DO NOT clear, reset, or modify the existing error state.
    /// </description>
    /// </item>
    /// <item>
    /// <term>User Expectations &amp; Error Extraction Logic</term>
    /// <description>
    /// <list type="bullet">
    /// <item><description>
    /// Callers MUST check the <see cref="StatusCode"/> returned by an API function first
    /// (see "Return Shape" below) to determine whether an operation actually failed.
    /// </description></item>
    /// <item><description>
    /// Callers MUST NOT rely on <c>litert_lm_get_last_error_message() != IntPtr.Zero</c>
    /// to infer failure, because a successful call following a failed one will leave
    /// the prior error message intact in thread-local storage.
    /// </description></item>
    /// <item><description>
    /// Error retrieval must be performed on the SAME thread that called the failing
    /// API function before that thread encounters another failure or clears the error.
    /// </description></item>
    /// </list>
    /// </description>
    /// </item>
    /// <item>
    /// <term>Memory Ownership and Lifetime</term>
    /// <description>
    /// <list type="bullet">
    /// <item><description>
    /// The error message string returned by <see cref="litert_lm_get_last_error_message"/>
    /// is owned by the library and resides in thread-local storage.
    /// </description></item>
    /// <item><description>
    /// The caller MUST NOT attempt to free, delete, or modify this pointer.
    /// </description></item>
    /// <item><description>
    /// The pointer remains valid only until the next failing LiteRT LM C API call on the
    /// same thread, until <see cref="litert_lm_clear_last_error"/> is called, or until
    /// the calling thread terminates.
    /// </description></item>
    /// <item><description>
    /// If the error message needs to be retained across subsequent API calls or transferred
    /// to another thread, callers MUST create a managed copy (e.g., as a <see cref="string"/>).
    /// </description></item>
    /// </list>
    /// </description>
    /// </item>
    /// <item>
    /// <term>Error Clearing Logic (Optional / Defensive)</term>
    /// <description>
    /// <list type="bullet">
    /// <item><description>
    /// Calling <see cref="litert_lm_clear_last_error"/> explicitly resets the calling
    /// thread's error state (message = <see cref="IntPtr.Zero"/>).
    /// </description></item>
    /// <item><description>
    /// Calling this function is NOT required during normal error handling: callers determine
    /// failure from API return values, and subsequent errors will automatically overwrite
    /// previous error state.
    /// </description></item>
    /// <item><description>
    /// This function is provided as an optional utility for defensive programming
    /// (e.g., resetting state before a call sequence or between test cases, analogous to
    /// setting <c>errno = 0</c> in POSIX) or to release thread-local error message memory
    /// on long-lived threads.
    /// </description></item>
    /// </list>
    /// </description>
    /// </item>
    /// <item>
    /// <term>Return Shape</term>
    /// <description>
    /// <list type="bullet">
    /// <item><description>
    /// Every function that can fail returns a <see cref="StatusCode"/>:
    /// <see cref="StatusCode.Ok"/> (0) on success, or another (positive) code on failure.
    /// Negative values are never returned.
    /// </description></item>
    /// <item><description>
    /// Results are delivered through trailing <c>out</c> parameters named <c>out*</c>.
    /// Passing <see langword="null"/> for an out-parameter yields
    /// <see cref="StatusCode.InvalidArgument"/>. Pointer out-parameters (<c>out IntPtr</c>)
    /// are set to <see cref="IntPtr.Zero"/> on failure. Scalar out-parameters
    /// (<c>int</c>, <c>bool</c>, <c>float</c>, <c>UIntPtr</c>, enums) are written only on
    /// success and are left untouched on failure.
    /// </description></item>
    /// <item><description>
    /// A value that is legitimately absent is a success: the function returns
    /// <see cref="StatusCode.Ok"/> and writes <see cref="IntPtr.Zero"/> to the out-parameter
    /// (for example, an engine without a configured start token).
    /// </description></item>
    /// <item><description>
    /// Every non-<see cref="StatusCode.Ok"/> return also records the calling thread's last error
    /// message, which can be retrieved immediately afterwards via
    /// <see cref="litert_lm_get_last_error_message"/>.
    /// </description></item>
    /// <item><description>
    /// The only functions that do not return a status are the <see cref="IntPtr.Zero"/>-safe
    /// <c>litert_lm_*_delete</c> destructors and the error-reporter functions declared in this class.
    /// </description></item>
    /// </list>
    /// </description>
    /// </item>
    /// </list>
    /// <example>
    /// <code>
    /// StatusCode status = EngineH.litert_lm_engine_create(settings, out IntPtr engine);
    /// if (status != StatusCode.Ok)
    /// {
    ///     IntPtr msgPtr = ErrorReporterH.litert_lm_get_last_error_message();
    ///     string msg = msgPtr != IntPtr.Zero ? Marshal.PtrToStringUTF8(msgPtr) : string.Empty;
    ///     Console.Error.WriteLine($"engine_create failed ({status}): {msg}");
    ///     return status;
    /// }
    /// </code>
    /// </example>
    /// </remarks>
    public static class ErrorReporterH
    {
        /// <summary>
        /// Returns the last error message recorded on the calling thread.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Extraction Precondition &amp; Expectations:
        /// Callers should only call this function AFTER an API function has signaled
        /// failure by returning a <see cref="StatusCode"/> other than <see cref="StatusCode.Ok"/>.
        /// Following standard C conventions, functions that succeed DO NOT clear or
        /// modify the error state; inspecting this function without verifying a return
        /// failure may return stale error messages from an earlier failure.
        /// </para>
        /// <para>
        /// Thread Locality:
        /// The error message is stored in thread-local storage and must be retrieved
        /// from the same thread that executed the failed API function.
        /// </para>
        /// <para>
        /// Memory Ownership &amp; Lifetime:
        /// The returned pointer is owned by the library (pointing to internal
        /// thread-local storage). The caller MUST NOT free, modify, or deallocate it.
        /// The pointer remains valid until the next LiteRT LM C API error occurs on the
        /// same thread, until <see cref="litert_lm_clear_last_error"/> is called on that
        /// thread, or until the thread terminates. If the caller needs the error message
        /// beyond this lifetime or across threads, the caller must copy it into a managed
        /// <see cref="string"/>.
        /// </para>
        /// </remarks>
        /// <returns>
        /// A pointer to a null-terminated UTF-8 string containing the error description, or
        /// <see cref="IntPtr.Zero"/> if no error has occurred on the calling thread or if the
        /// error state has been cleared.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr litert_lm_get_last_error_message();

        /// <summary>
        /// Clears the last error recorded on the calling thread, resetting the error
        /// message to <see cref="IntPtr.Zero"/>.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Clearing Logic &amp; Expectations:
        /// Calling this function is optional during normal API usage since return
        /// values indicate failure and any subsequent error automatically overwrites the
        /// thread-local error state. It is primarily intended as a defensive measure
        /// (e.g. establishing a clean baseline before an API call sequence or in test
        /// fixtures) or to promptly free thread-local memory held by error messages
        /// on long-lived worker threads.
        /// </para>
        /// <para>
        /// Thread Locality:
        /// Clears only the error state for the calling thread. Error states on other
        /// threads remain unaffected.
        /// </para>
        /// </remarks>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern void litert_lm_clear_last_error();
    }
}