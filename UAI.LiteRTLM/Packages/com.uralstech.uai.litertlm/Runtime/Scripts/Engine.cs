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
using Uralstech.UAI.LiteRTLM.Native;

#nullable enable
namespace Uralstech.UAI.LiteRTLM
{
    /// <summary>Sampler parameters for a model.</summary>
    public sealed class SamplerParams : LiteRTLMWrapper<SamplerParamsHandle>
    {
        /// <summary>Creates LiteRT LM Sampler Parameters with a specific sampler type.</summary>
        /// <param name="type">The sampler type to use.</param>
        public SamplerParams(SamplerType type)
        {
            LiteRTLMException.Check(
                EngineH.litert_lm_sampler_params_create(type, out SamplerParamsHandle handle));
            SetHandle(handle);
        }

        /// <summary>Sets the top-k value.</summary>
        /// <param name="topK">The top-k value.</param>
        public void SetTopK(int topK) =>
            LockedExecute(handle => EngineH.litert_lm_sampler_params_set_top_k(handle, topK));
        
        /// <summary>Sets the top-p value.</summary>
        /// <param name="topP">The top-p value.</param>
        public void SetTopP(float topP) =>
            LockedExecute(handle => EngineH.litert_lm_sampler_params_set_top_p(handle, topP));
        
        /// <summary>Sets the temperature.</summary>
        /// <param name="temperature">The temperature.</param>
        public void SetTemperature(float temperature) =>
            LockedExecute(handle => EngineH.litert_lm_sampler_params_set_temperature(handle, temperature));
        
        /// <summary>Sets the seed.</summary>
        /// <param name="seed">The seed.</param>
        public void SetSeed(int seed) =>
            LockedExecute(handle => EngineH.litert_lm_sampler_params_set_seed(handle, seed));
    }

    public sealed class SessionConfig : LiteRTLMWrapper<SessionConfigHandle>
    {
        /// <summary>Creates a LiteRT LM Session Config.</summary>
        public SessionConfig()
        {
            LiteRTLMException.Check(
                EngineH.litert_lm_session_config_create(out SessionConfigHandle handle));
            SetHandle(handle);
        }
        
        /// <summary>
        /// Sets the maximum number of output tokens per decode step for this session.
        /// For thinking models, both thinking (reasoning) tokens and the final response
        /// tokens count towards this limit.
        /// </summary>
        /// <param name="maxOutputTokens">The maximum number of tokens to generate (including thinking tokens).</param>
        public void SetMaxOutputTokens(int maxOutputTokens) =>
            LockedExecute(handle => EngineH.litert_lm_session_config_set_max_output_tokens(handle, maxOutputTokens));
        
        /// <summary>Sets whether to apply prompt template for this session.</summary>
        /// <param name="applyPromptTemplate">Whether to apply prompt template.</param>
        public void SetApplyPromptTemplate(bool applyPromptTemplate) =>
            LockedExecute(handle => EngineH.litert_lm_session_config_set_apply_prompt_template(handle, applyPromptTemplate));
    }
}