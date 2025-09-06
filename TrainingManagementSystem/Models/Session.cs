using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Options;

namespace TrainingManagementSystem.Models
{
    public class Session : IValidatableObject
    {
        public int SessionId { get; set; }

        [Required(ErrorMessage = "Start date is required")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "End date is required")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "Course selection is required")]
        public int CourseId { get; set; }


        //Navigation Property
        public Course Course { get; set; }
        public ICollection<Grade> Grades { get; set; } = new List<Grade>(); // Trainee grades for this session

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (StartDate.Date < DateTime.Now.Date)
            {
                yield return new ValidationResult(
                    "StartDate cannot be in the past.",
                    new[] { nameof(StartDate) }
                );
            }

            if (EndDate <= StartDate)
            {
                yield return new ValidationResult(
                    "EndDate must be after StartDate.",
                    new[] { nameof(EndDate) }
                );
            }
        }




    }
}
