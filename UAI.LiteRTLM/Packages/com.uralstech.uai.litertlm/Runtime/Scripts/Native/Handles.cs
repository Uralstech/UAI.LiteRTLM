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

#nullable enable
namespace Uralstech.UAI.LiteRTLM.Native
{
    /// <summary>Abstract class for all LiteRT-LM native handles.</summary>
    public abstract class LiteRTHandle : SafeHandle
    {
        /// <summary>Does this refer to a valid LiteRT-LM object?</summary>
        public override bool IsInvalid => handle == IntPtr.Zero;

        protected LiteRTHandle(IntPtr handle, bool ownsHandle) : base(handle, ownsHandle) { }
        protected LiteRTHandle(bool ownsHandle) : this(IntPtr.Zero, ownsHandle) { }
    }

    #region EngineH
    
    public sealed class SamplerParamsHandle : LiteRTHandle
    {
        public static SamplerParamsHandle Null => new(ownsHandle: false);
        
        private SamplerParamsHandle(bool ownsHandle) : base(ownsHandle) { }
        public SamplerParamsHandle() : this(ownsHandle: true) { }
        
        protected override bool ReleaseHandle()
        {
            EngineH.litert_lm_sampler_params_delete(handle);
            return true;
        }
    }

    public sealed class SessionConfigHandle : LiteRTHandle
    {
        public static SessionConfigHandle Null => new(ownsHandle: false);
        
        private SessionConfigHandle(bool ownsHandle) : base(ownsHandle) { }
        public SessionConfigHandle() : this(ownsHandle: true) { }

        protected override bool ReleaseHandle()
        {
            EngineH.litert_lm_session_config_delete(handle);
            return true;
        }
    }

    public sealed class SpeculativeDecodingConfigHandle : LiteRTHandle
    {
        public static SpeculativeDecodingConfigHandle Null => new(ownsHandle: false);
        
        private SpeculativeDecodingConfigHandle(bool ownsHandle) : base(ownsHandle) { }
        public SpeculativeDecodingConfigHandle() : this(ownsHandle: true) { }

        protected override bool ReleaseHandle()
        {
            EngineH.litert_lm_speculative_decoding_config_delete(handle);
            return true;
        }
    }

    public sealed class RepetitionPenaltyConfigHandle : LiteRTHandle
    {
        public static RepetitionPenaltyConfigHandle Null => new(ownsHandle: false);
        
        private RepetitionPenaltyConfigHandle(bool ownsHandle) : base(ownsHandle) { }
        public RepetitionPenaltyConfigHandle() : this(ownsHandle: true) { }

        protected override bool ReleaseHandle()
        {
            EngineH.litert_lm_repetition_penalty_config_delete(handle);
            return true;
        }
    }

    public sealed class NoRepeatNgramConfigHandle : LiteRTHandle
    {
        public static NoRepeatNgramConfigHandle Null => new(ownsHandle: false);
        
        private NoRepeatNgramConfigHandle(bool ownsHandle) : base(ownsHandle) { }
        public NoRepeatNgramConfigHandle() : this(ownsHandle: true) { }

        protected override bool ReleaseHandle()
        {
            EngineH.litert_lm_no_repeat_ngram_config_delete(handle);
            return true;
        }
    }

    public sealed class SuppressTokensConfigHandle : LiteRTHandle
    {
        public static SuppressTokensConfigHandle Null => new(ownsHandle: false);
        
        private SuppressTokensConfigHandle(bool ownsHandle) : base(ownsHandle) { }
        public SuppressTokensConfigHandle() : this(ownsHandle: true) { }

        protected override bool ReleaseHandle()
        {
            EngineH.litert_lm_suppress_tokens_config_delete(handle);
            return true;
        }
    }

    public sealed class InputDataHandle : LiteRTHandle
    {
        public static InputDataHandle Null => new(ownsHandle: false);
        
        private InputDataHandle(bool ownsHandle) : base(ownsHandle) { }
        public InputDataHandle() : this(ownsHandle: true) { }

        protected override bool ReleaseHandle()
        {
            EngineH.litert_lm_input_data_delete(handle);
            return true;
        }
    }

    public sealed class EngineSettingsHandle : LiteRTHandle
    {
        public static EngineSettingsHandle Null => new(ownsHandle: false);
        
        private EngineSettingsHandle(bool ownsHandle) : base(ownsHandle) { }
        public EngineSettingsHandle() : this(ownsHandle: true) { }

        protected override bool ReleaseHandle()
        {
            EngineH.litert_lm_engine_settings_delete(handle);
            return true;
        }
    }

    public sealed class EngineHandle : LiteRTHandle
    {
        public static EngineHandle Null => new(ownsHandle: false);
        
