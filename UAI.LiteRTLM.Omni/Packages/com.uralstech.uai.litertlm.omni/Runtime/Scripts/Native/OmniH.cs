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
using Uralstech.UAI.LiteRTLM.Omni;

#nullable enable
namespace Uralstech.UAI.LiteRTLM.Omni.Native
{
    /// <summary>
    /// P/Invoke bindings for the LiteRT-LM Omni C API declared in c/omni/omni.h.
    /// Functions that may fail return the shared canonical <see cref="StatusCode"/>
    /// and report details through <see cref="ErrorReporterH"/>.
    /// </summary>
    public static class OmniH
    {
        // Opaque C handles use SafeHandle wrappers; C size_t values use UIntPtr.

        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_omni_tts_engine_settings_create(
            OmniTtsModelType modelType, out OmniTtsEngineSettingsHandle outSettings);

        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern void litert_omni_tts_engine_settings_delete(IntPtr settings);

        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_omni_tts_engine_settings_set_model_folder(
            OmniTtsEngineSettingsHandle settings, [MarshalAs(UnmanagedType.LPUTF8Str)] string? modelFolder);

        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_omni_tts_engine_settings_set_cache_dir(
            OmniTtsEngineSettingsHandle settings, [MarshalAs(UnmanagedType.LPUTF8Str)] string? cacheDir);

        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_omni_tts_engine_settings_set_backend(
            OmniTtsEngineSettingsHandle settings, [MarshalAs(UnmanagedType.LPUTF8Str)] string? backendStr);

        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_omni_tts_engine_settings_set_num_threads(
            OmniTtsEngineSettingsHandle settings, int numThreads);

        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_omni_tts_engine_settings_set_gpu_context_low_priority(
            OmniTtsEngineSettingsHandle settings, [MarshalAs(UnmanagedType.I1)] bool lowPriority);

        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_omni_tts_engine_settings_set_hint_kernel_batch_size(
            OmniTtsEngineSettingsHandle settings, int batchSize);

        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_omni_tts_engine_settings_set_max_frames(
            OmniTtsEngineSettingsHandle settings, int maxFrames);

        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_omni_tts_engine_settings_set_language(
            OmniTtsEngineSettingsHandle settings, [MarshalAs(UnmanagedType.LPUTF8Str)] string? language);

        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_omni_tts_engine_settings_set_voice(
            OmniTtsEngineSettingsHandle settings, [MarshalAs(UnmanagedType.LPUTF8Str)] string? voice);

        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_omni_tts_engine_create(
            OmniTtsEngineSettingsHandle settings, out OmniTtsEngineHandle outEngine);

        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern void litert_omni_tts_engine_delete(IntPtr engine);

        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_omni_tts_engine_create_session(
            OmniTtsEngineHandle engine, out OmniTtsSessionHandle outSession);

        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern void litert_omni_tts_session_delete(IntPtr session);

        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_omni_tts_session_stream_begin(
            OmniTtsSessionHandle session, out OmniStreamStatus outStreamStatus);

        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_omni_tts_session_stream_push_text(
            OmniTtsSessionHandle session, [MarshalAs(UnmanagedType.LPUTF8Str)] string? textChunk,
            out OmniStreamStatus outStreamStatus);

        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_omni_tts_session_stream_finish(
            OmniTtsSessionHandle session, out OmniStreamStatus outStreamStatus);

        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_omni_tts_session_stream_process_next(
            OmniTtsSessionHandle session, out OmniStreamStatus outStreamStatus,
            out OmniAudioOutputHandle outAudio);

        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_omni_tts_session_synthesize(
            OmniTtsSessionHandle session, [MarshalAs(UnmanagedType.LPUTF8Str)] string? text,
            out OmniAudioOutputHandle outAudio);

        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern void litert_omni_audio_output_delete(IntPtr audio);

        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_omni_audio_output_get_pcm_samples(
            OmniAudioOutputHandle audio, out IntPtr outPcmSamples);

        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_omni_audio_output_get_num_pcm_samples(
            OmniAudioOutputHandle audio, out UIntPtr outNumPcmSamples);

        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_omni_audio_output_get_sample_rate_hz(
            OmniAudioOutputHandle audio, out int outSampleRateHz);

        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_omni_asr_engine_settings_create(
            OmniAsrModelType modelType, out OmniAsrEngineSettingsHandle outSettings);

        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern void litert_omni_asr_engine_settings_delete(IntPtr settings);

        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_omni_asr_engine_settings_set_model_folder(
            OmniAsrEngineSettingsHandle settings, [MarshalAs(UnmanagedType.LPUTF8Str)] string? modelFolder);

        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_omni_asr_engine_settings_set_cache_dir(
            OmniAsrEngineSettingsHandle settings, [MarshalAs(UnmanagedType.LPUTF8Str)] string? cacheDir);

        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_omni_asr_engine_settings_set_backend(
            OmniAsrEngineSettingsHandle settings, [MarshalAs(UnmanagedType.LPUTF8Str)] string? backendStr);

        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_omni_asr_engine_settings_set_num_threads(
            OmniAsrEngineSettingsHandle settings, int numThreads);

        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_omni_asr_engine_settings_set_gpu_context_low_priority(
            OmniAsrEngineSettingsHandle settings, [MarshalAs(UnmanagedType.I1)] bool lowPriority);

        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_omni_asr_engine_settings_set_hint_kernel_batch_size(
            OmniAsrEngineSettingsHandle settings, int batchSize);

        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_omni_asr_engine_settings_set_overlap_ratio(
            OmniAsrEngineSettingsHandle settings, float overlapRatio);

        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_omni_asr_engine_create(
            OmniAsrEngineSettingsHandle settings, out OmniAsrEngineHandle outEngine);

        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern void litert_omni_asr_engine_delete(IntPtr engine);

        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_omni_asr_engine_create_session(
            OmniAsrEngineHandle engine, out OmniAsrSessionHandle outSession);

        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern void litert_omni_asr_session_delete(IntPtr session);

        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_omni_asr_session_stream_begin(
            OmniAsrSessionHandle session, int sampleRateHz, out OmniStreamStatus outStreamStatus);

        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_omni_asr_session_stream_push_audio(
            OmniAsrSessionHandle session, [In] float[]? pcmSamples, UIntPtr numPcmSamples,
            out OmniStreamStatus outStreamStatus);

        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_omni_asr_session_stream_finish(
            OmniAsrSessionHandle session, out OmniStreamStatus outStreamStatus);

        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_omni_asr_session_stream_process_next(
            OmniAsrSessionHandle session, out OmniStreamStatus outStreamStatus,
            out OmniTextOutputHandle outText);

        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_omni_asr_session_transcribe(
            OmniAsrSessionHandle session, [In] float[]? pcmSamples, UIntPtr numPcmSamples,
            int sampleRateHz, out OmniTextOutputHandle outText);

        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern void litert_omni_text_output_delete(IntPtr text);

        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_omni_text_output_get_transcript(
            OmniTextOutputHandle text, out IntPtr outTranscript);

        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_omni_text_output_get_confirmed_text(
            OmniTextOutputHandle text, out IntPtr outConfirmedText);

        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_omni_text_output_get_unconfirmed_text(
            OmniTextOutputHandle text, out IntPtr outUnconfirmedText);
    }
}
