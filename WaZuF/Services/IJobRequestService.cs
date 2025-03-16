using WaZuF.Models;

public interface IJobRequestService
{
    Task<List<JobRequest>> GetAllJobRequestsAsync();
    Task<JobRequest?> GetJobRequestByIdAsync(int id);
    Task AddJobRequestAsync(JobRequest jobRequest);
    Task UpdateJobRequestAsync(JobRequest jobRequest);
    Task DeleteJobRequestAsync(int id);
    Task<int> GetTotalJobsAsync();
    Task<List<JobRequest>> GetLatestJobRequestsAsync(int count);

}
