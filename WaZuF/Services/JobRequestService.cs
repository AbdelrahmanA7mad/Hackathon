using Microsoft.EntityFrameworkCore;
using WaZuF.Data;
using WaZuF.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WaZuF.ViewModels;

namespace WaZuF.Services
{
    public class JobRequestService : IJobRequestService
    {
        private readonly AppDbContext _context;

        public JobRequestService(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<List<JobRequest>> GetAllJobRequestsAsync()
        {
            return await _context.JobRequests.ToListAsync();
        }

        public async Task<JobRequest?> GetJobRequestByIdAsync(int id)
        {
            return await _context.JobRequests
                .Include(j => j.Questions)
                .FirstOrDefaultAsync(j => j.Id == id);
        }

        public async Task AddJobRequestAsync(JobRequest jobRequest)
        {
            if (jobRequest == null)
                throw new ArgumentNullException(nameof(jobRequest));

            _context.JobRequests.Add(jobRequest);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateJobRequestAsync(JobRequest jobRequest)
        {
            if (jobRequest == null)
                throw new ArgumentNullException(nameof(jobRequest));

            var existingJobRequest = await _context.JobRequests
                .FirstOrDefaultAsync(j => j.Id == jobRequest.Id);

            if (existingJobRequest != null)
            {
                existingJobRequest.ExamLink = jobRequest.ExamLink;
                _context.JobRequests.Update(existingJobRequest);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new InvalidOperationException($"JobRequest with ID {jobRequest.Id} not found.");
            }
        }

        public async Task DeleteJobRequestAsync(int id)
        {
            var jobRequest = await _context.JobRequests.FindAsync(id);
            if (jobRequest != null)
            {
                _context.JobRequests.Remove(jobRequest);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<int> GetTotalJobsAsync(string companyId)
        {
            if (string.IsNullOrEmpty(companyId))
                throw new ArgumentNullException(nameof(companyId));

            return await _context.JobRequests
                                 .Where(j => j.CompanyId == companyId)
                                 .CountAsync();
        }

        public async Task<List<JobRequest>> GetJobRequestsByCompanyIdAsync(string companyId)
        {
            if (string.IsNullOrEmpty(companyId))
                throw new ArgumentNullException(nameof(companyId));

            return await _context.JobRequests
                                 .Where(j => j.CompanyId == companyId)
                                 .ToListAsync();
        }

        public async Task<List<JobRequest>> GetLatestJobRequestsAsync(string companyId, int count = 5)
        {
            if (string.IsNullOrEmpty(companyId))
                throw new ArgumentNullException(nameof(companyId));

            if (count <= 0)
                throw new ArgumentOutOfRangeException(nameof(count), "Count must be greater than zero.");

            return await _context.JobRequests
                                 .Where(j => j.CompanyId == companyId)
                                 .OrderByDescending(j => j.Id)
                                 .Take(count)
                                 .ToListAsync();
        }

        public async Task<bool> DeleteJobRequestAsync(int jobId, string companyId)
        {
            if (string.IsNullOrEmpty(companyId))
                throw new ArgumentNullException(nameof(companyId));

            var job = await _context.JobRequests
                .FirstOrDefaultAsync(j => j.Id == jobId && j.CompanyId == companyId);

            if (job == null)
                return false;

            _context.JobRequests.Remove(job);
            await _context.SaveChangesAsync();
            return true;
        }


        // الإضافة الجديدة
        public async Task CreateAsync(CreateJopViewModel viewModel, string companyId)
        {
            if (viewModel == null)
                throw new ArgumentNullException(nameof(viewModel));
            if (string.IsNullOrEmpty(companyId))
                throw new ArgumentNullException(nameof(companyId));

            var jobRequest = new JobRequest
            {
                JobTitle = viewModel.JobTitle,
                Description = viewModel.Description,
                RequiredSkills = viewModel.RequiredSkills,
                NumberOfQuestions = viewModel.NumberOfQuestions,
                DifficultyLevel = viewModel.DifficultyLevel,
                CompanyId = companyId
            };

            _context.JobRequests.Add(jobRequest);
            await _context.SaveChangesAsync();
        }
    }
}