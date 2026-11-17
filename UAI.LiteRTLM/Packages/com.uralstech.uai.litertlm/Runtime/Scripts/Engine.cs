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
using System.Runtime.InteropServices;
using Uralstech.UAI.LiteRTLM.Native;
using Uralstech.UAI.LiteRTLM.Unsafe;

#nullable enable
namespace Uralstech.UAI.LiteRTLM
{
    /// <summary>Sampler parameters for a model.</summary>
    public sealed class SamplerParams : LiteRTLMWrapper<SamplerParamsHandle>
    {
        /// <summary>Creates LiteRT LM Sampler Parameters with a specific sampler type.</summary>
        /// <param name="type">The sampler type to use.</param>
        public SamplerParams(SamplerType type)
        {
            LiteRTLMException.Check(
                EngineH.litert_lm_sampler_params_create(type, out SamplerParamsHandle handle));
            SetHandle(handle);
        }

        /// <summary>Sets the top-k value.</summary>
        /// <param name="topK">The top-k value.</param>
        public void SetTopK(int topK) =>
            LockedExecute(handle => EngineH.litert_lm_sampler_params_set_top_k(handle, topK));

        /// <summary>Sets the top-p value.</summary>
        /// <param name="topP">The top-p value.</param>
        public void SetTopP(float topP) =>
            LockedExecute(handle => EngineH.litert_lm_sampler_params_set_top_p(handle, topP));

        /// <summary>Sets the temperature.</summary>
        /// <param name="temperature">The temperature.</param>
        public void SetTemperature(float temperature) =>
            LockedExecute(handle => EngineH.litert_lm_sampler_params_set_temperature(handle, temperature));

        /// <summary>Sets the seed.</summary>
        /// <param name="seed">The seed.</param>
        public void SetSeed(int seed) =>
            LockedExecute(handle => EngineH.litert_lm_sampler_params_set_seed(handle, seed));
    }

    /// <summary>Speculative decoding configuration for an engine or session.</summary>
    public sealed class SpeculativeDecodingConfig : LiteRTLMWrapper<SpeculativeDecodingConfigHandle>
    {
        /// <summary>Creates a LiteRT LM Speculative Decoding Config with every field unset.</summary>
        public SpeculativeDecodingConfig()
        {
            LiteRTLMException.Check(
                EngineH.litert_lm_speculative_decoding_config_create(out SpeculativeDecodingConfigHandle handle));
            SetHandle(handle);
        }

        /// <summary>Sets the speculative decoding mode.</summary>
        /// <param name="mode">The speculative decoding mode.</param>
        public void SetMode(SpeculativeDecodingMode mode) =>
            LockedExecute(handle => EngineH.litert_lm_speculative_decoding_config_set_mode(handle, mode));
    }

    /// <summary>Configuration options for a LiteRT LM session.</summary>
    public sealed class SessionConfig : LiteRTLMWrapper<SessionConfigHandle>
    {
        /// <summary>Creates a LiteRT LM Session Config.</summary>
        public SessionConfig()
        {
            LiteRTLMException.Check(
                EngineH.litert_lm_session_config_create(out SessionConfigHandle handle));
            SetHandle(handle);
        }

        /// <summary>
        /// Sets the maximum number of output tokens per decode step for this session.
        /// For thinking models, both thinking (reasoning) tokens and the final response
        /// tokens count towards this limit.
        /// </summary>
        /// <param name="maxOutputTokens">The maximum number of tokens to generate (including thinking tokens).</param>
        public void SetMaxOutputTokens(int maxOutputTokens) =>
            LockedExecute(handle => EngineH.litert_lm_session_config_set_max_output_tokens(handle, maxOutputTokens));

        /// <summary>Sets whether to apply prompt template for this session.</summary>
        /// <param name="applyPromptTemplate">Whether to apply prompt template.</param>
        public void SetApplyPromptTemplate(bool applyPromptTemplate) =>
            LockedExecute(handle => EngineH.litert_lm_session_config_set_apply_prompt_template(handle, applyPromptTemplate));

        /// <summary>Sets whether to enable speculative decoding for this session.</summary>
        /// <param name="enableSpeculativeDecoding">Whether to enable speculative decoding.</param>
        [Obsolete("Use SetSpeculativeDecodingConfig instead.")]
        public void SetEnableSpeculativeDecoding(bool enableSpeculativeDecoding) =>
            LockedExecute(handle => EngineH.litert_lm_session_config_set_enable_speculative_decoding(handle, enableSpeculativeDecoding));

        /// <summary>Sets the speculative decoding config of this session, overriding the engine's config.</summary>
        /// <param name="speculativeDecodingConfig">The speculative decoding config.</param>
        public void SetSpeculativeDecodingConfig(SpeculativeDecodingConfig speculativeDecodingConfig)
        {
            if (speculativeDecodingConfig is null)
                throw new ArgumentNullException(nameof(speculativeDecodingConfig));

            LockedExecute(handle => EngineH.litert_lm_session_config_set_speculative_decoding_config(
                handle, speculativeDecodingConfig.DangerousGetHandle()));
            GC.KeepAlive(speculativeDecodingConfig);
        }

        /// <summary>Sets the sampler parameters for this session config.</summary>
        /// <param name="samplerParams">The sampler parameters to use.</param>
        public void SetSamplerParams(SamplerParams samplerParams)
        {
            if (samplerParams is null)
                throw new ArgumentNullException(nameof(samplerParams));

            LockedExecute(handle => EngineH.litert_lm_session_config_set_sampler_params(
                handle, samplerParams.DangerousGetHandle()));
            GC.KeepAlive(samplerParams);
        }

        /// <summary>Sets the path to the text LoRA weights file.</summary>
        /// <param name="loraPath">The path to the text LoRA weights file.</param>
        public void SetLoraPath(string loraPath) =>
            LockedExecute(handle => EngineH.litert_lm_session_config_set_lora_path(handle, loraPath));

        /// <summary>Sets the path to the Audio LoRA weights file.</summary>
        /// <param name="audioLoraPath">The path to the audio LoRA weights file.</param>
        public void SetAudioLoraPath(string audioLoraPath) =>
            LockedExecute(handle => EngineH.litert_lm_session_config_set_audio_lora_path(handle, audioLoraPath));
    }

