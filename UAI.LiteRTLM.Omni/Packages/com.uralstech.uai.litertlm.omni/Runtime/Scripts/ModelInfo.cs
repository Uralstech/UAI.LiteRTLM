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
    /// <summary>Inspects capabilities and metadata of a <c>.litertlm</c> model file without initializing an engine.</summary>
    public sealed class ModelInfo : LiteRTLMWrapper<LoadedFileHandle>
    {
        /// <summary>Loads a LiteRT-LM file from the given path for capability queries.</summary>
        /// <param name="litertlmPath">The path to the <c>.litertlm</c> file.</param>
        public ModelInfo(string litertlmPath)
        {
            LiteRTLMException.Check(
                ModelInfoH.litert_lm_loaded_file_create(litertlmPath, out LoadedFileHandle handle));
            SetHandle(handle);
        }

        /// <summary>Gets whether the model supports speculative decoding.</summary>
        public bool HasSpeculativeDecodingSupport() =>
            LockedExecute(handle => (ModelInfoH.litert_lm_loaded_file_has_speculative_decoding_support(handle, out bool supported), supported));

        /// <summary>Gets whether the model supports thinking / reasoning steps.</summary>
        public bool SupportsThinking() =>
            LockedExecute(handle => (ModelInfoH.litert_lm_loaded_file_supports_thinking(handle, out bool supported), supported));

        /// <summary>Gets whether the model supports function calling / tool use.</summary>
        public bool SupportsFunctionCalling() =>
            LockedExecute(handle => (ModelInfoH.litert_lm_loaded_file_supports_function_calling(handle, out bool supported), supported));

        /// <summary>Gets the default sampler type for the model, or <see langword="null"/> if not an LLM.</summary>
        public SamplerType? GetSamplerType() =>
            LockedExecute(handle =>
            {
                StatusCode status = ModelInfoH.litert_lm_loaded_file_sampler_type(handle, out SamplerType samplerType);
                return status is StatusCode.NotFound
                    ? (StatusCode.Ok, (SamplerType?)null)
                    : (status, samplerType);
            });

        /// <summary>Gets the default sampler temperature for the model, or <see langword="null"/> if not an LLM.</summary>
        public float? GetSamplerTemperature() =>
            LockedExecute(handle =>
            {
                StatusCode status = ModelInfoH.litert_lm_loaded_file_sampler_temperature(handle, out float temperature);
                return status is StatusCode.NotFound
                    ? (StatusCode.Ok, (float?)null)
                    : (status, temperature);
            });

        /// <summary>Gets the default sampler <c>top_k</c> for the model, or <see langword="null"/> if not an LLM.</summary>
        public int? GetSamplerTopK() =>
            LockedExecute(handle =>
            {
                StatusCode status = ModelInfoH.litert_lm_loaded_file_sampler_top_k(handle, out int topK);
                return status is StatusCode.NotFound
                    ? (StatusCode.Ok, (int?)null)
                    : (status, topK);
            });

        /// <summary>Gets the default sampler <c>top_p</c> for the model, or <see langword="null"/> if not an LLM.</summary>
        public float? GetSamplerTopP() =>
            LockedExecute(handle =>
            {
                StatusCode status = ModelInfoH.litert_lm_loaded_file_sampler_top_p(handle, out float topP);
                return status is StatusCode.NotFound
                    ? (StatusCode.Ok, (float?)null)
                    : (status, topP);
            });

        /// <summary>Gets whether the specified input modality is supported by the model.</summary>
        /// <param name="modality">The input modality to check.</param>
        public bool SupportsInputModality(Modality modality) =>
            LockedExecute(handle => (ModelInfoH.litert_lm_loaded_file_supports_input_modality(handle, modality, out bool supported), supported));

        /// <summary>Gets the maximum vision token budget for the model, or <see langword="null"/> if not defined or unsupported.</summary>
        public int? GetMaxVisionTokenBudget() =>
            LockedExecute(handle =>
            {
                StatusCode status = ModelInfoH.litert_lm_loaded_file_max_vision_token_budget(handle, out int budget);
                return status is StatusCode.NotFound
                    ? (StatusCode.Ok, (int?)null)
                    : (status, budget);
            });

        /// <summary>Gets the context size, in tokens, for the loaded LiteRT-LM file, or <see langword="null"/> if not defined.</summary>
        public uint? GetMaxContextTokens() =>
            LockedExecute(handle =>
            {
                StatusCode status = ModelInfoH.litert_lm_loaded_file_max_context_tokens(handle, out uint maxTokens);
                return status is StatusCode.NotFound
                    ? (StatusCode.Ok, (uint?)null)
                    : (status, maxTokens);
            });

        /// <summary>Gets whether the model has a dynamic context size configurable by the caller.</summary>
        public bool IsDynamicContext() =>
            LockedExecute(handle => (ModelInfoH.litert_lm_loaded_file_is_dynamic_context(handle, out bool isDynamic), isDynamic));

        /// <summary>Gets the supported discrete vision token lengths, or <see langword="null"/> if not defined or unsupported.</summary>
        public int[]? GetVisionSignatureSelection() =>
            LockedExecute(handle =>
            {
                StatusCode result = ModelInfoH.litert_lm_loaded_file_vision_signature_selection(
                    handle, null, UIntPtr.Zero, out UIntPtr count);
                if (result is not StatusCode.Ok)
                    return (result is StatusCode.NotFound ? StatusCode.Ok : result, null);

                int[] lengths = new int[(int)count];
                return (ModelInfoH.litert_lm_loaded_file_vision_signature_selection(
                    handle, lengths, count, out _), lengths);
            });
    
        /// <summary>Gets the supported backends for a given modality, ordered by priority.</summary>
        /// <param name="modality">The modality to query.</param>
        public BackendType[] GetModalitySupportedBackends(Modality modality) =>
            LockedExecute(handle =>
            {
                StatusCode result = ModelInfoH.litert_lm_loaded_file_modality_supported_backends(
                    handle, modality, null, UIntPtr.Zero, out UIntPtr count);
                if (result is not StatusCode.Ok)
                    return (result, null!);

                BackendType[] backendTypes = new BackendType[(int)count];
                return (ModelInfoH.litert_lm_loaded_file_modality_supported_backends(
                    handle, modality, backendTypes, count, out _), backendTypes);
            });

        /// <summary>Gets the detected NPU brand of the model for a given modality.</summary>
        /// <param name="modality">The modality to query.</param>
        public NpuBrand GetModalityNpuBrand(Modality modality) =>
            LockedExecute(handle => (ModelInfoH.litert_lm_loaded_file_modality_npu_brand(handle, modality, out NpuBrand npuBrand), npuBrand));

        /// <summary>Gets the target SoC name for a given modality, or <see langword="null"/> if not specified.</summary>
        /// <param name="modality">The modality to query.</param>
        public string? GetModalitySocName(Modality modality) =>
            LockedExecute(handle => (ModelInfoH.litert_lm_loaded_file_modality_soc_name(handle, modality, out IntPtr socPtr),
                socPtr != IntPtr.Zero ? Marshal.PtrToStringUTF8(socPtr) : null));

        /// <summary>Gets the minimum LiteRT-LM runtime version required to run this model, or <see langword="null"/> if not defined.</summary>
        public string? GetMinRuntimeVersion() =>
            LockedExecute(handle => (ModelInfoH.litert_lm_loaded_file_min_runtime_version(handle, out IntPtr versionPtr),
                versionPtr != IntPtr.Zero ? Marshal.PtrToStringUTF8(versionPtr) : null));

        /// <summary>Gets the model type of the loaded LiteRT-LM file.</summary>
        public ModelType GetModelType() =>
            LockedExecute(handle => (ModelInfoH.litert_lm_loaded_file_model_type(handle, out ModelType modelType), modelType));

        /// <summary>Gets the output embedding dimension for an embedding model, or <see langword="null"/> if not an embedding model or undefined.</summary>
        public int? GetEmbeddingDimension() =>
            LockedExecute(handle =>
            {
                StatusCode status = ModelInfoH.litert_lm_loaded_file_embedding_dimension(handle, out int dimension);
                return status is StatusCode.NotFound
                    ? (StatusCode.Ok, (int?)null)
                    : (status, dimension);
            });

        /// <summary>Gets the supported embedding signature sequence lengths, or <see langword="null"/> if not an embedding model or undefined.</summary>
        public int[]? GetEmbeddingSignatureSelection() =>
            LockedExecute(handle =>
            {
                StatusCode result = ModelInfoH.litert_lm_loaded_file_embedding_signature_selection(
                    handle, null, UIntPtr.Zero, out UIntPtr count);
                if (result is not StatusCode.Ok)
                    return (result is StatusCode.NotFound ? StatusCode.Ok : result, null);

                int[] lengths = new int[(int)count];
                return (ModelInfoH.litert_lm_loaded_file_embedding_signature_selection(
                    handle, lengths, count, out _), lengths);
            });
    }
}