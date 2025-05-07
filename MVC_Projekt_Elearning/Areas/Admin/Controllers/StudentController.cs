using Microsoft.AspNetCore.Mvc;
using MVC_Projekt_Elearning.Helpers.Extensions;
using MVC_Projekt_Elearning.Models;
using MVC_Projekt_Elearning.Services;
using MVC_Projekt_Elearning.Services.Interfaces;
using MVC_Projekt_Elearning.ViewModels.Students;

namespace MVC_Projekt_Elearning.Areas.Admin.Controllers
{
    [Area("admin")]
    public class StudentController : Controller
    {
        private readonly IStudentService _studentService;
        private readonly IWebHostEnvironment _env;
        public StudentController(IStudentService studentService,
                                   IWebHostEnvironment env)
        {
            _studentService = studentService;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _studentService.GetAllAsync());
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StudentCreateVM request)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            if (!request.Image.CheckFileType("image/"))
            {
                ModelState.AddModelError("Image", "Input can accept only image format");
                return View();
            }
            if (!request.Image.CheckFileSize(200))
            {
                ModelState.AddModelError("Image", "Image size must be max 200 KB ");
                return View();
            }
            string fileName = Guid.NewGuid().ToString() + "-" + request.Image.FileName;


            string path = Path.Combine(_env.WebRootPath, "img", fileName);
            await request.Image.SaveFileToLocalAsync(path);



            await _studentService.CreateAsync(new Student { FullName = request.FullName, Profession = request.Profession, Image = fileName, Biography = request.Biography });
            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null) return BadRequest();
            var student = await _studentService.GetByIdAsync((int)id);
            if (student is null) return NotFound();
            string path = _env.GenerateFilePath("img", student.Image);
            path.DeleteFileFromLocal();
            await _studentService.DeleteAsync(student);
            return RedirectToAction(nameof(Index));

        }


        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {

            Student student = await _studentService.GetByIdAsync((int)id);
            return View(student);
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return BadRequest();
            var student = await _studentService.GetByIdAsync((int)id);
            if (student == null) return NotFound();
            //return View();
            return View(new StudentEditVM { FullName = student.FullName, Profession = student.Profession, Image = student.Image, Biography = student.Biography });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, StudentEditVM request)
        {
            if (id == null) return BadRequest();
            var student = await _studentService.GetByIdAsync((int)id);
            if (student == null) return NotFound();

            if (!ModelState.IsValid)
            {
                return View();
            }
            if (request.NewImage is not null)
            {

                if (!request.NewImage.CheckFileType("image/"))
                {
                    ModelState.AddModelError("NewImage", "Input can accept only image format");
                    request.Image = student.Image;
                    return View(request);

                }
                if (!request.NewImage.CheckFileSize(200))
                {
                    ModelState.AddModelError("NewImage", "Image size must be max 200 KB ");
                    request.Image = student.Image;
                    return View(request);
                }
                string oldPath = _env.GenerateFilePath("img", student.Image);
                oldPath.DeleteFileFromLocal();
                string fileName = Guid.NewGuid().ToString() + "-" + request.NewImage.FileName;
                string newPath = _env.GenerateFilePath("img", fileName);

                await request.NewImage.SaveFileToLocalAsync(newPath);
                
                    student.FullName = request.FullName;
                
                    student.Profession = request.Profession;

                    student.Biography = request.Biography;
                
                    student.Image = fileName;
                
            }

            student.FullName = request.FullName;

            student.Profession = request.Profession;

            student.Biography = request.Biography;

            await _studentService.EditAsync();
            return RedirectToAction(nameof(Index));
        }




    }
}
