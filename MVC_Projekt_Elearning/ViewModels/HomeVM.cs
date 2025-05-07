using MVC_Projekt_Elearning.Models;
using MVC_Projekt_Elearning.ViewModels.Abouts;
using MVC_Projekt_Elearning.ViewModels.Categories;
using MVC_Projekt_Elearning.ViewModels.Courses;
using MVC_Projekt_Elearning.ViewModels.Informations;
using MVC_Projekt_Elearning.ViewModels.Instructors;
using MVC_Projekt_Elearning.ViewModels.Sliders;
using MVC_Projekt_Elearning.ViewModels.Students;

namespace MVC_Projekt_Elearning.ViewModels
{
    public class HomeVM
    {
        public IEnumerable<SliderVM> Sliders { get; set; }
        public IEnumerable<InformationVM> Informations { get; set; }
        public About Abouts { get; set; }
        public CategoryCourseVM CategoryFirst { get; set; }
        public CategoryCourseVM CategoryLast { get; set; }
        public IEnumerable<CategoryCourseVM> Categories { get; set; }
        public IEnumerable<Course> Courses { get; set; }
        public IEnumerable<Instructor> Instructors { get; set; }
        public IEnumerable<StudentVM> Students { get; set; }
    }
}
