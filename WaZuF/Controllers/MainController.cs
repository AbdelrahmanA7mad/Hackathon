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
        private readonly UserManager<Company> _userManager;

        public MainController(IJobRequestService jobRequestService, UserManager<Company> userManager)
        {
            _jobRequestService = jobRequestService;
            _userManager = userManager;
        }

        [Authorize]
        public async Task<IActionResult> Dashboard()
        {

            var user = await _userManager.GetUserAsync(User);
            if (user == null) return Unauthorized();

            var totalJobs = await _jobRequestService.GetTotalJobsAsync(user.Id);
            var latestJobs = await _jobRequestService.GetLatestJobRequestsAsync(user.Id, 5);


            ViewBag.TotalJobs = totalJobs;
            ViewBag.LatestJobs = latestJobs;

            return View();
        }

        public IActionResult Myjobs()
        {
            return View();
        }

        public IActionResult Creatjob()
        {
            return View();
        }
    }
}