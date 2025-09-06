namespace TrainingManagementSystem.Models
{
    public class Enrollment
    {
        public int StudentId { get; set; }
        public int CourseId { get; set; }

        // Navigation Properties

        public User User { get; set; }
        public Course Course { get; set; }
    }
}
