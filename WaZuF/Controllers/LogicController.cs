using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WaZuF.Models;
using WaZuF.Services;
using WaZuF.ViewModels;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using WaZuF.Data;

namespace WaZuF.Controllers
{
    [Authorize(Policy = "CompanyOnly")]
    public class LogicController : Controller
    {
        private readonly IJopService _jobService;
        private readonly IJobRequestService _iJobRequestService;
        private readonly IQuestionService _questionService;
        private readonly UserManager<AppUser> _userManager;
        private readonly AppDbContext _db;

        public LogicController(IJopService jobService, IQuestionService questionService, UserManager<AppUser> userManager, AppDbContext db, IJobRequestService iJobRequestService)
        {
            _jobService = jobService;
            _questionService = questionService;
            _userManager = userManager;
            _db = db;
            _iJobRequestService = iJobRequestService;
        }

        // 🟢 عرض نموذج إضافة وظيفة جديدة
        [HttpGet]
        public IActionResult NewJob()
        {
            return View(new CreateJopViewModel());
        }

        // 🟢 إضافة وظيفة جديدة إلى قاعدة البيانات
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

        // عرض قائمة الأسئلة
        public async Task<IActionResult> Index()
        {
            int? latestJobRequestId = await _questionService.GetLatestJobRequestIdAsync();
            if (latestJobRequestId == null)
            {
                ViewBag.Message = "No Job Requests found.";
                return View(new List<Question>());
            }

            var questions = await _questionService.GetQuestionsByJobRequestIdAsync(latestJobRequestId.Value);
            ViewBag.JobRequestId = latestJobRequestId.Value;
            return View(questions);
        }

        // حذف سؤال
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteQuestion(int id)
        {
            var success = await _questionService.DeleteQuestionAsync(id);
            return RedirectToAction(nameof(Index));
        }

        // صفحة إدخال بيانات الموظف
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ExamEntry(int jobRequestId)
        {
            return View(new ExamEntryViewModel { JobRequestId = jobRequestId });
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> TakeExam(int employeeId)
        {
            var employee = await _db.Employees.FindAsync(employeeId);
            if (employee == null)
            {
                return NotFound("Employee not found.");
            }

            var questions = await _questionService.GetQuestionsByJobRequestIdAsync(employee.JobRequestId);
            if (questions == null || !questions.Any())
            {
                return NotFound("No questions found for this exam.");
            }

            var examViewModel = new ExamViewModel
            {
                EmployeeId = employee.Id,
                Questions = questions
            };

            return View(examViewModel);
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

            Employee emp = new Employee
            {
                Email = model.Email,
                Name = model.Name,
                JobRequestId = model.JobRequestId
            };

            _db.Employees.Add(emp);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(TakeExam), new { employeeId = emp.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> SubmitExam(Dictionary<int, string> answers, int employeeId)
        {
            var employee = await _db.Employees.FindAsync(employeeId);
            if (employee == null) return BadRequest("Employee not found.");

            var questions = await _db.Questions.Where(q => answers.Keys.Contains(q.Id)).ToListAsync();

            int score = 0;
            var employeeAnswers = new List<EmployeeAnswer>();

            foreach (var question in questions)
            {
                if (answers.TryGetValue(question.Id, out string selectedAnswer))
                {
                    bool isCorrect = selectedAnswer.Equals(question.CorrectAnswer.ToString(), StringComparison.OrdinalIgnoreCase);
                    if (isCorrect) score++;

                    employeeAnswers.Add(new EmployeeAnswer
                    {
                        EmployeeId = employee.Id,
                        QuestionId = question.Id,
                        SelectedAnswer = selectedAnswer[0] // تأكد أن القيمة حرف واحد
                    });
                }
            }

            employee.Score = score;
            _db.EmployeeAnswers.AddRange(employeeAnswers);
            await _db.SaveChangesAsync();

            return View("ExamSubmitted", employee);
        }


        // حفظ رابط الامتحان
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveExamLink(int jobRequestId, string examLink)
        {
            if (string.IsNullOrEmpty(examLink))
            {
                TempData["Error"] = "رابط الامتحان غير صالح.";
                return RedirectToAction(nameof(Index));
            }

            var jobRequest = await _db.JobRequests.FindAsync(jobRequestId);
            if (jobRequest == null)
            {
                TempData["Error"] = "لم يتم العثور على الطلب.";
                return RedirectToAction(nameof(Index));
            }

            jobRequest.ExamLink = examLink;
            _db.JobRequests.Update(jobRequest);
            await _db.SaveChangesAsync();

            TempData["Success"] = "تم حفظ رابط الامتحان بنجاح.";
            return RedirectToAction(nameof(Index));
        }
    }
}
