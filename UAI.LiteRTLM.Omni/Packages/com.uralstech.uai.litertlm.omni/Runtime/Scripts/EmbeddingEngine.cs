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
using Uralstech.UAI.LiteRTLM.Omni.Native;
using Uralstech.UAI.LiteRTLM.Omni.Unsafe;

#nullable enable
namespace Uralstech.UAI.LiteRTLM.Omni
{
    /// <summary>Settings used to initialize a LiteRT LM <see cref="EmbeddingEngine"/>.</summary>
    public sealed class EmbeddingEngineSettings : LiteRTLMWrapper<EmbeddingEngineSettingsHandle>
    {
        /// <summary>Creates LiteRT LM Embedding Engine Settings from a model file path.</summary>
        /// <param name="modelPath">The path to the model file.</param>
        /// <param name="backend">The main backend to use (e.g., "cpu", "gpu", "npu").</param>
        /// <param name="visionBackend">The vision backend to use, or <see langword="null"/> if not set.</param>
        /// <param name="audioBackend">The audio backend to use, or <see langword="null"/> if not set.</param>
        public EmbeddingEngineSettings(string modelPath, string backend, string? visionBackend = null, string? audioBackend = null)
        {
            LiteRTLMException.Check(EmbeddingEngineH.litert_lm_embedding_engine_settings_create(
                modelPath, backend, visionBackend, audioBackend, out EmbeddingEngineSettingsHandle handle));
            SetHandle(handle);
        }

        /// <summary>Sets the number of threads for the CPU backend.</summary>
        /// <param name="numThreads">The number of threads.</param>
        public void SetNumThreads(int numThreads) =>
            LockedExecute(handle => EmbeddingEngineH.litert_lm_embedding_engine_settings_set_num_threads(handle, numThreads));

        /// <summary>Sets the number of threads for the audio CPU backend.</summary>
        /// <param name="numThreads">The number of threads.</param>
        public void SetAudioNumThreads(int numThreads) =>
            LockedExecute(handle => EmbeddingEngineH.litert_lm_embedding_engine_settings_set_audio_num_threads(handle, numThreads));

        /// <summary>Sets the cache directory for the Embedding Engine.</summary>
        /// <param name="cacheDir">The cache directory.</param>
        public void SetCacheDir(string cacheDir) =>
            LockedExecute(handle => EmbeddingEngineH.litert_lm_embedding_engine_settings_set_cache_dir(handle, cacheDir));

        /// <summary>Sets the LiteRT dispatch library directory for the main NPU backend.</summary>
        /// <param name="libDir">The dispatch library directory.</param>
        public void SetLiteRTDispatchLibDir(string libDir) =>
            LockedExecute(handle => EmbeddingEngineH.litert_lm_embedding_engine_settings_set_litert_dispatch_lib_dir(handle, libDir));

        /// <summary>Sets the LiteRT dispatch library directory for the vision NPU backend.</summary>
        /// <param name="libDir">The dispatch library directory.</param>
        public void SetVisionLiteRTDispatchLibDir(string libDir) =>
            LockedExecute(handle => EmbeddingEngineH.litert_lm_embedding_engine_settings_set_vision_litert_dispatch_lib_dir(handle, libDir));

        /// <summary>Sets the LiteRT dispatch library directory for the audio NPU backend.</summary>
        /// <param name="libDir">The dispatch library directory.</param>
        public void SetAudioLiteRTDispatchLibDir(string libDir) =>
            LockedExecute(handle => EmbeddingEngineH.litert_lm_embedding_engine_settings_set_audio_litert_dispatch_lib_dir(handle, libDir));

        /// <summary>Sets the maximum sequence length (in tokens) for text encoder signatures (non-positive unsets).</summary>
        /// <param name="maxInputLength">The maximum input length.</param>
        public void SetMaxInputLength(int maxInputLength) =>
            LockedExecute(handle => EmbeddingEngineH.litert_lm_embedding_engine_settings_set_max_input_length(handle, maxInputLength));

        /// <summary>Sets the minimum sequence length (in tokens) for text encoder signatures (negative unsets).</summary>
        /// <param name="minInputLength">The minimum input length.</param>
        public void SetMinInputLength(int minInputLength) =>
            LockedExecute(handle => EmbeddingEngineH.litert_lm_embedding_engine_settings_set_min_input_length(handle, minInputLength));

