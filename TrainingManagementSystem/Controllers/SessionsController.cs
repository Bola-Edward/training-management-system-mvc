using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TrainingManagementSystem.Models;
using TrainingManagementSystem.ViewModels;
using TrainingManagementSystem.Data;
using Microsoft.EntityFrameworkCore;


namespace TrainingManagementSystem.Controllers
{
    public class SessionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SessionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ========================
        // INDEX
        // ========================
        public async Task<IActionResult> Index(string? search)
        {
            var sessions = string.IsNullOrWhiteSpace(search)
                ? await _context.Sessions.Include(s => s.Course).ToListAsync()
                : await _context.Sessions
                                .Include(s => s.Course)
                                .Where(s => s.Course.CourseName.Contains(search))
                                .ToListAsync();

            ViewData["CurrentFilter"] = search;
            return View(sessions);
        }

        // ========================
        // DETAILS
        // ========================
        public async Task<IActionResult> Details(int id)
        {
            var session = await _context.Sessions
                                        .Include(s => s.Course)
                                        .FirstOrDefaultAsync(s => s.SessionId == id);

            if (session == null) return NotFound();
            return View(session);
        }

        // ========================
        // CREATE (GET)
        // ========================
        public async Task<IActionResult> Create()
        {
            await PopulateCoursesDropDown();
            return View();
        }

        // ========================
        // CREATE (POST)
        // ========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SessionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await PopulateCoursesDropDown(model.CourseId);
                return View(model);
            }

            var session = new Session
            {
                CourseId = model.CourseId,
                StartDate = model.StartDate,
                EndDate = model.EndDate
            };

            _context.Sessions.Add(session);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // ========================
        // EDIT (GET)
        // ========================
        public async Task<IActionResult> Edit(int id)
        {
            var session = await _context.Sessions.FindAsync(id);
            if (session == null) return NotFound();

            await PopulateCoursesDropDown(session.CourseId);
            return View(session);
        }

        // ========================
        // EDIT (POST)
        // ========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SessionViewModel model)
        {
            if (id != model.SessionId) return BadRequest();

            if (!ModelState.IsValid)
            {
                await PopulateCoursesDropDown(model.CourseId);
                return View(model);
            }

            var session = await _context.Sessions.FindAsync(id);
            if (session == null) return NotFound();

            session.CourseId = model.CourseId;
            session.StartDate = model.StartDate;
            session.EndDate = model.EndDate;

            _context.Sessions.Update(session);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // ========================
        // DELETE (GET)
        // ========================
        public async Task<IActionResult> Delete(int id)
        {
            var session = await _context.Sessions
                                        .Include(s => s.Course)
                                        .FirstOrDefaultAsync(s => s.SessionId == id);

            if (session == null) return NotFound();
            return View(session);
        }

        // ========================
        // DELETE (POST)
        // ========================
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var session = await _context.Sessions.FindAsync(id);
            if (session == null) return NotFound();

            _context.Sessions.Remove(session);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // ========================
        // HELPERS
        // ========================
        private async Task PopulateCoursesDropDown(int? selectedId = null)
        {
            var courses = await _context.Courses.ToListAsync();
            ViewBag.CourseId = new SelectList(courses, "CourseID", "CourseName", selectedId);
        }
    }
}
