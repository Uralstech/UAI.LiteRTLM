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
    public static class EmbeddingEngineH
    {
        /// <summary>
        /// Creates LiteRT LM Embedding Engine Settings.
        /// </summary>
        /// <param name="modelPath">The path to the model file.</param>
        /// <param name="backendStr">The backend to use (e.g., "cpu", "gpu", "npu").</param>
        /// <param name="visionBackendStr">The vision backend to use, or <see langword="null"/> if not set.</param>
        /// <param name="audioBackendStr">The audio backend to use, or <see langword="null"/> if not set.</param>
        /// <param name="outSettings">
        /// On success, receives the created settings, owned by the caller; release with
        /// <see cref="EmbeddingEngineSettingsHandle.Dispose"/>. Set to an invalid handle
        /// (<see cref="LiteRTHandle.IsInvalid"/>) on failure.
        /// </param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or another <see cref="StatusCode"/> on failure.
        /// <see cref="StatusCode.InvalidArgument"/> if <paramref name="modelPath"/> or
        /// <paramref name="backendStr"/> is <see langword="null"/>, or a backend string is not
        /// recognized; otherwise the code of the error that prevented the settings from being
        /// created (e.g. the model file could not be opened).
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_embedding_engine_settings_create(
            [MarshalAs(UnmanagedType.LPUTF8Str)] string modelPath,
            [MarshalAs(UnmanagedType.LPUTF8Str)] string backendStr,
            [MarshalAs(UnmanagedType.LPUTF8Str)] string? visionBackendStr,
            [MarshalAs(UnmanagedType.LPUTF8Str)] string? audioBackendStr,
            out EmbeddingEngineSettingsHandle outSettings);

        /// <summary>
        /// Destroys LiteRT LM Embedding Engine Settings.
        /// </summary>
        /// <param name="settings">The settings to destroy.</param>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern void litert_lm_embedding_engine_settings_delete(
            IntPtr settings);

        /// <summary>
        /// Sets the number of threads for the CPU backend in Embedding Engine Settings.
        /// </summary>
        /// <param name="settings">The embedding engine settings.</param>
        /// <param name="numThreads">The number of threads.</param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or <see cref="StatusCode.InvalidArgument"/> if
        /// <paramref name="settings"/> is invalid. If <paramref name="numThreads"/> is
        /// not positive, this call has no effect and returns <see cref="StatusCode.Ok"/>.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_embedding_engine_settings_set_num_threads(
            EmbeddingEngineSettingsHandle settings, int numThreads);

        /// <summary>
        /// Sets the number of threads for the audio CPU backend in Embedding Engine Settings.
        /// </summary>
        /// <param name="settings">The embedding engine settings.</param>
        /// <param name="numThreads">The number of threads.</param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or <see cref="StatusCode.InvalidArgument"/> if
        /// <paramref name="settings"/> is invalid. If <paramref name="numThreads"/> is
        /// not positive or no audio backend is configured, this call has no effect and returns
        /// <see cref="StatusCode.Ok"/>.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_embedding_engine_settings_set_audio_num_threads(
            EmbeddingEngineSettingsHandle settings, int numThreads);

        /// <summary>
        /// Sets the cache directory for the Embedding Engine.
        /// </summary>
        /// <param name="settings">The embedding engine settings.</param>
        /// <param name="cacheDir">The cache directory.</param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or <see cref="StatusCode.InvalidArgument"/> if
        /// <paramref name="settings"/> is invalid or <paramref name="cacheDir"/> is
        /// <see langword="null"/>.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_embedding_engine_settings_set_cache_dir(
            EmbeddingEngineSettingsHandle settings, [MarshalAs(UnmanagedType.LPUTF8Str)] string cacheDir);

        /// <summary>
        /// Sets the LiteRT dispatch library directory for the main NPU backend.
        /// </summary>
        /// <param name="settings">The embedding engine settings.</param>
        /// <param name="libDir">The dispatch library directory.</param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or <see cref="StatusCode.InvalidArgument"/> if
        /// <paramref name="settings"/> is invalid or <paramref name="libDir"/> is
        /// <see langword="null"/>.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_embedding_engine_settings_set_litert_dispatch_lib_dir(
            EmbeddingEngineSettingsHandle settings, [MarshalAs(UnmanagedType.LPUTF8Str)] string libDir);

        /// <summary>
        /// Sets the LiteRT dispatch library directory for the vision NPU backend.
        /// </summary>
        /// <param name="settings">The embedding engine settings.</param>
        /// <param name="libDir">The dispatch library directory.</param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or <see cref="StatusCode.InvalidArgument"/> if
        /// <paramref name="settings"/> is invalid or <paramref name="libDir"/> is
        /// <see langword="null"/>. If no vision backend is configured, this call has no effect and
        /// returns <see cref="StatusCode.Ok"/>.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_embedding_engine_settings_set_vision_litert_dispatch_lib_dir(
            EmbeddingEngineSettingsHandle settings, [MarshalAs(UnmanagedType.LPUTF8Str)] string libDir);

        /// <summary>
        /// Sets the LiteRT dispatch library directory for the audio NPU backend.
        /// </summary>
        /// <param name="settings">The embedding engine settings.</param>
        /// <param name="libDir">The dispatch library directory.</param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or <see cref="StatusCode.InvalidArgument"/> if
        /// <paramref name="settings"/> is invalid or <paramref name="libDir"/> is
        /// <see langword="null"/>. If no audio backend is configured, this call has no effect and
        /// returns <see cref="StatusCode.Ok"/>.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_embedding_engine_settings_set_audio_litert_dispatch_lib_dir(
            EmbeddingEngineSettingsHandle settings, [MarshalAs(UnmanagedType.LPUTF8Str)] string libDir);

        /// <summary>
        /// Sets the maximum sequence length (in tokens) for text encoder signatures in
        /// Embedding Engine Settings.
        /// </summary>
        /// <param name="settings">The embedding engine settings.</param>
        /// <param name="maxInputLength">
        /// The maximum input length. Passing a non-positive value unsets the option.
        /// </param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or <see cref="StatusCode.InvalidArgument"/> if
        /// <paramref name="settings"/> is invalid.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_embedding_engine_settings_set_max_input_length(
            EmbeddingEngineSettingsHandle settings, int maxInputLength);

        /// <summary>
        /// Sets the minimum sequence length (in tokens) for text encoder signatures in
        /// Embedding Engine Settings.
        /// </summary>
        /// <param name="settings">The embedding engine settings.</param>
        /// <param name="minInputLength">
        /// The minimum input length. Passing a negative value unsets the option.
        /// </param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or <see cref="StatusCode.InvalidArgument"/> if
        /// <paramref name="settings"/> is invalid.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_embedding_engine_settings_set_min_input_length(
            EmbeddingEngineSettingsHandle settings, int minInputLength);

        /// <summary>
        /// Sets the desired number of vision tokens generated per image in Embedding
        /// Engine Settings.
        /// </summary>
        /// <param name="settings">The embedding engine settings.</param>
        /// <param name="visionTokensPerImage">
        /// The vision tokens per image. Passing a non-positive value unsets the option.
        /// </param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or <see cref="StatusCode.InvalidArgument"/> if
        /// <paramref name="settings"/> is invalid.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_embedding_engine_settings_set_vision_tokens_per_image(
            EmbeddingEngineSettingsHandle settings, int visionTokensPerImage);

        /// <summary>
        /// Sets the activation data type for the embedding engine settings.
        /// </summary>
        /// <param name="settings">The embedding engine settings.</param>
        /// <param name="activationDataType">The activation data type (FLOAT32, FLOAT16, etc.).</param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or <see cref="StatusCode.InvalidArgument"/> if
        /// <paramref name="settings"/> is invalid or <paramref name="activationDataType"/>
        /// is not a declared <see cref="ActivationDataType"/> value.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_embedding_engine_settings_set_activation_data_type(
            EmbeddingEngineSettingsHandle settings, ActivationDataType activationDataType);

        /// <summary>
        /// Creates LiteRT LM Embedding Options with default values (<c>normalize = true</c>,
        /// <c>insert_special_tokens = true</c>).
        /// </summary>
        /// <param name="outOptions">
        /// On success, receives the created options, owned by the caller; release with
        /// <see cref="EmbeddingOptionsHandle.Dispose"/>. Set to an invalid handle
        /// (<see cref="LiteRTHandle.IsInvalid"/>) on failure.
        /// </param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or another <see cref="StatusCode"/> on failure.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_embedding_options_create(
            out EmbeddingOptionsHandle outOptions);

        /// <summary>
        /// Destroys LiteRT LM Embedding Options.
        /// </summary>
        /// <param name="options">The options to destroy.</param>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern void litert_lm_embedding_options_delete(IntPtr options);

        /// <summary>
        /// Sets whether the embedding should be L2 normalized.
        /// </summary>
        /// <param name="options">The options to modify.</param>
        /// <param name="normalize">Whether to normalize.</param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or <see cref="StatusCode.InvalidArgument"/> if
        /// <paramref name="options"/> is invalid.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_embedding_options_set_normalize(
            EmbeddingOptionsHandle options, [MarshalAs(UnmanagedType.I1)] bool normalize);

        /// <summary>
        /// Gets whether the embedding should be L2 normalized.
        /// </summary>
        /// <param name="options">The options to inspect.</param>
        /// <param name="outNormalize">
        /// On success, receives <see langword="true"/> if normalization is enabled,
        /// <see langword="false"/> otherwise; not written on failure.
        /// </param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or another <see cref="StatusCode"/> on failure.
        /// <see cref="StatusCode.InvalidArgument"/> if <paramref name="options"/> is invalid.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_embedding_options_get_normalize(
            EmbeddingOptionsHandle options, [MarshalAs(UnmanagedType.I1)] out bool outNormalize);

        /// <summary>
        /// Sets whether special tokens (BOS, EOS, start/end of image, start/end of audio)
        /// should be automatically inserted.
        /// </summary>
        /// <param name="options">The options to modify.</param>
        /// <param name="insertSpecialTokens">Whether to insert special tokens.</param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or <see cref="StatusCode.InvalidArgument"/> if
        /// <paramref name="options"/> is invalid.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_embedding_options_set_insert_special_tokens(
            EmbeddingOptionsHandle options, [MarshalAs(UnmanagedType.I1)] bool insertSpecialTokens);

        /// <summary>
        /// Gets whether special tokens should be automatically inserted.
        /// </summary>
        /// <param name="options">The options to inspect.</param>
        /// <param name="outInsertSpecialTokens">
        /// On success, receives <see langword="true"/> if special tokens insertion is enabled,
        /// <see langword="false"/> otherwise; not written on failure.
        /// </param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or another <see cref="StatusCode"/> on failure.
        /// <see cref="StatusCode.InvalidArgument"/> if <paramref name="options"/> is invalid.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_embedding_options_get_insert_special_tokens(
            EmbeddingOptionsHandle options, [MarshalAs(UnmanagedType.I1)] out bool outInsertSpecialTokens);

        /// <summary>
        /// Sets the input overflow strategy.
        /// </summary>
        /// <param name="options">The options to modify.</param>
        /// <param name="strategy">The overflow strategy to use.</param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or <see cref="StatusCode.InvalidArgument"/> if
        /// <paramref name="options"/> is invalid or <paramref name="strategy"/> is
        /// not a declared <see cref="InputOverflowStrategy"/> value.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_embedding_options_set_input_overflow_strategy(
            EmbeddingOptionsHandle options, InputOverflowStrategy strategy);

        /// <summary>
        /// Gets the input overflow strategy.
        /// </summary>
        /// <param name="options">The options to inspect.</param>
        /// <param name="outStrategy">
        /// On success, receives the overflow strategy configured in <paramref name="options"/>;
        /// not written on failure.
        /// </param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or another <see cref="StatusCode"/> on failure.
        /// <see cref="StatusCode.InvalidArgument"/> if <paramref name="options"/> is invalid.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_embedding_options_get_input_overflow_strategy(
            EmbeddingOptionsHandle options, out InputOverflowStrategy outStrategy);

        /// <summary>
        /// Sets the output embedding size to truncate the embedding to.
        /// </summary>
        /// <param name="options">The options to modify.</param>
        /// <param name="outputSize">
        /// The output embedding size to truncate to. Pass 0 or a negative value (e.g., 0 or -1)
        /// to unset and use the default output embedding size.
        /// </param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or <see cref="StatusCode.InvalidArgument"/> if
        /// <paramref name="options"/> is invalid.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_embedding_options_set_output_size(
            EmbeddingOptionsHandle options, int outputSize);

        /// <summary>
        /// Gets the output embedding size.
        /// </summary>
        /// <param name="options">The options to inspect.</param>
        /// <param name="outOutputSize">
        /// On success, receives the output embedding size; not written on failure or if the size is not set.
        /// </param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or another <see cref="StatusCode"/> on failure.
        /// <see cref="StatusCode.InvalidArgument"/> if <paramref name="options"/> is invalid;
        /// <see cref="StatusCode.NotFound"/> if the output size is not set (the default output embedding size is used).
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_embedding_options_get_output_size(
            EmbeddingOptionsHandle options, out int outOutputSize);

        /// <summary>
        /// Sets the vision tokens per image.
        /// </summary>
        /// <param name="options">The options to modify.</param>
        /// <param name="visionTokensPerImage">
        /// The number of vision tokens per image. Passing a non-positive value unsets the option.
        /// </param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or <see cref="StatusCode.InvalidArgument"/> if
        /// <paramref name="options"/> is invalid.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_embedding_options_set_vision_tokens_per_image(
            EmbeddingOptionsHandle options, int visionTokensPerImage);

        /// <summary>
        /// Gets the vision tokens per image.
        /// </summary>
        /// <param name="options">The options to inspect.</param>
        /// <param name="outVisionTokensPerImage">
        /// On success, receives the vision tokens per image configured in <paramref name="options"/>;
        /// not written on failure or if the value is not set.
        /// </param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or another <see cref="StatusCode"/> on failure.
        /// <see cref="StatusCode.InvalidArgument"/> if <paramref name="options"/> is invalid;
        /// <see cref="StatusCode.NotFound"/> if the vision tokens per image is not set (the engine or model
        /// default is used).
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_embedding_options_get_vision_tokens_per_image(
            EmbeddingOptionsHandle options, out int outVisionTokensPerImage);

        /// <summary>
        /// Destroys a LiteRT LM Embedding Response.
        /// </summary>
        /// <param name="response">The response to destroy.</param>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern void litert_lm_embedding_response_delete(IntPtr response);

        /// <summary>
        /// Gets the dimension (number of float values) of the embedding response.
        /// </summary>
        /// <param name="response">The response to inspect.</param>
        /// <param name="outSize">
        /// On success, receives the number of float elements; not written on failure.
        /// </param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or another <see cref="StatusCode"/> on failure.
        /// <see cref="StatusCode.InvalidArgument"/> if <paramref name="response"/> is invalid.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_embedding_response_get_size(
            EmbeddingResponseHandle response, out UIntPtr outSize);

        /// <summary>
        /// Gets a pointer to the array of float embedding values.
        /// </summary>
        /// <param name="response">The response to inspect.</param>
        /// <param name="outValues">
        /// On success, receives a pointer to the <see cref="litert_lm_embedding_response_get_size"/>
        /// float values, or <see cref="IntPtr.Zero"/> if the embedding is empty. The array is owned by
        /// <paramref name="response"/> and valid until <paramref name="response"/> (or the
        /// <see cref="EmbeddingResponsesHandle"/> that owns it) is deleted. Set to <see cref="IntPtr.Zero"/>
        /// on failure.
        /// </param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success (including an empty embedding), or another
        /// <see cref="StatusCode"/> on failure. <see cref="StatusCode.InvalidArgument"/> if
        /// <paramref name="response"/> is invalid.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_embedding_response_get_values(
            EmbeddingResponseHandle response, out IntPtr outValues);

        /// <summary>
        /// Destroys a collection of LiteRT LM Embedding Responses.
        /// </summary>
        /// <param name="responses">The responses collection to destroy.</param>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern void litert_lm_embedding_responses_delete(
            IntPtr responses);

        /// <summary>
        /// Gets the number of responses in the collection.
        /// </summary>
        /// <param name="responses">The responses collection.</param>
        /// <param name="outSize">
        /// On success, receives the batch size; not written on failure.
        /// </param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or another <see cref="StatusCode"/> on failure.
        /// <see cref="StatusCode.InvalidArgument"/> if <paramref name="responses"/> is invalid.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_embedding_responses_get_size(
            EmbeddingResponsesHandle responses, out UIntPtr outSize);

        /// <summary>
        /// Gets the embedding response at the given index in the batch.
        /// </summary>
        /// <param name="responses">The responses collection.</param>
        /// <param name="index">The batch index.</param>
        /// <param name="outResponse">
        /// On success, receives the embedding response at <paramref name="index"/>. It is owned by
        /// <paramref name="responses"/> and valid until <paramref name="responses"/> is deleted; do not
        /// pass its raw pointer to <see cref="litert_lm_embedding_response_delete"/>. Set to an invalid
        /// handle (<see cref="LiteRTHandle.IsInvalid"/>) on failure.
        /// </param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or another <see cref="StatusCode"/> on failure.
        /// <see cref="StatusCode.InvalidArgument"/> if <paramref name="responses"/> is invalid;
        /// <see cref="StatusCode.OutOfRange"/> if <paramref name="index"/> is not less than the batch size.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_embedding_responses_get_at(
            EmbeddingResponsesHandle responses, UIntPtr index,
            out BorrowedEmbeddingResponseHandle outResponse);

        /// <summary>
        /// Creates a LiteRT LM Embedding Engine from the given settings.
        /// </summary>
        /// <param name="settings">The embedding engine settings.</param>
        /// <param name="outEngine">
        /// On success, receives the created engine, owned by the caller; release with
        /// <see cref="EmbeddingEngineHandle.Dispose"/>. Set to an invalid handle
        /// (<see cref="LiteRTHandle.IsInvalid"/>) on failure.
        /// </param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or another <see cref="StatusCode"/> on failure.
        /// <see cref="StatusCode.InvalidArgument"/> if <paramref name="settings"/> is invalid;
        /// otherwise the code of the error that prevented the engine from being created.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_embedding_engine_create(
            EmbeddingEngineSettingsHandle settings, out EmbeddingEngineHandle outEngine);

        /// <summary>
        /// Destroys a LiteRT LM Embedding Engine.
        /// </summary>
        /// <param name="engine">The engine to destroy.</param>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern void litert_lm_embedding_engine_delete(IntPtr engine);

        /// <summary>
        /// Computes embedding response for a single request.
        /// </summary>
        /// <param name="engine">The embedding engine.</param>
        /// <param name="inputs">
        /// Array of <c>LiteRtLmInputData</c> pointers representing multimodal input.
        /// </param>
        /// <param name="numInputs">Number of inputs in the array.</param>
        /// <param name="options">
        /// Optional embedding options. If <see cref="EmbeddingOptionsHandle.Null"/> (or invalid),
        /// default options are used.
        /// </param>
        /// <param name="outResponse">
        /// On success, receives the embedding response, owned by the caller; release with
        /// <see cref="EmbeddingResponseHandle.Dispose"/>. Set to an invalid handle
        /// (<see cref="LiteRTHandle.IsInvalid"/>) on failure.
        /// </param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or another <see cref="StatusCode"/> on failure.
        /// <see cref="StatusCode.InvalidArgument"/> if <paramref name="engine"/> is invalid;
        /// otherwise the code of the error that prevented the embedding from being computed.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_embedding_engine_compute_embedding(
            EmbeddingEngineHandle engine, IntPtr[] inputs, // GC.KeepAlive the InputDataHandle[]
            UIntPtr numInputs, EmbeddingOptionsHandle options,
            out EmbeddingResponseHandle outResponse);

        /// <summary>
        /// Computes embedding responses for a batch of requests.
        /// </summary>
        /// <param name="engine">The embedding engine.</param>
        /// <param name="inputsBatch">
        /// An array of pinned pointers to arrays of <c>LiteRtLmInputData</c> pointers.
        /// </param>
        /// <param name="numInputsPerBatch">
        /// An array specifying the number of inputs for each request in the batch.
        /// </param>
        /// <param name="batchSize">The number of requests in the batch.</param>
        /// <param name="options">
        /// Optional embedding options. If <see cref="EmbeddingOptionsHandle.Null"/> (or invalid),
        /// default options are used.
        /// </param>
        /// <param name="outResponses">
        /// On success, receives the batch responses, owned by the caller; release with
        /// <see cref="EmbeddingResponsesHandle.Dispose"/>. Set to an invalid handle
        /// (<see cref="LiteRTHandle.IsInvalid"/>) on failure.
        /// </param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or another <see cref="StatusCode"/> on failure.
        /// <see cref="StatusCode.InvalidArgument"/> if <paramref name="engine"/> is invalid;
        /// otherwise the code of the error that prevented the embeddings from being computed.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_embedding_engine_compute_embedding_batch(
            EmbeddingEngineHandle engine, IntPtr[] inputsBatch, // Pin inner IntPtr[]s and GC.KeepAlive the InputDataHandle[][]
            UIntPtr[] numInputsPerBatch, UIntPtr batchSize, EmbeddingOptionsHandle options,
            out EmbeddingResponsesHandle outResponses);
    }
}