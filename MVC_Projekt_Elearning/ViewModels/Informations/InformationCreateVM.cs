using System.ComponentModel.DataAnnotations;

namespace MVC_Projekt_Elearning.ViewModels.Informations
{
    public class InformationCreateVM
    {
        public string Description { get; set; }

        [Required(ErrorMessage = "This input can't be empty")]
        [StringLength(200)]
        public string Title { get; set; }

        public IFormFile Image { get; set; }
    }
}
