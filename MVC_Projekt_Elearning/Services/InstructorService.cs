using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVC_Projekt_Elearning.Data;
using MVC_Projekt_Elearning.Models;
using MVC_Projekt_Elearning.Services.Interfaces;
using MVC_Projekt_Elearning.ViewModels.Courses;

namespace MVC_Projekt_Elearning.Services
{
    public class InstructorService : IInstructorService
    {
        private readonly AppDbContext _context;
        public InstructorService(AppDbContext context)
        {
            _context = context;
        }


        public async Task CreateAsync(Instructor instructor)
        {
            await _context.AddAsync(instructor);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Instructor instructor)
        {
            _context.Instructors.Remove(instructor);
            await _context.SaveChangesAsync();
        }

        public async Task EditAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistEmailAsync(string email)
        {
            return await _context.Instructors.AnyAsync(m => m.Email.Trim() == email.Trim());
        }

        public async Task<bool> ExistExceptByIdAsync(int id, string email)
        {
            return await _context.Instructors.AnyAsync(m => m.Email.ToLower().Trim() == email.ToLower().Trim() && m.Id != id);
        }

        public async Task<IEnumerable<Instructor>> GetAllAsync()
        {
            return await _context.Instructors.Include(m => m.InstructorSocials).ToListAsync();
        }

        public async Task<SelectList> GetAllSelectedAsync()
        {
            var instructor = await _context.Instructors.Where(m => !m.SoftDeleted).ToListAsync();
            return new SelectList(instructor, "Id", "FullName");
        }

        public async Task<Instructor> GetByIdAsync(int id)
        {
            return await _context.Instructors.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<Instructor> GetByIdWithSocialAsync(int id)
        {
            return await _context.Instructors.Include(m => m.InstructorSocials).FirstOrDefaultAsync(m => m.Id == id);
        }
    }
}