        /// <summary>Sets the desired number of vision tokens generated per image (non-positive unsets).</summary>
        /// <param name="visionTokensPerImage">The vision tokens per image.</param>
        public void SetVisionTokensPerImage(int visionTokensPerImage) =>
            LockedExecute(handle => EmbeddingEngineH.litert_lm_embedding_engine_settings_set_vision_tokens_per_image(handle, visionTokensPerImage));

        /// <summary>Sets the activation data type.</summary>
        /// <param name="activationDataType">The activation data type.</param>
        public void SetActivationDataType(ActivationDataType activationDataType) =>
            LockedExecute(handle => EmbeddingEngineH.litert_lm_embedding_engine_settings_set_activation_data_type(handle, activationDataType));
    }

    /// <summary>Per-request options for <see cref="EmbeddingEngine"/> inference.</summary>
    public sealed class EmbeddingOptions : LiteRTLMWrapper<EmbeddingOptionsHandle>
    {
        /// <summary>Creates LiteRT LM Embedding Options with default values.</summary>
        public EmbeddingOptions()
        {
            LiteRTLMException.Check(
                EmbeddingEngineH.litert_lm_embedding_options_create(out EmbeddingOptionsHandle handle));
            SetHandle(handle);
        }

        /// <summary>Whether the embedding should be L2 normalized.</summary>
        public bool Normalize
        {
            get => LockedExecute(handle =>
                (EmbeddingEngineH.litert_lm_embedding_options_get_normalize(handle, out bool normalize), normalize));
            set => LockedExecute(handle =>
                EmbeddingEngineH.litert_lm_embedding_options_set_normalize(handle, value));
        }

        /// <summary>Whether special tokens (BOS, EOS, start/end of image/audio) should be automatically inserted.</summary>
        public bool InsertSpecialTokens
        {
            get => LockedExecute(handle =>
                (EmbeddingEngineH.litert_lm_embedding_options_get_insert_special_tokens(handle, out bool insert), insert));
            set => LockedExecute(handle =>
                EmbeddingEngineH.litert_lm_embedding_options_set_insert_special_tokens(handle, value));
        }

        /// <summary>The input overflow strategy.</summary>
        public InputOverflowStrategy InputOverflowStrategy
        {
            get => LockedExecute(handle =>
                (EmbeddingEngineH.litert_lm_embedding_options_get_input_overflow_strategy(handle, out InputOverflowStrategy strategy), strategy));
            set => LockedExecute(handle =>
                EmbeddingEngineH.litert_lm_embedding_options_set_input_overflow_strategy(handle, value));
        }

        /// <summary>
        /// The output embedding size to truncate the embedding to, or <see langword="null"/> to use the model default.
        /// Setting <see langword="null"/> or a non-positive value unsets this option.
        /// </summary>
        public int? OutputSize
        {
            get => LockedExecute(handle =>
            {
                StatusCode status = EmbeddingEngineH.litert_lm_embedding_options_get_output_size(handle, out int outputSize);
                return status is StatusCode.NotFound
                    ? (StatusCode.Ok, (int?)null)
                    : (status, outputSize);
            });
            set => LockedExecute(handle =>
                EmbeddingEngineH.litert_lm_embedding_options_set_output_size(handle, value ?? 0));
        }

        /// <summary>
        /// The desired number of vision tokens per image, or <see langword="null"/> to use the engine/model default.
        /// Setting <see langword="null"/> or a non-positive value unsets this option.
        /// </summary>
        public int? VisionTokensPerImage
        {
            get => LockedExecute(handle =>
            {
                StatusCode status = EmbeddingEngineH.litert_lm_embedding_options_get_vision_tokens_per_image(handle, out int visionTokens);
                return status is StatusCode.NotFound
                    ? (StatusCode.Ok, (int?)null)
                    : (status, visionTokens);
            });
            set => LockedExecute(handle =>
                EmbeddingEngineH.litert_lm_embedding_options_set_vision_tokens_per_image(handle, value ?? 0));
        }
    }

