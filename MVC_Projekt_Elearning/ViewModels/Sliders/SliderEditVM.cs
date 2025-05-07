using System.ComponentModel.DataAnnotations;

namespace MVC_Projekt_Elearning.ViewModels.Sliders
{
    public class SliderEditVM
    {
        
        public string ?Description { get; set; }
       
        public string Title { get; set; }
      
        public string? Image { get; set; }
        public IFormFile ? NewImage { get; set; }
    }
}
