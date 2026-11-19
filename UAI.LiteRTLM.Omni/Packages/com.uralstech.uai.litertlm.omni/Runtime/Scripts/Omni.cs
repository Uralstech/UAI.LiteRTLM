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
using Uralstech.UAI.LiteRTLM.Omni.Native;

#nullable enable
namespace Uralstech.UAI.LiteRTLM.Omni
{
    /// <summary>Settings used to initialize an Omni text-to-speech engine.</summary>
    public sealed class OmniTtsEngineSettings : LiteRTLMWrapper<OmniTtsEngineSettingsHandle>
    {
        /// <summary>Creates settings for the given TTS model and model folder.</summary>
        /// <param name="modelType">The TTS model type.</param>
        /// <param name="modelFolder">A .litertlm package path or model directory.</param>
        public OmniTtsEngineSettings(OmniTtsModelType modelType, string modelFolder)
        {
            if (modelFolder is null)
                throw new ArgumentNullException(nameof(modelFolder));

            StatusCode status = OmniH.litert_omni_tts_engine_settings_create(
                modelType, out OmniTtsEngineSettingsHandle outHandle);
            OmniTtsEngineSettingsHandle handle = LiteRTLMException.Check(status, outHandle);
            DisposeOnCatch(handle, created => LiteRTLMException.Check(
                OmniH.litert_omni_tts_engine_settings_set_model_folder(created, modelFolder)));
            SetHandle(handle);
        }

        /// <summary>Sets the model package path or model directory.</summary>
        public void SetModelFolder(string modelFolder)
        {
            if (modelFolder is null)
                throw new ArgumentNullException(nameof(modelFolder));
            LockedExecute(handle => OmniH.litert_omni_tts_engine_settings_set_model_folder(
                handle, modelFolder));
        }

        /// <summary>Sets the directory used for downloaded model files and caches.</summary>
        public void SetCacheDir(string cacheDir)
        {
            if (cacheDir is null)
                throw new ArgumentNullException(nameof(cacheDir));
            LockedExecute(handle => OmniH.litert_omni_tts_engine_settings_set_cache_dir(
                handle, cacheDir));
        }

        /// <summary>Sets the execution backend to "cpu", "gpu", or "npu".</summary>
        public void SetBackend(string backend)
        {
            if (backend is null)
                throw new ArgumentNullException(nameof(backend));
            LockedExecute(handle => OmniH.litert_omni_tts_engine_settings_set_backend(
                handle, backend));
        }

        /// <summary>Sets the number of CPU threads.</summary>
        public void SetNumThreads(int numThreads) =>
            LockedExecute(handle => OmniH.litert_omni_tts_engine_settings_set_num_threads(
                handle, numThreads));

        /// <summary>Requests low GPU scheduling priority where the runtime supports it.</summary>
        public void SetGpuContextLowPriority(bool lowPriority) =>
            LockedExecute(handle => OmniH.litert_omni_tts_engine_settings_set_gpu_context_low_priority(
                handle, lowPriority));

        /// <summary>Sets the GPU kernel batch-size hint. Zero uses the runtime default.</summary>
        public void SetHintKernelBatchSize(int batchSize) =>
            LockedExecute(handle => OmniH.litert_omni_tts_engine_settings_set_hint_kernel_batch_size(
                handle, batchSize));

        /// <summary>Sets the maximum number of generated frames. Zero uses the model default.</summary>
        public void SetMaxFrames(int maxFrames) =>
            LockedExecute(handle => OmniH.litert_omni_tts_engine_settings_set_max_frames(
                handle, maxFrames));

        /// <summary>Sets the synthesis language.</summary>
        public void SetLanguage(string language)
        {
            if (language is null)
                throw new ArgumentNullException(nameof(language));
            LockedExecute(handle => OmniH.litert_omni_tts_engine_settings_set_language(
                handle, language));
        }