        private EngineHandle(bool ownsHandle) : base(ownsHandle) { }
        public EngineHandle() : this(ownsHandle: true) { }

        protected override bool ReleaseHandle()
        {
            EngineH.litert_lm_engine_delete(handle);
            return true;
        }
    }

    public sealed class SessionHandle : LiteRTHandle
    {
        public static SessionHandle Null => new(ownsHandle: false);
        
        /// <summary>The engine this session was created from.</summary>
        public EngineHandle? EngineHandle { get; private set; }
        
        private SessionHandle(bool ownsHandle) : base(ownsHandle) { }
        public SessionHandle() : this(ownsHandle: true) { }

        /// <summary>Call ONCE right after the session is created.</summary>
        public void AttachParent(EngineHandle parentHandle)
        {
            if (EngineHandle is not null)
                throw new InvalidOperationException("Cannot attach a new parent to handle.");
            
            if (parentHandle.IsInvalid)
                throw new ArgumentException("Tried to attach invalid parent to handle.");

            if (IsInvalid)
                throw new InvalidOperationException("Tried to attach parent to invalid handle.");
            
            bool success = false;
            parentHandle.DangerousAddRef(ref success);
            if (!success)
                throw new InvalidOperationException("Could not increment reference to parent handle.");
            
            EngineHandle = parentHandle;
        }

        protected override bool ReleaseHandle()
        {
            EngineH.litert_lm_session_delete(handle);
            EngineHandle?.DangerousRelease();
            return true;
        }
    }

    public sealed class ResponsesHandle : LiteRTHandle
    {
        public static ResponsesHandle Null => new(ownsHandle: false);
        
        private ResponsesHandle(bool ownsHandle) : base(ownsHandle) { }
        public ResponsesHandle() : this(ownsHandle: true) { }

        protected override bool ReleaseHandle()
        {
            EngineH.litert_lm_responses_delete(handle);
            return true;
        }
    }

    public sealed class BenchmarkInfoHandle : LiteRTHandle
    {
        public static BenchmarkInfoHandle Null => new(ownsHandle: false);
        
        private BenchmarkInfoHandle(bool ownsHandle) : base(ownsHandle) { }
        public BenchmarkInfoHandle() : this(ownsHandle: true) { }

        protected override bool ReleaseHandle()
        {
            EngineH.litert_lm_benchmark_info_delete(handle);
            return true;
        }
    }

    public sealed class StreamChunkHandle : LiteRTHandle
    {
        public static StreamChunkHandle Null => new(handle: IntPtr.Zero);
        
        public StreamChunkHandle(IntPtr handle) : base(handle, ownsHandle: false) { }
        
        protected override bool ReleaseHandle() => false;
    }

    public sealed class TokenizeResultHandle : LiteRTHandle
    {
        public static TokenizeResultHandle Null => new(ownsHandle: false);
        
        private TokenizeResultHandle(bool ownsHandle) : base(ownsHandle) { }
        public TokenizeResultHandle() : this(ownsHandle: true) { }

        protected override bool ReleaseHandle()
        {
            EngineH.litert_lm_tokenize_result_delete(handle);
            return true;
        }
    }

    public sealed class DetokenizeResultHandle : LiteRTHandle
    {
        public static DetokenizeResultHandle Null => new(ownsHandle: false);
        
        private DetokenizeResultHandle(bool ownsHandle) : base(ownsHandle) { }
        public DetokenizeResultHandle() : this(ownsHandle: true) { }

        protected override bool ReleaseHandle()
        {
            EngineH.litert_lm_detokenize_result_delete(handle);
            return true;
        }
    }

    public class TokenUnionHandle : LiteRTHandle
    {
        public static TokenUnionHandle Null => new(ownsHandle: false);
        
        protected TokenUnionHandle(bool ownsHandle) : base(ownsHandle) { }
        public TokenUnionHandle() : this(ownsHandle: true) { }

        protected override bool ReleaseHandle()
        {
            EngineH.litert_lm_token_union_delete(handle);
            return true;
        }
    }

    public sealed class BorrowedTokenUnionHandle : TokenUnionHandle
    {
        public static new BorrowedTokenUnionHandle Null => new(ownsHandle: false);

        /// <summary>The token unions this union was borrowed from.</summary>
        public TokenUnionsHandle? TokenUnionsHandle { get; private set; }

        private BorrowedTokenUnionHandle(bool ownsHandle) : base(ownsHandle) { }
        public BorrowedTokenUnionHandle() : this(ownsHandle: true) { }

