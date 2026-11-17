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
namespace Uralstech.UAI.LiteRTLM.Omni.Native
{
    /// <summary>Abstract class for all LiteRT-LM native handles.</summary>
    public abstract class LiteRTHandle : SafeHandle
    {
        /// <summary>Does this refer to a valid LiteRT-LM object?</summary>
        public override bool IsInvalid => handle == IntPtr.Zero;

        /// <summary>The native owner of this handle's object.</summary>
        public LiteRTHandle? OwnerHandle { get; private set; }

        protected LiteRTHandle(IntPtr handle, bool ownsHandle) : base(handle, ownsHandle) { }
        protected LiteRTHandle(bool ownsHandle) : this(IntPtr.Zero, ownsHandle) { }
        
        protected void DangerousAttachOwner(LiteRTHandle ownerHandle)
        {
            if (ownerHandle is null)
                throw new ArgumentNullException(nameof(ownerHandle));

            if (OwnerHandle is not null)
                throw new InvalidOperationException("Cannot attach a new owner to handle.");
            
            if (ownerHandle.IsInvalid || ownerHandle.IsClosed)
                throw new ArgumentException("Tried to attach invalid/closed owner to handle.");

            if (IsInvalid || IsClosed)
                throw new InvalidOperationException("Tried to attach owner to invalid/closed handle.");
            
            bool success = false;
            ownerHandle.DangerousAddRef(ref success);
            if (!success)
                throw new InvalidOperationException("Could not increment reference to owner handle.");
            
            OwnerHandle = ownerHandle;
        }

        protected override bool ReleaseHandle()
        {
            try
            {
                return ReleaseLiteRTHandle();
            }
            finally
            {
                OwnerHandle?.DangerousRelease();
                OwnerHandle = null;
            }
        }

        protected abstract bool ReleaseLiteRTHandle();
    }

    #region EngineH
    
    public sealed class SamplerParamsHandle : LiteRTHandle
    {
        public static SamplerParamsHandle Null => new(ownsHandle: false);
        
        private SamplerParamsHandle(bool ownsHandle) : base(ownsHandle) { }
        public SamplerParamsHandle() : this(ownsHandle: true) { }
        
        protected override bool ReleaseLiteRTHandle()
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

        protected override bool ReleaseLiteRTHandle()
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

        protected override bool ReleaseLiteRTHandle()
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

        protected override bool ReleaseLiteRTHandle()
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

        protected override bool ReleaseLiteRTHandle()
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

        protected override bool ReleaseLiteRTHandle()
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

        protected override bool ReleaseLiteRTHandle()
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

        protected override bool ReleaseLiteRTHandle()
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

        protected override bool ReleaseLiteRTHandle()
        {
            EngineH.litert_lm_engine_delete(handle);
            return true;
        }
    }

    public sealed class SessionHandle : LiteRTHandle
    {
        public static SessionHandle Null => new(ownsHandle: false);

        /// <summary>The engine which owns this session.</summary>
        public new EngineHandle? OwnerHandle => (EngineHandle?)base.OwnerHandle;
        
        private SessionHandle(bool ownsHandle) : base(ownsHandle) { }
        public SessionHandle() : this(ownsHandle: true) { }

        /// <summary>
        /// Attaches the parent <see cref="EngineHandle"/> to this session so the engine cannot
        /// be destroyed before this session is released.
        /// </summary>
        /// <remarks>
        /// Call ONCE immediately after <see cref="EngineH.litert_lm_engine_create_session"/> or
        /// <see cref="EngineH.litert_lm_session_clone"/> succeeds.
        /// </remarks>
        /// <param name="ownerHandle">The engine handle that owns this session.</param>
        public void DangerousAttachOwner(EngineHandle ownerHandle) =>
            base.DangerousAttachOwner(ownerHandle);
        
        protected override bool ReleaseLiteRTHandle()
        {
            EngineH.litert_lm_session_delete(handle);
            return true;
        }
    }

    public sealed class ResponsesHandle : LiteRTHandle
    {
        public static ResponsesHandle Null => new(ownsHandle: false);
        
        private ResponsesHandle(bool ownsHandle) : base(ownsHandle) { }
        public ResponsesHandle() : this(ownsHandle: true) { }

        protected override bool ReleaseLiteRTHandle()
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

        protected override bool ReleaseLiteRTHandle()
        {
            EngineH.litert_lm_benchmark_info_delete(handle);
            return true;
        }
    }

    public sealed class StreamChunkHandle : LiteRTHandle
    {
        public static StreamChunkHandle Null => new(handle: IntPtr.Zero);
        
        public StreamChunkHandle(IntPtr handle) : base(handle, ownsHandle: false) { }
        protected override bool ReleaseLiteRTHandle() => false;
    }

    public sealed class TokenizeResultHandle : LiteRTHandle
    {
        public static TokenizeResultHandle Null => new(ownsHandle: false);
        
        private TokenizeResultHandle(bool ownsHandle) : base(ownsHandle) { }
        public TokenizeResultHandle() : this(ownsHandle: true) { }

        protected override bool ReleaseLiteRTHandle()
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

        protected override bool ReleaseLiteRTHandle()
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