    /// <summary>Configuration for repetition, presence, and frequency penalties during decoding.</summary>
    public sealed class RepetitionPenaltyConfig : LiteRTLMWrapper<RepetitionPenaltyConfigHandle>
    {
        /// <summary>Creates a LiteRT LM Repetition Penalty Config with default values.</summary>
        public RepetitionPenaltyConfig()
        {
            LiteRTLMException.Check(
                EngineH.litert_lm_repetition_penalty_config_create(out RepetitionPenaltyConfigHandle handle));
            SetHandle(handle);
        }

        /// <summary>Sets the multiplicative repetition penalty.</summary>
        /// <param name="repetitionPenalty">The multiplicative penalty (&gt;= 1.0f).</param>
        public void SetRepetitionPenalty(float repetitionPenalty) =>
            LockedExecute(handle => EngineH.litert_lm_repetition_penalty_config_set_repetition_penalty(handle, repetitionPenalty));

        /// <summary>Sets the subtractive presence penalty.</summary>
        /// <param name="presencePenalty">The presence penalty.</param>
        public void SetPresencePenalty(float presencePenalty) =>
            LockedExecute(handle => EngineH.litert_lm_repetition_penalty_config_set_presence_penalty(handle, presencePenalty));

        /// <summary>Sets the subtractive frequency penalty.</summary>
        /// <param name="frequencyPenalty">The frequency penalty.</param>
        public void SetFrequencyPenalty(float frequencyPenalty) =>
            LockedExecute(handle => EngineH.litert_lm_repetition_penalty_config_set_frequency_penalty(handle, frequencyPenalty));

        /// <summary>Sets the recent-token window size for repetition penalization (0 = unlimited).</summary>
        /// <param name="windowSize">The window size (&gt;= 0).</param>
        public void SetWindowSize(int windowSize) =>
            LockedExecute(handle => EngineH.litert_lm_repetition_penalty_config_set_window_size(handle, windowSize));
    }

    /// <summary>Configuration for banning repeating n-grams during decoding.</summary>
    public sealed class NoRepeatNgramConfig : LiteRTLMWrapper<NoRepeatNgramConfigHandle>
    {
        /// <summary>Creates a LiteRT LM No Repeat Ngram Config with default values.</summary>
        public NoRepeatNgramConfig()
        {
            LiteRTLMException.Check(
                EngineH.litert_lm_no_repeat_ngram_config_create(out NoRepeatNgramConfigHandle handle));
            SetHandle(handle);
        }

        /// <summary>Sets the n-gram size banned from repeating (&lt;= 0 disables).</summary>
        /// <param name="noRepeatNgramSize">The n-gram size.</param>
        public void SetNoRepeatNgramSize(int noRepeatNgramSize) =>
            LockedExecute(handle => EngineH.litert_lm_no_repeat_ngram_config_set_no_repeat_ngram_size(handle, noRepeatNgramSize));

        /// <summary>Sets the recent-token window size for checking repeating n-grams (0 = unlimited).</summary>
        /// <param name="windowSize">The window size (&gt;= 0).</param>
        public void SetWindowSize(int windowSize) =>
            LockedExecute(handle => EngineH.litert_lm_no_repeat_ngram_config_set_window_size(handle, windowSize));
    }

    /// <summary>Configuration for suppressing specific token IDs during decoding.</summary>
    public sealed class SuppressTokensConfig : LiteRTLMWrapper<SuppressTokensConfigHandle>
    {
        /// <summary>Creates a LiteRT LM Suppress Tokens Config with an empty set of suppressed tokens.</summary>
        public SuppressTokensConfig()
        {
            LiteRTLMException.Check(
                EngineH.litert_lm_suppress_tokens_config_create(out SuppressTokensConfigHandle handle));
            SetHandle(handle);
        }

        /// <summary>Sets the list of token IDs to suppress (pass <see langword="null"/> or empty to clear).</summary>
        /// <param name="suppressTokens">The token IDs to suppress.</param>
        public void SetSuppressTokens(params int[]? suppressTokens) =>
            LockedExecute(handle => EngineH.litert_lm_suppress_tokens_config_set_suppress_tokens(
                handle, suppressTokens, (UIntPtr)(suppressTokens?.Length ?? 0)));
    }

    /// <summary>Multimodal input chunk passed to a LiteRT LM session.</summary>
    public sealed class InputData : LiteRTLMWrapper<InputDataHandle>
    {
        /// <summary>Creates a text <see cref="InputData"/> instance.</summary>
        /// <param name="text">The UTF-8 text prompt or fragment.</param>
        public InputData(string text)
        {
            InputDataHandle handle;
            if (string.IsNullOrEmpty(text))
            {
                LiteRTLMException.Check(EngineH.litert_lm_input_data_create(
                    InputDataType.Text, IntPtr.Zero, UIntPtr.Zero, out handle));
            }
            else
            {
                using PackageUnsafeUtils.TempMem tempMem = UnsafeUtils.AllocateStringUTF8(text);
                LiteRTLMException.Check(EngineH.litert_lm_input_data_create(
                    InputDataType.Text, tempMem.Ptr, tempMem.Size, out handle));
            }
            
            SetHandle(handle);
        }
        
        /// <summary>Creates an <see cref="InputData"/> instance from a read-only span of bytes.</summary>
        /// <param name="type">The input data type.</param>
        /// <param name="data">The raw input bytes.</param>
        public unsafe InputData(InputDataType type, ReadOnlySpan<byte> data)
        {
            fixed (byte* dataPtr = data)
            {
                LiteRTLMException.Check(EngineH.litert_lm_input_data_create(
                    type, (IntPtr)dataPtr, (UIntPtr)data.Length, out InputDataHandle handle));
                SetHandle(handle);
            }
        }

        /// <summary>Creates an <see cref="InputData"/> instance from a native buffer pointer.</summary>
        /// <param name="type">The input data type.</param>
        /// <param name="data">Pointer to the native data buffer (copied internally).</param>
        /// <param name="size">The size of the buffer in bytes.</param>
        public InputData(InputDataType type, IntPtr data, UIntPtr size)
        {
            LiteRTLMException.Check(
                EngineH.litert_lm_input_data_create(type, data, size, out InputDataHandle handle));
            SetHandle(handle);
        }

