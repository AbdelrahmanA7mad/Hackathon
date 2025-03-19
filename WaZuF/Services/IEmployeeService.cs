using WaZuF.Models;
using System.Threading.Tasks;

namespace WaZuF.Services
{
    public interface IEmployeeService
    {
        Task<Employee> GetEmployeeByDetailsAsync(string name, string email, int jobRequestId);
    }
}