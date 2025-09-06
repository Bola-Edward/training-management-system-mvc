using System.ComponentModel.DataAnnotations;

namespace TrainingManagementSystem.ViewModels
{
    public class GradeViewModel
    {
        [Required]
        [Range(0, 100, ErrorMessage = "Value must be between 0 and 100.")]
        public double Value { get; set; }

        [Required(ErrorMessage = "Please select a trainee.")]
        public int TraineeId { get; set; }

        [Required(ErrorMessage = "Please select a session.")]
        public int SessionId { get; set; }
    }
}