        protected override bool ReleaseLiteRTHandle()
        {
            EngineH.litert_lm_token_union_delete(handle);
            return true;
        }
    }

    public sealed class BorrowedTokenUnionHandle : TokenUnionHandle
    {
        public static new BorrowedTokenUnionHandle Null => new(ownsHandle: false);

        /// <summary>The TokenUnions which owns this union.</summary>
        public new TokenUnionsHandle? OwnerHandle => (TokenUnionsHandle?)base.OwnerHandle;

        private BorrowedTokenUnionHandle(bool ownsHandle) : base(ownsHandle) { }
        public BorrowedTokenUnionHandle() : this(ownsHandle: true) { }
        
        /// <summary>
        /// Attaches the owning <see cref="TokenUnionsHandle"/> collection so it cannot be destroyed
        /// while this borrowed token union is still in use.
        /// </summary>
        /// <remarks>
        /// Call ONCE immediately after <see cref="EngineH.litert_lm_token_unions_get_token_at"/> succeeds.
        /// </remarks>
        /// <param name="ownerHandle">The collection handle that owns this token union.</param>
        public void DangerousAttachOwner(TokenUnionsHandle ownerHandle) =>
            base.DangerousAttachOwner(ownerHandle);

        protected override bool ReleaseLiteRTHandle() => true;
    }
    
    public sealed class TokenUnionsHandle : LiteRTHandle
    {
        public static TokenUnionsHandle Null => new(ownsHandle: false);
        
        private TokenUnionsHandle(bool ownsHandle) : base(ownsHandle) { }
        public TokenUnionsHandle() : this(ownsHandle: true) { }

        protected override bool ReleaseLiteRTHandle()
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

        protected override bool ReleaseLiteRTHandle()
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

        protected override bool ReleaseLiteRTHandle()
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

        protected override bool ReleaseLiteRTHandle()
        {
            ConversationH.litert_lm_conversation_optional_args_delete(handle);
            return true;
        }
    }
    
    public sealed class ConversationHandle : LiteRTHandle
    {
        public static ConversationHandle Null => new(ownsHandle: false);

        /// <summary>The engine which owns this conversation.</summary>
        public new EngineHandle? OwnerHandle => (EngineHandle?)base.OwnerHandle;
        
        private ConversationHandle(bool ownsHandle) : base(ownsHandle) { }
        public ConversationHandle() : this(ownsHandle: true) { }
        
        /// <summary>
        /// Attaches the parent <see cref="EngineHandle"/> to this conversation so the engine cannot
        /// be destroyed before this conversation is released.
        /// </summary>
        /// <remarks>
        /// Call ONCE immediately after <see cref="ConversationH.litert_lm_conversation_create"/> or
        /// <see cref="ConversationH.litert_lm_conversation_clone"/> succeeds.
        /// </remarks>
        /// <param name="ownerHandle">The engine handle that owns this conversation.</param>
        public void DangerousAttachOwner(EngineHandle ownerHandle) =>
            base.DangerousAttachOwner(ownerHandle);

        protected override bool ReleaseLiteRTHandle()
        {
            ConversationH.litert_lm_conversation_delete(handle);
            return true;
        }
    }
    
    public sealed class JsonResponseHandle : LiteRTHandle
    {
        public static JsonResponseHandle Null => new(ownsHandle: false);
        
        private JsonResponseHandle(bool ownsHandle) : base(ownsHandle) { }
        public JsonResponseHandle() : this(ownsHandle: true) { }

        protected override bool ReleaseLiteRTHandle()
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
        
        protected override bool ReleaseLiteRTHandle()
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
        
        protected override bool ReleaseLiteRTHandle()
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
        
        protected override bool ReleaseLiteRTHandle()
        {
            EmbeddingEngineH.litert_lm_embedding_response_delete(handle);
            return true;
        }
    }
    
    public sealed class BorrowedEmbeddingResponseHandle : EmbeddingResponseHandle
    {
        public static new BorrowedEmbeddingResponseHandle Null => new(ownsHandle: false);

        /// <summary>The EmbeddingResponsesHandle which owns this response.</summary>
        public new EmbeddingResponsesHandle? OwnerHandle => (EmbeddingResponsesHandle?)base.OwnerHandle;

        private BorrowedEmbeddingResponseHandle(bool ownsHandle) : base(ownsHandle) { }
        public BorrowedEmbeddingResponseHandle() : this(ownsHandle: true) { }

        /// <summary>
        /// Attaches the owning <see cref="EmbeddingResponsesHandle"/> collection so it cannot be
        /// destroyed while this borrowed response is still in use.
        /// </summary>
        /// <remarks>
        /// Call ONCE immediately after <see cref="EmbeddingEngineH.litert_lm_embedding_responses_get_at"/> succeeds.
        /// </remarks>
        /// <param name="ownerHandle">The collection handle that owns this embedding response.</param>
        public void DangerousAttachOwner(EmbeddingResponsesHandle ownerHandle) =>
            base.DangerousAttachOwner(ownerHandle);
        
        protected override bool ReleaseLiteRTHandle() => true;
    }
    
