using System.ComponentModel.DataAnnotations;

namespace MVC_Projekt_Elearning.ViewModels.Categories
{
    public class CategoryEditVM
    {


        public string Name { get; set; }

        public string? Image { get; set; }
        public IFormFile? NewImage { get; set; }
    }   
}

