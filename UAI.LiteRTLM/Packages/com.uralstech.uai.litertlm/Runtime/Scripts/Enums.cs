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

#nullable enable
namespace Uralstech.UAI.LiteRTLM
{
    /// <summary>Represents the log severity / level.</summary>
    public enum LogSeverity : int
    {
        Verbose = 0,
        Debug   = 1,
        Info    = 2,
        Warning = 3,
        Error   = 4,
        Fatal   = 5,
        Silent  = 1000,
    }
    
    /// <summary>Represents the type of sampler.</summary>
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

    /// <summary>Represents the type of input data.</summary>
    public enum InputDataType : int
    {
        /// <summary>A UTF-8 string.</summary>
        Text,
        Image,
        ImageEnd,
        Audio,
        AudioEnd,
    }
    
    /// <summary>Represents the activation data type.</summary>
    public enum ActivationDataType : int
    {
        /// <summary>Use float32 as the activation data type.</summary>
        Float32 = 0,
        
        /// <summary>Use float16 as the activation data type.</summary>
        Float16 = 1,
        
        /// <summary>Use int16 as the activation data type.</summary>
        Int16   = 2,
        
        /// <summary>Use int8 as the activation data type.</summary>
        Int8    = 3,
    }
    
    /// <summary>Represents the type of a TokenUnion.</summary>
    public enum TokenUnionType : int
    {
        String  = 0,
        Ids     = 1,
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
        LlGuidance = 1,
    }

    /// <summary>Input and output modalities supported by LiteRT-LM models.</summary>
    public enum Modality : int
    {
        Text    = 0,
        Vision  = 1,
        Audio   = 2,
        Video   = 3,
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

    public static class BackendNames
    {
        public const string CPU = "cpu";
        public const string GPU = "gpu";
        public const string NPU = "npu";
        public const string GPUArtisan = "gpu_artisan";
        public const string CPUArtisan = "cpu_artisan";
        public const string GoogleTensorArtisan = "google_tensor_artisan";
    }
}