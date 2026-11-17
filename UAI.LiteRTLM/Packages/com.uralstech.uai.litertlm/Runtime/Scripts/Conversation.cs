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
using Uralstech.UAI.LiteRTLM.Native;

#nullable enable
namespace Uralstech.UAI.LiteRTLM
{
    /// <summary>Thinking / reasoning configuration for a conversation.</summary>
    public sealed class ThinkingConfig : LiteRTLMWrapper<ThinkingConfigHandle>
    {
        /// <summary>Creates a default LiteRT LM Thinking Config (enabled with infinite budget -1).</summary>
        public ThinkingConfig()
        {
            LiteRTLMException.Check(
                ConversationH.litert_lm_thinking_config_create(out ThinkingConfigHandle handle));
            SetHandle(handle);
        }

        /// <summary>Sets whether thinking/reasoning generation is enabled.</summary>
        /// <param name="enableThinking">Whether thinking is enabled.</param>
        public void SetEnableThinking(bool enableThinking) =>
            LockedExecute(handle => ConversationH.litert_lm_thinking_config_set_enable_thinking(handle, enableThinking));

        /// <summary>Sets the thinking token budget (-1 for infinite).</summary>
        /// <param name="thinkingTokenBudget">Budget for token-by-token reasoning generation.</param>
        public void SetThinkingTokenBudget(int thinkingTokenBudget) =>
            LockedExecute(handle => ConversationH.litert_lm_thinking_config_set_thinking_token_budget(handle, thinkingTokenBudget));
    }

    /// <summary>Configuration options for a LiteRT LM <see cref="Conversation"/>.</summary>
    public sealed class ConversationConfig : LiteRTLMWrapper<ConversationConfigHandle>
    {
        /// <summary>Creates a LiteRT LM Conversation Config.</summary>
        public ConversationConfig()
        {
            LiteRTLMException.Check(
                ConversationH.litert_lm_conversation_config_create(out ConversationConfigHandle handle));
            SetHandle(handle);
        }

        /// <summary>Sets the session config for this conversation config.</summary>
        /// <param name="sessionConfig">The session config to use.</param>
        public void SetSessionConfig(SessionConfig sessionConfig)
        {
            if (sessionConfig is null)
                throw new ArgumentNullException(nameof(sessionConfig));

            LockedExecute(handle => ConversationH.litert_lm_conversation_config_set_session_config(
                handle, sessionConfig.DangerousGetHandle()));
            GC.KeepAlive(sessionConfig);
        }

        /// <summary>Sets the system message for this conversation config.</summary>
        /// <param name="systemMessageJson">The system message in JSON format.</param>
        public void SetSystemMessage(string systemMessageJson) =>
            LockedExecute(handle => ConversationH.litert_lm_conversation_config_set_system_message(handle, systemMessageJson));

        /// <summary>Sets the tools for this conversation config.</summary>
        /// <param name="toolsJson">The tools description in JSON array format.</param>
        public void SetTools(string toolsJson) =>
            LockedExecute(handle => ConversationH.litert_lm_conversation_config_set_tools(handle, toolsJson));

        /// <summary>Sets the initial messages for this conversation config.</summary>
        /// <param name="messagesJson">The initial messages in JSON array format.</param>
        public void SetMessages(string messagesJson) =>
            LockedExecute(handle => ConversationH.litert_lm_conversation_config_set_messages(handle, messagesJson));

        /// <summary>Sets the extra context for the conversation preface.</summary>
        /// <param name="extraContextJson">A JSON string representing the extra context object.</param>
        public void SetExtraContext(string extraContextJson) =>
            LockedExecute(handle => ConversationH.litert_lm_conversation_config_set_extra_context(handle, extraContextJson));

        /// <summary>Sets the prompt template (e.g. Jinja template) for this conversation config.</summary>
        /// <param name="promptTemplate">The prompt template string.</param>
        public void SetPromptTemplate(string promptTemplate) =>
            LockedExecute(handle => ConversationH.litert_lm_conversation_config_set_prompt_template(handle, promptTemplate));

        /// <summary>Sets whether to enable constrained decoding for this conversation config.</summary>
        /// <param name="enableConstrainedDecoding">Whether to enable constrained decoding.</param>
        public void SetEnableConstrainedDecoding(bool enableConstrainedDecoding) =>
            LockedExecute(handle => ConversationH.litert_lm_conversation_config_set_enable_constrained_decoding(handle, enableConstrainedDecoding));

