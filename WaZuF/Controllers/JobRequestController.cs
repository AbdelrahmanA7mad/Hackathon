
﻿using Microsoft.AspNetCore.Authorization;

﻿using Microsoft.AspNetCore.Identity;

using Microsoft.AspNetCore.Mvc;
using WaZuF.Models;
using System.Threading.Tasks;

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

    public async Task<IActionResult> Index()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return Unauthorized();

        var jobRequests = await _jobRequestService.GetJobRequestsByCompanyIdAsync(user.Id);
        return View(jobRequests);
    }

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
        if (!result) return NotFound(); // إذا لم يتم العثور على الوظيفة أو لا تمتلك الصلاحية

        return RedirectToAction(nameof(Index));
    }

}

