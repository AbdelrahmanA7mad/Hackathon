using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WaZuF.EmpServices;
using WaZuF.EmpViewModel;
using System.Threading.Tasks;

namespace WaZuF.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        private readonly IEmpService _empService;

        public EmployeeController(IEmpService empService)
        {
            _empService = empService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Exam()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GenerateQuestions([FromBody] EmployeeViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.JobDescription))
            {
                return BadRequest(new { error = "Job description is required." });
            }

            var question = await _empService.GenerateCodingTasksAsync(new WaZuF.EmpServices.CodeQuiz
            {
                Description = model.JobDescription
            });

            if (question == null)
            {
                return BadRequest(new { error = "Failed to generate a coding question." });
            }

            return Ok(new { generatedQuestion = question });
        }


        [HttpPost]
        public async Task<IActionResult> SubmitSolution([FromBody] SolutionViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Problem) || string.IsNullOrWhiteSpace(model.Solution))
            {
                return BadRequest(new { error = "Problem and solution are required." });
            }

            var feedback = await _empService.CheckSolutionAsync(model.Problem, model.Solution);

            return Json(new { status = feedback.Contains("Correct!") ? "correct" : "incorrect", feedback });
        }
    }
}
