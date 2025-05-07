using Microsoft.AspNetCore.Mvc;
using MVC_Projekt_Elearning.Models;
using MVC_Projekt_Elearning.Services.Interfaces;

namespace MVC_Projekt_Elearning.Areas.Admin.Controllers
{
    [Area("admin")]
    public class SocialController : Controller
    {
        private readonly ISocialService _socialService;
        private readonly IWebHostEnvironment _env;
        public SocialController(ISocialService socialService,
                                  IWebHostEnvironment env)
        {
            _socialService = socialService;
            _env = env;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _socialService.GetAllAsync());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Social request)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            bool existSocial = await _socialService.ExistAsync(request.Name);
            if (existSocial)
            {
                ModelState.AddModelError("Name", "This name already exist");
                return View();
            }
            await _socialService.CreateAsync(new Social { Name = request.Name });
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null) return BadRequest();
            var deleteSocial = await _socialService.GetByIdAsync((int)id);
            if (deleteSocial is null) return NotFound();

            _socialService.DeleteAsync(deleteSocial);
            return RedirectToAction(nameof(Index));

        }

       

    }
}
