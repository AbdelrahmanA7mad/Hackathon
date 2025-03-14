﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WaZuF.Models;
using WaZuF.Services;
using WaZuF.ViewModels;

namespace WaZuF.Controllers
{
    public class LogicController : Controller
    {
        private readonly IJopService _jobService;
        private readonly UserManager<Company> _userManager;

        public LogicController(IJopService jobService, UserManager<Company> userManager)
        {
            _jobService = jobService;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
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
                ModelState.AddModelError("", "User company not found");
                return View(viewModel);
            }

            try
            {
                await _jobService.CreateAsync(viewModel, user.Id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Log exception here
                ModelState.AddModelError("", "Error creating job. Please try again.");
                return View(viewModel);
            }
        }
    }
}
