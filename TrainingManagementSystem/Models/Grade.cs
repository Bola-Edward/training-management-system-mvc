using System.ComponentModel.DataAnnotations;

namespace TrainingManagementSystem.Models
{
    public class Grade
    {
        public int GradeId { get; set; }

        [Required, Range(0, 100)]
        public double Value { get; set; }

        public int TraineeId { get; set; }

        public int SessionId { get; set; }

        // Navigation Properties
        public User User { get; set; }
        public Session Session { get; set; }
    }
}
