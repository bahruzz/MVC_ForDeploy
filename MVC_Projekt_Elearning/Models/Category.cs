namespace MVC_Projekt_Elearning.Models
{
    public class Category:BaseEntity
    {
        public string Name { get; set; }
        
        public string Image { get; set; }
        public ICollection<Course> Courses { get; set; }
    }
}
