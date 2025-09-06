using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TrainingManagementSystem.Models;
using TrainingManagementSystem.Repositories.Interfaces;
using TrainingManagementSystem.ViewModels;

namespace TrainingManagementSystem.Controllers
{
    public class CourseController : Controller
    {
        private readonly ICourseRepository _courseRepo;
        private readonly IUserRepository _userRepo;

        public CourseController(ICourseRepository courseRepo, IUserRepository userRepo)
        {
            _courseRepo = courseRepo;
            _userRepo = userRepo;
        }

        // GET: Course
        public async Task<IActionResult> Index(string searchString)
        {
            IEnumerable<Course> courses;

            if (!string.IsNullOrEmpty(searchString))
            {
                courses = await _courseRepo.FindWithInstructor(
                    c => c.CourseName.Contains(searchString) || c.Category.Contains(searchString)
                );
            }
            else
            {
                courses = await _courseRepo.GetAllWithInstructorAsync();
            }

            var courseVMs = courses.Select(c => new CourseViewModel
            {
                CourseId = c.CourseID,
                CourseName = c.CourseName,
                Category = c.Category,
                InstructorId = c.InstructorID,
                InstructorName = c.User != null ? c.User.UserName : "No Instructor"
            });

            return View(courseVMs);
        }

        // GET: Course/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var course = await _courseRepo.GetById(id);
            if (course == null)
                return NotFound();

            return View(course);
        }

        // GET: Course/Create
        public async Task<IActionResult> Create()
        {
            var instructors = await _userRepo.GetAllInstructorsAsync();
            ViewBag.Instructors = new SelectList(instructors, "UserID", "UserName");
            return View();
        }




        // POST: Course/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CourseViewModel model)
        {
            if (ModelState.IsValid)
            {
                var existingCourse = await _courseRepo.Find(c => c.CourseName == model.CourseName);
                if (existingCourse.Any())
                {
                    ModelState.AddModelError("CourseName", "This course name already exists.");
                    var instructors = await _userRepo.GetAllInstructorsAsync();
                    ViewBag.Instructors = new SelectList(instructors, "UserID", "UserName", model.InstructorId);
                    return View(model);
                }

                var course = new Course
                {
                    CourseName = model.CourseName,
                    Category = model.Category,
                    InstructorID = model.InstructorId
                };

                await _courseRepo.Add(course);
                return RedirectToAction(nameof(Index));
            }

            var allInstructors = await _userRepo.GetAllInstructorsAsync();
            ViewBag.Instructors = new SelectList(allInstructors, "UserID", "UserName", model.InstructorId);
            return View(model);
        }

        // GET: Course/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var course = await _courseRepo.GetById(id);
            if (course == null)
                return NotFound();

            var model = new CourseViewModel
            {
                CourseId = course.CourseID,
                CourseName = course.CourseName,
                Category = course.Category,
                InstructorId = course.InstructorID
            };

            var instructors = await _userRepo.GetAllInstructorsAsync();
            ViewBag.Instructors = new SelectList(instructors, "UserID", "UserName", course.InstructorID);

            return View(model);
        }

        // POST: Course/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CourseViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.CourseId == null)
                    return BadRequest();

                var course = await _courseRepo.GetById(model.CourseId.Value);
                course.CourseName = model.CourseName;
                course.Category = model.Category;
                course.InstructorID = model.InstructorId;

                await _courseRepo.Update(course);
                return RedirectToAction(nameof(Index));
            }

            var instructors = await _userRepo.GetAllInstructorsAsync();
            ViewBag.Instructors = new SelectList(instructors, "UserID", "UserName", model.InstructorId);

            return View(model);
        }

        // GET: Course/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var course = await _courseRepo.GetCourseWithInstructor(id);
            if (course == null)
                return NotFound();

            var viewModel = new CourseViewModel
            {
                CourseId = course.CourseID,
                CourseName = course.CourseName,
                Category = course.Category,
                InstructorId = course.InstructorID,
                InstructorName = course.User != null ? course.User.UserName : "N/A"
            };

            return View(viewModel);
        }

        // POST: Course/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(CourseViewModel model)
        {
            if (model.CourseId == null)
                return BadRequest();

            var course = await _courseRepo.GetById(model.CourseId.Value);
            if (course != null)
            {
                await _courseRepo.Delete(course);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