        /// <summary>Sets the synthesis voice.</summary>
        public void SetVoice(string voice)
        {
            if (voice is null)
                throw new ArgumentNullException(nameof(voice));
            LockedExecute(handle => OmniH.litert_omni_tts_engine_settings_set_voice(
                handle, voice));
        }
    }

    /// <summary>An Omni text-to-speech engine. Sessions share its model resources.</summary>
    public sealed class OmniTtsEngine : LiteRTLMWrapper<OmniTtsEngineHandle>
    {
        /// <summary>Creates a TTS engine from the given settings.</summary>
        public OmniTtsEngine(OmniTtsEngineSettings settings)
        {
            if (settings is null)
                throw new ArgumentNullException(nameof(settings));

            OmniTtsEngineHandle engineHandle = settings.LockedExecute(settingsHandle =>
            {
                StatusCode status = OmniH.litert_omni_tts_engine_create(
                    settingsHandle, out OmniTtsEngineHandle created);
                return (status, created);
            });
            SetHandle(engineHandle);
        }

        /// <summary>Creates a new TTS session.</summary>
        public OmniTtsSession CreateSession()
        {
            OmniTtsSessionHandle sessionHandle = LockedExecute(engineHandle =>
            {
                StatusCode status = OmniH.litert_omni_tts_engine_create_session(
                    engineHandle, out OmniTtsSessionHandle created);
                if (status is StatusCode.Ok)
                    DisposeOnCatch(created, session => session.DangerousAttachOwner(engineHandle));
                return (status, created);
            });
            return new OmniTtsSession(sessionHandle);
        }
    }

    /// <summary>A synchronous or incremental text-to-speech session.</summary>
    public sealed class OmniTtsSession : LiteRTLMWrapper<OmniTtsSessionHandle>
    {
        internal OmniTtsSession(OmniTtsSessionHandle handle) => SetHandle(handle);

        /// <summary>Starts a new stream and resets any previous stream state.</summary>
        public OmniStreamStatus StreamBegin()
        {
            OmniStreamStatus nativeStatus = default;
            LockedExecute(handle =>
            {
                StatusCode status = OmniH.litert_omni_tts_session_stream_begin(
                    handle, out nativeStatus);
                return (status, nativeStatus);
            });
            return nativeStatus;
        }

        /// <summary>Queues a text chunk while synthesis proceeds.</summary>
        public OmniStreamStatus StreamPushText(string textChunk)
        {
            if (textChunk is null)
                throw new ArgumentNullException(nameof(textChunk));

            OmniStreamStatus nativeStatus = default;
            LockedExecute(handle =>
            {
                StatusCode status = OmniH.litert_omni_tts_session_stream_push_text(
                    handle, textChunk, out nativeStatus);
                return (status, nativeStatus);
            });
            return nativeStatus;
        }

        /// <summary>Marks the end of the text input stream.</summary>
        public OmniStreamStatus StreamFinish()
        {
            OmniStreamStatus nativeStatus = default;
            LockedExecute(handle =>
            {
                StatusCode status = OmniH.litert_omni_tts_session_stream_finish(
                    handle, out nativeStatus);
                return (status, nativeStatus);
            });
            return nativeStatus;
        }

        /// <summary>Processes the next TTS chunk, returning audio when available.</summary>
        /// <param name="audio">The generated audio chunk, or null when no output is available.</param>
        /// <returns>The stream state.</returns>
        public OmniStreamStatus StreamProcessNext(out OmniAudioOutput? audio)
        {
            OmniStreamStatus nativeStatus = default;
            OmniAudioOutputHandle outputHandle = LockedExecute(handle =>
            {
                StatusCode status = OmniH.litert_omni_tts_session_stream_process_next(
                    handle, out nativeStatus, out OmniAudioOutputHandle output);
                return (status, output);
            });

            audio = nativeStatus == OmniStreamStatus.OutputAvailable && !outputHandle.IsInvalid
                ? new OmniAudioOutput(outputHandle)
                : null;
            if (audio is null)
                outputHandle.Dispose();
            return nativeStatus;
        }

