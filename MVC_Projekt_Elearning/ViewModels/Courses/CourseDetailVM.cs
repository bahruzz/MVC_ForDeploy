namespace MVC_Projekt_Elearning.ViewModels.Courses
{
    public class CourseDetailVM
    {
        public string Name { get; set; }
        public int Duration { get; set; }
        public int Rating { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }
        public string Instructor { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public List<CourseImageVM> Images { get; set; }
    }
}
