using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WaZuF.Models;
using System.Threading.Tasks;
using WaZuF.Services;

[Authorize(Policy = "CompanyOnly")]
public class JobRequestController : Controller
{
    private readonly IJobRequestService _jobRequestService;
    private readonly UserManager<AppUser> _userManager;

    public JobRequestController(IJobRequestService jobRequestService, UserManager<AppUser> userManager)
    {
        _jobRequestService = jobRequestService;
        _userManager = userManager;
    }

    // GET: Retrieve and display all job requests for the current company user
    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Unauthorized();

        var jobRequests = await _jobRequestService.GetJobRequestsByCompanyIdAsync(user.Id);
        return View(jobRequests);
    }

    // GET: Create new job request
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    // POST: Create new job request
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(JobRequest jobRequest)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Unauthorized();

        if (ModelState.IsValid)
        {
            jobRequest.CompanyId = user.Id;
            await _jobRequestService.AddJobRequestAsync(jobRequest);
            return RedirectToAction(nameof(Index));
        }
        return View(jobRequest);
    }

    // GET: Edit existing job request
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Unauthorized();

        var jobRequest = await _jobRequestService.GetJobRequestByIdAsync(id);
        if (jobRequest == null || jobRequest.CompanyId != user.Id) return NotFound();

        return View(jobRequest);
    }

    // POST: Edit existing job request
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(JobRequest jobRequest)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Unauthorized();

        if (jobRequest.CompanyId != user.Id) return Forbid();

        if (ModelState.IsValid)
        {
            await _jobRequestService.UpdateJobRequestAsync(jobRequest);
            return RedirectToAction(nameof(Index));
        }
        return View(jobRequest);
    }

    // POST: Delete job request

    public async Task<IActionResult> Details(int id)
    {
        var employees = await _jobRequestService.GetAllEmployee(id);

        if (employees == null)
        {
            return NotFound(); 
        }

        return View(employees);
    }



    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Unauthorized();

        var result = await _jobRequestService.DeleteJobRequestAsync(id, user.Id);
        if (!result) return NotFound();

        return RedirectToAction(nameof(Index));
    }
}