        /// <summary>Synthesizes a complete text string.</summary>
        public OmniAudioOutput Synthesize(string text)
        {
            if (text is null)
                throw new ArgumentNullException(nameof(text));

            OmniAudioOutputHandle outputHandle = LockedExecute(handle =>
            {
                StatusCode status = OmniH.litert_omni_tts_session_synthesize(
                    handle, text, out OmniAudioOutputHandle output);
                return (status, output);
            });
            return new OmniAudioOutput(outputHandle);
        }
    }

    /// <summary>PCM audio produced by an Omni TTS session.</summary>
    public sealed class OmniAudioOutput : LiteRTLMWrapper<OmniAudioOutputHandle>
    {
        internal OmniAudioOutput(OmniAudioOutputHandle handle) => SetHandle(handle);

        /// <summary>The PCM sample rate in hertz.</summary>
        public int SampleRateHz => LockedExecute(handle =>
        {
            StatusCode status = OmniH.litert_omni_audio_output_get_sample_rate_hz(
                handle, out int sampleRateHz);
            return (status, sampleRateHz);
        });

        /// <summary>The number of PCM samples.</summary>
        public int NumPcmSamples
        {
            get
            {
                UIntPtr count = LockedExecute(handle =>
                {
                    StatusCode status = OmniH.litert_omni_audio_output_get_num_pcm_samples(
                        handle, out UIntPtr numSamples);
                    return (status, numSamples);
                });
                ulong value = count.ToUInt64();
                if (value > int.MaxValue)
                    throw new OverflowException("The PCM sample count exceeds the maximum managed array length.");
                return (int)value;
            }
        }

        /// <summary>Copies the output PCM samples into a managed array.</summary>
        public float[] GetPcmSamples() => LockedExecute(handle =>
        {
            StatusCode countStatus = OmniH.litert_omni_audio_output_get_num_pcm_samples(
                handle, out UIntPtr nativeCount);
            if (countStatus != StatusCode.Ok)
                return (countStatus, Array.Empty<float>());

            ulong count = nativeCount.ToUInt64();
            if (count > int.MaxValue)
                throw new OverflowException("The PCM sample count exceeds the maximum managed array length.");

            var samples = new float[(int)count];
            if (samples.Length == 0)
                return (StatusCode.Ok, samples);

            StatusCode samplesStatus = OmniH.litert_omni_audio_output_get_pcm_samples(
                handle, out IntPtr nativeSamples);
            if (samplesStatus != StatusCode.Ok)
                return (samplesStatus, samples);

            Marshal.Copy(nativeSamples, samples, 0, samples.Length);
            return (StatusCode.Ok, samples);
        });
    }

    /// <summary>Settings used to initialize an Omni speech-recognition engine.</summary>
    public sealed class OmniAsrEngineSettings : LiteRTLMWrapper<OmniAsrEngineSettingsHandle>
    {
        /// <summary>Creates settings for the given ASR model and model folder.</summary>
        public OmniAsrEngineSettings(OmniAsrModelType modelType, string modelFolder)
        {
            if (modelFolder is null)
                throw new ArgumentNullException(nameof(modelFolder));

            StatusCode status = OmniH.litert_omni_asr_engine_settings_create(
                modelType, out OmniAsrEngineSettingsHandle outHandle);
            OmniAsrEngineSettingsHandle handle = LiteRTLMException.Check(status, outHandle);
            DisposeOnCatch(handle, created => LiteRTLMException.Check(
                OmniH.litert_omni_asr_engine_settings_set_model_folder(created, modelFolder)));
            SetHandle(handle);
        }

        /// <summary>Sets the model package path or model directory.</summary>
        public void SetModelFolder(string modelFolder)
        {
            if (modelFolder is null)
                throw new ArgumentNullException(nameof(modelFolder));
            LockedExecute(handle => OmniH.litert_omni_asr_engine_settings_set_model_folder(
                handle, modelFolder));
        }