        /// <summary>Creates an empty <see cref="InputData"/> marker (e.g. image-end or audio-end).</summary>
        /// <param name="type">The input data type.</param>
        public InputData(InputDataType type)
        {
            LiteRTLMException.Check(
                EngineH.litert_lm_input_data_create(type, IntPtr.Zero, UIntPtr.Zero, out InputDataHandle handle));
            SetHandle(handle);
        }
    }

    /// <summary>Settings used to initialize a LiteRT LM <see cref="Engine"/>.</summary>
    public sealed class EngineSettings : LiteRTLMWrapper<EngineSettingsHandle>
    {
        /// <summary>Creates LiteRT LM Engine Settings from a model file path.</summary>
        /// <param name="modelPath">The path to the model file.</param>
        /// <param name="backend">The main backend to use (e.g., "cpu", "gpu", "npu").</param>
        /// <param name="visionBackend">The vision backend to use, or <see langword="null"/> if not set.</param>
        /// <param name="audioBackend">The audio backend to use, or <see langword="null"/> if not set.</param>
        public EngineSettings(string modelPath, string backend, string? visionBackend = null, string? audioBackend = null)
        {
            LiteRTLMException.Check(EngineH.litert_lm_engine_settings_create(
                modelPath, backend, visionBackend, audioBackend, out EngineSettingsHandle handle));
            SetHandle(handle);
        }

        /// <summary>
        /// Creates LiteRT LM Engine Settings from a raw file descriptor.
        /// The engine takes ownership of <paramref name="fileDescriptor"/> and closes it when done.
        /// </summary>
        /// <param name="fileDescriptor">The file descriptor of the model.</param>
        /// <param name="backend">The main backend to use (e.g., "cpu", "gpu", "npu").</param>
        /// <param name="visionBackend">The vision backend to use, or <see langword="null"/> if not set.</param>
        /// <param name="audioBackend">The audio backend to use, or <see langword="null"/> if not set.</param>
        public EngineSettings(int fileDescriptor, string backend, string? visionBackend = null, string? audioBackend = null)
        {
            LiteRTLMException.Check(EngineH.litert_lm_engine_settings_create_from_raw_file_descriptor(
                fileDescriptor, backend, visionBackend, audioBackend, out EngineSettingsHandle handle));
            SetHandle(handle);
        }

        /// <summary>Sets the maximum number of tokens for the engine.</summary>
        /// <param name="maxNumTokens">The maximum number of tokens.</param>
        public void SetMaxNumTokens(int maxNumTokens) =>
            LockedExecute(handle => EngineH.litert_lm_engine_settings_set_max_num_tokens(handle, maxNumTokens));

        /// <summary>Sets the number of threads for the CPU backend.</summary>
        /// <param name="numThreads">The number of threads.</param>
        public void SetNumThreads(int numThreads) =>
            LockedExecute(handle => EngineH.litert_lm_engine_settings_set_num_threads(handle, numThreads));

        /// <summary>Sets the number of threads for the audio CPU backend.</summary>
        /// <param name="numThreads">The number of threads.</param>
        public void SetAudioNumThreads(int numThreads) =>
            LockedExecute(handle => EngineH.litert_lm_engine_settings_set_audio_num_threads(handle, numThreads));

        /// <summary>Sets whether the engine should load different sections of the litertlm file in parallel.</summary>
        /// <param name="parallelFileSectionLoading">Whether to load in parallel.</param>
        public void SetParallelFileSectionLoading(bool parallelFileSectionLoading) =>
            LockedExecute(handle => EngineH.litert_lm_engine_settings_set_parallel_file_section_loading(handle, parallelFileSectionLoading));

        /// <summary>Sets whether to enable single-threaded execution.</summary>
        /// <param name="singleThreadedExecution">Whether to enable single-threaded execution.</param>
        public void SetSingleThreadedExecution(bool singleThreadedExecution) =>
            LockedExecute(handle => EngineH.litert_lm_engine_settings_set_single_threaded_execution(handle, singleThreadedExecution));

        /// <summary>Sets the maximum number of images for the legacy engine implementation.</summary>
        /// <param name="maxNumImages">The maximum number of images.</param>
        public void SetMaxNumImages(int maxNumImages) =>
            LockedExecute(handle => EngineH.litert_lm_engine_settings_set_max_num_images(handle, maxNumImages));

        /// <summary>Sets the maximum vision tokens generated per image for the engine.</summary>
        /// <param name="maxVisionTokensPerImage">The maximum vision tokens per image.</param>
        public void SetMaxVisionTokensPerImage(int maxVisionTokensPerImage) =>
            LockedExecute(handle => EngineH.litert_lm_engine_settings_set_max_vision_tokens_per_image(handle, maxVisionTokensPerImage));

        /// <summary>Sets the cache directory for the engine.</summary>
        /// <param name="cacheDir">The cache directory.</param>
        public void SetCacheDir(string cacheDir) =>
            LockedExecute(handle => EngineH.litert_lm_engine_settings_set_cache_dir(handle, cacheDir));

        /// <summary>Sets the LiteRT dispatch library directory for the NPU backend.</summary>
        /// <param name="libDir">The dispatch library directory.</param>
        public void SetLiteRTDispatchLibDir(string libDir) =>
            LockedExecute(handle => EngineH.litert_lm_engine_settings_set_litert_dispatch_lib_dir(handle, libDir));

        /// <summary>Sets the activation data type.</summary>
        /// <param name="activationDataType">The activation data type.</param>
        public void SetActivationDataType(ActivationDataType activationDataType) =>
            LockedExecute(handle => EngineH.litert_lm_engine_settings_set_activation_data_type(handle, activationDataType));

        /// <summary>Sets the prefill chunk size for the CPU backend with dynamic models.</summary>
        /// <param name="prefillChunkSize">The prefill chunk size.</param>
        public void SetPrefillChunkSize(int prefillChunkSize) =>
            LockedExecute(handle => EngineH.litert_lm_engine_settings_set_prefill_chunk_size(handle, prefillChunkSize));

        /// <summary>Sets whether YNNPACK should delegate supported operations before XNNPACK.</summary>
        /// <param name="enableYnnpack">Whether to enable YNNPACK.</param>
        public void SetEnableYnnpack(bool enableYnnpack) =>
            LockedExecute(handle => EngineH.litert_lm_engine_settings_set_enable_ynnpack(handle, enableYnnpack));

        /// <summary>Enables benchmarking for the engine.</summary>
        public void EnableBenchmark() =>
            LockedExecute(EngineH.litert_lm_engine_settings_enable_benchmark);

