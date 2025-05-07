using Microsoft.AspNetCore.Mvc.Rendering;
using MVC_Projekt_Elearning.Models;
using MVC_Projekt_Elearning.ViewModels.Categories;

namespace MVC_Projekt_Elearning.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryCourseVM>> GetAlWithProductCountAsync();
        Task<bool> ExistAsync(string name);
        Task CreateAsync(Category category);
        Task<Category> GetByIdAsync(int id);
        Task DeleteAsync(Category category);
        Task<IEnumerable<Category>> GetAllAsync();

        Task<SelectList> GetAllSelectedAsync();
        //Task<Category> GetByIdWithCoursesAsync(int id);
        Task<bool> ExistExceptByIdAsync(int id, string name);
        Task EditAsync();
    }
}
