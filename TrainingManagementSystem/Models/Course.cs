using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace TrainingManagementSystem.Models
{
    public class Course
    {

        [Key]
        public int CourseID { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string CourseName { get; set; }

        [Required]
        public string Category { get; set; }

        [Required]
        public int InstructorID { get; set; } // InstructorId

        // Navigation Properties

        [ValidateNever]
        public User User { get; set; }  //course Explained by one instructor

        [ValidateNever]
        public ICollection<Session> Sessions { get; set; }

        [ValidateNever]
        public ICollection<Enrollment> Enrollments { get; set; }
    }
}
