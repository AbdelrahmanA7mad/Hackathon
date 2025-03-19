using Microsoft.EntityFrameworkCore;
using WaZuF.Models;
using System.Threading.Tasks;
using WaZuF.Data;

namespace WaZuF.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly AppDbContext _context; // استبدل YourDbContext باسم الـ DbContext بتاعك

        public EmployeeService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Employee> GetEmployeeByDetailsAsync(string name, string email, int jobRequestId)
        {
            return await _context.Employees
                .FirstOrDefaultAsync(e => e.Name == name && e.Email == email && e.JobRequestId == jobRequestId);
        }
    }
}