        /// <summary>Call ONCE right after the union is created.</summary>
        public void AttachParent(TokenUnionsHandle parentHandle)
        {
            if (TokenUnionsHandle is not null)
                throw new InvalidOperationException("Cannot attach a new parent to handle.");
            
            if (parentHandle.IsInvalid)
                throw new ArgumentException("Tried to attach invalid parent to handle.");

            if (IsInvalid)
                throw new InvalidOperationException("Tried to attach parent to invalid handle.");
            
            bool success = false;
            parentHandle.DangerousAddRef(ref success);
            if (!success)
                throw new InvalidOperationException("Could not increment reference to parent handle.");
            
            TokenUnionsHandle = parentHandle;
        }

        protected override bool ReleaseHandle()
        {
            TokenUnionsHandle?.DangerousRelease();
            return true;
        }
    }
    
    public sealed class TokenUnionsHandle : LiteRTHandle
    {
        public static TokenUnionsHandle Null => new(ownsHandle: false);
        
        private TokenUnionsHandle(bool ownsHandle) : base(ownsHandle) { }
        public TokenUnionsHandle() : this(ownsHandle: true) { }

        protected override bool ReleaseHandle()
        {
            EngineH.litert_lm_token_unions_delete(handle);
            return true;
        }
    }
    
    #endregion
    
    #region ConversationH
    
    public sealed class ConversationConfigHandle : LiteRTHandle
    {
        public static ConversationConfigHandle Null => new(ownsHandle: false);
        
        private ConversationConfigHandle(bool ownsHandle) : base(ownsHandle) { }
        public ConversationConfigHandle() : this(ownsHandle: true) { }

        protected override bool ReleaseHandle()
        {
            ConversationH.litert_lm_conversation_config_delete(handle);
            return true;
        }
    }
    
    public sealed class ThinkingConfigHandle : LiteRTHandle
    {
        public static ThinkingConfigHandle Null => new(ownsHandle: false);
        
        private ThinkingConfigHandle(bool ownsHandle) : base(ownsHandle) { }
        public ThinkingConfigHandle() : this(ownsHandle: true) { }

        protected override bool ReleaseHandle()
        {
            ConversationH.litert_lm_thinking_config_delete(handle);
            return true;
        }
    }

    public sealed class ConversationOptionalArgsHandle : LiteRTHandle
    {
        public static ConversationOptionalArgsHandle Null => new(ownsHandle: false);
        
        private ConversationOptionalArgsHandle(bool ownsHandle) : base(ownsHandle) { }
        public ConversationOptionalArgsHandle() : this(ownsHandle: true) { }

        protected override bool ReleaseHandle()
        {
            ConversationH.litert_lm_conversation_optional_args_delete(handle);
            return true;
        }
    }
    
    public sealed class ConversationHandle : LiteRTHandle
    {
        public static ConversationHandle Null => new(ownsHandle: false);
        
        /// <summary>The engine this conversation was created from.</summary>
        public EngineHandle? EngineHandle { get; private set; }
        
        private ConversationHandle(bool ownsHandle) : base(ownsHandle) { }
        public ConversationHandle() : this(ownsHandle: true) { }

        /// <summary>Call ONCE right after the conversation is created.</summary>
        public void AttachParent(EngineHandle parentHandle)
        {
            if (EngineHandle is not null)
                throw new InvalidOperationException("Cannot attach a new parent to handle.");
            
            if (parentHandle.IsInvalid)
                throw new ArgumentException("Tried to attach invalid parent to handle.");

            if (IsInvalid)
                throw new InvalidOperationException("Tried to attach parent to invalid handle.");
            
            bool success = false;
            parentHandle.DangerousAddRef(ref success);
            if (!success)
                throw new InvalidOperationException("Could not increment reference to parent handle.");
            
            EngineHandle = parentHandle;
        }

        protected override bool ReleaseHandle()
        {
            ConversationH.litert_lm_conversation_delete(handle);
            EngineHandle?.DangerousRelease();
            return true;
        }
    }
    
    public sealed class JsonResponseHandle : LiteRTHandle
    {
        public static JsonResponseHandle Null => new(ownsHandle: false);
        
        private JsonResponseHandle(bool ownsHandle) : base(ownsHandle) { }
        public JsonResponseHandle() : this(ownsHandle: true) { }

        protected override bool ReleaseHandle()
        {
            ConversationH.litert_lm_json_response_delete(handle);
            return true;
        }
    }
    
    #endregion
    
    #region EmbeddingEngineH
    
    public sealed class EmbeddingEngineSettingsHandle : LiteRTHandle
    {
        public static EmbeddingEngineSettingsHandle Null => new(ownsHandle: false);
        
        private EmbeddingEngineSettingsHandle(bool ownsHandle) : base(ownsHandle) { }
        public EmbeddingEngineSettingsHandle() : this(ownsHandle: true) { }
        