        /// <summary>Sets the constraint provider type for this conversation config.</summary>
        /// <param name="providerType">The constraint provider type, or <see cref="ConstraintProviderType.None"/> to unset.</param>
        public void SetConstraintProvider(ConstraintProviderType providerType) =>
            LockedExecute(handle => ConversationH.litert_lm_conversation_config_set_constraint_provider(handle, providerType));

        /// <summary>Sets whether to filter channel content from the KV cache.</summary>
        /// <param name="filterChannelContentFromKvCache">Whether to filter channel content.</param>
        public void SetFilterChannelContentFromKvCache(bool filterChannelContentFromKvCache) =>
            LockedExecute(handle => ConversationH.litert_lm_conversation_config_set_filter_channel_content_from_kv_cache(handle, filterChannelContentFromKvCache));

        /// <summary>Sets whether to stream tool call tokens.</summary>
        /// <param name="streamToolCalls">Whether to stream tool call tokens.</param>
        /// <param name="channelName">The channel name to use for tool call tokens.</param>
        public void SetStreamToolCalls(bool streamToolCalls, string? channelName = null) =>
            LockedExecute(handle => ConversationH.litert_lm_conversation_config_set_stream_tool_calls(handle, streamToolCalls, channelName));

        /// <summary>Sets the thinking config for this conversation config, or clears it if <see langword="null"/>.</summary>
        /// <param name="thinkingConfig">The thinking config to set, or <see langword="null"/>.</param>
        public void SetThinkingConfig(ThinkingConfig? thinkingConfig)
        {
            LockedExecute(handle => ConversationH.litert_lm_conversation_config_set_thinking_config(
                handle, thinkingConfig?.DangerousGetHandle() ?? ThinkingConfigHandle.Null));
            GC.KeepAlive(thinkingConfig);
        }
    }

    /// <summary>Per-turn optional arguments for <see cref="Conversation"/> inference calls.</summary>
    public sealed class ConversationOptionalArgs : LiteRTLMWrapper<ConversationOptionalArgsHandle>
    {
        /// <summary>Creates a LiteRT LM Conversation Optional Args instance.</summary>
        public ConversationOptionalArgs()
        {
            LiteRTLMException.Check(
                ConversationH.litert_lm_conversation_optional_args_create(out ConversationOptionalArgsHandle handle));
            SetHandle(handle);
        }

        /// <summary>Sets the repetition penalty config for this turn, or clears it if <see langword="null"/>.</summary>
        /// <param name="repetitionPenaltyConfig">The repetition penalty config, or <see langword="null"/>.</param>
        public void SetRepetitionPenaltyConfig(RepetitionPenaltyConfig? repetitionPenaltyConfig)
        {
            LockedExecute(handle => ConversationH.litert_lm_conversation_optional_args_set_repetition_penalty_config(
                handle, repetitionPenaltyConfig?.DangerousGetHandle() ?? RepetitionPenaltyConfigHandle.Null));
            GC.KeepAlive(repetitionPenaltyConfig);
        }

        /// <summary>Sets the no-repeat n-gram config for this turn, or clears it if <see langword="null"/>.</summary>
        /// <param name="noRepeatNgramConfig">The no-repeat n-gram config, or <see langword="null"/>.</param>
        public void SetNoRepeatNgramConfig(NoRepeatNgramConfig? noRepeatNgramConfig)
        {
            LockedExecute(handle => ConversationH.litert_lm_conversation_optional_args_set_no_repeat_ngram_config(
                handle, noRepeatNgramConfig?.DangerousGetHandle() ?? NoRepeatNgramConfigHandle.Null));
            GC.KeepAlive(noRepeatNgramConfig);
        }

        /// <summary>Sets the suppress-tokens config for this turn, or clears it if <see langword="null"/>.</summary>
        /// <param name="suppressTokensConfig">The suppress-tokens config, or <see langword="null"/>.</param>
        public void SetSuppressTokensConfig(SuppressTokensConfig? suppressTokensConfig)
        {
            LockedExecute(handle => ConversationH.litert_lm_conversation_optional_args_set_suppress_tokens_config(
                handle, suppressTokensConfig?.DangerousGetHandle() ?? SuppressTokensConfigHandle.Null));
            GC.KeepAlive(suppressTokensConfig);
        }

