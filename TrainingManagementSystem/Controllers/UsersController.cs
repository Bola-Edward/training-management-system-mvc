using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrainingManagementSystem.Data;
using TrainingManagementSystem.Models;
using TrainingManagementSystem.ViewModels;

public class UsersController : Controller
{
    private readonly ApplicationDbContext _context;

    public UsersController(ApplicationDbContext context)
    {
        _context = context;
    }

    ///////////////////////////////////////////////////Home/////////////////////////////////////////
    public async Task<IActionResult> Index(string name, string role)
    {
        var users = await _context.Users.ToListAsync();

        if (!string.IsNullOrEmpty(name))
        {
            users = users.Where(u => u.UserName.Contains(name)).ToList();
        }

        if (!string.IsNullOrEmpty(role) && role != "All")
        {
            users = users.Where(u => u.Role == role).ToList();
        }

        ViewBag.CurrentName = name;
        ViewBag.CurrentRole = role;

        return View(users);
    }

    ////////////////////////////////////////////////////Add//////////////////////////////////////////
    [HttpGet]
    public IActionResult Add()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Add(UserViewModel vm)
    {
        if (!ModelState.IsValid) return View(vm);

        var user = new User
        {
            UserName = vm.UserName,
            Email = vm.Email,
            Role = vm.Role
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        TempData["Message"] = "User added successfully!";
        return RedirectToAction("Index");
    }

    //////////////////////////////////////////////Edit///////////////////////////////////////////////////
    [HttpGet]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return BadRequest();

        var user = await _context.Users.FindAsync(id.Value);
        if (user == null) return NotFound();

        var vm = new UserViewModel
        {
            UserId = user.UserID,
            UserName = user.UserName,
            Email = user.Email,
            Role = user.Role
        };

        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, UserViewModel vm)
    {
        if (!ModelState.IsValid) return View(vm);

        var user = await _context.Users.FindAsync(id);
        if (user == null) return NotFound();

        user.UserName = vm.UserName;
        user.Email = vm.Email;
        user.Role = vm.Role;

        _context.Users.Update(user);
        await _context.SaveChangesAsync();

        TempData["Message"] = "User updated successfully!";
        return RedirectToAction("Index");
    }

    //////////////////////////////////////////////Delete///////////////////////////////////////////////////
    [HttpGet]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var user = await _context.Users.FindAsync(id.Value);
        if (user == null) return NotFound();

        return View(user);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user != null)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        return RedirectToAction(nameof(Index));
    }
}
