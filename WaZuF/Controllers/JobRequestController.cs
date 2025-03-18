using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WaZuF.Models;
using System.Threading.Tasks;

public class JobRequestController : Controller
{
    private readonly IJobRequestService _jobRequestService;
    private readonly UserManager<Company> _userManager;

    public JobRequestController(IJobRequestService jobRequestService, UserManager<Company> userManager)
    {
        _jobRequestService = jobRequestService;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Unauthorized();

        var jobRequests = await _jobRequestService.GetJobRequestsByCompanyIdAsync(user.Id);
        return View(jobRequests);
    }
}
