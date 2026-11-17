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
using System.Collections.Concurrent;
using System.Runtime.InteropServices;
using System.Threading;
using Uralstech.UAI.LiteRTLM.Native;

#nullable enable
namespace Uralstech.UAI.LiteRTLM
{
    /// <summary>Invoked when chunks of generated content is received from an LiteRT-LM model.</summary>
    /// <remarks>The callback is NOT invoked on the main thread!</remarks>
    /// <param name="chunk">A borrowed reference to the generated chunk, you should not dispose it.</param>
    public delegate void StreamCallback(StreamChunk chunk);
    
    /// <summary>Exception thrown when a LiteRT LM native operation fails.</summary>
    public sealed class LiteRTLMException : Exception
    {
        /// <summary>The native LiteRT LM status code associated with this error.</summary>
        public StatusCode StatusCode { get; }
        
        private LiteRTLMException(StatusCode status, string message) : base(message)
        {
            StatusCode = status;
        }

        internal static void Check(StatusCode status)
        {
            if (status is StatusCode.Ok)
                return;

            IntPtr errorPtr = ErrorReporterH.litert_lm_get_last_error_message();
            if (errorPtr == IntPtr.Zero || Marshal.PtrToStringUTF8(errorPtr) is not string errorStr)
                throw new LiteRTLMException(status, $"LiteRT LM operation failed with status {status}.");
            
            ErrorReporterH.litert_lm_clear_last_error();
            throw new LiteRTLMException(status, errorStr);
        }

        internal static T Check<T>(StatusCode status, T result)
        {
            try
            {
                Check(status);
                return result;
            }
            catch
            {
                if (result is LiteRTHandle handle)
                    handle.Dispose();
                throw;
            }
        }
    }
    
    internal static partial class StreamCallbackHandler
    {
#if UNITY_6000_5_OR_NEWER
        [Unity.Scripting.LifecycleManagement.NoAutoStaticsCleanup]
#endif
        private static int s_idCounter;

#if UNITY_6000_5_OR_NEWER
        [Unity.Scripting.LifecycleManagement.AutoStaticsCleanup]
#endif
        private static readonly ConcurrentDictionary<IntPtr, StreamCallback> s_callbacks = new();

#if UNITY_5_3_OR_NEWER
        private static readonly EngineH.StreamCallback s_globalCallbackListenerInst =
            GlobalStreamCallbackListener;
        
        private static readonly IntPtr s_globalStreamCallbackListenerPtr =
            Marshal.GetFunctionPointerForDelegate(s_globalCallbackListenerInst);
#else
        private static readonly unsafe IntPtr s_globalStreamCallbackListenerPtr =
            (IntPtr)(delegate* unmanaged[Cdecl]<IntPtr, IntPtr, void>)(&GlobalStreamCallbackListener);
#endif

        public static IntPtr GetGlobalStreamCallbackListenerPtr() => s_globalStreamCallbackListenerPtr;

        public static IntPtr Register(StreamCallback callback)
        {
            IntPtr key = (IntPtr)Interlocked.Increment(ref s_idCounter);
            s_callbacks[key] = callback;
            return key;
        }

        public static void Deregister(IntPtr key) =>
            s_callbacks.TryRemove(key, out _);

#if UNITY_5_3_OR_NEWER
        [AOT.MonoPInvokeCallback(typeof(EngineH.StreamCallback))]
#else
        [System.Runtime.InteropServices.UnmanagedCallersOnly(CallConvs = new[] { typeof(System.Runtime.CompilerServices.CallConvCdecl) })]
#endif
        private static void GlobalStreamCallbackListener(IntPtr key, IntPtr chunk)
        {
            StreamChunk? managedChunk = null;
            bool isFinal = false;
            
            try
            {
                managedChunk = new StreamChunk(new StreamChunkHandle(chunk));
                isFinal = managedChunk.IsFinal();

                if (s_callbacks.TryGetValue(key, out StreamCallback? callback))
                    callback?.Invoke(managedChunk);
            }
            catch (Exception ex)
            {
                PackageLogger.LogException(ex);
            }
            finally
            {
                if (isFinal)
                    s_callbacks.TryRemove(key, out _);
                
                managedChunk?.Dispose();
            }
        }
    }
}