        /// <summary>Sets the number of prefill tokens for benchmarking.</summary>
        /// <param name="numPrefillTokens">The number of prefill tokens.</param>
        public void SetNumPrefillTokens(int numPrefillTokens) =>
            LockedExecute(handle => EngineH.litert_lm_engine_settings_set_num_prefill_tokens(handle, numPrefillTokens));

        /// <summary>Sets the number of decode tokens for benchmarking.</summary>
        /// <param name="numDecodeTokens">The number of decode tokens.</param>
        public void SetNumDecodeTokens(int numDecodeTokens) =>
            LockedExecute(handle => EngineH.litert_lm_engine_settings_set_num_decode_tokens(handle, numDecodeTokens));

        /// <summary>Sets whether to enable speculative decoding.</summary>
        /// <param name="enableSpeculativeDecoding">Whether to enable speculative decoding.</param>
        [Obsolete("Use SetSpeculativeDecodingConfig instead.")]
        public void SetEnableSpeculativeDecoding(bool enableSpeculativeDecoding) =>
            LockedExecute(handle => EngineH.litert_lm_engine_settings_set_enable_speculative_decoding(handle, enableSpeculativeDecoding));

        /// <summary>Sets the default speculative decoding config of the engine's sessions.</summary>
        /// <param name="speculativeDecodingConfig">The speculative decoding config.</param>
        public void SetSpeculativeDecodingConfig(SpeculativeDecodingConfig speculativeDecodingConfig)
        {
            if (speculativeDecodingConfig is null)
                throw new ArgumentNullException(nameof(speculativeDecodingConfig));

            LockedExecute(handle => EngineH.litert_lm_engine_settings_set_speculative_decoding_config(
                handle, speculativeDecodingConfig.DangerousGetHandle()));
            GC.KeepAlive(speculativeDecodingConfig);
        }

        /// <summary>Sets the number of decode steps per sync for the Artisan GPU backend.</summary>
        /// <param name="numDecodeStepsPerSync">The number of decode steps per sync.</param>
        public void SetGPUDecodeStepsPerSync(int numDecodeStepsPerSync) =>
            LockedExecute(handle => EngineH.litert_lm_engine_settings_set_gpu_decode_steps_per_sync(handle, numDecodeStepsPerSync));

        /// <summary>Sets whether to wait for weight uploads for the Artisan GPU backend.</summary>
        /// <param name="waitForWeightUploads">Whether to wait for weight uploads.</param>
        public void SetGPUWaitForWeightUploads(bool waitForWeightUploads) =>
            LockedExecute(handle => EngineH.litert_lm_engine_settings_set_gpu_wait_for_weight_uploads(handle, waitForWeightUploads));

        /// <summary>Sets whether to use ringbuffers for local attention KV cache.</summary>
        /// <param name="useRingbuffersLocalAttention">Whether to use ringbuffers for local attention.</param>
        public void SetUseRingbuffersLocalAttention(bool useRingbuffersLocalAttention) =>
            LockedExecute(handle => EngineH.litert_lm_engine_settings_set_use_ringbuffers_local_attention(handle, useRingbuffersLocalAttention));

        /// <summary>Sets the LoRA rank for the engine.</summary>
        /// <param name="loraRank">The LoRA rank.</param>
        public void SetLoraRank(int loraRank) =>
            LockedExecute(handle => EngineH.litert_lm_engine_settings_set_lora_rank(handle, loraRank));

        /// <summary>Sets the supported LoRA ranks for the engine.</summary>
        /// <param name="loraRanks">The supported LoRA ranks.</param>
        public void SetSupportedLoraRanks(params int[] loraRanks)
        {
            if (loraRanks is null)
                throw new ArgumentNullException(nameof(loraRanks));

            LockedExecute(handle => EngineH.litert_lm_engine_settings_set_supported_lora_ranks(
                handle, loraRanks, (UIntPtr)loraRanks.Length));
        }

        /// <summary>Sets the Audio LoRA rank for the engine.</summary>
        /// <param name="loraRank">The Audio LoRA rank.</param>
        public void SetAudioLoraRank(int loraRank) =>
            LockedExecute(handle => EngineH.litert_lm_engine_settings_set_audio_lora_rank(handle, loraRank));

        /// <summary>Sets the supported Audio LoRA ranks for the engine.</summary>
        /// <param name="loraRanks">The supported Audio LoRA ranks.</param>
        public void SetSupportedAudioLoraRanks(params int[] loraRanks)
        {
            if (loraRanks is null)
                throw new ArgumentNullException(nameof(loraRanks));

            LockedExecute(handle => EngineH.litert_lm_engine_settings_set_supported_audio_lora_ranks(
                handle, loraRanks, (UIntPtr)loraRanks.Length));
        }

        /// <summary>Sets whether to enable Metal residency set on GPU (Apple platforms only).</summary>
        /// <param name="enableMetalResidencySet">Whether to enable Metal residency set.</param>
        public void SetGPUEnableMetalResidencySet(bool enableMetalResidencySet) =>
            LockedExecute(handle => EngineH.litert_lm_engine_settings_set_gpu_enable_metal_residency_set(handle, enableMetalResidencySet));
    }

    /// <summary>
    /// LiteRT LM Engine owning model resources shared by all sessions and conversations created from it.
    /// Every child <see cref="Session"/> must be disposed before disposing this engine.
    /// </summary>
    public sealed class Engine : LiteRTLMWrapper<EngineHandle>
    {
        /// <summary>Creates a LiteRT LM Engine from the given settings.</summary>
        /// <param name="settings">The engine settings.</param>
        public Engine(EngineSettings settings)
        {
            if (settings is null)
                throw new ArgumentNullException(nameof(settings));

            LiteRTLMException.Check(
                EngineH.litert_lm_engine_create(settings.DangerousGetHandle(), out EngineHandle handle));
            GC.KeepAlive(settings);
            SetHandle(handle);
        }

