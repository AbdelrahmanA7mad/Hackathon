using Newtonsoft.Json;
using System.Collections.Generic;

namespace WaZuF.Models
{
    public class GeminiResponse
    {
        [JsonProperty("candidates")]
        public List<Candidate> Candidates { get; set; }
    }

    public class Candidate
    {
        [JsonProperty("content")]
        public Content Content { get; set; }
    }

    public class Content
    {
        [JsonProperty("parts")]
        public List<Part> Parts { get; set; }
    }

    public class Part
    {
        [JsonProperty("text")]
        public string Text { get; set; }
    }
}