        /// <summary>Sets the directory used for downloaded model files and caches.</summary>
        public void SetCacheDir(string cacheDir)
        {
            if (cacheDir is null)
                throw new ArgumentNullException(nameof(cacheDir));
            LockedExecute(handle => OmniH.litert_omni_asr_engine_settings_set_cache_dir(
                handle, cacheDir));
        }

        /// <summary>Sets the execution backend to "cpu", "gpu", or "npu".</summary>
        public void SetBackend(string backend)
        {
            if (backend is null)
                throw new ArgumentNullException(nameof(backend));
            LockedExecute(handle => OmniH.litert_omni_asr_engine_settings_set_backend(
                handle, backend));
        }

        /// <summary>Sets the number of CPU threads.</summary>
        public void SetNumThreads(int numThreads) =>
            LockedExecute(handle => OmniH.litert_omni_asr_engine_settings_set_num_threads(
                handle, numThreads));

        /// <summary>Requests low GPU scheduling priority where the runtime supports it.</summary>
        public void SetGpuContextLowPriority(bool lowPriority) =>
            LockedExecute(handle => OmniH.litert_omni_asr_engine_settings_set_gpu_context_low_priority(
                handle, lowPriority));

        /// <summary>Sets the GPU kernel batch-size hint. Zero uses the runtime default.</summary>
        public void SetHintKernelBatchSize(int batchSize) =>
            LockedExecute(handle => OmniH.litert_omni_asr_engine_settings_set_hint_kernel_batch_size(
                handle, batchSize));

        /// <summary>Sets the ASR overlap ratio in the range [0, 1).</summary>
        public void SetOverlapRatio(float overlapRatio) =>
            LockedExecute(handle => OmniH.litert_omni_asr_engine_settings_set_overlap_ratio(
                handle, overlapRatio));
    }

    /// <summary>An Omni speech-recognition engine. Sessions share its model resources.</summary>
    public sealed class OmniAsrEngine : LiteRTLMWrapper<OmniAsrEngineHandle>
    {
        /// <summary>Creates an ASR engine from the given settings.</summary>
        public OmniAsrEngine(OmniAsrEngineSettings settings)
        {
            if (settings is null)
                throw new ArgumentNullException(nameof(settings));

            OmniAsrEngineHandle engineHandle = settings.LockedExecute(settingsHandle =>
            {
                StatusCode status = OmniH.litert_omni_asr_engine_create(
                    settingsHandle, out OmniAsrEngineHandle created);
                return (status, created);
            });
            SetHandle(engineHandle);
        }

        /// <summary>Creates a new ASR session.</summary>
        public OmniAsrSession CreateSession()
        {
            OmniAsrSessionHandle sessionHandle = LockedExecute(engineHandle =>
            {
                StatusCode status = OmniH.litert_omni_asr_engine_create_session(
                    engineHandle, out OmniAsrSessionHandle created);
                if (status is StatusCode.Ok)
                    DisposeOnCatch(created, session => session.DangerousAttachOwner(engineHandle));
                return (status, created);
            });
            return new OmniAsrSession(sessionHandle);
        }
    }

    /// <summary>A synchronous or incremental speech-recognition session.</summary>
    public sealed class OmniAsrSession : LiteRTLMWrapper<OmniAsrSessionHandle>
    {
        internal OmniAsrSession(OmniAsrSessionHandle handle) => SetHandle(handle);

        /// <summary>Starts a new mono-audio stream at the specified sample rate.</summary>
        public OmniStreamStatus StreamBegin(int sampleRateHz)
        {
            OmniStreamStatus nativeStatus = default;
            LockedExecute(handle =>
            {
                StatusCode status = OmniH.litert_omni_asr_session_stream_begin(
                    handle, sampleRateHz, out nativeStatus);
                return (status, nativeStatus);
            });
            return nativeStatus;
        }

