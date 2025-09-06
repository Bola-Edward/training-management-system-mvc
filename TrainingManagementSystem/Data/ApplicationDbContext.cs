using Microsoft.EntityFrameworkCore;
using TrainingManagementSystem.Models;

namespace TrainingManagementSystem.Data
{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }


        // Fluent API
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Course>()
              .HasOne(c => c.User)
              .WithMany(u => u.Courses)
              .HasForeignKey(c => c.InstructorID)
              .OnDelete(DeleteBehavior.Cascade)
              .IsRequired();

            modelBuilder.Entity<Enrollment>()
            .HasKey(e => new { e.StudentId, e.CourseId });

            modelBuilder.Entity<Enrollment>()
              .HasOne(e => e.User)
              .WithMany(u => u.Enrollments)
              .HasForeignKey(e => e.StudentId)
              .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Enrollment>()
             .HasOne(e => e.Course)
             .WithMany(c => c.Enrollments)
             .HasForeignKey(e => e.CourseId)
             .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Session>()
             .HasOne(s => s.Course)
             .WithMany(c => c.Sessions)
             .HasForeignKey(s => s.CourseId)
             .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<Grade>()
             .HasOne(g => g.Session)
             .WithMany(s => s.Grades)
             .HasForeignKey(g => g.SessionId)
             .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Grade>()
              .HasOne(g => g.User)
              .WithMany(u => u.Grades)
              .HasForeignKey(g => g.TraineeId)
              .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Course>()
            .HasIndex(c => c.CourseName)
            .IsUnique();

        }
    }
}
