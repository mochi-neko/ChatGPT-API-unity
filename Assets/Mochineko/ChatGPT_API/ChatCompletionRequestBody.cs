#nullable enable
using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Mochineko.ChatGPT_API
{
    /// <summary>
    /// Request body of ChatGPT chat completion API.
    /// See https://platform.openai.com/docs/api-reference/chat/create
    /// </summary>
    [JsonObject]
    public sealed class ChatCompletionRequestBody
    {
        /// <summary>
        /// [Required]
        /// ID of the model to use.
        /// Currently, only gpt-3.5-turbo and gpt-3.5-turbo-0301 are supported.
        /// </summary>
        [JsonProperty("model"), JsonRequired]
        public string Model { get; }

        /// <summary>
        /// [Required]
        /// The messages to generate chat completions for, in the chat format.
        /// </summary>
        [JsonProperty("messages"), JsonRequired]
        public IReadOnlyList<Message> Messages { get; }

        /// <summary>
        /// [Optional] Defaults to null.
        /// A list of functions the model may generate JSON inputs for.
        /// </summary>
        [JsonProperty("functions")]
        public IReadOnlyList<Function>? Functions { get; }
        
        /// <summary>
        /// [Optional] Defaults to "auto".
        /// Controls how the model responds to function calls.
        /// "none" means the model does not call a function, and responds to the end-user.
        /// "auto" means the model can pick between an end-user or calling a function.
        /// Specifying a particular function via {"name":\ "my_function"} forces the model to call that function.
        /// "none" is the default when no functions are present.
        /// "auto" is the default if functions are present.
        /// </summary>
        [JsonProperty("function_call")]
        public object? FunctionCall { get; }

        /// <summary>
        /// [Optional] Defaults to 1.
        /// What sampling temperature to use, between 0 and 2.
        /// Higher values like 0.8 will make the output more random, while lower values like 0.2 will make it more focused and deterministic.
        /// We generally recommend altering this or top_p but not both.
        /// </summary>
        [JsonProperty("temperature")]
        public float? Temperature { get; }

        /// <summary>
        /// [Optional] Defaults to 1.
        /// An alternative to sampling with temperature, called nucleus sampling, where the model considers the results of the tokens with top_p probability mass.
        /// So 0.1 means only the tokens comprising the top 10% probability mass are considered.
        /// We generally recommend altering this or temperature but not both.
        /// </summary>
        [JsonProperty("top_p")]
        public float? TopP { get; }

        /// <summary>
        /// [Optional] Defaults to 1.
        /// How many chat completion choices to generate for each input message.
        /// </summary>
        [JsonProperty("n")]
        public uint? N { get; }

        /// <summary>
        /// [Optional] Defaults to false.
        /// If set, partial message deltas will be sent, like in ChatGPT.
        /// Tokens will be sent as data-only server-sent events as they become available, with the stream terminated by a data: [DONE] message.
        /// </summary>
        [JsonProperty("stream")]
        public bool? Stream { get; }

        /// <summary>
        /// [Optional] Defaults to null.
        /// Up to 4 sequences where the API will stop generating further tokens.
        /// </summary>
        [JsonProperty("stop")]
        public string[]? Stop { get; }

        /// <summary>
        /// [Optional] Defaults to inf.
        /// The maximum number of tokens allowed for the generated answer.
        /// By default, the number of tokens the model can return will be (4096 - prompt tokens).
        /// </summary>
        [JsonProperty("max_tokens")]
        public int? MaxTokens { get; }

        /// <summary>
        /// [Optional] Defaults to 0.
        /// Number between -2.0 and 2.0.
        /// Positive values penalize new tokens based on whether they appear in the text so far, increasing the model's likelihood to talk about new topics.
        /// See more information about frequency and presence penalties.
        /// https://platform.openai.com/docs/api-reference/parameter-details
        /// </summary>
        [JsonProperty("presence_penalty")]
        public float? PresencePenalty { get; }

        /// <summary>
        /// [Optional] Defaults to 0.
        /// Number between -2.0 and 2.0.
        /// Positive values penalize new tokens based on their existing frequency in the text so far, decreasing the model's likelihood to repeat the same line verbatim.
        /// See more information about frequency and presence penalties.
        /// https://platform.openai.com/docs/api-reference/parameter-details
        /// </summary>
        [JsonProperty("frequency_penalty")]
        public float? FrequencyPenalty { get; }

        /// <summary>
        /// [Optional] Defaults to null.
        /// Modify the likelihood of specified tokens appearing in the completion.
        /// Accepts a json object that maps tokens (specified by their token ID in the tokenizer) to an associated bias value from -100 to 100.
        /// Mathematically, the bias is added to the logits generated by the model prior to sampling.
        /// The exact effect will vary per model, but values between -1 and 1 should decrease or increase likelihood of selection; values like -100 or 100 should result in a ban or exclusive selection of the relevant token.
        /// </summary>
        [JsonProperty("logit_bias")]
        public Dictionary<int, int>? LogitBias { get; }

        /// <summary>
        /// [Optional]
        /// A unique identifier representing your end-user, which can help OpenAI to monitor and detect abuse. Learn more.
        /// https://platform.openai.com/docs/guides/safety-best-practices/end-user-ids
        /// </summary>
        [JsonProperty("user")]
        public string? User { get; }

        public ChatCompletionRequestBody(
            string model,
            IReadOnlyList<Message> messages,
            IReadOnlyList<Function>? functions = null,
            string? functionCallString = null,
            FunctionCallSpecifying? functionCallSpecifying = null,
            float? temperature = 1f,
            float? topP = 1f,
            uint? n = 1,
            bool? stream = false,
            string[]? stop = null,
            int? maxTokens = null,
            float? presencePenalty = 0f,
            float? frequencyPenalty = 0f,
            Dictionary<int, int>? logitBias = null,
            string? user = null)
        {
            this.Model = model;
            this.Messages = messages;
            this.Functions = functions;

            if (functionCallString != null && functionCallSpecifying != null)
            {
                throw new ArgumentException($"Cannot specify both {nameof(functionCallString)} and {nameof(functionCallSpecifying)}.");
            }
            if (functionCallString != null)
            {
                this.FunctionCall = functionCallString;
            }
            else if (functionCallSpecifying != null)
            {
                this.FunctionCall = functionCallSpecifying;
            }
            else
            {
                this.FunctionCall = null;
            }
            
            this.Temperature = temperature;
            this.TopP = topP;
            this.N = n;
            this.Stream = stream;
            this.Stop = stop;
            this.MaxTokens = maxTokens;
            this.PresencePenalty = presencePenalty;
            this.FrequencyPenalty = frequencyPenalty;
            this.LogitBias = logitBias;
            this.User = user;
        }

        public string ToJson()
            => JsonConvert.SerializeObject(
                this,
                Formatting.Indented,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                });

        public static ChatCompletionRequestBody? FromJson(string json)
            => JsonConvert.DeserializeObject<ChatCompletionRequestBody>(json);
    }
}