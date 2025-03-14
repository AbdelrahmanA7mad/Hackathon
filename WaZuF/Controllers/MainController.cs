using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WaZuF.Controllers
{
    public class MainController : Controller
    {
        [Authorize]
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}
