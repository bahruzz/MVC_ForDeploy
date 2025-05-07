using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using MVC_Projekt_Elearning.Helpers.Extensions;
using MVC_Projekt_Elearning.Models;
using MVC_Projekt_Elearning.Services;
using MVC_Projekt_Elearning.Services.Interfaces;
using MVC_Projekt_Elearning.ViewModels.Categories;
using MVC_Projekt_Elearning.ViewModels.Informations;

namespace MVC_Projekt_Elearning.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IWebHostEnvironment _env;
        public CategoryController(ICategoryService categoryService,
                                  IWebHostEnvironment env)
        {
            _categoryService = categoryService;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _categoryService.GetAlWithProductCountAsync());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(CategoryCreateVM category)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            bool existCategory = await _categoryService.ExistAsync(category.Name);
            if (existCategory)
            {
                ModelState.AddModelError("Name", "This category already exist");
                return View();
            }
            if (!category.Image.CheckFileType("image/"))
            {
                ModelState.AddModelError("Image", "Input accept only image format");
                return View();
            }
            if (!category.Image.CheckFileSize(500))
            {
                ModelState.AddModelError("Image", "Image size must be 500 kb");
                return View();
            }

            string fileName = Guid.NewGuid().ToString() + "-" + category.Image.FileName;
            string path = _env.GenerateFilePath("img", fileName);
            await category.Image.SaveFileToLocalAsync(path);
            await _categoryService.CreateAsync(new Category { Name = category.Name, Image = fileName });

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null) return BadRequest();
            var about = await _categoryService.GetByIdAsync((int)id);
            if (about is null) return NotFound();
            string path = _env.GenerateFilePath("img", about.Image);
            path.DeleteFileFromLocal();
            await _categoryService.DeleteAsync(about);
            return RedirectToAction(nameof(Index));

        }



        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            Category category = await _categoryService.GetByIdAsync((int)id);


            return View(category);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var category = await _categoryService.GetByIdAsync((int)id);
            if (category == null)
            {
                return NotFound();
            }

            var viewModel = new CategoryEditVM
            {
                Image = category.Image,
                Name = category.Name
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, CategoryEditVM request)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var category = await _categoryService.GetByIdAsync((int)id);

            if (await _categoryService.ExistExceptByIdAsync((int)id, request.Name))
            {
                ModelState.AddModelError("Name", "This name already exist");
                request.Image = category.Image;
                return View(request);

            }

            if (category == null)
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

                string oldPath = _env.GenerateFilePath("img", category.Image);
                oldPath.DeleteFileFromLocal();
                string fileName = Guid.NewGuid().ToString() + "-" + request.NewImage.FileName;
                string newPath = _env.GenerateFilePath("img", fileName);
                await request.NewImage.SaveFileToLocalAsync(newPath);
                category.Image = fileName;
            }


            category.Name = request.Name;

            await _categoryService.EditAsync();


            return RedirectToAction(nameof(Index));
        }

    }
}
