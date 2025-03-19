using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WaZuF.Models;
using WaZuF.Services;
using WaZuF.ViewModels;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace WaZuF.Controllers
{
    [Authorize(Policy = "CompanyOnly")]
    public class LogicController : Controller
    {
        private readonly IJopService _jobService;
        private readonly IQuestionService _questionService;
        private readonly UserManager<AppUser> _userManager;

        public LogicController(IJopService jobService, IQuestionService questionService, UserManager<AppUser> userManager)
        {
            _jobService = jobService;
            _questionService = questionService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            int? latestJobRequestId = await _questionService.GetLatestJobRequestIdAsync();

            if (latestJobRequestId == null)
            {
                ViewBag.Message = "No Job Requests found.";
                return View(new List<Question>());
            }

            var questions = await _questionService.GetQuestionsByJobRequestIdAsync(latestJobRequestId.Value);

            if (questions == null || questions.Count == 0)
            {
                ViewBag.Message = "No questions found for the latest Job Request.";
                return View(new List<Question>());
            }

            ViewBag.JobRequestId = latestJobRequestId.Value;
            return View(questions);
        }

        [HttpGet]
        public IActionResult NewJob()
        {
            return View(new CreateJopViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NewJob(CreateJopViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null || string.IsNullOrEmpty(user.Id))
            {
                ModelState.AddModelError("", "User AppUser not found");
                return View(viewModel);
            }

            try
            {
                await _jobService.CreateAsync(viewModel, user.Id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Error creating job. Please try again.");
                return View(viewModel);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteQuestion(int id)
        {
            var success = await _questionService.DeleteQuestionAsync(id);
            if (!success)
            {
                TempData["Error"] = "Failed to delete the question.";
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> TakeExam(int jobRequestId)
        {
            var questions = await _questionService.GetQuestionsByJobRequestIdAsync(jobRequestId);
            if (questions == null || !questions.Any())
            {
                return NotFound("No questions found for this exam.");
            }

            var model = new ExamEntryViewModel
            {
                JobRequestId = jobRequestId
            };
            return View("ExamEntry", model); // عرض صفحة إدخال البيانات أولاً
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> ExamEntry(ExamEntryViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var questions = await _questionService.GetQuestionsByJobRequestIdAsync(model.JobRequestId);
            if (questions == null || !questions.Any())
            {
                return NotFound("No questions found for this exam.");
            }

            ViewBag.Name = model.Name;
            ViewBag.Email = model.Email;
            return View("TakeExam", questions); // عرض الامتحان بعد إدخال البيانات
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public IActionResult TakeExam(Dictionary<int, string> answers, string name, string email)
        {
            if (answers == null || !answers.Any())
            {
                return BadRequest("No answers provided.");
            }

            // معالجة الإجابات مع بيانات الموظف
            foreach (var answer in answers)
            {
                // answer.Key هو QuestionId
                // answer.Value هو الإجابة المختارة (a, b, c, d)
                // يمكنك حفظها في قاعدة البيانات هنا مع الاسم والبريد الإلكتروني
            }

            return View("ExamSubmitted");
        }


    }
}