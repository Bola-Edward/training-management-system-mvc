using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TrainingManagementSystem.Models;
using TrainingManagementSystem.Repositories.Interfaces;
using TrainingManagementSystem.ViewModels;

namespace TrainingManagementSystem.Controllers
{
    public class GradesController : Controller
    {
        private readonly IGradeRepository _gradeRepo;
        private readonly ISessionRepository _sessionRepo;
        private readonly IUserRepository _userRepo;

        public GradesController(
            IGradeRepository gradeRepo,
            ISessionRepository sessionRepo,
            IUserRepository userRepo)
        {
            _gradeRepo = gradeRepo;
            _sessionRepo = sessionRepo;
            _userRepo = userRepo;
        }

        // ========================
        // INDEX
        // ========================
        public async Task<ActionResult> Index()
        {
            var grades = await _gradeRepo.GetAllWithTraineeAndCourseAsync();
            return View(grades);
        }

        // ========================
        // DETAILS
        // ========================
        public async Task<ActionResult> Details(int id)
        {
            var grade = await _gradeRepo.GetByIdWithIncludesAsync(id);
            if (grade == null) return NotFound();
            return View(grade);
        }

        // ========================
        // CREATE
        // ========================
        public async Task<ActionResult> Create()
        {
            await PopulateSessionsDropDown();
            await PopulateUsersDropDown();
            return View(new GradeViewModel());
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GradeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await PopulateUsersDropDown(model.TraineeId);
                await PopulateSessionsDropDown(model.SessionId);
                return View(model);
            }

            var grade = new Grade
            {
                Value = model.Value,
                TraineeId = model.TraineeId,
                SessionId = model.SessionId
            };

            await _gradeRepo.Add(grade);

            return RedirectToAction(nameof(Index));
        }

        // ========================
        // EDIT
        // ========================
        public async Task<ActionResult> Edit(int id)
        {
            var grade = await _gradeRepo.GetByIdWithIncludesAsync(id);
            if (grade == null) return NotFound();

            var vm = new GradeViewModel
            {
                Value = grade.Value,
                TraineeId = grade.TraineeId,
                SessionId = grade.SessionId
            };

            await PopulateUsersDropDown(grade.TraineeId);
            await PopulateSessionsDropDown(grade.SessionId);

            ViewBag.GradeId = grade.GradeId;
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, GradeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await PopulateUsersDropDown(model.TraineeId);
                await PopulateSessionsDropDown(model.SessionId);
                ViewBag.GradeId = id;
                return View(model);
            }

            var grade = await _gradeRepo.GetByIdWithIncludesAsync(id);
            if (grade == null) return NotFound();

            grade.Value = model.Value;
            grade.TraineeId = model.TraineeId;
            grade.SessionId = model.SessionId;

            await _gradeRepo.Update(grade);

            return RedirectToAction(nameof(Index));
        }

        // ========================
        // DELETE
        // ========================
        public async Task<ActionResult> Delete(int id)
        {
            var grade = await _gradeRepo.GetByIdWithIncludesAsync(id);
            if (grade == null) return NotFound();

            return View(grade);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var grade = await _gradeRepo.GetByIdWithIncludesAsync(id);
            if (grade == null) return NotFound();

            await _gradeRepo.Delete(grade);

            return RedirectToAction(nameof(Index));
        }

        // ========================
        // HELPERS
        // ========================
        private async Task PopulateSessionsDropDown(int? selectedId = null)
        {
            var sessions = await _sessionRepo.GetAll();
            ViewBag.SessionId = new SelectList(sessions, "SessionId", "SessionId", selectedId);
        }

        private async Task PopulateUsersDropDown(int? selectedId = null)
        {
            var trainees = (await _userRepo.GetAll())
                           .Where(u => u.Role == "Trainee");

            ViewBag.TraineeId = new SelectList(trainees, "UserID", "UserName", selectedId);
        }
    }
}
