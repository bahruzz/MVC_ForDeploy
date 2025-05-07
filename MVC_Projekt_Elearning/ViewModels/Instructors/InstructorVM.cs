﻿using MVC_Projekt_Elearning.Models;

namespace MVC_Projekt_Elearning.ViewModels.Instructors
{
    public class InstructorVM
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Image { get; set; }
        public string Email { get; set; }
        public string Designation { get; set; }
        public List<InstructorSocial> InstructorSocials { get; set; }
    }
}
