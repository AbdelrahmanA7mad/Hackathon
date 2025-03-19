using Microsoft.EntityFrameworkCore;
using WaZuF.Data;
using WaZuF.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WaZuF.Services
{
    public class JobRequestService : IJobRequestService
    {
        private readonly AppDbContext _context;

        public JobRequestService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Employee>> GetAllEmployee(int id)
        {
            return await _context.Employees
                .Where(e => e.JobRequestId == id)
                .ToListAsync();
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

        public async Task<int> GetTotalJobsAsync(string companyId)
        {
            return await _context.JobRequests
                                 .Where(j => j.CompanyId == companyId)
                                 .CountAsync();
        }

        public async Task<List<JobRequest>> GetJobRequestsByCompanyIdAsync(string companyId)
        {
            return await _context.JobRequests
                                 .Where(j => j.CompanyId == companyId)
                                 .ToListAsync();
        }

        public async Task<List<JobRequest>> GetLatestJobRequestsAsync(string companyId, int count = 5)
        {
            return await _context.JobRequests
                                 .Where(j => j.CompanyId == companyId)
                                 .OrderByDescending(j => j.Id)
                                 .Take(count)
                                 .ToListAsync();
        }
        public async Task<bool> DeleteJobRequestAsync(int jobId, string companyId)
        {
            var job = await _context.JobRequests.FirstOrDefaultAsync(j => j.Id == jobId && j.CompanyId == companyId);
            if (job == null) return false;

            _context.JobRequests.Remove(job);
            await _context.SaveChangesAsync();
            return true;
        }

  
    }
}