        /// <summary>Creates a LiteRT LM <see cref="Session"/>.</summary>
        /// <param name="config">The session config, or <see langword="null"/> to use the default config.</param>
        /// <returns>The created session.</returns>
        public Session CreateSession(SessionConfig? config = null)
        {
            SessionHandle sessionHandle = LockedExecute(handle =>
            {
                StatusCode result = EngineH.litert_lm_engine_create_session(handle,
                    config?.DangerousGetHandle() ?? SessionConfigHandle.Null, out SessionHandle outSession);
                
                if (result is StatusCode.Ok)
                    DisposeOnCatch(outSession, session => session.DangerousAttachOwner(handle));
                return (result, outSession);
            });
            
            GC.KeepAlive(config);
            return new Session(sessionHandle);
        }

        /// <summary>Tokenizes text using the engine's tokenizer.</summary>
        /// <param name="text">The UTF-8 string to tokenize.</param>
        /// <returns>The tokenization result.</returns>
        public TokenizeResult Tokenize(string text)
        {
            TokenizeResultHandle resultHandle = LockedExecute(handle =>
                (EngineH.litert_lm_engine_tokenize(handle, text, out TokenizeResultHandle outResult), outResult));
            return new TokenizeResult(resultHandle);
        }

        /// <summary>Detokenizes token IDs using the engine's tokenizer.</summary>
        /// <param name="tokens">The token IDs to detokenize.</param>
        /// <returns>The detokenization result.</returns>
        public DetokenizeResult Detokenize(params int[] tokens)
        {
            if (tokens is null)
                throw new ArgumentNullException(nameof(tokens));

            DetokenizeResultHandle resultHandle = LockedExecute(handle => (EngineH.litert_lm_engine_detokenize(
                handle, tokens, (UIntPtr)tokens.Length, out DetokenizeResultHandle outResult), outResult));
            return new DetokenizeResult(resultHandle);
        }

        /// <summary>Returns the configured start token (BOS), or <see langword="null"/> if none is configured.</summary>
        /// <returns>The start token, or <see langword="null"/>.</returns>
        public TokenUnion? GetStartToken()
        {
            TokenUnionHandle tokenHandle = LockedExecute(handle =>
                (EngineH.litert_lm_engine_get_start_token(handle, out TokenUnionHandle outToken), outToken));
            if (!tokenHandle.IsInvalid)
                return new TokenUnion(tokenHandle);

            tokenHandle.Dispose();
            return null;
        }

        /// <summary>Returns the configured stop tokens (EOS), or <see langword="null"/> if none are configured.</summary>
        /// <returns>The stop tokens collection, or <see langword="null"/>.</returns>
        public TokenUnions? GetStopTokens()
        {
            TokenUnionsHandle tokensHandle = LockedExecute(handle =>
                (EngineH.litert_lm_engine_get_stop_tokens(handle, out TokenUnionsHandle outTokens), outTokens));
            if (!tokensHandle.IsInvalid)
                return new TokenUnions(tokensHandle);

            tokensHandle.Dispose();
            return null;
        }
    }

    /// <summary>Low-level stateful inference session created from an <see cref="Engine"/>.</summary>
    public sealed class Session : LiteRTLMWrapper<SessionHandle>
    {
        internal Session(SessionHandle handle)
        {
            SetHandle(handle);
        }

        /// <summary>Clones this session, copying its state so the new session can be used independently.</summary>
        /// <returns>The cloned session.</returns>
        public Session Clone()
        {
            SessionHandle clonedHandle = LockedExecute(handle =>
            {
                StatusCode result = EngineH.litert_lm_session_clone(
                    handle, out SessionHandle outSession);
                
                if (result is StatusCode.Ok)
                    DisposeOnCatch(outSession, session => session.DangerousAttachOwner(handle.OwnerHandle!));
                return (result, outSession);
            });
            
            return new Session(clonedHandle);
        }

        /// <summary>
        /// Requests cancellation of the current processing in the session.
        /// After cancellation, the session MUST NOT be reused; only dispose it.
        /// </summary>
        public void CancelProcess() =>
            LockedExecute(EngineH.litert_lm_session_cancel_process);

        /// <summary>Saves the current state of the session to a checkpoint with the given label.</summary>
        /// <param name="label">Label for the checkpoint.</param>
        public void SaveCheckpoint(string label) =>
            LockedExecute(handle => EngineH.litert_lm_session_save_checkpoint(handle, label));

        /// <summary>Rewinds the session to the given checkpoint label.</summary>
        /// <param name="label">Label of the checkpoint to rewind to.</param>
        public void RewindToCheckpoint(string label) =>
            LockedExecute(handle => EngineH.litert_lm_session_rewind_to_checkpoint(handle, label));

        /// <summary>Rewinds the session to a specific step number.</summary>
        /// <param name="step">The step number to rewind to.</param>
        public void RewindToStep(int step) =>
            LockedExecute(handle => EngineH.litert_lm_session_rewind_to_step(handle, step));

        /// <summary>Prefills the model with the given multimodal inputs (blocking).</summary>
        /// <param name="inputs">The multimodal input chunks.</param>
        public void RunPrefill(params InputData[] inputs) =>
            RunPrefill((IReadOnlyList<InputData>)inputs);

        /// <summary>Prefills the model with the given multimodal inputs (blocking).</summary>
        /// <param name="inputs">The multimodal input chunks.</param>
        public void RunPrefill(IReadOnlyList<InputData> inputs) =>
            ExecuteWithHandles<InputData, InputDataHandle>(inputs, ptrs =>
                LockedExecute(handle => EngineH.litert_lm_session_run_prefill(handle, ptrs, (UIntPtr)ptrs.Length)));

        /// <summary>Decodes a response based on previously prefilled inputs (blocking).</summary>
        /// <returns>The generated responses.</returns>
        public Responses RunDecode()
        {
            ResponsesHandle responsesHandle = LockedExecute(handle =>
                (EngineH.litert_lm_session_run_decode(handle, out ResponsesHandle outResponses), outResponses));
            return new Responses(responsesHandle);
        }

        /// <summary>Scores a single target text against the prefilled context (blocking).</summary>
        /// <param name="targetText">The target text to score.</param>
        /// <param name="storeTokenLengths">Whether to store token lengths in the responses.</param>
        /// <returns>The scoring responses.</returns>
        public Responses RunTextScoring(string targetText, bool storeTokenLengths = false) =>
            RunTextScoring(new[] { targetText }, storeTokenLengths);

