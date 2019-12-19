using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebFinalProject.Data;
using WebFinalProject.Models.SchoolViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebFinalProject.Controllers
{
    public class CourseAssignmentController : Controller
    {
        // GET: /<controller>/
        private readonly UTEUniversityContext _context;
        public CourseAssignmentController(UTEUniversityContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var viewModel = new CourseIndexData();
            viewModel.Instructors = await _context.Instructors
                .Include(i => i.CourseAssignments)
                    .ThenInclude(i => i.Course)
                .AsNoTracking().ToListAsync();
                
            return View(viewModel);
        }
    }
}