    public sealed class EmbeddingResponsesHandle : LiteRTHandle
    {
        public static EmbeddingResponsesHandle Null => new(ownsHandle: false);
        
        private EmbeddingResponsesHandle(bool ownsHandle) : base(ownsHandle) { }
        public EmbeddingResponsesHandle() : this(ownsHandle: true) { }
        
        protected override bool ReleaseLiteRTHandle()
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
        
        protected override bool ReleaseLiteRTHandle()
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
        
        protected override bool ReleaseLiteRTHandle()
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
        
        protected override bool ReleaseLiteRTHandle()
        {
            ExperimentalH.litert_lm_experimental_session_debug_info_delete(handle);
            return true;
        }
    }
    
    #endregion
    
    #region OmniH

    public sealed class OmniTtsEngineSettingsHandle : LiteRTHandle
    {
        public static OmniTtsEngineSettingsHandle Null => new(ownsHandle: false);
        private OmniTtsEngineSettingsHandle(bool ownsHandle) : base(ownsHandle) { }
        public OmniTtsEngineSettingsHandle() : this(ownsHandle: true) { }
        protected override bool ReleaseLiteRTHandle()
        {
            OmniH.litert_omni_tts_engine_settings_delete(handle);
            return true;
        }
    }

    public sealed class OmniTtsEngineHandle : LiteRTHandle
    {
        public static OmniTtsEngineHandle Null => new(ownsHandle: false);
        private OmniTtsEngineHandle(bool ownsHandle) : base(ownsHandle) { }
        public OmniTtsEngineHandle() : this(ownsHandle: true) { }
        protected override bool ReleaseLiteRTHandle()
        {
            OmniH.litert_omni_tts_engine_delete(handle);
            return true;
        }
    }

    public sealed class OmniTtsSessionHandle : LiteRTHandle
    {
        public static OmniTtsSessionHandle Null => new(ownsHandle: false);
        public new OmniTtsEngineHandle? OwnerHandle => (OmniTtsEngineHandle?)base.OwnerHandle;
        private OmniTtsSessionHandle(bool ownsHandle) : base(ownsHandle) { }
        public OmniTtsSessionHandle() : this(ownsHandle: true) { }
        public void DangerousAttachOwner(OmniTtsEngineHandle ownerHandle) =>
            base.DangerousAttachOwner(ownerHandle);
        protected override bool ReleaseLiteRTHandle()
        {
            OmniH.litert_omni_tts_session_delete(handle);
            return true;
        }
    }

    public sealed class OmniAudioOutputHandle : LiteRTHandle
    {
        public static OmniAudioOutputHandle Null => new(ownsHandle: false);
        private OmniAudioOutputHandle(bool ownsHandle) : base(ownsHandle) { }
        public OmniAudioOutputHandle() : this(ownsHandle: true) { }
        protected override bool ReleaseLiteRTHandle()
        {
            OmniH.litert_omni_audio_output_delete(handle);
            return true;
        }
    }

    public sealed class OmniAsrEngineSettingsHandle : LiteRTHandle
    {
        public static OmniAsrEngineSettingsHandle Null => new(ownsHandle: false);
        private OmniAsrEngineSettingsHandle(bool ownsHandle) : base(ownsHandle) { }
        public OmniAsrEngineSettingsHandle() : this(ownsHandle: true) { }
        protected override bool ReleaseLiteRTHandle()
        {
            OmniH.litert_omni_asr_engine_settings_delete(handle);
            return true;
        }
    }

    public sealed class OmniAsrEngineHandle : LiteRTHandle
    {
        public static OmniAsrEngineHandle Null => new(ownsHandle: false);
        private OmniAsrEngineHandle(bool ownsHandle) : base(ownsHandle) { }
        public OmniAsrEngineHandle() : this(ownsHandle: true) { }
        protected override bool ReleaseLiteRTHandle()
        {
            OmniH.litert_omni_asr_engine_delete(handle);
            return true;
        }
    }

    public sealed class OmniAsrSessionHandle : LiteRTHandle
    {
        public static OmniAsrSessionHandle Null => new(ownsHandle: false);
        public new OmniAsrEngineHandle? OwnerHandle => (OmniAsrEngineHandle?)base.OwnerHandle;
        private OmniAsrSessionHandle(bool ownsHandle) : base(ownsHandle) { }
        public OmniAsrSessionHandle() : this(ownsHandle: true) { }
        public void DangerousAttachOwner(OmniAsrEngineHandle ownerHandle) =>
            base.DangerousAttachOwner(ownerHandle);
        protected override bool ReleaseLiteRTHandle()
        {
            OmniH.litert_omni_asr_session_delete(handle);
            return true;
        }
    }

    public sealed class OmniTextOutputHandle : LiteRTHandle
    {
        public static OmniTextOutputHandle Null => new(ownsHandle: false);
        private OmniTextOutputHandle(bool ownsHandle) : base(ownsHandle) { }
        public OmniTextOutputHandle() : this(ownsHandle: true) { }
        protected override bool ReleaseLiteRTHandle()
        {
            OmniH.litert_omni_text_output_delete(handle);
            return true;
        }
    }

    #endregion
}