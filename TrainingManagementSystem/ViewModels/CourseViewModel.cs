namespace TrainingManagementSystem.ViewModels
{
    public class CourseViewModel
    {
        public int? CourseId { get; set; }
        public string CourseName { get; set; }
        public string Category { get; set; }
        public int InstructorId { get; set; }
        public string? InstructorName { get; set; }
    }
}
