namespace MVC_Projekt_Elearning.ViewModels.Courses
{
    public class CourseEditVM
    {
        public string? Name { get; set; }

        public int? Duration { get; set; }
        public int? Rating { get; set; }

        public string? Price { get; set; }
        public int CategoryId { get; set; }
        public int InstructorId { get; set; }

        public List<CourseImageVM>? Images { get; set; }

        public List<IFormFile>? NewImages { get; set; }
    }
}
