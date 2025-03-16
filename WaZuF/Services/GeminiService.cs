using System.Net.Http.Json;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WaZuF.Models;

namespace WaZuF.Services
{

    public class GeminiService : IGeminiService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;
        private readonly ILogger<GeminiService> _logger;
        private const int MaxRetries = 2;

        public GeminiService(
            HttpClient httpClient,
            IConfiguration config,
            ILogger<GeminiService> logger)
        {
            _httpClient = httpClient;
            _config = config;
            _logger = logger;
        }

        public async Task<List<Question>> GenerateQuizQuestionsAsync(JobRequest jobRequest)
        {
            for (var retry = 0; retry < MaxRetries; retry++)
            {
                try
                {
                    var apiKey = GetApiKey();
                    var prompt = BuildPrompt(jobRequest);
                    var geminiResponse = await CallGeminiApiWithRetry(apiKey, prompt, retry);
                    return ParseQuestions(geminiResponse);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Gemini API attempt {Retry} failed", retry + 1);
                    if (retry == MaxRetries - 1) throw;
                    await Task.Delay(1000 * (retry + 1));
                }
            }
            return new List<Question>();
        }

        private async Task<GeminiResponse> CallGeminiApiWithRetry(string apiKey, string prompt, int retryCount)
        {
            try
            {
                var requestBody = new
                {
                    contents = new[]
                    {
                        new
                        {
                            parts = new[]
                            {
                                new { text = prompt }
                            }
                        }
                    },
                    safetySettings = new[]
                    {
                        new
                        {
                            category = "HARM_CATEGORY_DANGEROUS_CONTENT",
                            threshold = "BLOCK_NONE"
                        }
                    }
                };

                var response = await _httpClient.PostAsJsonAsync(
                    $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent?key={apiKey}",
                    requestBody);

                var content = await response.Content.ReadAsStringAsync();
                _logger.LogDebug("API Response (Attempt {Retry}): {Content}", retryCount + 1, content);

                if (!response.IsSuccessStatusCode)
                {
                    throw new ApplicationException($"API Error {response.StatusCode}: {content}");
                }

                var geminiResponse = JsonConvert.DeserializeObject<GeminiResponse>(content)
                    ?? throw new ApplicationException("Empty API response");

                if (geminiResponse.PromptFeedback?.BlockReason != null)
                {
                    throw new ApplicationException($"Content blocked: {geminiResponse.PromptFeedback.BlockReason}");
                }

                return geminiResponse;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP request failed");
                throw new ApplicationException("API communication error", ex);
            }
        }

        private string BuildPrompt(JobRequest jobRequest)
        {
            return $@"You are a technical hiring expert. Generate {jobRequest.NumberOfQuestions} multiple-choice questions
                about {jobRequest.JobTitle} position. Follow these rules STRICTLY:

                1. Return ONLY VALID JSON ARRAY
                2. No markdown or formatting
                3. Use this EXACT structure:
                    [
                        {{
                            ""Text"": ""Question text"",
                            ""OptionA"": ""..."",
                            ""OptionB"": ""..."",
                            ""OptionC"": ""..."",
                            ""OptionD"": ""..."",
                            ""CorrectAnswer"": ""A""
                        }}
                    ]
                4. Questions should cover: {jobRequest.Description}
                5. Focus on: {jobRequest.RequiredSkills}
                6. Difficulty: {jobRequest.DifficultyLevel}
                7. Ensure options are distinct and plausible";
        }

        private List<Question> ParseQuestions(GeminiResponse response)
        {
            try
            {
                var rawContent = response.Candidates.FirstOrDefault()?.Content?.Parts?.FirstOrDefault()?.Text;
                _logger.LogInformation("Raw API Content: {RawContent}", rawContent);

                if (string.IsNullOrWhiteSpace(rawContent))
                {
                    throw new ApplicationException("Empty response content");
                }

                var jsonContent = CleanJsonResponse(rawContent);
                ValidateJsonStructure(jsonContent);

                return JsonConvert.DeserializeObject<List<Question>>(jsonContent)
                    ?? throw new ApplicationException("Deserialization returned null");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "JSON parsing failed");
                throw new ApplicationException("Failed to parse questions", ex);
            }
        }

        private string CleanJsonResponse(string rawResponse)
        {
            // Remove markdown code blocks
            var cleaned = Regex.Replace(rawResponse, @"^```(json)?|```$", "", RegexOptions.Multiline)
                .Trim();

            // Fix common Gemini formatting issues
            cleaned = Regex.Replace(cleaned, @"\\+", @"\");
            cleaned = cleaned.Replace("\\n", "")
                .Replace("\\t", "")
                .Replace("\\\"", "\"");

            return cleaned;
        }

        private void ValidateJsonStructure(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                throw new ApplicationException("Empty JSON content");
            }

            if (!json.Trim().StartsWith("["))
            {
                _logger.LogError("Invalid JSON start. Content: {Json}", json);
                throw new ApplicationException("Response is not a JSON array");
            }

            try
            {
                var parsed = JToken.Parse(json);
                if (parsed.Type != JTokenType.Array)
                {
                    throw new ApplicationException("Root element is not an array");
                }
            }
            catch (JsonReaderException ex)
            {
                _logger.LogError("Invalid JSON: {Error} | Content: {Json}", ex.Message, json);
                throw;
            }
        }

        private string GetApiKey()
        {
            var apiKey = _config["Gemini:ApiKey"];
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                throw new ApplicationException("Missing Gemini API key");
            }
            return apiKey;
        }
    }

    public class GeminiResponse
    {
        [JsonProperty("candidates")]
        public List<Candidate> Candidates { get; set; } = new();

        [JsonProperty("promptFeedback")]
        public PromptFeedback? PromptFeedback { get; set; }
    }

    public class Candidate
    {
        [JsonProperty("content")]
        public Content? Content { get; set; }
    }

    public class Content
    {
        [JsonProperty("parts")]
        public List<Part> Parts { get; set; } = new();
    }

    public class Part
    {
        [JsonProperty("text")]
        public string? Text { get; set; }
    }

    public class PromptFeedback
    {
        [JsonProperty("blockReason")]
        public string? BlockReason { get; set; }
    }
}