        /// <summary>Sets the visual token budget for this turn.</summary>
        /// <param name="visualTokenBudget">The visual token budget.</param>
        public void SetVisualTokenBudget(int visualTokenBudget) =>
            LockedExecute(handle => ConversationH.litert_lm_conversation_optional_args_set_visual_token_budget(handle, visualTokenBudget));

        /// <summary>Sets the maximum number of output tokens for this turn (including thinking tokens).</summary>
        /// <param name="maxOutputTokens">The maximum number of tokens to generate.</param>
        public void SetMaxOutputTokens(int maxOutputTokens) =>
            LockedExecute(handle => ConversationH.litert_lm_conversation_optional_args_set_max_output_tokens(handle, maxOutputTokens));

        /// <summary>Sets the thinking config for this turn, or clears it if <see langword="null"/>.</summary>
        /// <param name="thinkingConfig">The thinking config, or <see langword="null"/>.</param>
        public void SetThinkingConfig(ThinkingConfig? thinkingConfig)
        {
            LockedExecute(handle => ConversationH.litert_lm_conversation_optional_args_set_thinking_config(
                handle, thinkingConfig?.DangerousGetHandle() ?? ThinkingConfigHandle.Null));
            GC.KeepAlive(thinkingConfig);
        }

        /// <summary>Sets the decoding constraint for this turn.</summary>
        /// <param name="constraintType">The constraint type.</param>
        /// <param name="constraintString">The constraint pattern, schema, or grammar string.</param>
        public void SetConstraint(ConstraintType constraintType, string? constraintString) =>
            LockedExecute(handle => ConversationH.litert_lm_conversation_optional_args_set_constraint(
                handle, constraintType, constraintString));
    }

    /// <summary>JSON response returned by <see cref="Conversation.SendMessage"/>.</summary>
    public sealed class JsonResponse : LiteRTLMWrapper<JsonResponseHandle>
    {
        internal JsonResponse(JsonResponseHandle handle)
        {
            SetHandle(handle);
        }

        /// <summary>Returns the JSON response string.</summary>
        public string? GetString() =>
            LockedExecute(handle => (ConversationH.litert_lm_json_response_get_string(handle, out IntPtr textPtr),
                textPtr != IntPtr.Zero ? Marshal.PtrToStringUTF8(textPtr) : null));
    }

    /// <summary>
    /// High-level multi-turn conversation created from an <see cref="Engine"/>.
    /// Every <see cref="Conversation"/> should be disposed before disposing its parent <see cref="Engine"/>.
    /// </summary>
    public sealed class Conversation : LiteRTLMWrapper<ConversationHandle>
    {
        /// <summary>Creates a LiteRT LM <see cref="Conversation"/> from the given <see cref="Engine"/>.</summary>
        /// <param name="engine">The engine to create the conversation from.</param>
        /// <param name="config">The conversation config, or <see langword="null"/> to use the default config.</param>
        public Conversation(Engine engine, ConversationConfig? config = null)
        {
            if (engine is null)
                throw new ArgumentNullException(nameof(engine));

            ConversationHandle conversation = engine.LockedExecute(handle =>
            {
                StatusCode result = ConversationH.litert_lm_conversation_create(
                    handle, config?.DangerousGetHandle() ?? ConversationConfigHandle.Null,
                    out ConversationHandle outConversation);
                
                if (result is StatusCode.Ok)
                    DisposeOnCatch(outConversation, conversation => conversation.DangerousAttachOwner(handle));
                return (result, outConversation);
            });
            
            GC.KeepAlive(config);
            SetHandle(conversation);
        }

        private Conversation(ConversationHandle handle)
        {
            SetHandle(handle);
        }

        /// <summary>Clones this conversation, duplicating its prefilled state.</summary>
        /// <returns>The cloned conversation.</returns>
        public Conversation Clone()
        {
            ConversationHandle clonedHandle = LockedExecute(handle =>
            {
                StatusCode result = ConversationH.litert_lm_conversation_clone(
                    handle, out ConversationHandle outConversation);

                if (result is StatusCode.Ok)
                    DisposeOnCatch(outConversation, conversation => conversation.DangerousAttachOwner(handle.OwnerHandle!));
                return (result, outConversation);
            });

            return new Conversation(clonedHandle);
        }

