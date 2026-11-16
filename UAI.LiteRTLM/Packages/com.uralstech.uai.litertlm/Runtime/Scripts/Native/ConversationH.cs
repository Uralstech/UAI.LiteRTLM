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
    public static class ConversationH
    {
        /// <summary>
        /// Creates a LiteRT LM Conversation Config.
        /// </summary>
        /// <param name="outConfig">
        /// On success, receives the created config, owned by the caller; release with
        /// <see cref="ConversationConfigHandle.Dispose"/>. Set to an invalid handle
        /// (<see cref="LiteRTHandle.IsInvalid"/>) on failure.
        /// </param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or another <see cref="StatusCode"/> on failure.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_conversation_config_create(
            out ConversationConfigHandle outConfig);

        /// <summary>
        /// Sets the session config for this conversation config.
        /// </summary>
        /// <param name="config">The config to modify.</param>
        /// <param name="sessionConfig">The session config to use.</param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or <see cref="StatusCode.InvalidArgument"/> if
        /// <paramref name="config"/> or <paramref name="sessionConfig"/> is invalid.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_conversation_config_set_session_config(
            ConversationConfigHandle config, SessionConfigHandle sessionConfig);

        /// <summary>
        /// Sets the system message for this conversation config.
        /// </summary>
        /// <param name="config">The config to modify.</param>
        /// <param name="systemMessageJson">The system message in JSON format.</param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or <see cref="StatusCode.InvalidArgument"/> if
        /// <paramref name="config"/> is invalid or <paramref name="systemMessageJson"/>
        /// is <see langword="null"/>.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_conversation_config_set_system_message(
            ConversationConfigHandle config, [MarshalAs(UnmanagedType.LPUTF8Str)] string systemMessageJson);

        /// <summary>
        /// Sets the tools for this conversation config.
        /// </summary>
        /// <param name="config">The config to modify.</param>
        /// <param name="toolsJson">The tools description in JSON array format.</param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or <see cref="StatusCode.InvalidArgument"/> if
        /// <paramref name="config"/> is invalid or <paramref name="toolsJson"/>
        /// is <see langword="null"/>.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_conversation_config_set_tools(
            ConversationConfigHandle config, [MarshalAs(UnmanagedType.LPUTF8Str)] string toolsJson);

        /// <summary>
        /// Sets the initial messages for this conversation config.
        /// </summary>
        /// <param name="config">The config to modify.</param>
        /// <param name="messagesJson">The initial messages in JSON array format.</param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or <see cref="StatusCode.InvalidArgument"/> if
        /// <paramref name="config"/> is invalid or <paramref name="messagesJson"/>
        /// is <see langword="null"/>.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_conversation_config_set_messages(
            ConversationConfigHandle config, [MarshalAs(UnmanagedType.LPUTF8Str)] string messagesJson);

        /// <summary>
        /// Sets the extra context for the conversation preface.
        /// </summary>
        /// <param name="config">The config to modify.</param>
        /// <param name="extraContextJson">A JSON string representing the extra context object.</param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or <see cref="StatusCode.InvalidArgument"/> if
        /// <paramref name="config"/> is invalid or <paramref name="extraContextJson"/>
        /// is <see langword="null"/>.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_conversation_config_set_extra_context(
            ConversationConfigHandle config, [MarshalAs(UnmanagedType.LPUTF8Str)] string extraContextJson);

        /// <summary>
        /// Sets the prompt template for this conversation config.
        /// </summary>
        /// <param name="config">The config to modify.</param>
        /// <param name="promptTemplate">
        /// The prompt template string (e.g. Jinja template). If not set, use the default provided by
        /// the model or the engine.
        /// </param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or <see cref="StatusCode.InvalidArgument"/> if
        /// <paramref name="config"/> is invalid or <paramref name="promptTemplate"/>
        /// is <see langword="null"/>.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_conversation_config_set_prompt_template(
            ConversationConfigHandle config, [MarshalAs(UnmanagedType.LPUTF8Str)] string promptTemplate);

        /// <summary>
        /// Sets whether to enable constrained decoding for this conversation config.
        /// </summary>
        /// <param name="config">The config to modify.</param>
        /// <param name="enableConstrainedDecoding">Whether to enable constrained decoding.</param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or <see cref="StatusCode.InvalidArgument"/> if
        /// <paramref name="config"/> is invalid.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_conversation_config_set_enable_constrained_decoding(
            ConversationConfigHandle config, [MarshalAs(UnmanagedType.I1)] bool enableConstrainedDecoding);

        /// <summary>
        /// Sets the constraint provider type for this conversation config.
        /// </summary>
        /// <param name="config">The config to modify.</param>
        /// <param name="providerType">
        /// The constraint provider type to use, or <see cref="ConstraintProviderType.None"/> to unset it.
        /// </param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or <see cref="StatusCode.InvalidArgument"/> if
        /// <paramref name="config"/> is invalid or <paramref name="providerType"/> is not
        /// a declared <see cref="ConstraintProviderType"/> value.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_conversation_config_set_constraint_provider(
            ConversationConfigHandle config, ConstraintProviderType providerType);

        /// <summary>
        /// Sets whether to filter channel content from the KV cache.
        /// </summary>
        /// <param name="config">The config to modify.</param>
        /// <param name="filterChannelContentFromKvCache">Whether to filter channel content.</param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or <see cref="StatusCode.InvalidArgument"/> if
        /// <paramref name="config"/> is invalid.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_conversation_config_set_filter_channel_content_from_kv_cache(
            ConversationConfigHandle config, [MarshalAs(UnmanagedType.I1)] bool filterChannelContentFromKvCache);

        /// <summary>
        /// Sets whether to stream tool call tokens.
        /// </summary>
        /// <param name="config">The config to modify.</param>
        /// <param name="streamToolCalls">Whether to stream tool call tokens.</param>
        /// <param name="channelName">The channel name to use for tool call tokens.</param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or <see cref="StatusCode.InvalidArgument"/> if
        /// <paramref name="config"/> is invalid.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_conversation_config_set_stream_tool_calls(
            ConversationConfigHandle config, [MarshalAs(UnmanagedType.I1)] bool streamToolCalls,
            [MarshalAs(UnmanagedType.LPUTF8Str)] string? channelName);

        /// <summary>
        /// Creates a default LiteRT LM Thinking Config (enabled with infinite budget -1).
        /// </summary>
        /// <param name="outConfig">
        /// On success, receives the created config, owned by the caller; release with
        /// <see cref="ThinkingConfigHandle.Dispose"/>. Set to an invalid handle
        /// (<see cref="LiteRTHandle.IsInvalid"/>) on failure.
        /// </param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or another <see cref="StatusCode"/> on failure.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_thinking_config_create(
            out ThinkingConfigHandle outConfig);

        /// <summary>
        /// Destroys a LiteRT LM Thinking Config.
        /// </summary>
        /// <param name="config">The config to destroy.</param>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern void litert_lm_thinking_config_delete(IntPtr config);

        /// <summary>
        /// Sets whether thinking/reasoning generation is enabled.
        /// </summary>
        /// <param name="config">The config to modify.</param>
        /// <param name="enableThinking">Whether thinking is enabled.</param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or <see cref="StatusCode.InvalidArgument"/> if
        /// <paramref name="config"/> is invalid.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_thinking_config_set_enable_thinking(
            ThinkingConfigHandle config, [MarshalAs(UnmanagedType.I1)] bool enableThinking);

        /// <summary>
        /// Sets the thinking token budget.
        /// </summary>
        /// <param name="config">The config to modify.</param>
        /// <param name="thinkingTokenBudget">
        /// Budget for token-by-token reasoning generation (-1 for infinite).
        /// </param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or <see cref="StatusCode.InvalidArgument"/> if
        /// <paramref name="config"/> is invalid.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_thinking_config_set_thinking_token_budget(
            ThinkingConfigHandle config, int thinkingTokenBudget);

        /// <summary>
        /// Sets the thinking config for this conversation config.
        /// </summary>
        /// <param name="config">The config to modify.</param>
        /// <param name="thinkingConfig">
        /// The thinking config to set. If <see cref="ThinkingConfigHandle.Null"/> (or invalid),
        /// clears any previously set thinking config.
        /// </param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or <see cref="StatusCode.InvalidArgument"/> if
        /// <paramref name="config"/> is invalid.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_conversation_config_set_thinking_config(
            ConversationConfigHandle config, ThinkingConfigHandle thinkingConfig);

        /// <summary>
        /// Destroys a LiteRT LM Conversation Config.
        /// </summary>
        /// <param name="config">The config to destroy.</param>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern void litert_lm_conversation_config_delete(IntPtr config);

        /// <summary>
        /// Creates a LiteRT LM Conversation Optional Args.
        /// </summary>
        /// <param name="outOptionalArgs">
        /// On success, receives the created optional args, owned by the caller; release with
        /// <see cref="ConversationOptionalArgsHandle.Dispose"/>. Set to an invalid handle
        /// (<see cref="LiteRTHandle.IsInvalid"/>) on failure.
        /// </param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or another <see cref="StatusCode"/> on failure.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_conversation_optional_args_create(
            out ConversationOptionalArgsHandle outOptionalArgs);

        /// <summary>
        /// Destroys a LiteRT LM Conversation Optional Args.
        /// </summary>
        /// <param name="optionalArgs">The optional args to destroy.</param>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern void litert_lm_conversation_optional_args_delete(
            IntPtr optionalArgs);

        /// <summary>
        /// Sets the repetition penalty configuration for the per-turn conversation
        /// optional arguments (<c>OptionalArgs</c>).
        /// <para>
        /// The configured penalties (<c>repetition_penalty</c>, <c>presence_penalty</c>,
        /// <c>frequency_penalty</c>, <c>window_size</c>) apply exclusively to the output sequence
        /// generated during the current <c>send_message</c> or <c>send_message_async</c> call.
        /// </para>
        /// </summary>
        /// <param name="optionalArgs">The optional arguments structure (<c>OptionalArgs</c>) to modify.</param>
        /// <param name="repetitionPenaltyConfig">
        /// The repetition penalty configuration created via
        /// <see cref="EngineH.litert_lm_repetition_penalty_config_create"/>. The contents are deep-copied when set.
        /// If <see cref="RepetitionPenaltyConfigHandle.Null"/> (or invalid), clears any previously set
        /// repetition penalty config so no penalties apply.
        /// </param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or <see cref="StatusCode.InvalidArgument"/> if
        /// <paramref name="optionalArgs"/> is invalid.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_conversation_optional_args_set_repetition_penalty_config(
            ConversationOptionalArgsHandle optionalArgs, RepetitionPenaltyConfigHandle repetitionPenaltyConfig);

        /// <summary>
        /// Sets the no repeat ngram configuration for the per-turn conversation
        /// optional arguments (<c>OptionalArgs</c>).
        /// <para>
        /// The configured parameters (<c>no_repeat_ngram_size</c>, <c>window_size</c>) apply
        /// exclusively to the output sequence generated during the current <c>send_message</c> or
        /// <c>send_message_async</c> call.
        /// </para>
        /// </summary>
        /// <param name="optionalArgs">The optional arguments structure (<c>OptionalArgs</c>) to modify.</param>
        /// <param name="noRepeatNgramConfig">
        /// The no repeat ngram configuration created via
        /// <see cref="EngineH.litert_lm_no_repeat_ngram_config_create"/>. The contents are deep-copied when set.
        /// If <see cref="NoRepeatNgramConfigHandle.Null"/> (or invalid), clears any previously set no repeat
        /// ngram config so no repeat ngram banning applies.
        /// </param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or <see cref="StatusCode.InvalidArgument"/> if
        /// <paramref name="optionalArgs"/> is invalid.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_conversation_optional_args_set_no_repeat_ngram_config(
            ConversationOptionalArgsHandle optionalArgs, NoRepeatNgramConfigHandle noRepeatNgramConfig);

        /// <summary>
        /// Sets the suppress tokens configuration for the per-turn conversation
        /// optional arguments (<c>OptionalArgs</c>).
        /// <para>
        /// The configured list of suppressed tokens applies exclusively to the output sequence
        /// generated during the current <c>send_message</c> or <c>send_message_async</c> call.
        /// </para>
        /// </summary>
        /// <param name="optionalArgs">The optional arguments structure (<c>OptionalArgs</c>) to modify.</param>
        /// <param name="suppressTokensConfig">
        /// The suppress tokens configuration created via
        /// <see cref="EngineH.litert_lm_suppress_tokens_config_create"/>. The contents are deep-copied when set.
        /// If <see cref="SuppressTokensConfigHandle.Null"/> (or invalid) or if the inner token set is
        /// disabled/empty, clears any previously set suppress tokens config so no token suppression applies.
        /// </param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or <see cref="StatusCode.InvalidArgument"/> if
        /// <paramref name="optionalArgs"/> is invalid.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_conversation_optional_args_set_suppress_tokens_config(
            ConversationOptionalArgsHandle optionalArgs, SuppressTokensConfigHandle suppressTokensConfig);

        /// <summary>
        /// Sets the visual token budget for the conversation optional args.
        /// </summary>
        /// <param name="optionalArgs">The optional args to modify.</param>
        /// <param name="visualTokenBudget">The visual token budget.</param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or <see cref="StatusCode.InvalidArgument"/> if
        /// <paramref name="optionalArgs"/> is invalid.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_conversation_optional_args_set_visual_token_budget(
            ConversationOptionalArgsHandle optionalArgs, int visualTokenBudget);

        /// <summary>
        /// Sets the maximum number of output tokens for the conversation optional args.
        /// For thinking models, both thinking (reasoning) tokens and the final response
        /// tokens count towards this limit.
        /// </summary>
        /// <param name="optionalArgs">The optional args to modify.</param>
        /// <param name="maxOutputTokens">
        /// The maximum number of tokens to generate (including thinking tokens).
        /// </param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or <see cref="StatusCode.InvalidArgument"/> if
        /// <paramref name="optionalArgs"/> is invalid.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_conversation_optional_args_set_max_output_tokens(
            ConversationOptionalArgsHandle optionalArgs, int maxOutputTokens);

        /// <summary>
        /// Sets the thinking config for the conversation optional args.
        /// </summary>
        /// <param name="optionalArgs">The optional args to modify.</param>
        /// <param name="thinkingConfig">
        /// The thinking config to set. If <see cref="ThinkingConfigHandle.Null"/> (or invalid),
        /// clears any previously set thinking config.
        /// </param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or <see cref="StatusCode.InvalidArgument"/> if
        /// <paramref name="optionalArgs"/> is invalid.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_conversation_optional_args_set_thinking_config(
            ConversationOptionalArgsHandle optionalArgs, ThinkingConfigHandle thinkingConfig);

        /// <summary>
        /// Sets the constraint for the conversation optional args.
        /// </summary>
        /// <param name="optionalArgs">The optional args to modify.</param>
        /// <param name="constraintType">The type of constraint.</param>
        /// <param name="constraintString">The constraint pattern/schema/grammar string.</param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or <see cref="StatusCode.InvalidArgument"/> if
        /// <paramref name="optionalArgs"/> is invalid or <paramref name="constraintType"/>
        /// is not a declared <see cref="ConstraintType"/> value.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_conversation_optional_args_set_constraint(
            ConversationOptionalArgsHandle optionalArgs, ConstraintType constraintType,
            [MarshalAs(UnmanagedType.LPUTF8Str)] string? constraintString);

        /// <summary>
        /// Creates a LiteRT LM Conversation.
        /// </summary>
        /// <param name="engine">The engine to create the conversation from.</param>
        /// <param name="config">
        /// The conversation config to use. If <see cref="ConversationConfigHandle.Null"/> (or invalid),
        /// the default config will be used.
        /// </param>
        /// <param name="outConversation">
        /// On success, receives the created conversation, owned by the caller; release with
        /// <see cref="ConversationHandle.Dispose"/>. Set to an invalid handle
        /// (<see cref="LiteRTHandle.IsInvalid"/>) on failure.
        /// </param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or another <see cref="StatusCode"/> on failure.
        /// <see cref="StatusCode.InvalidArgument"/> if <paramref name="engine"/> is invalid;
        /// otherwise the code of the error that prevented the conversation from being created.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_conversation_create(
            EngineHandle engine, ConversationConfigHandle config,
            out ConversationHandle outConversation);

        /// <summary>
        /// Destroys a LiteRT LM Conversation.
        /// </summary>
        /// <param name="conversation">The conversation to destroy.</param>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern void litert_lm_conversation_delete(IntPtr conversation);

        /// <summary>
        /// Clones a LiteRT LM Conversation, duplicating its prefilled state.
        /// </summary>
        /// <param name="conversation">The conversation to clone.</param>
        /// <param name="outConversation">
        /// On success, receives the cloned conversation, owned by the caller; release with
        /// <see cref="ConversationHandle.Dispose"/>. Set to an invalid handle
        /// (<see cref="LiteRTHandle.IsInvalid"/>) on failure.
        /// </param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or another <see cref="StatusCode"/> on failure.
        /// <see cref="StatusCode.InvalidArgument"/> if <paramref name="conversation"/> is invalid;
        /// otherwise the code of the error that prevented the clone.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_conversation_clone(
            ConversationHandle conversation, out ConversationHandle outConversation);

        /// <summary>
        /// Sends a message to the conversation and returns the response.
        /// This is a blocking call.
        /// </summary>
        /// <param name="conversation">The conversation to use.</param>
        /// <param name="messageJson">A JSON string representing the message to send.</param>
        /// <param name="extraContext">A JSON string representing the extra context to use.</param>
        /// <param name="optionalArgs">
        /// The optional arguments to use, or <see cref="ConversationOptionalArgsHandle.Null"/>.
        /// </param>
        /// <param name="outResponse">
        /// On success, receives the JSON response, owned by the caller; release with
        /// <see cref="JsonResponseHandle.Dispose"/>. Set to an invalid handle
        /// (<see cref="LiteRTHandle.IsInvalid"/>) on failure.
        /// </param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or another <see cref="StatusCode"/> on failure.
        /// <see cref="StatusCode.InvalidArgument"/> if <paramref name="conversation"/> is invalid,
        /// <paramref name="messageJson"/> is <see langword="null"/>, or
        /// <paramref name="messageJson"/> is not valid JSON; otherwise the code of the generation error.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_conversation_send_message(
            ConversationHandle conversation, [MarshalAs(UnmanagedType.LPUTF8Str)] string messageJson,
            [MarshalAs(UnmanagedType.LPUTF8Str)] string? extraContext, ConversationOptionalArgsHandle optionalArgs,
            out JsonResponseHandle outResponse);

        /// <summary>
        /// Destroys a LiteRT LM JSON Response object.
        /// </summary>
        /// <param name="response">The response to destroy.</param>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern void litert_lm_json_response_delete(IntPtr response);

        /// <summary>
        /// Returns the JSON response string from a response object.
        /// </summary>
        /// <param name="response">The response object.</param>
        /// <param name="outJson">
        /// On success, receives the response JSON string. The string is owned by the
        /// <paramref name="response"/> object and is valid only for its lifetime.
        /// Set to <see cref="IntPtr.Zero"/> on failure.
        /// </param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or another <see cref="StatusCode"/> on failure.
        /// <see cref="StatusCode.InvalidArgument"/> if <paramref name="response"/> is invalid.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_json_response_get_string(
            JsonResponseHandle response, out IntPtr outJson);

        /// <summary>
        /// Sends a message to the conversation and streams the response via a callback.
        /// This is a non-blocking call that will invoke the callback from a background thread
        /// for each chunk (see the threading model documentation in <see cref="EngineH"/>).
        /// </summary>
        /// <param name="conversation">The conversation to use.</param>
        /// <param name="messageJson">A JSON string representing the message to send.</param>
        /// <param name="extraContext">A JSON string representing the extra context to use.</param>
        /// <param name="optionalArgs">
        /// The optional arguments to use, or <see cref="ConversationOptionalArgsHandle.Null"/>.
        /// </param>
        /// <param name="callback">The callback function pointer to receive response chunks.</param>
        /// <param name="callbackData">A pointer to user data that will be passed to the callback.</param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or another <see cref="StatusCode"/> on failure.
        /// <see cref="StatusCode.InvalidArgument"/> if <paramref name="conversation"/> is invalid,
        /// <paramref name="messageJson"/> is <see langword="null"/>, <paramref name="callback"/> is
        /// <see cref="IntPtr.Zero"/>, or <paramref name="messageJson"/> is not valid JSON; otherwise
        /// the code of the error that prevented the stream from starting. Errors that occur after
        /// the stream has started are reported through the callback.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_conversation_send_message_stream(
            ConversationHandle conversation, [MarshalAs(UnmanagedType.LPUTF8Str)] string messageJson,
            [MarshalAs(UnmanagedType.LPUTF8Str)] string? extraContext, ConversationOptionalArgsHandle optionalArgs,
            IntPtr /*EngineH.StreamCallback*/ callback, IntPtr callbackData);

        /// <summary>
        /// Renders the message into a string according to the template.
        /// <para>
        /// This function does not need to be called for actual message sending, as the
        /// <see cref="litert_lm_conversation_send_message"/> and
        /// <see cref="litert_lm_conversation_send_message_stream"/> functions will handle rendering
        /// internally.
        /// </para>
        /// <para>
        /// This function is not safe to call concurrently on the same conversation:
        /// each call replaces the string returned by the previous one. See the threading
        /// model documentation in <see cref="EngineH"/>.
        /// </para>
        /// </summary>
        /// <param name="conversation">The conversation instance.</param>
        /// <param name="messageJson">A JSON string representing the message to render.</param>
        /// <param name="outText">
        /// On success, receives the rendered string. The string is owned by the
        /// <paramref name="conversation"/> object and is valid until the next call to this function
        /// or until the conversation is deleted. Set to <see cref="IntPtr.Zero"/> on failure.
        /// </param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or another <see cref="StatusCode"/> on failure.
        /// <see cref="StatusCode.InvalidArgument"/> if <paramref name="conversation"/> is invalid,
        /// <paramref name="messageJson"/> is <see langword="null"/>, or
        /// <paramref name="messageJson"/> is not valid JSON; otherwise the code of the rendering error.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_conversation_render_message_to_string(
            ConversationHandle conversation, [MarshalAs(UnmanagedType.LPUTF8Str)] string messageJson,
            out IntPtr outText);

        /// <summary>
        /// Renders the preface into a string according to the template.
        /// <para>
        /// This function is not safe to call concurrently on the same conversation:
        /// each call replaces the string returned by the previous one. See the threading
        /// model documentation in <see cref="EngineH"/>.
        /// </para>
        /// </summary>
        /// <param name="conversation">The conversation instance.</param>
        /// <param name="outText">
        /// On success, receives the rendered string. The string is owned by the
        /// <paramref name="conversation"/> object and is valid until the next call to this function
        /// or until the conversation is deleted. Set to <see cref="IntPtr.Zero"/> on failure.
        /// </param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or another <see cref="StatusCode"/> on failure.
        /// <see cref="StatusCode.InvalidArgument"/> if <paramref name="conversation"/> is invalid;
        /// otherwise the code of the rendering error.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_conversation_render_preface_to_string(
            ConversationHandle conversation, out IntPtr outText);

        /// <summary>
        /// Cancels the ongoing inference process, for asynchronous inference.
        /// <para>
        /// This function only requests cancellation and does not wait for it: the
        /// stream callback can still run after this function returns. The conversation
        /// MUST NOT be reused afterwards; only delete it. See the threading model
        /// documentation in <see cref="EngineH"/>.
        /// </para>
        /// </summary>
        /// <param name="conversation">The conversation to cancel the inference for.</param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or <see cref="StatusCode.InvalidArgument"/> if
        /// <paramref name="conversation"/> is invalid.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_conversation_cancel_process(
            ConversationHandle conversation);

        /// <summary>
        /// Triggers execution of, and waits for, all pending tasks in the conversation
        /// session to complete.
        /// <para>
        /// This must be called after <see cref="litert_lm_conversation_send_message_stream"/> when
        /// the engine was created with single threaded execution enabled (see
        /// <see cref="EngineH.litert_lm_engine_settings_set_single_threaded_execution"/>): in that
        /// mode the queued work only makes progress while a caller drives it, so the streaming
        /// callback is invoked from within this call.
        /// </para>
        /// </summary>
        /// <param name="conversation">The conversation to wait for.</param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or another <see cref="StatusCode"/> on failure.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_conversation_wait_until_done(
            ConversationHandle conversation);

        /// <summary>
        /// Retrieves the benchmark information from the conversation.
        /// </summary>
        /// <param name="conversation">The conversation to get the benchmark info from.</param>
        /// <param name="outBenchmarkInfo">
        /// On success, receives the benchmark info, owned by the caller; release with
        /// <see cref="BenchmarkInfoHandle.Dispose"/>. Set to an invalid handle
        /// (<see cref="LiteRTHandle.IsInvalid"/>) on failure.
        /// </param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or another <see cref="StatusCode"/> on failure.
        /// <see cref="StatusCode.InvalidArgument"/> if <paramref name="conversation"/> is invalid;
        /// otherwise the code of the runtime error (e.g. if benchmarking is not enabled).
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_conversation_get_benchmark_info(
            ConversationHandle conversation, out BenchmarkInfoHandle outBenchmarkInfo);

        /// <summary>
        /// Gets the number of tokens in the conversation KV Cache (prefill + decode).
        /// </summary>
        /// <param name="conversation">The conversation instance.</param>
        /// <param name="outCount">
        /// On success, receives the number of tokens; not written on failure.
        /// </param>
        /// <returns>
        /// <see cref="StatusCode.Ok"/> on success, or another <see cref="StatusCode"/> on failure.
        /// <see cref="StatusCode.InvalidArgument"/> if <paramref name="conversation"/> is invalid;
        /// otherwise the code of the runtime error.
        /// </returns>
        [DllImport(Core.Lib, CallingConvention = CallingConvention.Cdecl)]
        public static extern StatusCode litert_lm_conversation_get_token_count(
            ConversationHandle conversation, out UIntPtr outCount);
    }
}