    /// <summary>Single embedding vector returned by <see cref="EmbeddingEngine"/>.</summary>
    public sealed class EmbeddingResponse : LiteRTLMWrapper<EmbeddingResponseHandle>
    {
        internal EmbeddingResponse(EmbeddingResponseHandle handle)
        {
            SetHandle(handle);
        }

        /// <summary>
        /// Zero-copy view of the float embedding values, valid only for the lifetime of this
        /// <see cref="EmbeddingResponse"/> object (or the <see cref="EmbeddingResponses"/> that owns it).
        /// </summary>
        public unsafe ReadOnlySpan<float> Span
        {
            get
            {
                IntPtr valuesPtr = IntPtr.Zero;
                int count = 0;

                LockedExecute(handle =>
                {
                    StatusCode status = EmbeddingEngineH.litert_lm_embedding_response_get_size(handle, out UIntPtr sizePtr);
                    if (status is not StatusCode.Ok)
                        return status;

                    count = (int)sizePtr;
                    return count > 0
                        ? EmbeddingEngineH.litert_lm_embedding_response_get_values(handle, out valuesPtr)
                        : StatusCode.Ok;
                });

                return count > 0 && valuesPtr != IntPtr.Zero
                    ? new ReadOnlySpan<float>((void*)valuesPtr, count)
                    : ReadOnlySpan<float>.Empty;
            }
        }
    }

    /// <summary>Batch collection of <see cref="EmbeddingResponse"/> instances.</summary>
    public sealed class EmbeddingResponses : LiteRTLMWrapper<EmbeddingResponsesHandle>
    {
        internal EmbeddingResponses(EmbeddingResponsesHandle handle)
        {
            SetHandle(handle);
        }

        /// <summary>Returns the number of embedding responses in this batch.</summary>
        public int Count => LockedExecute(handle =>
            (EmbeddingEngineH.litert_lm_embedding_responses_get_size(handle, out UIntPtr count), (int)count));

