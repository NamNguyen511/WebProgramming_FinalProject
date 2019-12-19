using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebFinalProject.Data;
using WebFinalProject.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebFinalProject.Controllers
{
    public class DepartmentController : Controller
    {
        // GET: /<controller>/
        private readonly UTEUniversityContext _context;
        public DepartmentController(UTEUniversityContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.Departments.ToListAsync());
        }

        //Method : Create Department
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name","Budget","StartDate")] Department department)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    _context.Departments.Add(department);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            catch(DbUpdateException)
            {
                ModelState.AddModelError("", "Unable to create new department");
            }
            return View(department);
        }
        //Method : Edit Department
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var department = await _context.Departments.FindAsync(id);
            if(department == null)
            {
                return NotFound();
            }
            return View(department);
        }
        [HttpPost(Name = "Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var departmentUpdate = await _context.Departments.FirstOrDefaultAsync(s => s.DepartmentID == id);
            if(await TryUpdateModelAsync<Department>
                (departmentUpdate,"",s => s.Name,s => s.Budget,s => s.StartDate))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch(DbUpdateException)
                {
                    ModelState.AddModelError("", "Unable to edit department");
                }
            }
            return View(departmentUpdate);
        }
        //Method : Department Details
        public async Task<IActionResult> Details(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var department = await _context.Departments.AsNoTracking().FirstOrDefaultAsync(m => m.DepartmentID == id);
            if (department == null)
            {
                return NotFound();
            }
            return View(department);
        }
        //Method : delete Department
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var department = await _context.Departments.AsNoTracking().FirstOrDefaultAsync(m => m.DepartmentID == id);
            if (department == null)
            {
                return NotFound();
            }
            return View(department);
        }
        [HttpPost(Name = "Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Deleted(int? id)
        {
            var department = await _context.Departments.FindAsync(id);
            if(id == null)
            {
                return RedirectToAction(nameof(Index));
            }
            try
            {
                _context.Remove(department);
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
