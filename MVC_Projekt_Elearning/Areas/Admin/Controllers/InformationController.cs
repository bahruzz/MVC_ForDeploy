using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient.DataClassification;
using MVC_Projekt_Elearning.Helpers.Extensions;
using MVC_Projekt_Elearning.Models;
using MVC_Projekt_Elearning.Services;
using MVC_Projekt_Elearning.Services.Interfaces;
using MVC_Projekt_Elearning.ViewModels.Informations;
using MVC_Projekt_Elearning.ViewModels.Sliders;

namespace MVC_Projekt_Elearning.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class InformationController : Controller
    {
        private readonly IInformationService _informationService;
        private readonly IWebHostEnvironment _env;

        public InformationController(IInformationService informationService, IWebHostEnvironment env)
        {

            _informationService = informationService;
            _env = env;

        }
        public async Task<IActionResult> Index()
        {
            return View(await _informationService.GetAllAsync());
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(InformationCreateVM information)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            bool existInformation = await _informationService.ExistAsync(information.Title);
            if (existInformation)
            {
                ModelState.AddModelError("Title", "This title already exist");
                return View();
            }
            if (!information.Image.CheckFileType("image/"))
            {
                ModelState.AddModelError("Image", "Input accept only image format");
                return View();
            }
            if (!information.Image.CheckFileSize(200))
            {
                ModelState.AddModelError("Image", "Image size must be 200 kb");
                return View();
            }

            string fileName = Guid.NewGuid().ToString() + "-" + information.Image.FileName;
            string path = _env.GenerateFilePath("img", fileName);

            await information.Image.SaveFileToLocalAsync(path);
            await _informationService.CreateAsync(new Information { Title = information.Title, Description = information.Description, Image = fileName });

            return RedirectToAction(nameof(Index));


        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null) return BadRequest();
            var information = await _informationService.GetByIdAsync((int)id);
            if (information is null) return NotFound();
            string path = _env.GenerateFilePath("img", information.Image);
            path.DeleteFileFromLocal();
            await _informationService.DeleteAsync(information);
            return RedirectToAction(nameof(Index));

        }


        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            Information information = await _informationService.GetByIdAsync((int)id);


            return View(information);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var information = await _informationService.GetByIdAsync((int)id);
            if (information == null)
            {
                return NotFound();
            }

            var viewModel = new InformationEditVM
            {
                Image = information.Image,
                Description = information.Description,
                Title = information.Title
            };

            return View(viewModel);




        }

        [HttpPost]
        [ValidateAntiForgeryToken]


        public async Task<IActionResult> Edit(int? id, InformationEditVM request)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var information = await _informationService.GetByIdAsync((int)id);
            if (await _informationService.ExistByIdAsync((int)id, request.Title))
            {
                ModelState.AddModelError("Title", "This title already exist");
                request.Image = information.Image;
                return View(request);

            }

            if (information == null)
            {
                return NotFound();
            }



            if (request.NewImage != null)
            {

                if (!request.NewImage.CheckFileType("image/"))
                {
                    ModelState.AddModelError("NewImage", "Accept only image format");
                    return View(request);
                }
                if (!request.NewImage.CheckFileSize(500))
                {
                    ModelState.AddModelError("NewImage", "Image size must be max 500 KB");
                    return View(request);
                }

                string oldPath = _env.GenerateFilePath("img", information.Image);
                oldPath.DeleteFileFromLocal();
                string fileName = Guid.NewGuid().ToString() + "-" + request.NewImage.FileName;
                string newPath = _env.GenerateFilePath("img", fileName);
                await request.NewImage.SaveFileToLocalAsync(newPath);
                information.Image = fileName;
            }

            information.Description = request.Description;
            information.Title = request.Title;

            await _informationService.EditAsync();


            return RedirectToAction(nameof(Index));
        }


    }
}
