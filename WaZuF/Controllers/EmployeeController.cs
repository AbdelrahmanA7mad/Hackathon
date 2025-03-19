﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WaZuF.EmpServices;
using WaZuF.EmpViewModel;
using System.Threading.Tasks;
using WaZuF.Models;
using WaZuF.Data;
using Microsoft.EntityFrameworkCore;

namespace WaZuF.Controllers
{
    [Authorize(Policy = "PersonOnly")]
    public class EmployeeController : Controller
    {
        private readonly IEmpService _empService;
        private readonly AppDbContext _db;

        public EmployeeController(IEmpService empService , AppDbContext db)
        {
            _empService = empService;
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> History()
        {
            var examss = await _empService.GetAllExams(); // No need to pass viewModel

            var historyViewModel = new HistoryViewModel
            {
                exams = examss.Select(e => new Exam
                {
                    Id = e.Id,
                    Solved = e.Solved,
                    Tries = e.Tries,
                }).ToList()
            };

            return View(historyViewModel);
        }
        public async Task<IActionResult> Details(int id)
        {
            var exam = await _db.Exams
                .FirstOrDefaultAsync(e => e.Id == id);

            if (exam == null)
            {
                return NotFound(); // Handle case when exam is not found
            }
            double rate;
            if (exam.Solved)
            {
               rate = (100 - ((exam.Tries - 1) * 10));

            }
            else
            {
                rate = 0;
            }
            var detailsViewModel = new ExamDetailsViewModel
            {
                Id = exam.Id,
                question = exam.Question,
                solution = exam.solution,
                Status = exam.Solved,
                Success_Rate = rate,
                Attempts = exam.Tries,
            };

            return View(detailsViewModel);
        }



        [HttpPost]
        public async Task<IActionResult> GenerateQuestions([FromBody] EmployeeViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.JobDescription))
            {
                return BadRequest(new { error = "Job description is required." });
            }

            var question = await _empService.GenerateCodingTasksAsync(new Exam
            {
                description = model.JobDescription
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
