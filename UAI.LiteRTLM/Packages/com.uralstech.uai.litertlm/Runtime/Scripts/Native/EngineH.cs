// Copyright 2026 URAV ADVANCED LEARNING SYSTEMS PRIVATE LIMITED
// Copyright 2025 The ODML Authors.
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
    /// P/Invoke bindings for the LiteRT LM engine, session, sampler, benchmark, and tokenizer APIs.
    /// </summary>
    /// <remarks>
    /// <para><b>Threading model</b></para>
    /// <para>
    /// NOTE: This section describes the current behavior of the implementation so
    /// that callers can use the API safely. It is not a commitment to keep these
    /// constraints; they may be changed in a future version.
    /// </para>
    /// <list type="bullet">
    /// <item><description>
    /// Handles are not thread-safe: calls that use the same handle MUST NOT
    /// overlap. When single-threaded execution is enabled (see
    /// <see cref="litert_lm_engine_settings_set_single_threaded_execution"/>), this extends to
    /// an engine and every session and conversation created from it.
    /// </description></item>
    /// <item><description>
    /// Stream callbacks can run on library-owned threads that are shared by all
    /// streams of an engine, so a callback should return quickly. When
    /// single-threaded execution is enabled, queued work, and with it the
    /// callbacks, runs only while a caller drives it, for example inside
    /// <see cref="ConversationH.litert_lm_conversation_wait_until_done"/>.
    /// </description></item>
    /// <item><description>
    /// A callback MUST NOT call LiteRT LM functions on the engine that runs the
    /// stream, or on any session or conversation created from it, with one
    /// exception: while handling a chunk that is not final, it may stop the stream
    /// with <see cref="litert_lm_session_cancel_process"/> or
    /// <see cref="ConversationH.litert_lm_conversation_cancel_process"/>. Hand any other such work off to
    /// another thread.
    /// </description></item>
    /// <item><description>
    /// After <see cref="litert_lm_session_cancel_process"/> or
    /// <see cref="ConversationH.litert_lm_conversation_cancel_process"/>, the session or conversation MUST
    /// NOT be reused; only delete it.
    /// </description></item>
    /// <item><description>
    /// The last error is thread-local: read it on the thread that received the failing status code.
    /// Errors that occur after a stream has started are reported through its chunks instead (see
    /// <see cref="litert_lm_stream_chunk_get_error"/>).
    /// </description></item>
    /// </list>
    /// <para><b>Ownership and lifetime</b></para>
    /// <para>
    /// An engine owns the resources shared by every session and conversation
    /// created from it. Handles are NOT reference counted across the C boundary
    /// and are NOT reparented.
    /// </para>
    /// <para>
    /// The caller MUST destroy every session and conversation created from an
    /// engine before destroying the engine itself. Destroying an engine with
    /// live children, or using a child after its engine has been destroyed, is
    /// undefined behavior.
    /// </para>
    /// <para>
    /// Handles are not thread-safe; see the threading model notes above.
    /// </para>
    /// </remarks>
    public static class EngineH
    {
        public const int LiteRTLMTargetVersionMajor = 1;
        public const int LiteRTLMTargetVersionMinor = 0;
        public const int LiteRTLMTargetVersionPatch = 0;

        /// <summary>
        /// Returns the version of the library, which may differ from the
        /// <c>LiteRTLMTargetVersion*</c> constants this package was written for.
        /// </summary>
        /// <param name="outMajor">
        /// On success, receives the major version of the library; not written on failure.
        /// </param>
        /// <param name="outMinor">
        /// On success, receives the minor version of the library; not written on failure.
        /// </param>
        /// <param name="outPatch">
        /// On success, receives the patch version of the library; not written on failure.
        /// </param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_get_api_version(
            out int outMajor, out int outMinor, out int outPatch);

        /// <summary>
        /// Creates LiteRT LM Sampler Parameters with a specific sampler type.
        /// </summary>
        /// <param name="type">The sampler type to use.</param>
        /// <param name="outParams">
        /// On success, receives the created parameters, owned by the caller; release with
        /// <see cref="SamplerParamsHandle.Dispose"/>. Set to an invalid handle
        /// (<see cref="LiteRTHandle.IsInvalid"/>) on failure.
        /// </param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or another <see cref="StatusCode"/> on failure.
        /// <see cref="StatusCode.InvalidArgument"/> if <paramref name="type"/> is not a recognized
        /// <see cref="SamplerType"/>.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_sampler_params_create(
            SamplerType type, out SamplerParamsHandle outParams);

        /// <summary>
        /// Destroys LiteRT LM Sampler Parameters.
        /// </summary>
        /// <param name="params">The parameters to destroy.</param>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern void litert_lm_sampler_params_delete(IntPtr @params);

        /// <summary>
        /// Sets the top-k value.
        /// </summary>
        /// <param name="params">The sampler parameters to modify.</param>
        /// <param name="topK">The top-k value.</param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or <see cref="StatusCode.InvalidArgument"/> if
        /// <paramref name="params"/> is invalid.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_sampler_params_set_top_k(
            SamplerParamsHandle @params, int topK);

        /// <summary>
        /// Sets the top-p value.
        /// </summary>
        /// <param name="params">The sampler parameters to modify.</param>
        /// <param name="topP">The top-p value.</param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or <see cref="StatusCode.InvalidArgument"/> if
        /// <paramref name="params"/> is invalid.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_sampler_params_set_top_p(
            SamplerParamsHandle @params, float topP);

        /// <summary>
        /// Sets the temperature.
        /// </summary>
        /// <param name="params">The sampler parameters to modify.</param>
        /// <param name="temperature">The sampling temperature.</param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or <see cref="StatusCode.InvalidArgument"/> if
        /// <paramref name="params"/> is invalid.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_sampler_params_set_temperature(
            SamplerParamsHandle @params, float temperature);

        /// <summary>
        /// Sets the seed.
        /// </summary>
        /// <param name="params">The sampler parameters to modify.</param>
        /// <param name="seed">The random seed.</param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or <see cref="StatusCode.InvalidArgument"/> if
        /// <paramref name="params"/> is invalid.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_sampler_params_set_seed(
            SamplerParamsHandle @params, int seed);

        /// <summary>
        /// Creates a LiteRT LM Session Config.
        /// </summary>
        /// <param name="outConfig">
        /// On success, receives the created config, owned by the caller; release with
        /// <see cref="SessionConfigHandle.Dispose"/>. Set to an invalid handle
        /// (<see cref="LiteRTHandle.IsInvalid"/>) on failure.
        /// </param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or another <see cref="StatusCode"/> on failure.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_session_config_create(
            out SessionConfigHandle outConfig);

        /// <summary>
        /// Sets the maximum number of output tokens per decode step for this session.
        /// For thinking models, both thinking (reasoning) tokens and the final response
        /// tokens count towards this limit.
        /// </summary>
        /// <param name="config">The config to modify.</param>
        /// <param name="maxOutputTokens">
        /// The maximum number of tokens to generate (including thinking tokens).
        /// </param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or <see cref="StatusCode.InvalidArgument"/> if
        /// <paramref name="config"/> is invalid.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_session_config_set_max_output_tokens(
            SessionConfigHandle config, int maxOutputTokens);

        /// <summary>
        /// Sets whether to apply prompt template for this session.
        /// </summary>
        /// <param name="config">The config to modify.</param>
        /// <param name="applyPromptTemplate">Whether to apply prompt template.</param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or <see cref="StatusCode.InvalidArgument"/> if
        /// <paramref name="config"/> is invalid.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_session_config_set_apply_prompt_template(
            SessionConfigHandle config, [MarshalAs(UnmanagedType.I1)] bool applyPromptTemplate);

        /// <summary>
        /// Sets whether to enable speculative decoding for this session.
        /// </summary>
        /// <remarks>
        /// Deprecated: use <see cref="litert_lm_session_config_set_speculative_decoding_config"/>
        /// instead. Passing <see langword="true"/> is equivalent to a config whose mode is
        /// <see cref="SpeculativeDecodingMode.On"/> and passing <see langword="false"/> to
        /// <see cref="SpeculativeDecodingMode.Off"/>.
        /// </remarks>
        /// <param name="config">The config to modify.</param>
        /// <param name="enableSpeculativeDecoding">
        /// Whether to enable speculative decoding. If set to <see langword="true"/>, speculative
        /// decoding is enabled for this session. If the engine was not initialized with speculative
        /// decoding enabled, setting this flag to <see langword="true"/> causes the executor to
        /// perform lazy loading of the MTP drafter on the first session request. If set to
        /// <see langword="false"/>, speculative decoding is explicitly disabled for this session even
        /// if the engine was initialized with speculative decoding enabled. If this function is not
        /// called on the config, the session inherits the engine's speculative decoding setting by
        /// default. This function and <see cref="litert_lm_session_config_set_speculative_decoding_config"/>
        /// write the same mode, so the last call wins.
        /// </param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or <see cref="StatusCode.InvalidArgument"/> if
        /// <paramref name="config"/> is invalid.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        [Obsolete("Use litert_lm_session_config_set_speculative_decoding_config instead.")]
        public static extern StatusCode litert_lm_session_config_set_enable_speculative_decoding(
            SessionConfigHandle config, [MarshalAs(UnmanagedType.I1)] bool enableSpeculativeDecoding);

        /// <summary>
        /// Creates a LiteRT LM Speculative Decoding Config with every field unset.
        /// <para>
        /// A speculative decoding config is set on the engine settings, as the default
        /// of every session (<see cref="litert_lm_engine_settings_set_speculative_decoding_config"/>),
        /// and on a session config, as an override
        /// (<see cref="litert_lm_session_config_set_speculative_decoding_config"/>). A field left
        /// unset on the session inherits the engine's value, and a field left unset on
        /// the engine uses the runtime default.
        /// </para>
        /// </summary>
        /// <param name="outConfig">
        /// On success, receives the created config, owned by the caller; release with
        /// <see cref="SpeculativeDecodingConfigHandle.Dispose"/>. Set to an invalid handle
        /// (<see cref="LiteRTHandle.IsInvalid"/>) on failure.
        /// </param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or another <see cref="StatusCode"/> on failure.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_speculative_decoding_config_create(
            out SpeculativeDecodingConfigHandle outConfig);

        /// <summary>
        /// Destroys a LiteRT LM Speculative Decoding Config.
        /// </summary>
        /// <param name="config">The config to destroy.</param>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern void litert_lm_speculative_decoding_config_delete(IntPtr config);

        /// <summary>
        /// Sets the speculative decoding mode. If unset, the session inherits the
        /// engine's mode, and the engine's mode defaults to <see cref="SpeculativeDecodingMode.Off"/>.
        /// <para>
        /// On the engine, <see cref="SpeculativeDecodingMode.On"/> and
        /// <see cref="SpeculativeDecodingMode.Adaptive"/> load the speculative decoding
        /// drafter (e.g. MTP) when the engine is created. On a session of an engine
        /// whose mode is <see cref="SpeculativeDecodingMode.Off"/>, they load the drafter
        /// lazily on the first request that needs it (CPU and GPU only; the NPU backend
        /// decodes without speculation).
        /// </para>
        /// </summary>
        /// <param name="config">The config to modify.</param>
        /// <param name="mode">The speculative decoding mode.</param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or <see cref="StatusCode.InvalidArgument"/> if
        /// <paramref name="config"/> or <paramref name="mode"/> is invalid. An invalid
        /// <paramref name="mode"/> leaves the config unchanged.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_speculative_decoding_config_set_mode(
            SpeculativeDecodingConfigHandle config, SpeculativeDecodingMode mode);

        /// <summary>
        /// Sets the speculative decoding config of this session, which overrides the
        /// engine's config (see <see cref="litert_lm_engine_settings_set_speculative_decoding_config"/>).
        /// Fields left unset in <paramref name="speculativeDecodingConfig"/> inherit the engine's
        /// values. The config is copied, so the caller keeps ownership of
        /// <paramref name="speculativeDecodingConfig"/>. Replaces any config set before, including
        /// the mode set by the deprecated
        /// <see cref="litert_lm_session_config_set_enable_speculative_decoding"/>.
        /// </summary>
        /// <param name="config">The config to modify.</param>
        /// <param name="speculativeDecodingConfig">The speculative decoding config.</param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or <see cref="StatusCode.InvalidArgument"/> if
        /// <paramref name="config"/> or <paramref name="speculativeDecodingConfig"/> is invalid.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_session_config_set_speculative_decoding_config(
            SessionConfigHandle config, SpeculativeDecodingConfigHandle speculativeDecodingConfig);

        /// <summary>
        /// Sets the sampler parameters for this session config.
        /// </summary>
        /// <param name="config">The config to modify.</param>
        /// <param name="samplerParams">The sampler parameters to use.</param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or <see cref="StatusCode.InvalidArgument"/> if
        /// <paramref name="config"/> or <paramref name="samplerParams"/> is invalid.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_session_config_set_sampler_params(
            SessionConfigHandle config, SamplerParamsHandle samplerParams);

        /// <summary>
        /// Destroys a LiteRT LM Session Config.
        /// </summary>
        /// <param name="config">The config to destroy.</param>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern void litert_lm_session_config_delete(IntPtr config);

        /// <summary>
        /// Sets the path to the LoRA weights file.
        /// </summary>
        /// <param name="config">The config to modify.</param>
        /// <param name="loraPath">The path to the text LoRA weights file.</param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or another <see cref="StatusCode"/> on failure.
        /// <see cref="StatusCode.InvalidArgument"/> if <paramref name="config"/> is invalid or
        /// <paramref name="loraPath"/> is <see langword="null"/> or empty; otherwise the code of
        /// the error encountered while opening the file.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_session_config_set_lora_path(
            SessionConfigHandle config, [MarshalAs(UnmanagedType.LPUTF8Str)] string loraPath);

        /// <summary>
        /// Sets the path to the Audio LoRA weights file.
        /// </summary>
        /// <param name="config">The config to modify.</param>
        /// <param name="audioLoraPath">The path to the audio LoRA weights file.</param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or another <see cref="StatusCode"/> on failure.
        /// <see cref="StatusCode.InvalidArgument"/> if <paramref name="config"/> is invalid or
        /// <paramref name="audioLoraPath"/> is <see langword="null"/> or empty; otherwise the code
        /// of the error encountered while opening the file.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_session_config_set_audio_lora_path(
            SessionConfigHandle config, [MarshalAs(UnmanagedType.LPUTF8Str)] string audioLoraPath);

        /// <summary>
        /// Creates a LiteRT LM Repetition Penalty Config with default values
        /// (<c>repetition_penalty</c> = 1.0f, <c>presence_penalty</c> = 0.0f,
        /// <c>frequency_penalty</c> = 0.0f, <c>window_size</c> = 0, which means all history with
        /// no penalties active).
        /// <para>
        /// When multiple penalties are configured and active, the order of application
        /// to output logits during decoding is:
        /// </para>
        /// <list type="number">
        /// <item><description>Multiplicative penalty (<c>repetition_penalty</c>)</description></item>
        /// <item><description>Subtractive penalties (<c>presence_penalty</c> and <c>frequency_penalty</c>)</description></item>
        /// </list>
        /// </summary>
        /// <param name="outConfig">
        /// On success, receives the created config, owned by the caller; release with
        /// <see cref="RepetitionPenaltyConfigHandle.Dispose"/>. Set to an invalid handle
        /// (<see cref="LiteRTHandle.IsInvalid"/>) on failure.
        /// </param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or another <see cref="StatusCode"/> on failure.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_repetition_penalty_config_create(
            out RepetitionPenaltyConfigHandle outConfig);

        /// <summary>
        /// Destroys a LiteRT LM Repetition Penalty Config.
        /// </summary>
        /// <param name="config">The config to destroy.</param>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern void litert_lm_repetition_penalty_config_delete(IntPtr config);

        /// <summary>
        /// Sets the multiplicative repetition penalty for the repetition penalty config.
        /// </summary>
        /// <param name="config">The config to modify.</param>
        /// <param name="repetitionPenalty">
        /// A multiplicative penalty applied to a token's logit if that token has appeared at least
        /// once inside the generated window history (e.g., 1.0 = no penalty, 1.2 = moderate penalty).
        /// Positive logits are divided by this parameter, and negative logits are multiplied
        /// (HuggingFace style). The parameter must be &gt;= 1.0f; values less than 1.0f are
        /// automatically clamped to 1.0f during execution.
        /// </param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or <see cref="StatusCode.InvalidArgument"/> if
        /// <paramref name="config"/> is invalid.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_repetition_penalty_config_set_repetition_penalty(
            RepetitionPenaltyConfigHandle config, float repetitionPenalty);

        /// <summary>
        /// Sets the subtractive presence penalty for the repetition penalty config.
        /// </summary>
        /// <param name="config">The config to modify.</param>
        /// <param name="presencePenalty">
        /// A scalar subtracted from a token's logit if that token has appeared at least once inside
        /// the generated window history. Positive values discourage repetition, while negative values
        /// reward repeating tokens (OpenAI style). Defaults to 0.0f.
        /// </param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or <see cref="StatusCode.InvalidArgument"/> if
        /// <paramref name="config"/> is invalid.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_repetition_penalty_config_set_presence_penalty(
            RepetitionPenaltyConfigHandle config, float presencePenalty);

        /// <summary>
        /// Sets the subtractive frequency penalty for the repetition penalty config.
        /// </summary>
        /// <param name="config">The config to modify.</param>
        /// <param name="frequencyPenalty">
        /// A scalar subtracted from a token's logit, scaled linearly by the number of times that
        /// token has previously appeared inside the generated window history. Positive values
        /// discourage repetition, while negative values reward repeating tokens (OpenAI style).
        /// Defaults to 0.0f.
        /// </param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or <see cref="StatusCode.InvalidArgument"/> if
        /// <paramref name="config"/> is invalid.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_repetition_penalty_config_set_frequency_penalty(
            RepetitionPenaltyConfigHandle config, float frequencyPenalty);

        /// <summary>
        /// Sets the window size for the repetition penalty config.
        /// </summary>
        /// <param name="config">The config to modify.</param>
        /// <param name="windowSize">
        /// The maximum number of recent tokens in generation history to consider when computing
        /// penalization. Tokens generated prior to this window are forgotten. A value of 0 means
        /// tracking all infinite generation history. Must be &gt;= 0; negative values are clamped
        /// to 0 during execution.
        /// </param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or <see cref="StatusCode.InvalidArgument"/> if
        /// <paramref name="config"/> is invalid.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_repetition_penalty_config_set_window_size(
            RepetitionPenaltyConfigHandle config, int windowSize);

        /// <summary>
        /// Creates a LiteRT LM No Repeat Ngram Config with default values
        /// (<c>no_repeat_ngram_size</c> = 0, <c>window_size</c> = 0, which means no repeat ngram
        /// banning is disabled).
        /// <para>
        /// When <c>no_repeat_ngram_size</c> is set greater than 0, any sequence of tokens (an
        /// ngram of that exact length) generated during decoding or present inside the
        /// window history can only occur at most once. If generating a candidate token
        /// would complete a repeating ngram, that candidate token's logit is set to -inf.
        /// </para>
        /// </summary>
        /// <param name="outConfig">
        /// On success, receives the created config, owned by the caller; release with
        /// <see cref="NoRepeatNgramConfigHandle.Dispose"/>. Set to an invalid handle
        /// (<see cref="LiteRTHandle.IsInvalid"/>) on failure.
        /// </param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or another <see cref="StatusCode"/> on failure.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_no_repeat_ngram_config_create(
            out NoRepeatNgramConfigHandle outConfig);

        /// <summary>
        /// Destroys a LiteRT LM No Repeat Ngram Config.
        /// </summary>
        /// <param name="config">The config to destroy.</param>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern void litert_lm_no_repeat_ngram_config_delete(IntPtr config);

        /// <summary>
        /// Sets the no repeat ngram size for the no repeat ngram config.
        /// </summary>
        /// <param name="config">The config to modify.</param>
        /// <param name="noRepeatNgramSize">
        /// The size of ngrams (consecutive token sequences) that are banned from repeating within
        /// the generation history window. If set &gt; 0, when generating the next token would complete
        /// an already observed <paramref name="noRepeatNgramSize"/> sequence, the logit of the
        /// candidate token is set to -inf. If set &lt;= 0, no repeat ngram banning is disabled.
        /// Negative values are automatically clamped to 0 during execution.
        /// </param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or <see cref="StatusCode.InvalidArgument"/> if
        /// <paramref name="config"/> is invalid.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_no_repeat_ngram_config_set_no_repeat_ngram_size(
            NoRepeatNgramConfigHandle config, int noRepeatNgramSize);

        /// <summary>
        /// Sets the window size for the no repeat ngram config.
        /// </summary>
        /// <param name="config">The config to modify.</param>
        /// <param name="windowSize">
        /// The maximum number of recent tokens in generation history to consider when checking for
        /// repeating ngrams. Tokens generated prior to this window are forgotten. A value of 0 means
        /// tracking all infinite generation history. Must be &gt;= 0; negative values are clamped to
        /// 0. If <paramref name="windowSize"/> is greater than 0 but less than
        /// <c>no_repeat_ngram_size</c>, it is automatically clamped to <c>no_repeat_ngram_size</c>
        /// so that the ngrams can fit and be tracked.
        /// </param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or <see cref="StatusCode.InvalidArgument"/> if
        /// <paramref name="config"/> is invalid.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_no_repeat_ngram_config_set_window_size(
            NoRepeatNgramConfigHandle config, int windowSize);

        /// <summary>
        /// Creates a LiteRT LM Suppress Tokens Config with default values (an empty set
        /// of suppressed tokens, which means token suppression is disabled).
        /// <para>
        /// When <c>suppress_tokens</c> is configured with one or more token IDs, the logits
        /// corresponding to those exact token IDs will be set directly to -inf during
        /// generation. This guarantees that those tokens can never be sampled by the model.
        /// </para>
        /// </summary>
        /// <param name="outConfig">
        /// On success, receives the created config, owned by the caller; release with
        /// <see cref="SuppressTokensConfigHandle.Dispose"/>. Set to an invalid handle
        /// (<see cref="LiteRTHandle.IsInvalid"/>) on failure.
        /// </param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or another <see cref="StatusCode"/> on failure.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_suppress_tokens_config_create(
            out SuppressTokensConfigHandle outConfig);

        /// <summary>
        /// Destroys a LiteRT LM Suppress Tokens Config.
        /// </summary>
        /// <param name="config">The config to destroy.</param>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern void litert_lm_suppress_tokens_config_delete(IntPtr config);

        /// <summary>
        /// Sets the list of token IDs to suppress for the suppress tokens config.
        /// </summary>
        /// <param name="config">The config to modify.</param>
        /// <param name="suppressTokens">
        /// An array of integer token IDs that should be banned from generation. During every decode
        /// step, each listed token ID's candidate logit will be forced to -inf. If
        /// <paramref name="numTokens"/> is 0, any previously set suppressed tokens are cleared and
        /// token suppression is disabled; in that case <paramref name="suppressTokens"/> may be
        /// <see langword="null"/>.
        /// </param>
        /// <param name="numTokens">
        /// The number of token IDs in the <paramref name="suppressTokens"/> array.
        /// </param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or <see cref="StatusCode.InvalidArgument"/> if
        /// <paramref name="config"/> is invalid, or if
        /// <paramref name="suppressTokens"/> is <see langword="null"/> while
        /// <paramref name="numTokens"/> is non-zero (the config is left unchanged).
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_suppress_tokens_config_set_suppress_tokens(
            SuppressTokensConfigHandle config, int[]? suppressTokens,
            UIntPtr numTokens);

        /// <summary>
        /// Sets the minimum log level for the LiteRT LM library.
        /// </summary>
        /// <param name="level">The minimum severity to log.</param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or <see cref="StatusCode.InvalidArgument"/> if
        /// <paramref name="level"/> is not a declared <see cref="LogSeverity"/> value.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_set_min_log_level(LogSeverity level);

        /// <summary>
        /// Creates a LiteRT LM Input Data.
        /// </summary>
        /// <param name="type">The type of the input data.</param>
        /// <param name="data">
        /// The data pointer. For <see cref="InputDataType.Text"/>, it is a UTF-8 string.
        /// For image/audio types, it is a pointer to the raw bytes. The data is copied internally.
        /// </param>
        /// <param name="size">The size of the data in bytes.</param>
        /// <param name="outInputData">
        /// On success, receives the created input data, owned by the caller; release with
        /// <see cref="InputDataHandle.Dispose"/>. Set to an invalid handle
        /// (<see cref="LiteRTHandle.IsInvalid"/>) on failure.
        /// </param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or another <see cref="StatusCode"/> on failure.
        /// <see cref="StatusCode.InvalidArgument"/> if <paramref name="data"/> is
        /// <see cref="IntPtr.Zero"/> while <paramref name="size"/> is non-zero, or
        /// <paramref name="type"/> is not a declared <see cref="InputDataType"/> value.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_input_data_create(
            InputDataType type, IntPtr data, UIntPtr size,
            out InputDataHandle outInputData);

        /// <summary>
        /// Destroys a LiteRT LM Input Data.
        /// </summary>
        /// <param name="inputData">The input data to destroy.</param>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern void litert_lm_input_data_delete(IntPtr inputData);

        /// <summary>
        /// Creates LiteRT LM Engine Settings.
        /// </summary>
        /// <param name="modelPath">The path to the model file.</param>
        /// <param name="backendStr">The backend to use (e.g., "cpu", "gpu").</param>
        /// <param name="visionBackendStr">
        /// The vision backend to use, or <see langword="null"/> if not set.
        /// </param>
        /// <param name="audioBackendStr">
        /// The audio backend to use, or <see langword="null"/> if not set.
        /// </param>
        /// <param name="outSettings">
        /// On success, receives the created settings, owned by the caller; release with
        /// <see cref="EngineSettingsHandle.Dispose"/>. Set to an invalid handle
        /// (<see cref="LiteRTHandle.IsInvalid"/>) on failure.
        /// </param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or another <see cref="StatusCode"/> on failure.
        /// <see cref="StatusCode.InvalidArgument"/> if <paramref name="modelPath"/> is
        /// <see langword="null"/> or a backend string is not recognized; otherwise the code of
        /// the error encountered while loading the model assets or building the settings.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_engine_settings_create(
            [MarshalAs(UnmanagedType.LPUTF8Str)] string modelPath,
            [MarshalAs(UnmanagedType.LPUTF8Str)] string backendStr,
            [MarshalAs(UnmanagedType.LPUTF8Str)] string? visionBackendStr,
            [MarshalAs(UnmanagedType.LPUTF8Str)] string? audioBackendStr,
            out EngineSettingsHandle outSettings);

        /// <summary>
        /// Creates LiteRT LM Engine Settings from a raw file descriptor. The engine
        /// takes ownership of the file descriptor and will close it when done. If
        /// <paramref name="fd"/> is negative, ownership is not taken.
        /// </summary>
        /// <param name="fd">The file descriptor of the model.</param>
        /// <param name="backendStr">The backend to use (e.g., "cpu", "gpu").</param>
        /// <param name="visionBackendStr">
        /// The vision backend to use, or <see langword="null"/> if not set.
        /// </param>
        /// <param name="audioBackendStr">
        /// The audio backend to use, or <see langword="null"/> if not set.
        /// </param>
        /// <param name="outSettings">
        /// On success, receives the created settings, owned by the caller; release with
        /// <see cref="EngineSettingsHandle.Dispose"/>. Set to an invalid handle
        /// (<see cref="LiteRTHandle.IsInvalid"/>) on failure.
        /// </param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or another <see cref="StatusCode"/> on failure.
        /// <see cref="StatusCode.InvalidArgument"/> if <paramref name="fd"/> is negative or a
        /// backend string is not recognized; otherwise the code of the error encountered while
        /// loading the model assets or building the settings.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_engine_settings_create_from_raw_file_descriptor(
            int fd, [MarshalAs(UnmanagedType.LPUTF8Str)] string backendStr,
            [MarshalAs(UnmanagedType.LPUTF8Str)] string? visionBackendStr,
            [MarshalAs(UnmanagedType.LPUTF8Str)] string? audioBackendStr,
            out EngineSettingsHandle outSettings);

        /// <summary>
        /// Destroys LiteRT LM Engine Settings.
        /// </summary>
        /// <param name="settings">The settings to destroy.</param>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern void litert_lm_engine_settings_delete(IntPtr settings);

        /// <summary>
        /// Sets the maximum number of tokens for the engine.
        /// </summary>
        /// <param name="settings">The engine settings.</param>
        /// <param name="maxNumTokens">The maximum number of tokens.</param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or <see cref="StatusCode.InvalidArgument"/> if
        /// <paramref name="settings"/> is invalid.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_engine_settings_set_max_num_tokens(
            EngineSettingsHandle settings, int maxNumTokens);

        /// <summary>
        /// Sets the number of threads for the CPU backend.
        /// </summary>
        /// <param name="settings">The engine settings.</param>
        /// <param name="numThreads">The number of threads.</param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or <see cref="StatusCode.InvalidArgument"/> if
        /// <paramref name="settings"/> is invalid. If the main backend is not CPU,
        /// this call has no effect and returns <see cref="StatusCode.Ok"/>.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_engine_settings_set_num_threads(
            EngineSettingsHandle settings, int numThreads);

        /// <summary>
        /// Sets the number of threads for the audio CPU backend.
        /// </summary>
        /// <param name="settings">The engine settings.</param>
        /// <param name="numThreads">The number of threads.</param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or <see cref="StatusCode.InvalidArgument"/> if
        /// <paramref name="settings"/> is invalid. If no audio backend is
        /// configured, this call has no effect and returns <see cref="StatusCode.Ok"/>.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_engine_settings_set_audio_num_threads(
            EngineSettingsHandle settings, int numThreads);

        /// <summary>
        /// Sets whether the engine should load different sections of the litertlm file
        /// in parallel. Defaults to <see langword="true"/>.
        /// </summary>
        /// <param name="settings">The engine settings.</param>
        /// <param name="parallelFileSectionLoading">Whether to load in parallel.</param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or <see cref="StatusCode.InvalidArgument"/> if
        /// <paramref name="settings"/> is invalid.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_engine_settings_set_parallel_file_section_loading(
            EngineSettingsHandle settings, [MarshalAs(UnmanagedType.I1)] bool parallelFileSectionLoading);

        /// <summary>
        /// Sets whether to enable single threaded execution.
        /// </summary>
        /// <param name="settings">The engine settings.</param>
        /// <param name="singleThreadedExecution">Whether to enable single threaded execution.</param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or <see cref="StatusCode.InvalidArgument"/> if
        /// <paramref name="settings"/> is invalid.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_engine_settings_set_single_threaded_execution(
            EngineSettingsHandle settings, [MarshalAs(UnmanagedType.I1)] bool singleThreadedExecution);

        /// <summary>
        /// Sets the maximum number of images for the engine.
        /// This is only used for the legacy implementation of the engine.
        /// </summary>
        /// <param name="settings">The engine settings.</param>
        /// <param name="maxNumImages">The maximum number of images.</param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or <see cref="StatusCode.InvalidArgument"/> if
        /// <paramref name="settings"/> is invalid.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_engine_settings_set_max_num_images(
            EngineSettingsHandle settings, int maxNumImages);

        /// <summary>
        /// Sets the maximum vision tokens generated per image for the engine.
        /// <para>
        /// When set, the engine automatically selects vision encoder and adapter
        /// signatures with capacity up to this limit and configures vision patch metadata.
        /// </para>
        /// </summary>
        /// <param name="settings">The engine settings.</param>
        /// <param name="maxVisionTokensPerImage">The maximum vision tokens per image.</param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or <see cref="StatusCode.InvalidArgument"/> if
        /// <paramref name="settings"/> is invalid.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_engine_settings_set_max_vision_tokens_per_image(
            EngineSettingsHandle settings, int maxVisionTokensPerImage);

        /// <summary>
        /// Sets the cache directory for the engine.
        /// </summary>
        /// <param name="settings">The engine settings.</param>
        /// <param name="cacheDir">The cache directory.</param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or <see cref="StatusCode.InvalidArgument"/> if
        /// <paramref name="settings"/> is invalid or <paramref name="cacheDir"/>
        /// is <see langword="null"/>.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_engine_settings_set_cache_dir(
            EngineSettingsHandle settings, [MarshalAs(UnmanagedType.LPUTF8Str)] string cacheDir);

        /// <summary>
        /// Sets the LiteRT dispatch library directory for NPU backend.
        /// </summary>
        /// <param name="settings">The engine settings.</param>
        /// <param name="libDir">The dispatch library directory.</param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or <see cref="StatusCode.InvalidArgument"/> if
        /// <paramref name="settings"/> is invalid or <paramref name="libDir"/> is
        /// <see langword="null"/>.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_engine_settings_set_litert_dispatch_lib_dir(
            EngineSettingsHandle settings, [MarshalAs(UnmanagedType.LPUTF8Str)] string libDir);

        /// <summary>
        /// Sets the activation data type.
        /// </summary>
        /// <param name="settings">The engine settings.</param>
        /// <param name="activationDataType">The activation data type.</param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or <see cref="StatusCode.InvalidArgument"/> if
        /// <paramref name="settings"/> is invalid or
        /// <paramref name="activationDataType"/> is not a declared
        /// <see cref="ActivationDataType"/> value.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_engine_settings_set_activation_data_type(
            EngineSettingsHandle settings, ActivationDataType activationDataType);

        /// <summary>
        /// Sets the prefill chunk size for the engine. Only applicable for CPU backend
        /// with dynamic models.
        /// </summary>
        /// <param name="settings">The engine settings.</param>
        /// <param name="prefillChunkSize">The prefill chunk size.</param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or <see cref="StatusCode.InvalidArgument"/> if
        /// <paramref name="settings"/> is invalid. If the main backend is not CPU,
        /// this call has no effect and returns <see cref="StatusCode.Ok"/>.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_engine_settings_set_prefill_chunk_size(
            EngineSettingsHandle settings, int prefillChunkSize);

        /// <summary>
        /// Sets whether YNNPACK should delegate supported operations before XNNPACK.
        /// </summary>
        /// <param name="settings">The engine settings.</param>
        /// <param name="enableYnnpack">Whether to enable YNNPACK.</param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or <see cref="StatusCode.InvalidArgument"/> if
        /// <paramref name="settings"/> is invalid. If the main backend is not CPU,
        /// this call has no effect and returns <see cref="StatusCode.Ok"/>.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_engine_settings_set_enable_ynnpack(
            EngineSettingsHandle settings, [MarshalAs(UnmanagedType.I1)] bool enableYnnpack);

        /// <summary>
        /// Enables benchmarking for the engine.
        /// </summary>
        /// <param name="settings">The engine settings.</param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or <see cref="StatusCode.InvalidArgument"/> if
        /// <paramref name="settings"/> is invalid.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_engine_settings_enable_benchmark(
            EngineSettingsHandle settings);

        /// <summary>
        /// Sets the number of prefill tokens for benchmarking.
        /// </summary>
        /// <param name="settings">The engine settings.</param>
        /// <param name="numPrefillTokens">The number of prefill tokens.</param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or <see cref="StatusCode.InvalidArgument"/> if
        /// <paramref name="settings"/> is invalid.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_engine_settings_set_num_prefill_tokens(
            EngineSettingsHandle settings, int numPrefillTokens);

        /// <summary>
        /// Sets the number of decode tokens for benchmarking.
        /// </summary>
        /// <param name="settings">The engine settings.</param>
        /// <param name="numDecodeTokens">The number of decode tokens.</param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or <see cref="StatusCode.InvalidArgument"/> if
        /// <paramref name="settings"/> is invalid.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_engine_settings_set_num_decode_tokens(
            EngineSettingsHandle settings, int numDecodeTokens);

        /// <summary>
        /// Sets whether to enable speculative decoding.
        /// </summary>
        /// <remarks>
        /// Deprecated: use <see cref="litert_lm_engine_settings_set_speculative_decoding_config"/>
        /// instead. Passing <see langword="true"/> is equivalent to a config whose mode is
        /// <see cref="SpeculativeDecodingMode.On"/> and passing <see langword="false"/> to
        /// <see cref="SpeculativeDecodingMode.Off"/>. This function and
        /// <see cref="litert_lm_engine_settings_set_speculative_decoding_config"/> write the same
        /// mode, so the last call wins.
        /// </remarks>
        /// <param name="settings">The engine settings.</param>
        /// <param name="enableSpeculativeDecoding">Whether to enable speculative decoding.</param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or <see cref="StatusCode.InvalidArgument"/> if
        /// <paramref name="settings"/> is invalid.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        [Obsolete("Use litert_lm_engine_settings_set_speculative_decoding_config instead.")]
        public static extern StatusCode litert_lm_engine_settings_set_enable_speculative_decoding(
            EngineSettingsHandle settings, [MarshalAs(UnmanagedType.I1)] bool enableSpeculativeDecoding);

        /// <summary>
        /// Sets the default speculative decoding config of the engine's sessions (see
        /// <see cref="litert_lm_speculative_decoding_config_create"/>). Each session can override
        /// any field with <see cref="litert_lm_session_config_set_speculative_decoding_config"/>.
        /// The config is copied, so the caller keeps ownership of
        /// <paramref name="speculativeDecodingConfig"/>. Replaces any config set before, including
        /// the mode set by the deprecated
        /// <see cref="litert_lm_engine_settings_set_enable_speculative_decoding"/>.
        /// </summary>
        /// <param name="settings">The engine settings.</param>
        /// <param name="speculativeDecodingConfig">The speculative decoding config.</param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or <see cref="StatusCode.InvalidArgument"/> if
        /// <paramref name="settings"/> or <paramref name="speculativeDecodingConfig"/> is invalid.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_engine_settings_set_speculative_decoding_config(
            EngineSettingsHandle settings, SpeculativeDecodingConfigHandle speculativeDecodingConfig);

        /// <summary>
        /// Sets the number of decode steps per sync for the GPU backend.
        /// </summary>
        /// <remarks>
        /// This setting is currently only supported for the Artisan GPU backend (Artisan).
        /// </remarks>
        /// <param name="settings">The engine settings.</param>
        /// <param name="numDecodeStepsPerSync">The number of decode steps per sync.</param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or <see cref="StatusCode.InvalidArgument"/> if
        /// <paramref name="settings"/> is invalid. If the main backend is not the
        /// Artisan GPU backend, this call has no effect and returns <see cref="StatusCode.Ok"/>.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_engine_settings_set_gpu_decode_steps_per_sync(
            EngineSettingsHandle settings, int numDecodeStepsPerSync);

        /// <summary>
        /// Sets whether to wait for weight uploads for the GPU backend.
        /// </summary>
        /// <remarks>
        /// This setting is currently only supported for the Artisan GPU backend.
        /// </remarks>
        /// <param name="settings">The engine settings.</param>
        /// <param name="waitForWeightUploads">Whether to wait for weight uploads.</param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or <see cref="StatusCode.InvalidArgument"/> if
        /// <paramref name="settings"/> is invalid. If the main backend is not the
        /// Artisan GPU backend, this call has no effect and returns <see cref="StatusCode.Ok"/>.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_engine_settings_set_gpu_wait_for_weight_uploads(
            EngineSettingsHandle settings, [MarshalAs(UnmanagedType.I1)] bool waitForWeightUploads);

        /// <summary>
        /// Sets whether to use ringbuffers for local attention KV cache.
        /// <para>
        /// When enabled for supported models, a ringbuffer stores only necessary KV
        /// cache memory for local attention layers, minimizing memory usage. When
        /// disabled, memory is allocated for the full context length, enabling instant
        /// rewinding at the cost of higher memory usage.
        /// </para>
        /// </summary>
        /// <remarks>
        /// This feature is backend-agnostic in interface design, but currently
        /// only supported by the GPU Artisan backend. Enabling it on unsupported models
        /// or backends will be ignored with a warning.
        /// </remarks>
        /// <param name="settings">The engine settings.</param>
        /// <param name="useRingbuffersLocalAttention">
        /// Whether to use ringbuffers for local attention.
        /// </param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or <see cref="StatusCode.InvalidArgument"/> if
        /// <paramref name="settings"/> is invalid. If the main backend is not the
        /// Artisan GPU backend, this call has no effect and returns <see cref="StatusCode.Ok"/>.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_engine_settings_set_use_ringbuffers_local_attention(
            EngineSettingsHandle settings, [MarshalAs(UnmanagedType.I1)] bool useRingbuffersLocalAttention);

        /// <summary>
        /// Sets the LoRA rank for the engine.
        /// </summary>
        /// <param name="settings">The engine settings.</param>
        /// <param name="loraRank">The LoRA rank.</param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or <see cref="StatusCode.InvalidArgument"/> if
        /// <paramref name="settings"/> is invalid.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_engine_settings_set_lora_rank(
            EngineSettingsHandle settings, int loraRank);

        /// <summary>
        /// Sets the supported LoRA ranks for the engine.
        /// </summary>
        /// <param name="settings">The engine settings.</param>
        /// <param name="loraRanks">An array of supported LoRA ranks.</param>
        /// <param name="numRanks">The number of ranks in the array.</param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or another <see cref="StatusCode"/> on failure.
        /// <see cref="StatusCode.InvalidArgument"/> if <paramref name="settings"/> is invalid,
        /// <paramref name="loraRanks"/> is <see langword="null"/>, or
        /// <paramref name="numRanks"/> is 0.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_engine_settings_set_supported_lora_ranks(
            EngineSettingsHandle settings, int[] loraRanks, UIntPtr numRanks);

        /// <summary>
        /// Sets the Audio LoRA rank for the engine.
        /// </summary>
        /// <param name="settings">The engine settings.</param>
        /// <param name="loraRank">The Audio LoRA rank.</param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or <see cref="StatusCode.InvalidArgument"/> if
        /// <paramref name="settings"/> is invalid. If no audio backend is
        /// configured, this call has no effect and returns <see cref="StatusCode.Ok"/>.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_engine_settings_set_audio_lora_rank(
            EngineSettingsHandle settings, int loraRank);

        /// <summary>
        /// Sets the supported Audio LoRA ranks for the engine.
        /// </summary>
        /// <param name="settings">The engine settings.</param>
        /// <param name="loraRanks">An array of supported Audio LoRA ranks.</param>
        /// <param name="numRanks">The number of ranks in the array.</param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or another <see cref="StatusCode"/> on failure.
        /// <see cref="StatusCode.InvalidArgument"/> if <paramref name="settings"/> is invalid,
        /// <paramref name="loraRanks"/> is <see langword="null"/>, or
        /// <paramref name="numRanks"/> is 0; <see cref="StatusCode.FailedPrecondition"/> if no
        /// audio executor is configured.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_engine_settings_set_supported_audio_lora_ranks(
            EngineSettingsHandle settings, int[] loraRanks, UIntPtr numRanks);

        /// <summary>
        /// Sets whether to enable Metal residency set on GPU.
        /// <para>
        /// When enabled on Apple platforms (macOS and iOS with Metal GPU backend), this
        /// uses Apple's MTLResidencySet API to ensure model weights and allocations
        /// remain resident in GPU memory, preventing memory swapping and reducing
        /// allocation overhead.
        /// </para>
        /// <para>
        /// This setting is only supported on Apple platforms (macOS / iOS) with the GPU
        /// backend. On other platforms (e.g. Linux, Android, Windows) or non-GPU
        /// backends, this setting has no effect and is safely ignored.
        /// </para>
        /// </summary>
        /// <param name="settings">The engine settings.</param>
        /// <param name="enableMetalResidencySet">Whether to enable Metal residency set.</param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or <see cref="StatusCode.InvalidArgument"/> if
        /// <paramref name="settings"/> is invalid. The setting is still recorded
        /// (and <see cref="StatusCode.Ok"/> returned) on platforms or backends where it has no effect.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_engine_settings_set_gpu_enable_metal_residency_set(
            EngineSettingsHandle settings, [MarshalAs(UnmanagedType.I1)] bool enableMetalResidencySet);

        /// <summary>
        /// Creates a LiteRT LM Engine from the given settings.
        /// </summary>
        /// <param name="settings">The engine settings.</param>
        /// <param name="outEngine">
        /// On success, receives the created engine, owned by the caller; release with
        /// <see cref="EngineHandle.Dispose"/>. Set to an invalid handle
        /// (<see cref="LiteRTHandle.IsInvalid"/>) on failure.
        /// </param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or another <see cref="StatusCode"/> on failure.
        /// <see cref="StatusCode.InvalidArgument"/> if <paramref name="settings"/> is invalid;
        /// otherwise the code of the error encountered while creating the engine (e.g. the model
        /// cannot be loaded).
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_engine_create(
            EngineSettingsHandle settings, out EngineHandle outEngine);

        /// <summary>
        /// Destroys a LiteRT LM Engine.
        /// <para>
        /// Every session and conversation created from <paramref name="engine"/> MUST be destroyed
        /// before calling this function; see the ownership and lifetime notes on <see cref="EngineH"/>.
        /// </para>
        /// </summary>
        /// <param name="engine">The engine to destroy.</param>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern void litert_lm_engine_delete(IntPtr engine);

        /// <summary>
        /// Creates a LiteRT LM Session.
        /// </summary>
        /// <param name="engine">The engine to create the session from.</param>
        /// <param name="config">
        /// The session config of the session. If <see cref="SessionConfigHandle.Null"/> (or invalid),
        /// use the default session config.
        /// </param>
        /// <param name="outSession">
        /// On success, receives the created session, owned by the caller; release with
        /// <see cref="SessionHandle.Dispose"/>. Set to an invalid handle
        /// (<see cref="LiteRTHandle.IsInvalid"/>) on failure.
        /// </param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or another <see cref="StatusCode"/> on failure.
        /// <see cref="StatusCode.InvalidArgument"/> if <paramref name="engine"/> is invalid;
        /// otherwise the code of the error encountered while creating the session.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_engine_create_session(
            EngineHandle engine, SessionConfigHandle config, out SessionHandle outSession);

        /// <summary>
        /// Destroys a LiteRT LM Session.
        /// </summary>
        /// <param name="session">The session to destroy.</param>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern void litert_lm_session_delete(IntPtr session);

        /// <summary>
        /// Clones a LiteRT LM Session, copying its state so the new session can be used independently.
        /// </summary>
        /// <param name="session">The session to clone.</param>
        /// <param name="outSession">
        /// On success, receives the cloned session, owned by the caller; release with
        /// <see cref="SessionHandle.Dispose"/>. Set to an invalid handle
        /// (<see cref="LiteRTHandle.IsInvalid"/>) on failure.
        /// </param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or another <see cref="StatusCode"/> on failure.
        /// <see cref="StatusCode.InvalidArgument"/> if <paramref name="session"/> is invalid.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_session_clone(
            SessionHandle session, out SessionHandle outSession);

        /// <summary>
        /// Cancels the current processing in the session.
        /// <para>
        /// This function only requests cancellation and does not wait for it: the
        /// stream callback can still run after this function returns. The session MUST
        /// NOT be reused afterwards; only delete it. See the threading model notes on
        /// <see cref="EngineH"/>.
        /// </para>
        /// </summary>
        /// <param name="session">The session to cancel processing on.</param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or <see cref="StatusCode.InvalidArgument"/> if
        /// <paramref name="session"/> is invalid.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_session_cancel_process(SessionHandle session);

        /// <summary>
        /// Saves the current state of the session to a checkpoint with the given label.
        /// </summary>
        /// <param name="session">The session to save checkpoint for.</param>
        /// <param name="label">Label for the checkpoint.</param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or another <see cref="StatusCode"/> on failure.
        /// <see cref="StatusCode.InvalidArgument"/> if <paramref name="session"/> is invalid or
        /// <paramref name="label"/> is <see langword="null"/>.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_session_save_checkpoint(
            SessionHandle session, [MarshalAs(UnmanagedType.LPUTF8Str)] string label);

        /// <summary>
        /// Rewinds the session to the given checkpoint label.
        /// </summary>
        /// <param name="session">The session to rewind.</param>
        /// <param name="label">Label of the checkpoint to rewind to.</param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or another <see cref="StatusCode"/> on failure.
        /// <see cref="StatusCode.InvalidArgument"/> if <paramref name="session"/> is invalid or
        /// <paramref name="label"/> is <see langword="null"/>; otherwise the runtime error
        /// (e.g. if no checkpoint with <paramref name="label"/> exists).
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_session_rewind_to_checkpoint(
            SessionHandle session, [MarshalAs(UnmanagedType.LPUTF8Str)] string label);

        /// <summary>
        /// Rewinds the session to a specific step number.
        /// </summary>
        /// <param name="session">The session to rewind.</param>
        /// <param name="step">The step number to rewind to.</param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or another <see cref="StatusCode"/> on failure.
        /// <see cref="StatusCode.InvalidArgument"/> if <paramref name="session"/> is invalid;
        /// otherwise the runtime error (e.g. if <paramref name="step"/> is invalid).
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_session_rewind_to_step(
            SessionHandle session, int step);

        /// <summary>
        /// Adds the input prompt/query to the model for starting the prefilling
        /// process. This is a blocking call and the function will return when the
        /// prefill process is done.
        /// </summary>
        /// <param name="session">The session to use.</param>
        /// <param name="inputs">
        /// An array of <c>InputData</c> structs representing the multimodal input.
        /// </param>
        /// <param name="numInputs">
        /// The number of <c>InputData</c> structs in the array.
        /// </param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or another <see cref="StatusCode"/> on failure.
        /// <see cref="StatusCode.InvalidArgument"/> if <paramref name="session"/> is invalid,
        /// <paramref name="inputs"/> is <see langword="null"/>, or
        /// <paramref name="numInputs"/> is 0.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_session_run_prefill(
            SessionHandle session, IntPtr[] inputs, UIntPtr numInputs); // GC.KeepAlive the InputDataHandle[]

        /// <summary>
        /// Starts the decoding process for the model to predict the response based
        /// on the input prompt/query added after using <see cref="litert_lm_session_run_prefill"/>.
        /// This is a blocking call and the function will return when the decoding
        /// process is done.
        /// </summary>
        /// <param name="session">The session to use.</param>
        /// <param name="outResponses">
        /// On success, receives the responses, owned by the caller; release with
        /// <see cref="ResponsesHandle.Dispose"/>. Set to an invalid handle
        /// (<see cref="LiteRTHandle.IsInvalid"/>) on failure.
        /// </param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or another <see cref="StatusCode"/> on failure.
        /// <see cref="StatusCode.InvalidArgument"/> if <paramref name="session"/> is invalid;
        /// otherwise the runtime error.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_session_run_decode(
            SessionHandle session, out ResponsesHandle outResponses);

        /// <summary>
        /// Scores the target text after the prefill process is done.
        /// </summary>
        /// <param name="session">The session to use.</param>
        /// <param name="targetText">An array of target text strings to score.</param>
        /// <param name="numTargets">
        /// The number of strings in the <paramref name="targetText"/> array.
        /// </param>
        /// <param name="storeTokenLengths">
        /// Whether to store the token lengths of the target texts in the responses.
        /// </param>
        /// <param name="outResponses">
        /// On success, receives the responses, owned by the caller; release with
        /// <see cref="ResponsesHandle.Dispose"/>. Set to an invalid handle
        /// (<see cref="LiteRTHandle.IsInvalid"/>) on failure.
        /// </param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or another <see cref="StatusCode"/> on failure.
        /// <see cref="StatusCode.InvalidArgument"/> if <paramref name="session"/> is invalid,
        /// <paramref name="targetText"/> is <see langword="null"/>, or
        /// <paramref name="numTargets"/> is 0; otherwise the runtime error.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_session_run_text_scoring(
            SessionHandle session, IntPtr[] targetText, UIntPtr numTargets,
            [MarshalAs(UnmanagedType.I1)] bool storeTokenLengths, out ResponsesHandle outResponses);

        /// <summary>
        /// Generates content from the input prompt.
        /// </summary>
        /// <param name="session">The session to use for generation.</param>
        /// <param name="inputs">
        /// An array of <c>LiteRtLmInputData</c> structs representing the multimodal input.
        /// </param>
        /// <param name="numInputs">
        /// The number of <c>LiteRtLmInputData</c> structs in the array.
        /// </param>
        /// <param name="outResponses">
        /// On success, receives the responses, owned by the caller; release with
        /// <see cref="ResponsesHandle.Dispose"/>. Set to an invalid handle
        /// (<see cref="LiteRTHandle.IsInvalid"/>) on failure.
        /// </param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or another <see cref="StatusCode"/> on failure.
        /// <see cref="StatusCode.InvalidArgument"/> if <paramref name="session"/> is invalid;
        /// otherwise the code of the error encountered while copying the inputs or generating content.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_session_generate_content(
            SessionHandle session, IntPtr[] inputs, UIntPtr numInputs, out ResponsesHandle outResponses); // GC.KeepAlive the InputDataHandle[]

        /// <summary>
        /// Destroys a LiteRT LM Responses object.
        /// </summary>
        /// <param name="responses">The responses to destroy.</param>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern void litert_lm_responses_delete(IntPtr responses);

        /// <summary>
        /// Returns the number of response candidates.
        /// <para>
        /// The number of candidates is the number of response texts or, if there are
        /// none (e.g. for text scoring), the number of scores or token lengths. Valid
        /// candidate indices for the <c>litert_lm_responses_*_at</c> functions are
        /// <c>[0, outNumCandidates)</c>, although individual values may still be absent
        /// at a valid index (see the <c>has_*_at</c> predicates).
        /// </para>
        /// </summary>
        /// <param name="responses">The responses object.</param>
        /// <param name="outNumCandidates">
        /// On success, receives the number of candidates; not written on failure.
        /// </param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or another <see cref="StatusCode"/> on failure.
        /// <see cref="StatusCode.InvalidArgument"/> if <paramref name="responses"/> is invalid.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_responses_get_num_candidates(
            ResponsesHandle responses, out UIntPtr outNumCandidates);

        /// <summary>
        /// Returns the response text at a given index.
        /// </summary>
        /// <param name="responses">The responses object.</param>
        /// <param name="index">The index of the response.</param>
        /// <param name="outText">
        /// On success, receives the response text. The string is owned by the
        /// <paramref name="responses"/> object and is valid only for its lifetime.
        /// Set to <see cref="IntPtr.Zero"/> on failure.
        /// </param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or another <see cref="StatusCode"/> on failure.
        /// <see cref="StatusCode.InvalidArgument"/> if <paramref name="responses"/> is invalid;
        /// <see cref="StatusCode.OutOfRange"/> if <paramref name="index"/> is out of bounds of
        /// the candidates; <see cref="StatusCode.NotFound"/> if there is no response text at
        /// <paramref name="index"/>.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_responses_get_response_text_at(
            ResponsesHandle responses, UIntPtr index, out IntPtr outText);

        /// <summary>
        /// Returns whether the response contains a score at the given index.
        /// </summary>
        /// <param name="responses">The responses object.</param>
        /// <param name="index">The index of the response.</param>
        /// <param name="outHasScore">
        /// On success, receives <see langword="true"/> if a score is available at
        /// <paramref name="index"/>, <see langword="false"/> otherwise; not written on failure.
        /// </param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or another <see cref="StatusCode"/> on failure.
        /// <see cref="StatusCode.InvalidArgument"/> if <paramref name="responses"/> is invalid;
        /// <see cref="StatusCode.OutOfRange"/> if <paramref name="index"/> is out of bounds of
        /// the candidates.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_responses_has_score_at(
            ResponsesHandle responses, UIntPtr index, [MarshalAs(UnmanagedType.I1)] out bool outHasScore);

        /// <summary>
        /// Returns the score at a given index.
        /// </summary>
        /// <param name="responses">The responses object.</param>
        /// <param name="index">The index of the response.</param>
        /// <param name="outScore">
        /// On success, receives the score; not written on failure.
        /// </param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or another <see cref="StatusCode"/> on failure.
        /// <see cref="StatusCode.InvalidArgument"/> if <paramref name="responses"/> is invalid;
        /// <see cref="StatusCode.OutOfRange"/> if <paramref name="index"/> is out of bounds of
        /// the candidates; <see cref="StatusCode.NotFound"/> if no score is available at
        /// <paramref name="index"/>.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_responses_get_score_at(
            ResponsesHandle responses, UIntPtr index, out float outScore);

        /// <summary>
        /// Returns whether the response contains a token length at the given index.
        /// </summary>
        /// <param name="responses">The responses object.</param>
        /// <param name="index">The index of the response.</param>
        /// <param name="outHasTokenLength">
        /// On success, receives <see langword="true"/> if a token length is available at
        /// <paramref name="index"/>, <see langword="false"/> otherwise; not written on failure.
        /// </param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or another <see cref="StatusCode"/> on failure.
        /// <see cref="StatusCode.InvalidArgument"/> if <paramref name="responses"/> is invalid;
        /// <see cref="StatusCode.OutOfRange"/> if <paramref name="index"/> is out of bounds of
        /// the candidates.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_responses_has_token_length_at(
            ResponsesHandle responses, UIntPtr index, [MarshalAs(UnmanagedType.I1)] out bool outHasTokenLength);

        /// <summary>
        /// Returns the token length at a given index.
        /// </summary>
        /// <param name="responses">The responses object.</param>
        /// <param name="index">The index of the response.</param>
        /// <param name="outTokenLength">
        /// On success, receives the token length; not written on failure.
        /// </param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or another <see cref="StatusCode"/> on failure.
        /// <see cref="StatusCode.InvalidArgument"/> if <paramref name="responses"/> is invalid;
        /// <see cref="StatusCode.OutOfRange"/> if <paramref name="index"/> is out of bounds of
        /// the candidates; <see cref="StatusCode.NotFound"/> if no token length is available at
        /// <paramref name="index"/>.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_responses_get_token_length_at(
            ResponsesHandle responses, UIntPtr index, out UIntPtr outTokenLength);

        /// <summary>
        /// Returns whether the response contains token scores at the given index.
        /// </summary>
        /// <param name="responses">The responses object.</param>
        /// <param name="index">The index of the response.</param>
        /// <param name="outHasTokenScores">
        /// On success, receives <see langword="true"/> if token scores are available at
        /// <paramref name="index"/>, <see langword="false"/> otherwise; not written on failure.
        /// </param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or another <see cref="StatusCode"/> on failure.
        /// <see cref="StatusCode.InvalidArgument"/> if <paramref name="responses"/> is invalid;
        /// <see cref="StatusCode.OutOfRange"/> if <paramref name="index"/> is out of bounds of
        /// the candidates.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_responses_has_token_scores_at(
            ResponsesHandle responses, UIntPtr index, [MarshalAs(UnmanagedType.I1)] out bool outHasTokenScores);

        /// <summary>
        /// Returns the number of tokens for which scores are present at a given index.
        /// </summary>
        /// <param name="responses">The responses object.</param>
        /// <param name="index">The index of the response.</param>
        /// <param name="outNumTokenScores">
        /// On success, receives the number of token scores; not written on failure.
        /// </param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or another <see cref="StatusCode"/> on failure.
        /// <see cref="StatusCode.InvalidArgument"/> if <paramref name="responses"/> is invalid;
        /// <see cref="StatusCode.OutOfRange"/> if <paramref name="index"/> is out of bounds of
        /// the candidates; <see cref="StatusCode.NotFound"/> if no token scores are available at
        /// <paramref name="index"/>.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_responses_get_num_token_scores_at(
            ResponsesHandle responses, UIntPtr index, out UIntPtr outNumTokenScores);

        /// <summary>
        /// Returns the token scores at a given index.
        /// </summary>
        /// <param name="responses">The responses object.</param>
        /// <param name="index">The index of the response.</param>
        /// <param name="outTokenScores">
        /// On success, receives a pointer to the internal array of token scores, whose length is
        /// given by <see cref="litert_lm_responses_get_num_token_scores_at"/>. The array is owned
        /// by the <paramref name="responses"/> object and is valid only for its lifetime.
        /// Set to <see cref="IntPtr.Zero"/> on failure.
        /// </param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or another <see cref="StatusCode"/> on failure.
        /// <see cref="StatusCode.InvalidArgument"/> if <paramref name="responses"/> is invalid;
        /// <see cref="StatusCode.OutOfRange"/> if <paramref name="index"/> is out of bounds of
        /// the candidates; <see cref="StatusCode.NotFound"/> if no token scores are available at
        /// <paramref name="index"/>.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_responses_get_token_scores_at(
            ResponsesHandle responses, UIntPtr index, out IntPtr outTokenScores);

        /// <summary>
        /// Retrieves the benchmark information from the session.
        /// </summary>
        /// <param name="session">The session to get the benchmark info from.</param>
        /// <param name="outBenchmarkInfo">
        /// On success, receives the benchmark info, owned by the caller; release with
        /// <see cref="BenchmarkInfoHandle.Dispose"/>. Set to an invalid handle
        /// (<see cref="LiteRTHandle.IsInvalid"/>) on failure.
        /// </param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or another <see cref="StatusCode"/> on failure.
        /// <see cref="StatusCode.InvalidArgument"/> if <paramref name="session"/> is invalid;
        /// otherwise the runtime error (e.g. if benchmarking is not enabled).
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_session_get_benchmark_info(
            SessionHandle session, out BenchmarkInfoHandle outBenchmarkInfo);

        /// <summary>
        /// Destroys a LiteRT LM Benchmark Info object.
        /// </summary>
        /// <param name="benchmarkInfo">The benchmark info to destroy.</param>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern void litert_lm_benchmark_info_delete(IntPtr benchmarkInfo);

        /// <summary>
        /// Returns the time to the first token in seconds.
        /// <para>
        /// Note that the first time to token doesn't include the time for
        /// initialization. It is the sum of the prefill time for the first turn and
        /// the time spent for decoding the first token.
        /// </para>
        /// </summary>
        /// <param name="benchmarkInfo">The benchmark info object.</param>
        /// <param name="outSeconds">
        /// On success, receives the time to the first token in seconds; not written on failure.
        /// </param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or another <see cref="StatusCode"/> on failure.
        /// <see cref="StatusCode.InvalidArgument"/> if <paramref name="benchmarkInfo"/> is invalid.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_benchmark_info_get_time_to_first_token(
            BenchmarkInfoHandle benchmarkInfo, out double outSeconds);

        /// <summary>
        /// Returns the total initialization time in seconds.
        /// </summary>
        /// <param name="benchmarkInfo">The benchmark info object.</param>
        /// <param name="outSeconds">
        /// On success, receives the total initialization time in seconds; not written on failure.
        /// </param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or another <see cref="StatusCode"/> on failure.
        /// <see cref="StatusCode.InvalidArgument"/> if <paramref name="benchmarkInfo"/> is invalid.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_benchmark_info_get_total_init_time_in_second(
            BenchmarkInfoHandle benchmarkInfo, out double outSeconds);

        /// <summary>
        /// Returns the number of prefill turns.
        /// </summary>
        /// <param name="benchmarkInfo">The benchmark info object.</param>
        /// <param name="outNumTurns">
        /// On success, receives the number of prefill turns; not written on failure.
        /// </param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or another <see cref="StatusCode"/> on failure.
        /// <see cref="StatusCode.InvalidArgument"/> if <paramref name="benchmarkInfo"/> is invalid.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_benchmark_info_get_num_prefill_turns(
            BenchmarkInfoHandle benchmarkInfo, out UIntPtr outNumTurns);

        /// <summary>
        /// Returns the number of decode turns.
        /// </summary>
        /// <param name="benchmarkInfo">The benchmark info object.</param>
        /// <param name="outNumTurns">
        /// On success, receives the number of decode turns; not written on failure.
        /// </param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or another <see cref="StatusCode"/> on failure.
        /// <see cref="StatusCode.InvalidArgument"/> if <paramref name="benchmarkInfo"/> is invalid.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_benchmark_info_get_num_decode_turns(
            BenchmarkInfoHandle benchmarkInfo, out UIntPtr outNumTurns);

        /// <summary>
        /// Returns the prefill token count at a given turn index.
        /// </summary>
        /// <param name="benchmarkInfo">The benchmark info object.</param>
        /// <param name="index">The index of the prefill turn.</param>
        /// <param name="outTokenCount">
        /// On success, receives the prefill token count; not written on failure.
        /// </param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or another <see cref="StatusCode"/> on failure.
        /// <see cref="StatusCode.InvalidArgument"/> if <paramref name="benchmarkInfo"/> is invalid;
        /// <see cref="StatusCode.OutOfRange"/> if <paramref name="index"/> is out of bounds of
        /// the prefill turns.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_benchmark_info_get_prefill_token_count_at(
            BenchmarkInfoHandle benchmarkInfo, UIntPtr index, out UIntPtr outTokenCount);

        /// <summary>
        /// Returns the decode token count at a given turn index.
        /// </summary>
        /// <param name="benchmarkInfo">The benchmark info object.</param>
        /// <param name="index">The index of the decode turn.</param>
        /// <param name="outTokenCount">
        /// On success, receives the decode token count; not written on failure.
        /// </param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or another <see cref="StatusCode"/> on failure.
        /// <see cref="StatusCode.InvalidArgument"/> if <paramref name="benchmarkInfo"/> is invalid;
        /// <see cref="StatusCode.OutOfRange"/> if <paramref name="index"/> is out of bounds of
        /// the decode turns.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_benchmark_info_get_decode_token_count_at(
            BenchmarkInfoHandle benchmarkInfo, UIntPtr index, out UIntPtr outTokenCount);

        /// <summary>
        /// Returns the prefill tokens per second at a given turn index.
        /// </summary>
        /// <param name="benchmarkInfo">The benchmark info object.</param>
        /// <param name="index">The index of the prefill turn.</param>
        /// <param name="outTokensPerSec">
        /// On success, receives the prefill tokens per second (0 if the turn has no measurable
        /// duration); not written on failure.
        /// </param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or another <see cref="StatusCode"/> on failure.
        /// <see cref="StatusCode.InvalidArgument"/> if <paramref name="benchmarkInfo"/> is invalid;
        /// <see cref="StatusCode.OutOfRange"/> if <paramref name="index"/> is out of bounds of
        /// the prefill turns.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_benchmark_info_get_prefill_tokens_per_sec_at(
            BenchmarkInfoHandle benchmarkInfo, UIntPtr index, out double outTokensPerSec);

        /// <summary>
        /// Returns the decode tokens per second at a given turn index.
        /// </summary>
        /// <param name="benchmarkInfo">The benchmark info object.</param>
        /// <param name="index">The index of the decode turn.</param>
        /// <param name="outTokensPerSec">
        /// On success, receives the decode tokens per second (0 if the turn has no measurable
        /// duration); not written on failure.
        /// </param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or another <see cref="StatusCode"/> on failure.
        /// <see cref="StatusCode.InvalidArgument"/> if <paramref name="benchmarkInfo"/> is invalid;
        /// <see cref="StatusCode.OutOfRange"/> if <paramref name="index"/> is out of bounds of
        /// the decode turns.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_benchmark_info_get_decode_tokens_per_sec_at(
            BenchmarkInfoHandle benchmarkInfo, UIntPtr index, out double outTokensPerSec);

        /// <summary>
        /// Returns the max number of tokens, i.e. the KV cache size, that the engine used.
        /// <para>
        /// It can differ from the value requested with
        /// <see cref="litert_lm_engine_settings_set_max_num_tokens"/>, e.g. for a model whose KV
        /// cache size is fixed when it is compiled, or when no value was requested.
        /// </para>
        /// </summary>
        /// <param name="benchmarkInfo">The benchmark info object.</param>
        /// <param name="outMaxNumTokens">
        /// On success, receives the max number of tokens, or 0 if the engine didn't report it
        /// (e.g. before the first prefill); not written on failure.
        /// </param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or another <see cref="StatusCode"/> on failure.
        /// <see cref="StatusCode.InvalidArgument"/> if <paramref name="benchmarkInfo"/> is invalid.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_benchmark_info_get_max_num_tokens(
            BenchmarkInfoHandle benchmarkInfo, out int outMaxNumTokens);

        /// <summary>
        /// Gets the text content of the chunk.
        /// </summary>
        /// <param name="chunk">The stream chunk.</param>
        /// <param name="outText">
        /// On success, receives the text content of the chunk, or <see cref="IntPtr.Zero"/> if the
        /// chunk has no text content (e.g. if it is an error or final metadata-only chunk). The string
        /// is owned by the chunk and is only valid as long as the chunk is valid. Set to
        /// <see cref="IntPtr.Zero"/> on failure.
        /// </param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success (including when the chunk has no text),
        /// or another <see cref="StatusCode"/> on failure. <see cref="StatusCode.InvalidArgument"/>
        /// if <paramref name="chunk"/> is invalid.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_stream_chunk_get_text(
            StreamChunkHandle chunk, out IntPtr outText);

        /// <summary>
        /// Returns whether this is the final chunk of the stream.
        /// </summary>
        /// <param name="chunk">The stream chunk.</param>
        /// <param name="outIsFinal">
        /// On success, receives <see langword="true"/> if this is the final chunk of the stream,
        /// <see langword="false"/> otherwise; not written on failure.
        /// </param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or another <see cref="StatusCode"/> on failure.
        /// <see cref="StatusCode.InvalidArgument"/> if <paramref name="chunk"/> is invalid.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_stream_chunk_is_final(
            StreamChunkHandle chunk, [MarshalAs(UnmanagedType.I1)] out bool outIsFinal);

        /// <summary>
        /// Gets the error message associated with this chunk, if any.
        /// </summary>
        /// <param name="chunk">The stream chunk.</param>
        /// <param name="outError">
        /// On success, receives the error message of the chunk, or <see cref="IntPtr.Zero"/> if the
        /// chunk carries no error. The string is owned by the chunk and is only valid as long as the
        /// chunk is valid. Set to <see cref="IntPtr.Zero"/> on failure.
        /// </param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success (including when the chunk carries no error),
        /// or another <see cref="StatusCode"/> on failure. <see cref="StatusCode.InvalidArgument"/>
        /// if <paramref name="chunk"/> is invalid.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_stream_chunk_get_error(
            StreamChunkHandle chunk, out IntPtr outError);

        /// <summary>
        /// Callback for streaming responses.
        /// </summary>
        /// <param name="callbackData">
        /// A pointer to user-defined data passed to the stream function.
        /// </param>
        /// <param name="chunk">
        /// A pointer to the stream chunk object. It is only valid for the duration of the call.
        /// </param>
        /// <remarks>
        /// See the threading model notes on <see cref="EngineH"/> for where the callback runs
        /// and which functions it must not call.
        /// </remarks>
        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void StreamCallback(IntPtr callbackData, IntPtr chunk); // Adopt as StreamChunkHandle

        /// <summary>
        /// Starts the decoding process for the model to predict the response based
        /// on the input prompt/query added after using <see cref="litert_lm_session_run_prefill"/>.
        /// This is a non-blocking call that will stream responses via a callback.
        /// </summary>
        /// <param name="session">The session to use.</param>
        /// <param name="callback">The callback function pointer to receive response chunks.</param>
        /// <param name="callbackData">
        /// A pointer to user data that will be passed to the callback.
        /// </param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or another <see cref="StatusCode"/> on failure.
        /// <see cref="StatusCode.InvalidArgument"/> if <paramref name="session"/> is invalid or
        /// <paramref name="callback"/> is <see cref="IntPtr.Zero"/>; otherwise the code of the
        /// error that prevented the stream from starting. Errors that occur after the stream has
        /// started are reported through the callback.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_session_run_decode_async(
            SessionHandle session, IntPtr /*EngineH.StreamCallback*/ callback, IntPtr callbackData);

        /// <summary>
        /// Generates content from the input prompt and streams the response via a
        /// callback. This is a non-blocking call that will invoke the callback from a
        /// background thread for each chunk (see the threading model notes on <see cref="EngineH"/>).
        /// </summary>
        /// <param name="session">The session to use for generation.</param>
        /// <param name="inputs">
        /// An array of <c>LiteRtLmInputData</c> structs representing the multimodal input.
        /// </param>
        /// <param name="numInputs">
        /// The number of <c>LiteRtLmInputData</c> structs in the array.
        /// </param>
        /// <param name="callback">The callback function pointer to receive response chunks.</param>
        /// <param name="callbackData">
        /// A pointer to user data that will be passed to the callback.
        /// </param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or another <see cref="StatusCode"/> on failure.
        /// <see cref="StatusCode.InvalidArgument"/> if <paramref name="session"/> is invalid or
        /// <paramref name="callback"/> is <see cref="IntPtr.Zero"/>; otherwise the code of the
        /// error that prevented the stream from starting. Errors that occur after the stream has
        /// started are reported through the callback.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_session_generate_content_stream(
            SessionHandle session, IntPtr[] inputs, UIntPtr numInputs, // GC.KeepAlive the InputDataHandle[]
            IntPtr /*EngineH.StreamCallback*/ callback, IntPtr callbackData);

        /// <summary>
        /// Tokenizes text using the engine's tokenizer.
        /// </summary>
        /// <param name="engine">The engine instance.</param>
        /// <param name="text">The UTF-8 string to tokenize.</param>
        /// <param name="outResult">
        /// On success, receives the tokenize result, owned by the caller; release with
        /// <see cref="TokenizeResultHandle.Dispose"/>. Set to an invalid handle
        /// (<see cref="LiteRTHandle.IsInvalid"/>) on failure.
        /// </param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or another <see cref="StatusCode"/> on failure.
        /// <see cref="StatusCode.InvalidArgument"/> if <paramref name="engine"/> is invalid or
        /// <paramref name="text"/> is <see langword="null"/>; otherwise the code of the tokenizer error.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_engine_tokenize(
            EngineHandle engine, [MarshalAs(UnmanagedType.LPUTF8Str)] string text,
            out TokenizeResultHandle outResult);

        /// <summary>
        /// Destroys a LiteRT LM Tokenize Result.
        /// </summary>
        /// <param name="result">The tokenize result to destroy.</param>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern void litert_lm_tokenize_result_delete(IntPtr result);

        /// <summary>
        /// Returns the token ids from a tokenize result.
        /// </summary>
        /// <param name="result">The tokenize result.</param>
        /// <param name="outTokens">
        /// On success, receives a pointer to the internal array of token ids, whose length is given
        /// by <see cref="litert_lm_tokenize_result_get_num_tokens"/>. The pointer is valid only for
        /// the lifetime of the <paramref name="result"/> object, and may be <see cref="IntPtr.Zero"/>
        /// if the result holds no tokens. Set to <see cref="IntPtr.Zero"/> on failure.
        /// </param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or another <see cref="StatusCode"/> on failure.
        /// <see cref="StatusCode.InvalidArgument"/> if <paramref name="result"/> is invalid.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_tokenize_result_get_tokens(
            TokenizeResultHandle result, out IntPtr outTokens);

        /// <summary>
        /// Returns the number of token ids from a tokenize result.
        /// </summary>
        /// <param name="result">The tokenize result.</param>
        /// <param name="outNumTokens">
        /// On success, receives the number of token ids; not written on failure.
        /// </param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or another <see cref="StatusCode"/> on failure.
        /// <see cref="StatusCode.InvalidArgument"/> if <paramref name="result"/> is invalid.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_tokenize_result_get_num_tokens(
            TokenizeResultHandle result, out UIntPtr outNumTokens);

        /// <summary>
        /// Detokenizes token ids using the engine's tokenizer.
        /// </summary>
        /// <param name="engine">The engine instance.</param>
        /// <param name="tokens">An array of token ids to detokenize.</param>
        /// <param name="numTokens">The number of token ids in the array.</param>
        /// <param name="outResult">
        /// On success, receives the detokenize result, owned by the caller; release with
        /// <see cref="DetokenizeResultHandle.Dispose"/>. Set to an invalid handle
        /// (<see cref="LiteRTHandle.IsInvalid"/>) on failure.
        /// </param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or another <see cref="StatusCode"/> on failure.
        /// <see cref="StatusCode.InvalidArgument"/> if <paramref name="engine"/> is invalid or
        /// <paramref name="tokens"/> is <see langword="null"/>; otherwise the code of the tokenizer error.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_engine_detokenize(
            EngineHandle engine, int[] tokens, UIntPtr numTokens,
            out DetokenizeResultHandle outResult);

        /// <summary>
        /// Destroys a LiteRT LM Detokenize Result.
        /// </summary>
        /// <param name="result">The detokenize result to destroy.</param>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern void litert_lm_detokenize_result_delete(IntPtr result);

        /// <summary>
        /// Returns the string from a detokenize result.
        /// </summary>
        /// <param name="result">The detokenize result.</param>
        /// <param name="outText">
        /// On success, receives the detokenized UTF-8 string. The string is owned by the
        /// <paramref name="result"/> object and is valid only for its lifetime. Set to
        /// <see cref="IntPtr.Zero"/> on failure.
        /// </param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or another <see cref="StatusCode"/> on failure.
        /// <see cref="StatusCode.InvalidArgument"/> if <paramref name="result"/> is invalid.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_detokenize_result_get_string(
            DetokenizeResultHandle result, out IntPtr outText);

        /// <summary>
        /// Destroys a LiteRT LM Token Union that the caller owns, such as one returned
        /// by <see cref="litert_lm_engine_get_start_token"/>. Do not pass a token union borrowed
        /// from a collection by <see cref="litert_lm_token_unions_get_token_at"/>.
        /// </summary>
        /// <param name="tokenUnion">The token union to destroy.</param>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern void litert_lm_token_union_delete(IntPtr tokenUnion);

        /// <summary>
        /// Returns the type of the token union.
        /// </summary>
        /// <param name="tokenUnion">The token union.</param>
        /// <param name="outType">
        /// On success, receives the type of the token union; not written on failure.
        /// </param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or another <see cref="StatusCode"/> on failure.
        /// <see cref="StatusCode.InvalidArgument"/> if <paramref name="tokenUnion"/> is invalid.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_token_union_get_type(
            TokenUnionHandle tokenUnion, out TokenUnionType outType);

        /// <summary>
        /// Returns the string value from a token union.
        /// </summary>
        /// <param name="tokenUnion">The token union.</param>
        /// <param name="outString">
        /// On success, receives the string value. The string is owned by the
        /// <paramref name="tokenUnion"/> object and is valid only for its lifetime. Set to
        /// <see cref="IntPtr.Zero"/> on failure.
        /// </param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or another <see cref="StatusCode"/> on failure.
        /// <see cref="StatusCode.InvalidArgument"/> if <paramref name="tokenUnion"/> is invalid or
        /// if the type of <paramref name="tokenUnion"/> is not <see cref="TokenUnionType.String"/>.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_token_union_get_string(
            TokenUnionHandle tokenUnion, out IntPtr outString);

        /// <summary>
        /// Returns the token ids from a token union.
        /// </summary>
        /// <param name="tokenUnion">The token union.</param>
        /// <param name="outTokens">
        /// On success, receives a pointer to the internal array of token ids. The received pointer
        /// is valid only for the lifetime of the <paramref name="tokenUnion"/> object. Set to
        /// <see cref="IntPtr.Zero"/> on failure.
        /// </param>
        /// <param name="outNumTokens">
        /// On success, receives the number of token ids; not written on failure.
        /// </param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or another <see cref="StatusCode"/> on failure.
        /// <see cref="StatusCode.InvalidArgument"/> if <paramref name="tokenUnion"/> is invalid or
        /// the type of <paramref name="tokenUnion"/> is not <see cref="TokenUnionType.Ids"/>.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_token_union_get_ids(
            TokenUnionHandle tokenUnion, out IntPtr outTokens,
            out UIntPtr outNumTokens);

        /// <summary>
        /// Destroys a LiteRT LM Token Unions object.
        /// </summary>
        /// <param name="tokens">The token unions object to destroy.</param>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern void litert_lm_token_unions_delete(IntPtr tokens);

        /// <summary>
        /// Returns the number of token unions in the collection.
        /// </summary>
        /// <param name="tokens">The token unions object.</param>
        /// <param name="outNumTokens">
        /// On success, receives the number of token unions; not written on failure.
        /// </param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or another <see cref="StatusCode"/> on failure.
        /// <see cref="StatusCode.InvalidArgument"/> if <paramref name="tokens"/> is invalid.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_token_unions_get_num_tokens(
            TokenUnionsHandle tokens, out UIntPtr outNumTokens);

        /// <summary>
        /// Returns the token union at a given index in a collection.
        /// </summary>
        /// <param name="tokens">The token unions collection.</param>
        /// <param name="index">The index of the token union.</param>
        /// <param name="outToken">
        /// On success, receives the token union at <paramref name="index"/>. The returned handle is
        /// owned by <paramref name="tokens"/> and is invalidated when it is deleted; do not pass its
        /// raw pointer to <see cref="litert_lm_token_union_delete"/>. Set to an invalid handle
        /// (<see cref="LiteRTHandle.IsInvalid"/>) on failure.
        /// </param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or another <see cref="StatusCode"/> on failure.
        /// <see cref="StatusCode.InvalidArgument"/> if <paramref name="tokens"/> is invalid;
        /// <see cref="StatusCode.OutOfRange"/> if <paramref name="index"/> is out of bounds.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_token_unions_get_token_at(
            TokenUnionsHandle tokens, UIntPtr index, out BorrowedTokenUnionHandle outToken);

        /// <summary>
        /// Returns the configured start token (BOS), if any.
        /// </summary>
        /// <param name="engine">The engine instance.</param>
        /// <param name="outToken">
        /// On success, receives the start token, owned by the caller; release with
        /// <see cref="TokenUnionHandle.Dispose"/>. Receives an invalid handle
        /// (<see cref="LiteRTHandle.IsInvalid"/>) on success if the engine has no start token
        /// configured, or on failure.
        /// </param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success (including when no start token is configured),
        /// or another <see cref="StatusCode"/> on failure. <see cref="StatusCode.InvalidArgument"/>
        /// if <paramref name="engine"/> is invalid.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_engine_get_start_token(
            EngineHandle engine, out TokenUnionHandle outToken);

        /// <summary>
        /// Returns the configured stop tokens (EOS).
        /// </summary>
        /// <param name="engine">The engine instance.</param>
        /// <param name="outTokens">
        /// On success, receives the stop tokens collection, owned by the caller; release with
        /// <see cref="TokenUnionsHandle.Dispose"/>. Receives an invalid handle
        /// (<see cref="LiteRTHandle.IsInvalid"/>) on success if the engine has no stop tokens
        /// configured, or on failure.
        /// </param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success (including when no stop tokens are configured),
        /// or another <see cref="StatusCode"/> on failure. <see cref="StatusCode.InvalidArgument"/>
        /// if <paramref name="engine"/> is invalid.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_engine_get_stop_tokens(
            EngineHandle engine, out TokenUnionsHandle outTokens);
    }
}