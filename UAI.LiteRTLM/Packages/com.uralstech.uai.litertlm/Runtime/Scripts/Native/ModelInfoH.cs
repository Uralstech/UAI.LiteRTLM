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
    public static class ModelInfoH
    {
        /// <summary>
        /// Loads a LiteRT-LM file from the given path for capability queries.
        /// </summary>
        /// <param name="litertlmPath">The path to the <c>.litertlm</c> file.</param>
        /// <param name="outLoadedFile">
        /// On success, receives the loaded file, owned by the caller; release with
        /// <see cref="LoadedFileHandle.Dispose"/>. Set to an invalid handle
        /// (<see cref="LiteRTHandle.IsInvalid"/>) on failure.
        /// </param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or another <see cref="StatusCode"/> on failure.
        /// <see cref="StatusCode.InvalidArgument"/> if <paramref name="litertlmPath"/> is
        /// <see langword="null"/>; otherwise the code of the error that prevented the file from
        /// being loaded (e.g. the file could not be opened or parsed).
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_loaded_file_create(
            [MarshalAs(UnmanagedType.LPUTF8Str)] string litertlmPath, out LoadedFileHandle outLoadedFile);

        /// <summary>
        /// Deletes a loaded LiteRT-LM file.
        /// </summary>
        /// <param name="loadedFile">The loaded file to delete.</param>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern void litert_lm_loaded_file_delete(IntPtr loadedFile);

        /// <summary>
        /// Gets whether the loaded LiteRT-LM file supports speculative decoding.
        /// </summary>
        /// <param name="loadedFile">The loaded file to inspect.</param>
        /// <param name="outSupported">
        /// On success, receives <see langword="true"/> if the model supports speculative decoding,
        /// <see langword="false"/> otherwise (including non-LLM models); not written on failure.
        /// </param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or another <see cref="StatusCode"/> on failure.
        /// <see cref="StatusCode.InvalidArgument"/> if <paramref name="loadedFile"/> is invalid.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_loaded_file_has_speculative_decoding_support(
            LoadedFileHandle loadedFile, [MarshalAs(UnmanagedType.I1)] out bool outSupported);

        /// <summary>
        /// Gets whether the model supports thinking / reasoning steps.
        /// </summary>
        /// <param name="loadedFile">The loaded file to inspect.</param>
        /// <param name="outSupported">
        /// On success, receives <see langword="true"/> if the model supports thinking,
        /// <see langword="false"/> otherwise (including when the metadata is not explicitly set
        /// in the model, or the model is not an LLM); not written on failure.
        /// </param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or another <see cref="StatusCode"/> on failure.
        /// <see cref="StatusCode.InvalidArgument"/> if <paramref name="loadedFile"/> is invalid.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_loaded_file_supports_thinking(
            LoadedFileHandle loadedFile, [MarshalAs(UnmanagedType.I1)] out bool outSupported);

        /// <summary>
        /// Gets whether the model supports function calling / tool use.
        /// </summary>
        /// <param name="loadedFile">The loaded file to inspect.</param>
        /// <param name="outSupported">
        /// On success, receives <see langword="true"/> if the model supports function calling,
        /// <see langword="false"/> otherwise (including when the metadata is not explicitly set
        /// in the model, or the model is not an LLM); not written on failure.
        /// </param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or another <see cref="StatusCode"/> on failure.
        /// <see cref="StatusCode.InvalidArgument"/> if <paramref name="loadedFile"/> is invalid.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_loaded_file_supports_function_calling(
            LoadedFileHandle loadedFile, [MarshalAs(UnmanagedType.I1)] out bool outSupported);

        /// <summary>
        /// Gets the default sampler type for the model.
        /// </summary>
        /// <param name="loadedFile">The loaded file to inspect.</param>
        /// <param name="outSamplerType">
        /// On success, receives the default sampler type. This is <see cref="SamplerType.Unspecified"/>
        /// if the LLM metadata does not specify a sampler type. Not written on failure.
        /// </param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or another <see cref="StatusCode"/> on failure.
        /// <see cref="StatusCode.InvalidArgument"/> if <paramref name="loadedFile"/> is invalid;
        /// <see cref="StatusCode.NotFound"/> if the model is not an LLM (it has no default sampler parameters).
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_loaded_file_sampler_type(
            LoadedFileHandle loadedFile, out SamplerType outSamplerType);

        /// <summary>
        /// Gets the default sampler temperature for the model.
        /// </summary>
        /// <param name="loadedFile">The loaded file to inspect.</param>
        /// <param name="outTemperature">
        /// On success, receives the default temperature as stored in the LLM metadata (0 if the metadata
        /// does not specify it); not written on failure.
        /// </param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or another <see cref="StatusCode"/> on failure.
        /// <see cref="StatusCode.InvalidArgument"/> if <paramref name="loadedFile"/> is invalid;
        /// <see cref="StatusCode.NotFound"/> if the model is not an LLM (it has no default sampler parameters).
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_loaded_file_sampler_temperature(
            LoadedFileHandle loadedFile, out float outTemperature);

        /// <summary>
        /// Gets the default sampler <c>top_k</c> for the model.
        /// </summary>
        /// <param name="loadedFile">The loaded file to inspect.</param>
        /// <param name="outTopK">
        /// On success, receives the default <c>top_k</c> as stored in the LLM metadata (0 if the metadata
        /// does not specify it); not written on failure.
        /// </param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or another <see cref="StatusCode"/> on failure.
        /// <see cref="StatusCode.InvalidArgument"/> if <paramref name="loadedFile"/> is invalid;
        /// <see cref="StatusCode.NotFound"/> if the model is not an LLM (it has no default sampler parameters).
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_loaded_file_sampler_top_k(
            LoadedFileHandle loadedFile, out int outTopK);

        /// <summary>
        /// Gets the default sampler <c>top_p</c> for the model.
        /// </summary>
        /// <param name="loadedFile">The loaded file to inspect.</param>
        /// <param name="outTopP">
        /// On success, receives the default <c>top_p</c> as stored in the LLM metadata (0 if the metadata
        /// does not specify it); not written on failure.
        /// </param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or another <see cref="StatusCode"/> on failure.
        /// <see cref="StatusCode.InvalidArgument"/> if <paramref name="loadedFile"/> is invalid;
        /// <see cref="StatusCode.NotFound"/> if the model is not an LLM (it has no default sampler parameters).
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_loaded_file_sampler_top_p(
            LoadedFileHandle loadedFile, out float outTopP);

        /// <summary>
        /// Gets whether the input modality is supported.
        /// </summary>
        /// <param name="loadedFile">The loaded file to inspect.</param>
        /// <param name="modality">The input modality to check.</param>
        /// <param name="outSupported">
        /// On success, receives <see langword="true"/> if <paramref name="modality"/> is a supported
        /// input modality, <see langword="false"/> otherwise; not written on failure.
        /// </param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or another <see cref="StatusCode"/> on failure.
        /// <see cref="StatusCode.InvalidArgument"/> if <paramref name="loadedFile"/> is invalid
        /// or <paramref name="modality"/> is not a valid <see cref="Modality"/>.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_loaded_file_supports_input_modality(
            LoadedFileHandle loadedFile, Modality modality,
            [MarshalAs(UnmanagedType.I1)] out bool outSupported);

        /// <summary>
        /// Gets the maximum vision token budget for the model.
        /// </summary>
        /// <param name="loadedFile">The loaded file to inspect.</param>
        /// <param name="outBudget">
        /// On success, receives the maximum vision token budget; not written on failure.
        /// </param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or another <see cref="StatusCode"/> on failure.
        /// <see cref="StatusCode.InvalidArgument"/> if <paramref name="loadedFile"/> is invalid;
        /// <see cref="StatusCode.NotFound"/> if the model does not support vision or does not define the budget.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_loaded_file_max_vision_token_budget(
            LoadedFileHandle loadedFile, out int outBudget);

        /// <summary>
        /// Gets the context size, in tokens, for the loaded LiteRT-LM file.
        /// <list type="bullet">
        /// <item><description>
        /// For LLMs, this is the default context size for initializing the model
        /// (<c>LlmMetadata</c> <c>max_num_tokens</c>). If the metadata does not set it, this is the
        /// engine's default (4096), capped by the context size of a static model graph.
        /// </description></item>
        /// <item><description>
        /// For embedding models, this is the fixed input length if the model is static
        /// (<see cref="litert_lm_loaded_file_is_dynamic_context"/> reports <see langword="false"/>),
        /// or the largest supported input length if it is dynamic.
        /// </description></item>
        /// </list>
        /// </summary>
        /// <param name="loadedFile">The loaded file to inspect.</param>
        /// <param name="outMaxContextTokens">
        /// On success, receives the number of context tokens (always positive); not written on failure.
        /// </param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or another <see cref="StatusCode"/> on failure.
        /// <see cref="StatusCode.InvalidArgument"/> if <paramref name="loadedFile"/> is invalid;
        /// <see cref="StatusCode.NotFound"/> if the model does not define its context size.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_loaded_file_max_context_tokens(
            LoadedFileHandle loadedFile, out uint outMaxContextTokens);

        /// <summary>
        /// Gets whether the model has dynamic context.
        /// Dynamic context means the context size can be configured by the caller.
        /// This is determined from the model graph, not from the LLM metadata.
        /// </summary>
        /// <param name="loadedFile">The loaded file to inspect.</param>
        /// <param name="outIsDynamic">
        /// On success, receives <see langword="true"/> if the model has dynamic context,
        /// <see langword="false"/> otherwise (including when the model has neither LLM nor
        /// embedding metadata); not written on failure.
        /// </param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or another <see cref="StatusCode"/> on failure.
        /// <see cref="StatusCode.InvalidArgument"/> if <paramref name="loadedFile"/> is invalid.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_loaded_file_is_dynamic_context(
            LoadedFileHandle loadedFile, [MarshalAs(UnmanagedType.I1)] out bool outIsDynamic);

        /// <summary>
        /// Gets the supported vision token lengths (the discrete vision token budgets
        /// supported by each vision signature).
        /// <para>
        /// Writes up to <paramref name="maxSize"/> lengths to <paramref name="lengths"/>. To query only
        /// the count, pass <paramref name="lengths"/> = <see langword="null"/> (and
        /// <paramref name="maxSize"/> = 0).
        /// </para>
        /// </summary>
        /// <param name="loadedFile">The loaded file to inspect.</param>
        /// <param name="lengths">
        /// Buffer that receives up to <paramref name="maxSize"/> lengths on success, or
        /// <see langword="null"/> to query only the count. Not written on failure.
        /// </param>
        /// <param name="maxSize">The capacity of <paramref name="lengths"/>, in elements.</param>
        /// <param name="outCount">
        /// On success, receives the total number of supported lengths, which may exceed
        /// <paramref name="maxSize"/>; not written on failure.
        /// </param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or another <see cref="StatusCode"/> on failure.
        /// <see cref="StatusCode.InvalidArgument"/> if <paramref name="loadedFile"/> is invalid;
        /// <see cref="StatusCode.NotFound"/> if the model does not support vision or does not define
        /// vision signature lengths.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_loaded_file_vision_signature_selection(
            LoadedFileHandle loadedFile, [Out] int[]? lengths,
            UIntPtr maxSize, out UIntPtr outCount);

        /// <summary>
        /// Gets the supported backends for a given modality, ordered by priority (first
        /// entry is the default/highest-priority backend).
        /// <para>
        /// Writes up to <paramref name="maxSize"/> backends to <paramref name="backends"/>. To query
        /// only the count, pass <paramref name="backends"/> = <see langword="null"/> (and
        /// <paramref name="maxSize"/> = 0).
        /// </para>
        /// </summary>
        /// <param name="loadedFile">The loaded file to inspect.</param>
        /// <param name="modality">The modality to query.</param>
        /// <param name="backends">
        /// Buffer that receives up to <paramref name="maxSize"/> backends on success, or
        /// <see langword="null"/> to query only the count. Not written on failure.
        /// </param>
        /// <param name="maxSize">The capacity of <paramref name="backends"/>, in elements.</param>
        /// <param name="outCount">
        /// On success, receives the total number of supported backends, which may exceed
        /// <paramref name="maxSize"/>; 0 if the modality is not supported. Not written on failure.
        /// </param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success (including an unsupported modality), or another
        /// <see cref="StatusCode"/> on failure. <see cref="StatusCode.InvalidArgument"/> if
        /// <paramref name="loadedFile"/> is invalid or <paramref name="modality"/> is not a valid
        /// <see cref="Modality"/>.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_loaded_file_modality_supported_backends(
            LoadedFileHandle loadedFile, Modality modality, [Out] BackendType[]? backends,
            UIntPtr maxSize, out UIntPtr outCount);

        /// <summary>
        /// Gets the detected NPU brand of the model for a given modality.
        /// </summary>
        /// <param name="loadedFile">The loaded file to inspect.</param>
        /// <param name="modality">The modality to query.</param>
        /// <param name="outNpuBrand">
        /// On success, receives the detected NPU brand, or <see cref="NpuBrand.Unknown"/> if the model
        /// is not NPU-compiled for <paramref name="modality"/> or its NPU brand could not be detected.
        /// Not written on failure.
        /// </param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or another <see cref="StatusCode"/> on failure.
        /// <see cref="StatusCode.InvalidArgument"/> if <paramref name="loadedFile"/> is invalid or
        /// <paramref name="modality"/> is not a valid <see cref="Modality"/>.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_loaded_file_modality_npu_brand(
            LoadedFileHandle loadedFile, Modality modality, out NpuBrand outNpuBrand);

        /// <summary>
        /// Gets the target SoC name for a given modality (e.g. "SM8750", "Tensor_G5").
        /// </summary>
        /// <param name="loadedFile">The loaded file to inspect.</param>
        /// <param name="modality">The modality to query.</param>
        /// <param name="outSocName">
        /// On success, receives the NUL-terminated SoC name, or <see cref="IntPtr.Zero"/> if the model
        /// does not specify one for <paramref name="modality"/> (e.g. it is not NPU-compiled). The string
        /// is owned by <paramref name="loadedFile"/> and valid until it is deleted. Set to
        /// <see cref="IntPtr.Zero"/> on failure.
        /// </param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success (including when no SoC name is specified), or another
        /// <see cref="StatusCode"/> on failure. <see cref="StatusCode.InvalidArgument"/> if
        /// <paramref name="loadedFile"/> is invalid or <paramref name="modality"/> is not a valid
        /// <see cref="Modality"/>.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_loaded_file_modality_soc_name(
            LoadedFileHandle loadedFile, Modality modality, out IntPtr outSocName);

        /// <summary>
        /// Gets the minimum LiteRT-LM runtime version required to run this model.
        /// </summary>
        /// <param name="loadedFile">The loaded file to inspect.</param>
        /// <param name="outVersion">
        /// On success, receives the NUL-terminated version string, or <see cref="IntPtr.Zero"/> if the
        /// model does not define a version requirement. The string is owned by
        /// <paramref name="loadedFile"/> and valid until it is deleted. Set to <see cref="IntPtr.Zero"/>
        /// on failure.
        /// </param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success (including when no version requirement is defined), or
        /// another <see cref="StatusCode"/> on failure. <see cref="StatusCode.InvalidArgument"/> if
        /// <paramref name="loadedFile"/> is invalid.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_loaded_file_min_runtime_version(
            LoadedFileHandle loadedFile, out IntPtr outVersion);

        /// <summary>
        /// Gets the model type of the loaded LiteRT-LM file.
        /// </summary>
        /// <param name="loadedFile">The loaded file to inspect.</param>
        /// <param name="outModelType">
        /// On success, receives the model type, or <see cref="ModelType.Unknown"/> if the file has
        /// neither LLM nor embedding metadata. Not written on failure.
        /// </param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or another <see cref="StatusCode"/> on failure.
        /// <see cref="StatusCode.InvalidArgument"/> if <paramref name="loadedFile"/> is invalid.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_loaded_file_model_type(
            LoadedFileHandle loadedFile, out ModelType outModelType);

        /// <summary>
        /// Gets the output embedding dimension for the model.
        /// </summary>
        /// <param name="loadedFile">The loaded file to inspect.</param>
        /// <param name="outDimension">
        /// On success, receives the embedding dimension (always positive); not written on failure.
        /// </param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or another <see cref="StatusCode"/> on failure.
        /// <see cref="StatusCode.InvalidArgument"/> if <paramref name="loadedFile"/> is invalid;
        /// <see cref="StatusCode.NotFound"/> if the model is not an embedding model or does not define
        /// the dimension.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_loaded_file_embedding_dimension(
            LoadedFileHandle loadedFile, out int outDimension);

        /// <summary>
        /// Gets the supported embedding signature sequence lengths.
        /// <para>
        /// Writes up to <paramref name="maxSize"/> lengths to <paramref name="lengths"/>. To query only
        /// the count, pass <paramref name="lengths"/> = <see langword="null"/> (and
        /// <paramref name="maxSize"/> = 0).
        /// </para>
        /// </summary>
        /// <param name="loadedFile">The loaded file to inspect.</param>
        /// <param name="lengths">
        /// Buffer that receives up to <paramref name="maxSize"/> lengths on success, or
        /// <see langword="null"/> to query only the count. Not written on failure.
        /// </param>
        /// <param name="maxSize">The capacity of <paramref name="lengths"/>, in elements.</param>
        /// <param name="outCount">
        /// On success, receives the total number of supported lengths, which may exceed
        /// <paramref name="maxSize"/>; not written on failure.
        /// </param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or another <see cref="StatusCode"/> on failure.
        /// <see cref="StatusCode.InvalidArgument"/> if <paramref name="loadedFile"/> is invalid;
        /// <see cref="StatusCode.NotFound"/> if the model is not an embedding model or does not define
        /// signature lengths.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_loaded_file_embedding_signature_selection(
            LoadedFileHandle loadedFile, [Out] int[]? lengths,
            UIntPtr maxSize, out UIntPtr outCount);
    }
}