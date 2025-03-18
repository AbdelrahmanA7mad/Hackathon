using WaZuF.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IJobRequestService
{
    Task<List<JobRequest>> GetAllJobRequestsAsync();
    Task<JobRequest?> GetJobRequestByIdAsync(int id);
    Task AddJobRequestAsync(JobRequest jobRequest);
    Task UpdateJobRequestAsync(JobRequest jobRequest);
    Task DeleteJobRequestAsync(int id);
    Task<int> GetTotalJobsAsync(string companyId);
    Task<List<JobRequest>> GetJobRequestsByCompanyIdAsync(string companyId);
    Task<List<JobRequest>> GetLatestJobRequestsAsync(string companyId, int count = 5);
}
