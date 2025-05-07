using Microsoft.EntityFrameworkCore;
using MVC_Projekt_Elearning.Data;
using MVC_Projekt_Elearning.Models;
using MVC_Projekt_Elearning.Services.Interfaces;
using MVC_Projekt_Elearning.ViewModels.Courses;

namespace MVC_Projekt_Elearning.Services
{
    public class CourseService : ICourseService
    {
        private readonly AppDbContext _context;

        public CourseService(AppDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(Course course)
        {
            await _context.AddAsync(course);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Course course)
        {
            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
        }

        public async Task EditAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistAsync(string name)
        {
            return await _context.Courses.AnyAsync(m => m.Name.Trim() == name.Trim());
        }

        public async Task<IEnumerable<Course>> GetAllAsync()
        {
            return await _context.Courses.Include(m => m.CoursesImages).Include(m => m.Category).ToListAsync();
        }

        public async Task<IEnumerable<Course>> GetAllPaginateAsync(int page, int take)
        {
            return await _context.Courses.Where(m => !m.SoftDeleted).Include(m => m.CoursesImages)
                .Include(m => m.Instructor).Include(m => m.Category)
                                          .Skip((page - 1) * take)
                                          .Take(take)
                                          .ToListAsync();
        }



        public async Task<Course> GetByIdAsync(int id)
        {
            return await _context.Courses.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<Course> GetByIdWithCoursesImagesAsync(int id)
        {
            return await _context.Courses.Include(m => m.CoursesImages).Include(m=>m.Instructor).FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<int> GetCountAsync()
        {
            return await _context.Courses.CountAsync();
        }

        public async Task<CourseImage> GetCourseImageByIdAsync(int id)
        {
            return await _context.CourseImages.FirstOrDefaultAsync(m => m.Id == id);
        }

        public IEnumerable<CourseVM> GetMappedDatas(IEnumerable<Course> course)
        {
            return course.Select(m => new CourseVM()
            {
                Id = m.Id,
                Name = m.Name,
                CategoryName = m.Category.Name,
                InstructorName = m.Instructor.FullName,
                Price = m.Price,
                Duration = m.Duration,
                Rating = m.Rating,
                MainImage = m.CoursesImages.FirstOrDefault(m => m.IsMain).Name
            }); ;
        }

        public async Task ImageDeleteAsync(CourseImage image)
        {
            _context.CourseImages.Remove(image);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Course>> GetAllWithAllDatasAsync()
        {
            return await _context.Courses.Include(m => m.CoursesImages).ToListAsync();
        }

        public async Task<bool> ExistExceptByIdAsync(int id, string name)
        {
            return await _context.Courses.AnyAsync(m => m.Name.ToLower().Trim() == name.ToLower().Trim() && m.Id != id);
        }
    }
}
