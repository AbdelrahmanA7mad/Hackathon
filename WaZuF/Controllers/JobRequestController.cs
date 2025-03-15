using Microsoft.AspNetCore.Mvc;
using WaZuF.Models;

public class JobRequestController : Controller
{
    private readonly IJobRequestService _jobRequestService;

    public JobRequestController(IJobRequestService jobRequestService)
    {
        _jobRequestService = jobRequestService;
    }

    // عرض جميع الطلبات
    public async Task<IActionResult> Index()
    {
        var jobRequests = await _jobRequestService.GetAllJobRequestsAsync();
        return View(jobRequests);
    }

    // عرض التفاصيل
    public async Task<IActionResult> Details(int id)
    {
        var jobRequest = await _jobRequestService.GetJobRequestByIdAsync(id);
        if (jobRequest == null)
        {
            return NotFound();
        }
        return View(jobRequest);
    }

    // عرض صفحة الإنشاء
    public IActionResult Create()
    {
        return View();
    }

 



    // تأكيد الحذف
    public async Task<IActionResult> Delete(int id)
    {
        var jobRequest = await _jobRequestService.GetJobRequestByIdAsync(id);
        if (jobRequest == null)
        {
            return NotFound();
        }
        return View(jobRequest);
    }

    // تنفيذ الحذف
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _jobRequestService.DeleteJobRequestAsync(id);
        return RedirectToAction(nameof(Index));
    }




}
