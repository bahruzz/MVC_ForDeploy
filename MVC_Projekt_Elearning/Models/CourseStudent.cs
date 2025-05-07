﻿using System.ComponentModel.DataAnnotations.Schema;

namespace MVC_Projekt_Elearning.Models
{
    public class CourseStudent
    {
        public int Id { get; set; }

        [ForeignKey(nameof(Course))]
        public int CourseId { get; set; }

        [ForeignKey(nameof(Student))]
        public int StudentId { get; set; }
    }
}
