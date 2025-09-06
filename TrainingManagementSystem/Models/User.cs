using System.ComponentModel.DataAnnotations;

namespace TrainingManagementSystem.Models
{
    public class User
    {
        [Key]
        public int UserID { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [RegularExpression("Admin|Instructor|Trainee", ErrorMessage = "Role must be Admin, Instructor, or Trainee.")]
        public string Role { get; set; }

        //Navigation Property
        public ICollection<Course> Courses { get; set; } // Instructor or trainee can Connect with many courses

        public ICollection<Enrollment> Enrollments { get; set; }

        public ICollection<Grade> Grades { get; set; }            // trainee
    }
}
