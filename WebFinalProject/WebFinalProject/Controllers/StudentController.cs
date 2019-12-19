using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebFinalProject.Data;
using WebFinalProject.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebFinalProject.Controllers
{
   
    public class StudentController : Controller
    {
        // GET: /<controller>/
        private readonly UTEUniversityContext _context;
        public StudentController(UTEUniversityContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.Students.ToListAsync());
        }
        [HttpGet]
        
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EnrollmentDate","Birthday","LastName","FirstName")] Student student)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    _context.Students.Add(student);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch(DbUpdateException)
            {
                ModelState.AddModelError("", "Unable to save change" + "Please contact to admin");
            }
            return View(student);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var student = await _context.Students.FindAsync(id);
            if(student == null)
            {
                return NotFound();
            }
            return View(student);
        }
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var studentUpdate = await _context.Students.FirstOrDefaultAsync(s => s.StudentID == id);
            if (await TryUpdateModelAsync<Student>(
                studentUpdate, "", s => s.EnrollmentDate, s => s.FirstName, s => s.LastName, s => s.Birthday))
            {
                try
                {
                    
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError("", "Unable to edit changes" + "Please contact to  admin");
                }
            }
            return View(studentUpdate);
        }
        //Method: Student Details
       
        public async Task<IActionResult> Details(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var student = await _context.Students
                .Include(i => i.Enrollments)
                    .ThenInclude(i => i.Course)

                .AsNoTracking().FirstOrDefaultAsync(s => s.StudentID == id);
            if(student == null)
            {
                return NotFound();
            }
            return View(student);
        }
        //Method: Deleted Student
        public async Task<IActionResult> Delete(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var student = await _context.Students.AsNoTracking().FirstOrDefaultAsync(s => s.StudentID == id);
            if(student == null)
            {
                return NotFound();
            }
            return View(student);
        }
        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Deleted(int? id)
        {
            var student = await _context.Students.FindAsync(id);
            if(student == null)
            {
                return RedirectToAction(nameof(Index));
            }
            try
            {
                _context.Remove(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch(DbUpdateException)
            {

                return RedirectToAction(nameof(Delete));
            }
            
        }
    }
}