        /// <summary>Queues more mono float PCM samples. Samples are copied before return.</summary>
        public OmniStreamStatus StreamPushAudio(float[] pcmSamples)
        {
            if (pcmSamples is null)
                throw new ArgumentNullException(nameof(pcmSamples));

            OmniStreamStatus nativeStatus = default;
            LockedExecute(handle =>
            {
                StatusCode status = OmniH.litert_omni_asr_session_stream_push_audio(
                    handle, pcmSamples, (UIntPtr)pcmSamples.Length,
                    out nativeStatus);
                return (status, nativeStatus);
            });
            GC.KeepAlive(pcmSamples);
            return nativeStatus;
        }

        /// <summary>Marks the end of the audio input stream.</summary>
        public OmniStreamStatus StreamFinish()
        {
            OmniStreamStatus nativeStatus = default;
            LockedExecute(handle =>
            {
                StatusCode status = OmniH.litert_omni_asr_session_stream_finish(
                    handle, out nativeStatus);
                return (status, nativeStatus);
            });
            return nativeStatus;
        }

        /// <summary>Processes the next ASR chunk, returning text when available.</summary>
        /// <param name="text">The recognized text chunk, or null when no output is available.</param>
        /// <returns>The stream state.</returns>
        public OmniStreamStatus StreamProcessNext(out OmniTextOutput? text)
        {
            OmniStreamStatus nativeStatus = default;
            OmniTextOutputHandle outputHandle = LockedExecute(handle =>
            {
                StatusCode status = OmniH.litert_omni_asr_session_stream_process_next(
                    handle, out nativeStatus, out OmniTextOutputHandle output);
                return (status, output);
            });

            text = nativeStatus == OmniStreamStatus.OutputAvailable && !outputHandle.IsInvalid
                ? new OmniTextOutput(outputHandle)
                : null;
            if (text is null)
                outputHandle.Dispose();
            return nativeStatus;
        }

        /// <summary>Transcribes a complete mono float PCM buffer.</summary>
        public OmniTextOutput Transcribe(float[] pcmSamples, int sampleRateHz)
        {
            if (pcmSamples is null)
                throw new ArgumentNullException(nameof(pcmSamples));

            OmniTextOutputHandle outputHandle = LockedExecute(handle =>
            {
                StatusCode status = OmniH.litert_omni_asr_session_transcribe(
                    handle, pcmSamples, (UIntPtr)pcmSamples.Length,
                    sampleRateHz, out OmniTextOutputHandle output);
                return (status, output);
            });
            GC.KeepAlive(pcmSamples);
            return new OmniTextOutput(outputHandle);
        }
    }

    /// <summary>Recognized text returned by an Omni ASR session.</summary>
    public sealed class OmniTextOutput : LiteRTLMWrapper<OmniTextOutputHandle>
    {
        private delegate StatusCode TextGetter(OmniTextOutputHandle text, out IntPtr output);

        internal OmniTextOutput(OmniTextOutputHandle handle) => SetHandle(handle);

        /// <summary>The joined transcript snapshot.</summary>
        public string Transcript => ReadText(OmniH.litert_omni_text_output_get_transcript);

        /// <summary>Confirmed text emitted in this output chunk.</summary>
        public string ConfirmedText => ReadText(OmniH.litert_omni_text_output_get_confirmed_text);

        /// <summary>The current unconfirmed partial hypothesis.</summary>
        public string UnconfirmedText => ReadText(OmniH.litert_omni_text_output_get_unconfirmed_text);

        private string ReadText(TextGetter getter)
        {
            return LockedExecute(handle =>
            {
                StatusCode status = getter(handle, out IntPtr nativeText);
                string value = status is StatusCode.Ok
                    ? Marshal.PtrToStringUTF8(nativeText) ?? string.Empty
                    : string.Empty;
                return (status, value);
            });
        }
    }
}
