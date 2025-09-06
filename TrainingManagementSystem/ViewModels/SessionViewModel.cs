using System.ComponentModel.DataAnnotations;

namespace TrainingManagementSystem.ViewModels
{
    public class SessionViewModel : IValidatableObject
    {
        public int? SessionId { get; set; }

        [Required]
        public int CourseId { get; set; }

        [Required(ErrorMessage = "Start date is required")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "End date is required")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        // Custom validation rules
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (StartDate < DateTime.Today)
            {
                yield return new ValidationResult(
                    "Start date cannot be in the past",
                    new[] { nameof(StartDate) });
            }

            if (EndDate <= StartDate)
            {
                yield return new ValidationResult(
                    "End date must be after the start date",
                    new[] { nameof(EndDate) });
            }
        }
    }
}