        /// <summary>Sends a message to the conversation and returns the JSON response (blocking).</summary>
        /// <param name="messageJson">A JSON string representing the message to send.</param>
        /// <param name="extraContext">An optional JSON string representing extra context.</param>
        /// <param name="optionalArgs">Optional per-turn arguments.</param>
        /// <returns>The JSON response.</returns>
        public JsonResponse SendMessage(string messageJson,
            string? extraContext = null, ConversationOptionalArgs? optionalArgs = null)
        {
            JsonResponseHandle responseHandle = LockedExecute(handle => (ConversationH.litert_lm_conversation_send_message(
                handle, messageJson, extraContext, optionalArgs?.DangerousGetHandle() ?? ConversationOptionalArgsHandle.Null,
                out JsonResponseHandle outResponse), outResponse));
            
            GC.KeepAlive(optionalArgs);
            return new JsonResponse(responseHandle);
        }

        /// <summary>Sends a message to the conversation and streams response chunks to <paramref name="callback"/>.</summary>
        /// <param name="callback">The callback invoked for each chunk.</param>
        /// <param name="messageJson">A JSON string representing the message to send.</param>
        /// <param name="extraContext">An optional JSON string representing extra context.</param>
        /// <param name="optionalArgs">Optional per-turn arguments.</param>
        public void SendMessageStream(StreamCallback callback, string messageJson,
            string? extraContext = null, ConversationOptionalArgs? optionalArgs = null)
        {
            if (callback is null)
                throw new ArgumentNullException(nameof(callback));

            IntPtr key = StreamCallbackHandler.Register(callback);
            try
            {
                LockedExecute(handle => ConversationH.litert_lm_conversation_send_message_stream(
                    handle, messageJson, extraContext, optionalArgs?.DangerousGetHandle() ?? ConversationOptionalArgsHandle.Null,
                    StreamCallbackHandler.GetGlobalStreamCallbackListenerPtr(), key));
                GC.KeepAlive(optionalArgs);
            }
            catch
            {
                StreamCallbackHandler.Deregister(key);
                throw;
            }
        }

        /// <summary>Renders a JSON message into a prompt string according to the conversation template.</summary>
        /// <param name="messageJson">A JSON string representing the message to render.</param>
        /// <returns>The rendered string.</returns>
        public string? RenderMessageToString(string messageJson) =>
            LockedExecute(handle => (ConversationH.litert_lm_conversation_render_message_to_string(handle, messageJson, out IntPtr textPtr),
                textPtr != IntPtr.Zero ? Marshal.PtrToStringUTF8(textPtr) : null));

        /// <summary>Renders the conversation preface into a string according to the conversation template.</summary>
        /// <returns>The rendered preface string.</returns>
        public string? RenderPrefaceToString() =>
            LockedExecute(handle => (ConversationH.litert_lm_conversation_render_preface_to_string(handle, out IntPtr textPtr),
                textPtr != IntPtr.Zero ? Marshal.PtrToStringUTF8(textPtr) : null));

        /// <summary>
        /// Requests cancellation of ongoing asynchronous inference.
        /// After cancellation, the conversation MUST NOT be reused; only dispose it.
        /// </summary>
        public void CancelProcess() =>
            LockedExecute(ConversationH.litert_lm_conversation_cancel_process);

        /// <summary>
        /// Drives execution of, and waits for, all pending tasks in the conversation to complete.
        /// Required after <see cref="SendMessageStream"/> when single-threaded execution is enabled.
        /// </summary>
        public void WaitUntilDone() =>
            LockedExecute(ConversationH.litert_lm_conversation_wait_until_done);

        /// <summary>Retrieves benchmark information collected on this conversation.</summary>
        /// <returns>The benchmark info.</returns>
        public BenchmarkInfo GetBenchmarkInfo()
        {
            BenchmarkInfoHandle benchmarkHandle = LockedExecute(handle =>
                (ConversationH.litert_lm_conversation_get_benchmark_info(handle, out BenchmarkInfoHandle outBenchmarkInfo), outBenchmarkInfo));
            return new BenchmarkInfo(benchmarkHandle);
        }

        /// <summary>Gets the number of tokens in the conversation KV cache (prefill + decode).</summary>
        public int GetTokenCount() =>
            LockedExecute(handle => (ConversationH.litert_lm_conversation_get_token_count(handle, out UIntPtr count), (int)count));
    }
}