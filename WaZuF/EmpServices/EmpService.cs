using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using WaZuF.Data;
using WaZuF.Models;

namespace WaZuF.EmpServices
{

    public class EmpService : IEmpService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;
        private readonly AppDbContext _db;

        public EmpService(HttpClient httpClient, IConfiguration config ,AppDbContext db)
        {
            _httpClient = httpClient;
            _config = config;
            _db = db;
        }

        public async Task<string> GenerateCodingTasksAsync(CodeQuiz viewModel)
        {
            var prompt = $"You are an expert programming instructor. Generate a coding problem based on the following description:\n\n{viewModel.Description}\n\nFollow these rules:\n1. The problem must require writing a function or algorithm to solve it.\n2. Provide clear input and output examples.\n3. Return the response as plain text without formatting.\n4. Do NOT include solutions, only problem statements.";

            string apiResponse = await CallGeminiApiWithRetry(prompt, 3);

            // استخراج السؤال فقط من JSON
            var jsonResponse = JsonConvert.DeserializeObject<dynamic>(apiResponse);
            string questionText = jsonResponse?.candidates[0]?.content?.parts[0]?.text ?? "Failed to generate question.";

            //Exam exam = new Exam
            //{
            //    QuizId = viewModel.Id,   // تأكد أن viewModel.Id يحتوي على قيمة صحيحة
            //    description = viewModel.Description,
            //    Question = questionText, // حفظ السؤال في قاعدة البيانات
            //};

            //_db.Exams.Add(exam);
            //await _db.SaveChangesAsync(); // استخدم await لحفظ التغييرات بشكل متزامن

            return questionText;
        }



        public async Task<string> CheckSolutionAsync(string problemStatement, string userSolution)
        {
            var prompt = $"You are an AI coding evaluator. Evaluate the following solution for correctness.\n\nProblem: {problemStatement}\nUser Solution:\n```\n{userSolution}\n```\n\nProvide feedback:\n- If correct, say: 'Correct! Here is your next challenge.'\n- If incorrect, explain why and ask the user to retry.";
            return await CallGeminiApiWithRetry(prompt, 3);
        }

        public async Task<string> EvaluateCodeAsync(CodeSubmission submission)
        {
            var prompt = $"Evaluate the following code:\n\n{submission.Code}\n\nProvide feedback on correctness and improvements.";
            return await CallGeminiApiWithRetry(prompt, 3);
        }

        private async Task<string> CallGeminiApiWithRetry(string prompt, int retryCount)
        {
            string apiKey = GetApiKey();
            for (int attempt = 0; attempt < retryCount; attempt++)
            {
                try
                {
                    var requestBody = new
                    {
                        contents = new[] { new { parts = new[] { new { text = prompt } } } },
                        safetySettings = new[] { new { category = "HARM_CATEGORY_DANGEROUS_CONTENT", threshold = "BLOCK_NONE" } }
                    };
                    var requestJson = JsonConvert.SerializeObject(requestBody);
                    var response = await _httpClient.PostAsync(
                        $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash:generateContent?key={apiKey}",
                        new StringContent(requestJson, Encoding.UTF8, "application/json"));
                    var content = await response.Content.ReadAsStringAsync();
                    if (!response.IsSuccessStatusCode)
                    {
                        throw new ApplicationException($"API Error {response.StatusCode}: {content}");
                    }
                    return content;
                }
                catch (HttpRequestException) when (attempt < retryCount - 1)
                {
                    await Task.Delay(2000);
                }
            }
            throw new ApplicationException("Failed to connect to Gemini API after multiple attempts.");
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


}