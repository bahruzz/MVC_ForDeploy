using System.ComponentModel.DataAnnotations;

namespace MVC_Projekt_Elearning.ViewModels.Categories
{
    public class CategoryCreateVM
    {
        [Required(ErrorMessage = "This input can't be empty")]
        [StringLength(50)]
        public string Name { get; set; }
        public IFormFile Image { get; set; }
    }
}