        /// <summary>Scores the target texts against the prefilled context (blocking).</summary>
        /// <param name="targetTexts">The target texts to score.</param>
        /// <param name="storeTokenLengths">Whether to store token lengths in the responses.</param>
        /// <returns>The scoring responses.</returns>
        public Responses RunTextScoring(IReadOnlyList<string> targetTexts, bool storeTokenLengths = false)
        {
            if (targetTexts is null)
                throw new ArgumentNullException(nameof(targetTexts));

            int count = targetTexts.Count;
            PackageUnsafeUtils.TempMem[] allocatedMemory = new PackageUnsafeUtils.TempMem[count];
            IntPtr[] ptrs = new IntPtr[count];

            try
            {
                for (int i = 0; i < count; i++)
                {
                    string targetText = targetTexts[i];
                    if (targetText is null)
                        throw new ArgumentNullException(nameof(targetTexts), $"Text at index {i} was null.");
                    if (targetText.Length == 0)
                        throw new ArgumentException($"Text at index {i} was empty", nameof(targetTexts));
                    
                    PackageUnsafeUtils.TempMem allocated = UnsafeUtils.AllocateNullTerminatedStringUTF8(targetText);
                    allocatedMemory[i] = allocated;
                    ptrs[i] = allocated.Ptr;
                }
                
                ResponsesHandle responsesHandle = LockedExecute(handle => (EngineH.litert_lm_session_run_text_scoring(
                        handle, ptrs, (UIntPtr)count, storeTokenLengths, out ResponsesHandle outResponses), outResponses));
                return new Responses(responsesHandle);
            }
            finally
            {
                foreach (PackageUnsafeUtils.TempMem? memory in allocatedMemory)
                    memory?.Dispose();
            }
        }

        /// <summary>Prefills and decodes content from the given inputs (blocking).</summary>
        /// <param name="inputs">The multimodal input chunks.</param>
        /// <returns>The generated responses.</returns>
        public Responses GenerateContent(params InputData[] inputs) =>
            GenerateContent((IReadOnlyList<InputData>)inputs);

        /// <summary>Prefills and decodes content from the given inputs (blocking).</summary>
        /// <param name="inputs">The multimodal input chunks.</param>
        /// <returns>The generated responses.</returns>
        public Responses GenerateContent(IReadOnlyList<InputData> inputs)
        {
            ResponsesHandle responsesHandle = null!;
            ExecuteWithHandles<InputData, InputDataHandle>(inputs, ptrs =>
            {
                responsesHandle = LockedExecute(handle => (EngineH.litert_lm_session_generate_content(
                    handle, ptrs, (UIntPtr)ptrs.Length, out ResponsesHandle outResponses), outResponses));
            });
            return new Responses(responsesHandle);
        }

        /// <summary>Retrieves benchmark information collected on this session.</summary>
        /// <returns>The benchmark info.</returns>
        public BenchmarkInfo GetBenchmarkInfo()
        {
            BenchmarkInfoHandle benchmarkHandle = LockedExecute(handle =>
                (EngineH.litert_lm_session_get_benchmark_info(handle, out BenchmarkInfoHandle outBenchmarkInfo), outBenchmarkInfo));
            return new BenchmarkInfo(benchmarkHandle);
        }

        /// <summary>Starts asynchronous decoding and streams chunks to <paramref name="callback"/>.</summary>
        /// <param name="callback">The callback invoked for each chunk.</param>
        public void RunDecodeAsync(StreamCallback callback)
        {
            if (callback is null)
                throw new ArgumentNullException(nameof(callback));

            IntPtr key = StreamCallbackHandler.Register(callback);
            try
            {
                LockedExecute(handle => EngineH.litert_lm_session_run_decode_async(
                    handle, StreamCallbackHandler.GetGlobalStreamCallbackListenerPtr(), key));
            }
            catch
            {
                StreamCallbackHandler.Deregister(key);
                throw;
            }
        }

        /// <summary>Prefills and streams content generation chunks to <paramref name="callback"/>.</summary>
        /// <param name="callback">The callback invoked for each chunk.</param>
        /// <param name="inputs">The multimodal input chunks.</param>
        public void GenerateContentStream(StreamCallback callback, params InputData[] inputs) =>
            GenerateContentStream(inputs, callback);

        /// <summary>Prefills and streams content generation chunks to <paramref name="callback"/>.</summary>
        /// <param name="inputs">The multimodal input chunks.</param>
        /// <param name="callback">The callback invoked for each chunk.</param>
        public void GenerateContentStream(IReadOnlyList<InputData> inputs, StreamCallback callback)
        {
            if (callback is null)
                throw new ArgumentNullException(nameof(callback));

            ExecuteWithHandles<InputData, InputDataHandle>(inputs, ptrs =>
            {
                IntPtr key = StreamCallbackHandler.Register(callback);
                try
                {
                    LockedExecute(handle => EngineH.litert_lm_session_generate_content_stream(
                        handle, ptrs, (UIntPtr)ptrs.Length, StreamCallbackHandler.GetGlobalStreamCallbackListenerPtr(), key));
                }
                catch
                {
                    StreamCallbackHandler.Deregister(key);
                    throw;
                }
            });
        }
    }

    /// <summary>Response candidates and scores produced by a <see cref="Session"/>.</summary>
    public sealed class Responses : LiteRTLMWrapper<ResponsesHandle>
    {
        /// <summary>A single response candidate.</summary>
        public readonly ref struct Candidate
        {
            /// <summary>The response text.</summary>
            public string? Text { get; init; }

            /// <summary>Does the response have a valid <see cref="Score"/>?</summary>
            public bool HasScore { get; init; }

            /// <summary>The response score.</summary>
            public float Score { get; init; }

            /// <summary>Does the response have a valid <see cref="TokenLength"/>?</summary>
            public bool HasTokenLength { get; init; }

            /// <summary>The token length of this response.</summary>
            public int TokenLength { get; init; }

            /// <summary>Does the response have a valid <see cref="TokenScores"/>?</summary>
            public bool HasTokenScores { get; init; }

            /// <summary>The token scores for this response, valid for the lifetime of the owning <see cref="Responses"/>.</summary>
            public ReadOnlySpan<float> TokenScores { get; init; }
        }

        internal Responses(ResponsesHandle handle)
        {
            SetHandle(handle);
        }

        /// <summary>Returns the number of response candidates.</summary>
        public int Count => GetNumCandidates();