        /// <summary>Returns a borrowed <see cref="EmbeddingResponse"/> view at the given batch index, must be disposed after use.</summary>
        /// <param name="index">The batch index.</param>
        public EmbeddingResponse At(int index)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index));

            BorrowedEmbeddingResponseHandle responseHandle = LockedExecute(handle =>
            {
                StatusCode result = EmbeddingEngineH.litert_lm_embedding_responses_get_at(
                    handle, (UIntPtr)index, out BorrowedEmbeddingResponseHandle outResponse);

                if (result is StatusCode.Ok)
                    DisposeOnCatch(outResponse, response => response.DangerousAttachOwner(handle));
                return (result, outResponse);
            });

            return new EmbeddingResponse(responseHandle);
        }
    }

    /// <summary>LiteRT LM Embedding Engine for computing multimodal embeddings.</summary>
    public sealed class EmbeddingEngine : LiteRTLMWrapper<EmbeddingEngineHandle>
    {
        /// <summary>Creates a LiteRT LM Embedding Engine from the given settings.</summary>
        /// <param name="settings">The embedding engine settings.</param>
        public EmbeddingEngine(EmbeddingEngineSettings settings)
        {
            if (settings is null)
                throw new ArgumentNullException(nameof(settings));

            LiteRTLMException.Check(EmbeddingEngineH.litert_lm_embedding_engine_create(
                settings.DangerousGetHandle(), out EmbeddingEngineHandle handle));
            GC.KeepAlive(settings);
            SetHandle(handle);
        }

        /// <summary>Computes an embedding response for a single multimodal request.</summary>
        /// <param name="inputs">The multimodal input chunks.</param>
        /// <returns>The computed embedding response.</returns>
        public EmbeddingResponse ComputeEmbedding(params InputData[] inputs) =>
            ComputeEmbedding((IReadOnlyList<InputData>)inputs);

        /// <summary>Computes an embedding response for a single multimodal request.</summary>
        /// <param name="options">Optional embedding options, or <see langword="null"/> for defaults.</param>
        /// <param name="inputs">The multimodal input chunks.</param>
        /// <returns>The computed embedding response.</returns>
        public EmbeddingResponse ComputeEmbedding(EmbeddingOptions? options, params InputData[] inputs) =>
            ComputeEmbedding(inputs, options);

        /// <summary>Computes an embedding response for a single multimodal request.</summary>
        /// <param name="inputs">The multimodal input chunks.</param>
        /// <param name="options">Optional embedding options, or <see langword="null"/> for defaults.</param>
        /// <returns>The computed embedding response.</returns>
        public EmbeddingResponse ComputeEmbedding(IReadOnlyList<InputData> inputs, EmbeddingOptions? options = null)
        {
            EmbeddingOptionsHandle optionsHandle = options?.DangerousGetHandle() ?? EmbeddingOptionsHandle.Null;
            EmbeddingResponseHandle responseHandle = null!;

            ExecuteWithHandles<InputData, InputDataHandle>(inputs, ptrs =>
            {
                responseHandle = LockedExecute(handle => (EmbeddingEngineH.litert_lm_embedding_engine_compute_embedding(
                    handle, ptrs, (UIntPtr)ptrs.Length, optionsHandle, out EmbeddingResponseHandle outResponse), outResponse));
            });

            GC.KeepAlive(options);
            return new EmbeddingResponse(responseHandle);
        }

        /// <summary>Computes embedding responses for a batch of multimodal requests.</summary>
        /// <param name="inputBatches">The batch of multimodal input sequences.</param>
        /// <param name="options">Optional embedding options, or <see langword="null"/> for defaults.</param>
        /// <returns>The batch of computed embedding responses.</returns>
        public EmbeddingResponses ComputeEmbeddingBatch(IReadOnlyList<IReadOnlyList<InputData>> inputBatches,
            EmbeddingOptions? options = null)
        {
            if (inputBatches is null)
                throw new ArgumentNullException(nameof(inputBatches));

            int batches = inputBatches.Count;

            PackageUnsafeUtils.TempMem?[] allocatedMemory = new PackageUnsafeUtils.TempMem?[batches];
            UIntPtr[] arraySizes = new UIntPtr[batches];
            IntPtr[] arrays = new IntPtr[batches];

            int totalInputs = 0;
            for (int i = 0; i < batches; i++)
            {
                IReadOnlyList<InputData> batch = inputBatches[i]
                    ?? throw new ArgumentNullException(nameof(inputBatches), $"Batch at index {i} is null.");
                totalInputs += batch.Count;
            }

            InputDataHandle?[] incrementedHandles = new InputDataHandle?[totalInputs];
            int handleIndex = 0;

            try
            {
                for (int i = 0; i < batches; i++)
                {
                    IReadOnlyList<InputData> batch = inputBatches[i];
                    int batchSize = batch.Count;
                    arraySizes[i] = (UIntPtr)batchSize;

                    if (batchSize == 0)
                        continue;

                    PackageUnsafeUtils.TempMem memory = PackageUnsafeUtils.Allocate(batchSize, out Span<IntPtr> span);
                    allocatedMemory[i] = memory;
                    arrays[i] = memory.Ptr;

                    for (int j = 0; j < batchSize; j++)
                    {
                        InputDataHandle inputHandle = batch[j]?.DangerousGetHandle()
                            ?? throw new ArgumentNullException(nameof(inputBatches), $"Input at batch {i}, index {j} is null.");

                        bool incremented = false;
                        inputHandle.DangerousAddRef(ref incremented);
                        if (!incremented)
                            throw new InvalidOperationException($"Could not increment reference to handle at batch {i}, index {j}.");

                        incrementedHandles[handleIndex++] = inputHandle;
                        span[j] = inputHandle.DangerousGetHandle();
                    }
                }

                EmbeddingResponsesHandle responsesHandle = LockedExecute(handle => (EmbeddingEngineH.litert_lm_embedding_engine_compute_embedding_batch(
                    handle, arrays, arraySizes, (UIntPtr)batches, options?.DangerousGetHandle() ?? EmbeddingOptionsHandle.Null,
                    out EmbeddingResponsesHandle outResponses), outResponses));

                GC.KeepAlive(inputBatches);
                GC.KeepAlive(options);
                return new EmbeddingResponses(responsesHandle);
            }
            finally
            {
                for (int i = 0; i < handleIndex; i++)
                    incrementedHandles[i]?.DangerousRelease();

                foreach (PackageUnsafeUtils.TempMem? memory in allocatedMemory)
                    memory?.Dispose();
            }
        }
    }
}