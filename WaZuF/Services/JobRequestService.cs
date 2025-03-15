namespace WaZuF.Services
{
    using Microsoft.EntityFrameworkCore;
    using WaZuF.Data;
    using WaZuF.Models;

    public class JobRequestService : IJobRequestService
    {
        private readonly AppDbContext _context;

        public JobRequestService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<JobRequest>> GetAllJobRequestsAsync()
        {
            return await _context.JobRequests.ToListAsync();
        }

        public async Task<JobRequest?> GetJobRequestByIdAsync(int id)
        {
            return await _context.JobRequests.FindAsync(id);
        }

        public async Task AddJobRequestAsync(JobRequest jobRequest)
        {
            _context.JobRequests.Add(jobRequest);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateJobRequestAsync(JobRequest jobRequest)
        {
            _context.JobRequests.Update(jobRequest);
            await _context.SaveChangesAsync();
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

        public async Task<int> GetTotalJobsAsync()
        {
            return await _context.JobRequests.CountAsync();
        }

        public async Task<List<JobRequest>> GetLatestJobRequestsAsync(int count)
        {
            return await _context.JobRequests
                .OrderByDescending(j => j.Id)
                .Take(count)
                .ToListAsync();
        }
    }
}
