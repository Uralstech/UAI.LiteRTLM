// Copyright 2026 URAV ADVANCED LEARNING SYSTEMS PRIVATE LIMITED
// Copyright 2025-2026 The ODML Authors.
//
// Modified by URAV ADVANCED LEARNING SYSTEMS PRIVATE LIMITED:
// Ported to C# enums from LiteRT-LM.
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

#nullable enable
namespace Uralstech.UAI.LiteRTLM.Omni
{
    /// <summary>Canonical status codes returned by LiteRT LM C API functions.</summary>
    /// <remarks>
    /// These values are 1:1 identical to Google canonical error codes
    /// (<c>absl::StatusCode</c> / <c>google.rpc.Code</c>).
    /// </remarks>
    public enum StatusCode : int
    {
        Ok                  = 0,
        Cancelled           = 1,
        Unknown             = 2,
        InvalidArgument     = 3,
        DeadlineExceeded    = 4,
        NotFound            = 5,
        AlreadyExists       = 6,
        PermissionDenied    = 7,
        ResourceExhausted   = 8,
        FailedPrecondition  = 9,
        Aborted             = 10,
        OutOfRange          = 11,
        Unimplemented       = 12,
        Internal            = 13,
        Unavailable         = 14,
        DataLoss            = 15,
        Unauthenticated     = 16,
    }

    /// <summary>Represents the type of a TokenUnion.</summary>
    public enum TokenUnionType : int
    {
        String  = 0,
        Ids     = 1,
    }

    /// <remarks>Represents the type of sampler.</remarks>
    public enum SamplerType : int
    {
        /// <summary>Default fallback/unspecified.</summary>
        Unspecified = 0,

        /// <summary>Probabilistically pick among the top k tokens.</summary>
        TopK        = 1,

        /// <summary>
        /// Probabilistically pick among the tokens such that the sum is greater
        /// than or equal to p tokens after first performing top-k sampling.
        /// </summary>
        TopP        = 2,

        /// <summary>Pick the token with maximum logit (i.e., argmax).</summary>
        Greedy      = 3,
    }

    /// <summary>Represents how speculative decoding (e.g. MTP) is used during decode.</summary>
    /// <remarks>
    /// The modes match the values of the <c>--speculative_decoding</c> flag of the
    /// LiteRT-LM CLI (<c>off</c>, <c>on</c> and <c>adaptive</c>).
    /// </remarks>
    public enum SpeculativeDecodingMode : int
    {
        /// <summary>Never speculate.</summary>
        Off         = 0,

        /// <summary>Speculate on every decode step.</summary>
        On          = 1,

        /// <summary>Speculate only while it is measured to be faster than plain decoding.</summary>
        /// <remarks>
        /// Backends that do not support adaptive speculation speculate on every
        /// decode step, like <c>kLiteRtLmSpeculativeDecodingModeOn</c>.
        /// </remarks>
        Adaptive    = 2,
    }

    /// <summary>Represents the log severity / level.</summary>
    public enum LogSeverity
    {
        Verbose = 0,
        Debug   = 1,
        Info    = 2,
        Warning = 3,
        Error   = 4,
        Fatal   = 5,
        Silent  = 1000,
    }

    /// <summary>Represents the type of input data.</summary>
    public enum InputDataType : int
    {
        Text        = 0,
        Image       = 1,
        ImageEnd    = 2,
        Audio       = 3,
        AudioEnd    = 4,
    }

    /// <summary>Represents the activation data type.</summary>
    public enum ActivationDataType : int
    {
        Float32 = 0,
        Float16 = 1,
        Int16   = 2,
        Int8    = 3,
    }

    /// <summary>Represents the type of constraint for constrained decoding.</summary>
    public enum ConstraintType : int
    {
        None        = 0,
        Regex       = 1,
        JsonSchema  = 2,
    }

    /// <summary>Represents the type of constraint provider.</summary>
    public enum ConstraintProviderType : int
    {
        /// <summary>No provider is set, and the conversation uses its default.</summary>
        None        = 0,
        LlGuidance  = 1,
    }

    /// <summary>Strategy for handling inputs longer than the maximum supported signature length.</summary>
    public enum InputOverflowStrategy : int
    {
        /// <summary>Chunks the input into sub-sequences, embeds each chunk, and returns the mean embedding across all chunks.</summary>
        ChunkAndAverage = 0,

        /// <summary>Truncates the input to the longest signature length.</summary>
        Truncate        = 1,

        /// <summary>Returns an error status if the input exceeds the longest signature length.</summary>
        Error           = 2,
    }

    /// <summary>Input and output modalities supported by LiteRT-LM models.</summary>
    public enum Modality : int
    {
        Text    = 0,
        Vision  = 1,
        Audio   = 2,
        Video   = 3,
    }

    /// <summary>Hardware backend type.</summary>
    public enum BackendType : int
    {
        Cpu = 1,
        Gpu = 2,
        Npu = 3,
    }

    /// <summary>NPU brand options.</summary>
    public enum NpuBrand : int
    {
        Unknown         = 0,
        Qualcomm        = 1,
        GoogleTensor    = 2,
        MediaTek        = 3,
        Intel           = 4,
        Samsung         = 5,
    }

    /// <summary>Model type of the loaded LiteRT-LM file.</summary>
    public enum ModelType : int
    {
        Unknown     = 0,
        Llm         = 1,
        Embedding   = 2,
    }
    
    /// <summary>Text-to-speech models supported by Omni.</summary>
    public enum OmniTtsModelType : int
    {
        Unspecified = 0,
        Kokoro = 1,
        Qwen3Tts = 2,
    }

    /// <summary>Speech-recognition models supported by Omni.</summary>
    public enum OmniAsrModelType : int
    {
        Unspecified = 0,
        ParakeetTdt = 1,
        ParakeetCtc = 2,
        Whisper = 3,
        Qwen3Asr = 4,
        Moonshine = 5,
        TinyGemma = 6,
    }

    /// <summary>Describes the result of a successful incremental Omni operation.</summary>
    public enum OmniStreamStatus : int
    {
        InputAccepted = 0,
        OutputAvailable = 1,
        NeedMoreInput = 2,
        EndOfStream = 3,
    }
    
    public static class BackendNames
    {
        public const string CPU = "cpu";
        public const string GPU = "gpu";
        public const string NPU = "npu";
    }
}