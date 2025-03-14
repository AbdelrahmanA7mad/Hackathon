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

        public JopService(AppDbContext db, ILogger<JopService> logger)
        {
            _db = db;
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
                await transaction.CommitAsync();
            }
            catch (DbUpdateException ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Database error creating job");
                throw new ApplicationException("Database error while saving job", ex);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error creating job");
                throw;
            }
        }
    }
}
