using Microsoft.EntityFrameworkCore;
using WaZuF.Data;
using WaZuF.Models;
using WaZuF.ViewModels;

namespace WaZuF.Services
{
    public class JopService : IJopService
    {
        private readonly AppDbContext _db;
        private readonly ILogger<JopService> _logger;
        private readonly IGeminiService _geminiService;

        public JopService(AppDbContext db, ILogger<JopService> logger, IGeminiService geminiService)
        {
            _db = db;
            _geminiService = geminiService;
            _logger = logger;
        }
        public async Task CreateAsync(CreateJopViewModel viewModel, string companyId)
        {
            using var transaction = await _db.Database.BeginTransactionAsync();

            try
            {
                var jobRequest = new JobRequest
                {
                    JobTitle = viewModel.JobTitle,
                    Description = viewModel.Description,
                    RequiredSkills = viewModel.RequiredSkills,
                    NumberOfQuestions = viewModel.NumberOfQuestions,
                    DifficultyLevel = viewModel.DifficultyLevel,
                    CompanyId = companyId
                };

                _db.JobRequests.Add(jobRequest);
                await _db.SaveChangesAsync();

                var questions = await _geminiService.GenerateQuizQuestionsAsync(jobRequest);

                foreach (var question in questions)
                {
                    // Now using Models.Question with JobRequestId

                    question.JobRequestId = jobRequest.Id;
                    _db.Questions.Add(question);
                }

                await _db.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
