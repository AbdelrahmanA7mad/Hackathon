
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WaZuF.Services; 

namespace WaZuF.Controllers
{
    public class MainController : Controller
    {
        private readonly IJobRequestService _jobRequestService;

        public MainController(IJobRequestService jobRequestService)
        {
            _jobRequestService = jobRequestService;
        }


        [Authorize]
        public async Task<IActionResult> Dashboard()
        {
            var totalJobs = await _jobRequestService.GetTotalJobsAsync();
            var latestJobs = await _jobRequestService.GetLatestJobRequestsAsync(5); // جلب آخر 5 طلبات

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
