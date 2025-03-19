using WaZuF.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using WaZuF.ViewModels;

namespace WaZuF.Services
{
    public interface IJobRequestService
    {
        Task<List<JobRequest>> GetAllJobRequestsAsync();
        Task<List<Employee>> GetAllEmployee(int id); // Assuming this is needed
        Task<JobRequest?> GetJobRequestByIdAsync(int id);
        Task AddJobRequestAsync(JobRequest jobRequest);
        Task UpdateJobRequestAsync(JobRequest jobRequest);
        Task DeleteJobRequestAsync(int id);
        Task<int> GetTotalJobsAsync(string companyId);
        Task<List<JobRequest>> GetJobRequestsByCompanyIdAsync(string companyId);
        Task<List<JobRequest>> GetLatestJobRequestsAsync(string companyId, int count = 5);
        Task<bool> DeleteJobRequestAsync(int jobId, string companyId);
        Task CreateAsync(CreateJopViewModel viewModel, string companyId); // الإضافة الجديدة
    }
}
