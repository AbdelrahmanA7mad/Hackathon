using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WaZuF.Services;
using Microsoft.AspNetCore.Identity;
using WaZuF.Models;

namespace WaZuF.Controllers
{
    [Authorize(Policy = "CompanyOnly")]

    public class MainController : Controller
    {
        private readonly IJobRequestService _jobRequestService;
        private readonly UserManager<AppUser> _userManager;

        public MainController(IJobRequestService jobRequestService, UserManager<AppUser> userManager)
        {
            _jobRequestService = jobRequestService;
            _userManager = userManager;
        }

        [Authorize]
        public async Task<IActionResult> Dashboard()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            // Retrieve all job requests for the company
            var jobRequests = await _jobRequestService.GetJobRequestsByCompanyIdAsync(user.Id);

            // Count jobs and employees
            var totalJobs = jobRequests.Count;
            var totalEmp = await _jobRequestService.GetTotalempsAsync(user.Id);

            // Get latest job requests
            var latestJobs = await _jobRequestService.GetLatestJobRequestsAsync(user.Id, 5);

            // Pass data to the view
            ViewBag.TotalJobs = totalJobs;
            ViewBag.TotalEmp = totalEmp;
            ViewBag.LatestJobs = latestJobs;

            return View();
        }

    }
}