        protected override bool ReleaseHandle()
        {
            EmbeddingEngineH.litert_lm_embedding_engine_settings_delete(handle);
            return true;
        }
    }
    
    public sealed class EmbeddingOptionsHandle : LiteRTHandle
    {
        public static EmbeddingOptionsHandle Null => new(ownsHandle: false);
        
        private EmbeddingOptionsHandle(bool ownsHandle) : base(ownsHandle) { }
        public EmbeddingOptionsHandle() : this(ownsHandle: true) { }
        
        protected override bool ReleaseHandle()
        {
            EmbeddingEngineH.litert_lm_embedding_options_delete(handle);
            return true;
        }
    }
    
    public class EmbeddingResponseHandle : LiteRTHandle
    {
        public static EmbeddingResponseHandle Null => new(ownsHandle: false);
        
        protected EmbeddingResponseHandle(bool ownsHandle) : base(ownsHandle) { }
        public EmbeddingResponseHandle() : this(ownsHandle: true) { }
        
        protected override bool ReleaseHandle()
        {
            EmbeddingEngineH.litert_lm_embedding_response_delete(handle);
            return true;
        }
    }
    
    public sealed class BorrowedEmbeddingResponseHandle : EmbeddingResponseHandle
    {
        public static new BorrowedEmbeddingResponseHandle Null => new(ownsHandle: false);

        /// <summary>The embedding responses this response was borrowed from.</summary>
        public EmbeddingResponsesHandle? EmbeddingResponsesHandle { get; private set; }

        private BorrowedEmbeddingResponseHandle(bool ownsHandle) : base(ownsHandle) { }
        public BorrowedEmbeddingResponseHandle() : this(ownsHandle: true) { }

        /// <summary>Call ONCE right after the response is created.</summary>
        public void AttachParent(EmbeddingResponsesHandle parentHandle)
        {
            if (EmbeddingResponsesHandle is not null)
                throw new InvalidOperationException("Cannot attach a new parent to handle.");
            
            if (parentHandle.IsInvalid)
                throw new ArgumentException("Tried to attach invalid parent to handle.");

            if (IsInvalid)
                throw new InvalidOperationException("Tried to attach parent to invalid handle.");
            
            bool success = false;
            parentHandle.DangerousAddRef(ref success);
            if (!success)
                throw new InvalidOperationException("Could not increment reference to parent handle.");
            
            EmbeddingResponsesHandle = parentHandle;
        }

        protected override bool ReleaseHandle()
        {
            EmbeddingResponsesHandle?.DangerousRelease();
            return true;
        }
    }
    
    public sealed class EmbeddingResponsesHandle : LiteRTHandle
    {
        public static EmbeddingResponsesHandle Null => new(ownsHandle: false);
        
        private EmbeddingResponsesHandle(bool ownsHandle) : base(ownsHandle) { }
        public EmbeddingResponsesHandle() : this(ownsHandle: true) { }
        
        protected override bool ReleaseHandle()
        {
            EmbeddingEngineH.litert_lm_embedding_responses_delete(handle);
            return true;
        }
    }
    
    public sealed class EmbeddingEngineHandle : LiteRTHandle
    {
        public static EmbeddingEngineHandle Null => new(ownsHandle: false);
        
        private EmbeddingEngineHandle(bool ownsHandle) : base(ownsHandle) { }
        public EmbeddingEngineHandle() : this(ownsHandle: true) { }
        
        protected override bool ReleaseHandle()
        {
            EmbeddingEngineH.litert_lm_embedding_engine_delete(handle);
            return true;
        }
    }
    
    #endregion
    
    #region ModelInfoH
    
    public sealed class LoadedFileHandle : LiteRTHandle
    {
        public static LoadedFileHandle Null => new(ownsHandle: false);
        
        private LoadedFileHandle(bool ownsHandle) : base(ownsHandle) { }
        public LoadedFileHandle() : this(ownsHandle: true) { }
        
        protected override bool ReleaseHandle()
        {
            ModelInfoH.litert_lm_loaded_file_delete(handle);
            return true;
        }
    }
    
    #endregion
    
    #region ExperimentalH
    
    public sealed class SessionDebugInfoHandle : LiteRTHandle
    {
        public static SessionDebugInfoHandle Null => new(ownsHandle: false);
        
        private SessionDebugInfoHandle(bool ownsHandle) : base(ownsHandle) { }
        public SessionDebugInfoHandle() : this(ownsHandle: true) { }
        
        protected override bool ReleaseHandle()
        {
            ExperimentalH.litert_lm_experimental_session_debug_info_delete(handle);
            return true;
        }
    }
    
    #endregion
}