using Microsoft.AspNetCore.Mvc;
using MVC_Projekt_Elearning.Services.Interfaces;
using MVC_Projekt_Elearning.ViewModels.Courses;
using MVC_Projekt_Elearning.Helpers;
using MVC_Projekt_Elearning.Helpers.Extensions;
using MVC_Projekt_Elearning.Models;
using MVC_Projekt_Elearning.Services;


namespace MVC_Projekt_Elearning.Areas.Admin.Controllers
{
    [Area("admin")]
    public class CourseController : Controller
    {
        private readonly ICourseService _courseService;
        private readonly ICategoryService _categoryService;
        private readonly IWebHostEnvironment _env;
        private readonly IInstructorService _instructorService;
        public CourseController(ICourseService courseService,
                                  IWebHostEnvironment env,
                                  ICategoryService categoryService,
                                  IInstructorService instructorService)
                             
        {
            _courseService = courseService;
            _env = env;
            _categoryService = categoryService;
            _instructorService = instructorService;
           
        }

        [HttpGet]
        public async Task<IActionResult> Index(int page = 1)
        {
            var courses = await _courseService.GetAllPaginateAsync(page, 4);

            var mappedDatas = _courseService.GetMappedDatas(courses);
            int totalPage = await GetPageCountAsync(4);

            Paginate<CourseVM> paginateDatas = new(mappedDatas, totalPage, page);

            return View(paginateDatas);
        }

        private async Task<int> GetPageCountAsync(int take)
        {
            int productCount = await _courseService.GetCountAsync();

            return (int)Math.Ceiling((decimal)productCount / take);
        }


        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.categories = await _categoryService.GetAllSelectedAsync();
            ViewBag.instructor = await _instructorService.GetAllSelectedAsync();
            return View();

        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CourseCreateVM request)
        {
            ViewBag.categories = await _categoryService.GetAllSelectedAsync();
            ViewBag.instructor = await _instructorService.GetAllSelectedAsync();
            if (!ModelState.IsValid)
            {
                return View();

            }

            foreach (var item in request.Images)
            {
                if (!item.CheckFileSize(500))
                {
                    ModelState.AddModelError("Images", "Image size must be max 500 KB");
                    return View();
                }

                if (!item.CheckFileType("image/"))
                {
                    ModelState.AddModelError("Images", "File type must be only image");

                    return View();
                }
            }
            List<CourseImage> images = new();
            foreach (var item in request.Images)
            {
                string fileName = $"{Guid.NewGuid()}-{item.FileName}";
                string path = _env.GenerateFilePath("img", fileName);
                await item.SaveFileToLocalAsync(path);
                images.Add(new CourseImage { Name = fileName });
            }

            images.FirstOrDefault().IsMain = true;
            Course course = new()
            {
                Name = request.Name,
                Duration = request.Duration,
                Rating = request.Rating,
                InstructorId = request.InstructorId,
                CategoryId = request.CategoryId,
                Price = decimal.Parse(request.Price.Replace(".", ",")),
                CoursesImages = images

            };

            await _courseService.CreateAsync(course);


            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null) return BadRequest();
            var existProduct = await _courseService.GetByIdWithCoursesImagesAsync((int)id);
            if (existProduct is null) return NotFound();

            foreach (var item in existProduct.CoursesImages)
            {
                string path = _env.GenerateFilePath("img", item.Name);

                path.DeleteFileFromLocal();
            }
            await _courseService.DeleteAsync(existProduct);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id is null) return BadRequest();
            var existCourse = await _courseService.GetByIdWithCoursesImagesAsync((int)id);
            if (existCourse is null) return NotFound();
            var category = await _categoryService.GetByIdAsync(existCourse.CategoryId);

            List<CourseImageVM> images = new();
            foreach (var item in existCourse.CoursesImages)
            {
                images.Add(new CourseImageVM
                {
                    Image = item.Name,
                    IsMain = item.IsMain

                });
            }
            CourseDetailVM response = new()
            {
                Name = existCourse.Name,
                Rating = existCourse.Rating,
                Category = category.Name,
                Price = existCourse.Price,
                Duration = existCourse.Duration,
                Instructor=existCourse.Instructor.FullName,
                Images = images,
                CreatedDate = existCourse.CreatedDate,
            };
            return View(response);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {


            if (id is null) return BadRequest();

            var existCourse = await _courseService.GetByIdWithCoursesImagesAsync((int)id);

            if (existCourse is null) return NotFound();



            ViewBag.categories = await _categoryService.GetAllSelectedAsync();
            ViewBag.instructors = await _instructorService.GetAllSelectedAsync();


            List<CourseImageVM> images = new();

            foreach (var item in existCourse.CoursesImages)
            {
                images.Add(new CourseImageVM
                {
                    Id = item.Id,
                    Image = item.Name,
                    IsMain = item.IsMain
                });
            }

            CourseEditVM response = new()
            {
                Name = existCourse.Name,
                Duration = existCourse.Duration,
                Rating = existCourse.Rating,
                Price = existCourse.Price.ToString().Replace(",", "."),
                Images = images,
                InstructorId=existCourse.InstructorId,
                CategoryId = existCourse.CategoryId,
            };





            return View(response);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, CourseEditVM request)
        {
            if (id == null) return BadRequest();
            var course = await _courseService.GetByIdWithCoursesImagesAsync((int)id);
            if (course == null) return NotFound();

            List<CourseImageVM> images = new();

            foreach (var item in course.CoursesImages)
            {
                images.Add(new CourseImageVM
                {
                    Id = item.Id,
                    Image = item.Name,
                    IsMain = item.IsMain
                });
            }
            request.Images = images;

            if (!ModelState.IsValid)
            {
                ViewBag.categories = await _categoryService.GetAllSelectedAsync();
              
                return View(request);

            }

            if (await _courseService.ExistExceptByIdAsync((int)id, request.Name))
            {
                ModelState.AddModelError("Name", "This name already exist");
                
                return View(request);

            }


            if (request.NewImages is not null)
            {
             
                foreach (var item in request.NewImages)
                {
                    if (!item.CheckFileType("image/"))
                    {
                        ModelState.AddModelError("NewImages", "Input can accept only image format");
                        return View(request);

                    }
                    if (!item.CheckFileSize(500))
                    {
                        ModelState.AddModelError("NewImages", "Image size must be max 500 KB ");
                        return View(request);
                    }


                }
                foreach (var item in request.NewImages)
                {
                    string oldPath = _env.GenerateFilePath("img", item.Name);
                    oldPath.DeleteFileFromLocal();
                    string fileName = Guid.NewGuid().ToString() + "-" + item.FileName;
                    string newPath = _env.GenerateFilePath("img", fileName);

                    await item.SaveFileToLocalAsync(newPath);

                    course.CoursesImages.Add(new CourseImage { Name = fileName });

                }

            }
            course.Name = request.Name;
            course.Duration = (int)request.Duration;
            course.Duration = (int)request.Rating;
            course.InstructorId = request.InstructorId;
            course.CategoryId = request.CategoryId;
            course.Price = decimal.Parse(request.Price);
            

            await _courseService.EditAsync();
            return RedirectToAction(nameof(Index));
        }


    }


}