        /// <summary>Returns the response candidate at the given index.</summary>
        /// <param name="index">The candidate index.</param>
        public unsafe Candidate At(int index)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index));

            UIntPtr idx = (UIntPtr)index;
            string? text = null;
            bool hasScore = false;
            float score = 0;
            bool hasTokenLength = false;
            int tokenLength = 0;
            bool hasTokenScores = false;
            IntPtr scoresPtr = IntPtr.Zero;
            int numScores = 0;

            LockedExecute(handle =>
            {
                StatusCode status = EngineH.litert_lm_responses_get_response_text_at(handle, idx, out IntPtr textPtr);
                if (status is not (StatusCode.Ok or StatusCode.NotFound))
                    return status;

                text = status is StatusCode.Ok && textPtr != IntPtr.Zero
                    ? Marshal.PtrToStringUTF8(textPtr) : null;

                status = EngineH.litert_lm_responses_has_score_at(handle, idx, out hasScore);
                if (status is not StatusCode.Ok)
                    return status;

                if (hasScore)
                {
                    status = EngineH.litert_lm_responses_get_score_at(handle, idx, out score);
                    if (status is not StatusCode.Ok)
                        return status;
                }

                status = EngineH.litert_lm_responses_has_token_length_at(handle, idx, out hasTokenLength);
                if (status is not StatusCode.Ok)
                    return status;

                if (hasTokenLength)
                {
                    status = EngineH.litert_lm_responses_get_token_length_at(handle, idx, out UIntPtr tokenLengthPtr);
                    if (status is not StatusCode.Ok)
                        return status;

                    tokenLength = (int)tokenLengthPtr;
                }

                status = EngineH.litert_lm_responses_has_token_scores_at(handle, idx, out hasTokenScores);
                if (status is not StatusCode.Ok)
                    return status;

                if (!hasTokenScores)
                    return StatusCode.Ok;
                
                status = EngineH.litert_lm_responses_get_num_token_scores_at(handle, idx, out UIntPtr numScoresPtr);
                if (status is not StatusCode.Ok)
                    return status;

                status = EngineH.litert_lm_responses_get_token_scores_at(handle, idx, out scoresPtr);
                if (status is not StatusCode.Ok)
                    return status;

                numScores = (int)numScoresPtr;
                return StatusCode.Ok;
            });

            return new Candidate
            {
                Text = text,
                HasScore = hasScore,
                Score = score,
                HasTokenLength = hasTokenLength,
                TokenLength = tokenLength,
                HasTokenScores = hasTokenScores,
                TokenScores = numScores > 0 && scoresPtr != IntPtr.Zero
                    ? new ReadOnlySpan<float>((void*)scoresPtr, numScores)
                    : ReadOnlySpan<float>.Empty,
            };
        }

        private int GetNumCandidates() =>
            LockedExecute(handle => (EngineH.litert_lm_responses_get_num_candidates(handle, out UIntPtr count), (int)count));
    }

    /// <summary>Benchmark metrics collected during a LiteRT LM <see cref="Session"/>.</summary>
    public sealed class BenchmarkInfo : LiteRTLMWrapper<BenchmarkInfoHandle>
    {
        /// <summary>Metrics for a single prefill or decode turn.</summary>
        public readonly struct Turn
        {
            /// <summary>The prefill/decode token count for this turn.</summary>
            public int TokenCount { get; init; }

            /// <summary>The prefill/decode tokens per second for this turn.</summary>
            public double TokensPerSecond { get; init; }

            public Turn(int tokenCount, double tokensPerSecond)
            {
                TokenCount = tokenCount;
                TokensPerSecond = tokensPerSecond;
            }
        }

        internal BenchmarkInfo(BenchmarkInfoHandle handle)
        {
            SetHandle(handle);
        }

        /// <summary>Returns the time to the first token in seconds.</summary>
        public double GetTimeToFirstToken() =>
            LockedExecute(handle => (EngineH.litert_lm_benchmark_info_get_time_to_first_token(handle, out double seconds), seconds));

        /// <summary>Returns the total initialization time in seconds.</summary>
        public double GetTotalInitTime() =>
            LockedExecute(handle => (EngineH.litert_lm_benchmark_info_get_total_init_time_in_second(handle, out double seconds), seconds));

        /// <summary>Returns all prefill turns.</summary>
        public Turn[] GetPrefillTurns() =>
            LockedExecute(handle =>
            {
                StatusCode status = EngineH.litert_lm_benchmark_info_get_num_prefill_turns(handle, out UIntPtr numTurnsPtr);
                if (status is not StatusCode.Ok)
                    return (status, null!);

                int count = (int)numTurnsPtr;
                Turn[] turns = new Turn[count];
                for (int i = 0; i < count; i++)
                {
                    UIntPtr idx = (UIntPtr)i;
                    status = EngineH.litert_lm_benchmark_info_get_prefill_token_count_at(handle, idx, out UIntPtr tokenCount);
                    if (status is not StatusCode.Ok)
                        return (status, null!);

                    status = EngineH.litert_lm_benchmark_info_get_prefill_tokens_per_sec_at(handle, idx, out double tokensPerSec);
                    if (status is not StatusCode.Ok)
                        return (status, null!);

                    turns[i] = new Turn((int)tokenCount, tokensPerSec);
                }

                return (StatusCode.Ok, turns);
            });

        /// <summary>Returns all decode turns.</summary>
        public Turn[] GetDecodeTurns() =>
            LockedExecute(handle =>
            {
                StatusCode status = EngineH.litert_lm_benchmark_info_get_num_decode_turns(handle, out UIntPtr numTurnsPtr);
                if (status is not StatusCode.Ok)
                    return (status, null!);

                int count = (int)numTurnsPtr;
                Turn[] turns = new Turn[count];
                for (int i = 0; i < count; i++)
                {
                    UIntPtr idx = (UIntPtr)i;
                    status = EngineH.litert_lm_benchmark_info_get_decode_token_count_at(handle, idx, out UIntPtr tokenCount);
                    if (status is not StatusCode.Ok)
                        return (status, null!);

                    status = EngineH.litert_lm_benchmark_info_get_decode_tokens_per_sec_at(handle, idx, out double tokensPerSec);
                    if (status is not StatusCode.Ok)
                        return (status, null!);

                    turns[i] = new Turn((int)tokenCount, tokensPerSec);
                }

                return (StatusCode.Ok, turns);
            });

        /// <summary>Returns the maximum number of tokens (KV cache size) used by the engine, or 0 if not reported.</summary>
        public int GetMaxNumTokens() =>
            LockedExecute(handle => (EngineH.litert_lm_benchmark_info_get_max_num_tokens(handle, out int maxNumTokens), maxNumTokens));
    }

    /// <summary>A single chunk emitted to a <see cref="StreamCallback"/>; valid only for the duration of the callback.</summary>
    public sealed class StreamChunk : LiteRTLMWrapper<StreamChunkHandle>
    {
        internal StreamChunk(StreamChunkHandle handle)
        {
            SetHandle(handle);
            GC.SuppressFinalize(this);
        }

        /// <summary>Gets the text content of the chunk, or <see langword="null"/> if none.</summary>
        public string? GetText() =>
            LockedExecute(handle => (EngineH.litert_lm_stream_chunk_get_text(handle, out IntPtr textPtr),
                textPtr != IntPtr.Zero ? Marshal.PtrToStringUTF8(textPtr) : null));

        /// <summary>Returns whether this is the final chunk of the stream.</summary>
        public bool IsFinal() =>
            LockedExecute(handle => (EngineH.litert_lm_stream_chunk_is_final(handle, out bool isFinal), isFinal));

        /// <summary>Gets the error message associated with this chunk, or <see langword="null"/> if none.</summary>
        public string? GetError() =>
            LockedExecute(handle => (EngineH.litert_lm_stream_chunk_get_error(handle, out IntPtr textPtr),
                textPtr != IntPtr.Zero ? Marshal.PtrToStringUTF8(textPtr) : null));
    }

    /// <summary>Result of tokenizing a string with <see cref="Engine.Tokenize"/>.</summary>
    public sealed class TokenizeResult : LiteRTLMWrapper<TokenizeResultHandle>
    {
        internal TokenizeResult(TokenizeResultHandle handle)
        {
            SetHandle(handle);
        }

        /// <summary>
        /// Zero-copy view of the token IDs, valid only for the lifetime of this
        /// <see cref="TokenizeResult"/> object.
        /// </summary>
        public unsafe ReadOnlySpan<int> Span
        {
            get
            {
                IntPtr tokensPtr = IntPtr.Zero;
                int count = 0;

                LockedExecute(handle =>
                {
                    StatusCode status = EngineH.litert_lm_tokenize_result_get_num_tokens(handle, out UIntPtr countPtr);
                    if (status is not StatusCode.Ok)
                        return status;

                    count = (int)countPtr;
                    return count > 0 ? EngineH.litert_lm_tokenize_result_get_tokens(handle, out tokensPtr) : StatusCode.Ok;
                });

                return count > 0 && tokensPtr != IntPtr.Zero
                    ? new ReadOnlySpan<int>((void*)tokensPtr, count)
                    : ReadOnlySpan<int>.Empty;
            }
        }
    }

    /// <summary>Result of detokenizing token IDs with <see cref="Engine.Detokenize"/>.</summary>
    public sealed class DetokenizeResult : LiteRTLMWrapper<DetokenizeResultHandle>
    {
        internal DetokenizeResult(DetokenizeResultHandle handle)
        {
            SetHandle(handle);
        }

        /// <summary>Returns the detokenized UTF-8 string.</summary>
        public string GetString() =>
            LockedExecute(handle => (EngineH.litert_lm_detokenize_result_get_string(handle, out IntPtr textPtr),
                textPtr != IntPtr.Zero ? Marshal.PtrToStringUTF8(textPtr) : string.Empty));
    }

    /// <summary>Represents either a token string or a sequence of token IDs.</summary>
    public sealed class TokenUnion : LiteRTLMWrapper<TokenUnionHandle>
    {
        internal TokenUnion(TokenUnionHandle handle)
        {
            SetHandle(handle);
        }

        /// <summary>Returns the representation type stored in this token union.</summary>
        public TokenUnionType GetTokenType() =>
            LockedExecute(handle =>
                (EngineH.litert_lm_token_union_get_type(handle, out TokenUnionType type), type));

        /// <summary>Returns the string value when <see cref="GetTokenType"/> is <see cref="TokenUnionType.String"/>.</summary>
        public string GetString() =>
            LockedExecute(handle => (EngineH.litert_lm_token_union_get_string(handle, out IntPtr textPtr),
                textPtr != IntPtr.Zero ? Marshal.PtrToStringUTF8(textPtr) : string.Empty));

        /// <summary>
        /// Returns a zero-copy span of token IDs when <see cref="GetTokenType"/> is <see cref="TokenUnionType.Ids"/>,
        /// valid only for the lifetime of this <see cref="TokenUnion"/> object.
        /// </summary>
        public unsafe ReadOnlySpan<int> GetIds()
        {
            IntPtr tokensPtr = IntPtr.Zero;
            int count = LockedExecute(handle =>
                (EngineH.litert_lm_token_union_get_ids(handle, out tokensPtr, out UIntPtr numTokensPtr), (int)numTokensPtr));

            return count > 0 && tokensPtr != IntPtr.Zero
                ? new ReadOnlySpan<int>((void*)tokensPtr, count)
                : ReadOnlySpan<int>.Empty;
        }
    }

    /// <summary>Collection of <see cref="TokenUnion"/> instances (for example, stop tokens).</summary>
    public sealed class TokenUnions : LiteRTLMWrapper<TokenUnionsHandle>
    {
        internal TokenUnions(TokenUnionsHandle handle)
        {
            SetHandle(handle);
        }

        /// <summary>Returns the number of token unions in this collection.</summary>
        public int Count => LockedExecute(handle =>
            (EngineH.litert_lm_token_unions_get_num_tokens(handle, out UIntPtr count), (int)count));

        /// <summary>Returns a borrowed <see cref="TokenUnion"/> view at the given index, must be disposed after use.</summary>
        /// <param name="index">The element index.</param>
        public TokenUnion At(int index)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index));
            
            BorrowedTokenUnionHandle tokenHandle = LockedExecute(handle =>
            {
                StatusCode result = EngineH.litert_lm_token_unions_get_token_at(
                    handle, (UIntPtr)index, out BorrowedTokenUnionHandle outToken);
                
                if (result is StatusCode.Ok)
                    DisposeOnCatch(outToken, tokenUnion => tokenUnion.DangerousAttachOwner(handle));
                return (result, outToken);
            });
            
            return new TokenUnion(tokenHandle);
        }
    }
}