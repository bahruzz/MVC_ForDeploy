using MVC_Projekt_Elearning.Models;
using MVC_Projekt_Elearning.ViewModels.Students;

namespace MVC_Projekt_Elearning.Services.Interfaces
{
    public interface IStudentService
    {
        Task CreateAsync(Student student);
        Task DeleteAsync(Student student);
        Task EditAsync();
        Task<IEnumerable<StudentVM>> GetAllAsync(int? take = null);
        Task<Student> GetByIdAsync(int id);
        Task<bool> ExistAsync(string fullname);


    }
}
