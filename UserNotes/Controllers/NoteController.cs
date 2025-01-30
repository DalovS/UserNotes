using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UserNotes.Data;
using UserNotes.Models;

namespace UserNotes.Controllers
{
    public class NoteController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public NoteController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [Authorize]
        // GET: Note
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var notes = await _context.Notes
                .Where(note => note.UserId == userId).Include(n=>n.Category).ToListAsync();
            return View(notes);
        }

        // GET: Note/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var note = await _context.Notes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (note == null)
            {
                return NotFound();
            }

            return View(note);
        }
        [Authorize]
        // GET: Note/Create
        public async Task<IActionResult> Create()
        {
            var categories = await _context.Categories.ToListAsync();
            ViewData["CategoryId"] = new SelectList(categories, "Id", "Name");
           
            return View();
        }

        // POST: Note/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create([Bind("Id,Title,Content,CategoryId")] Note note)
        {
        
            try {

                note.UserId = _userManager.GetUserId(User);
                _context.Add(note);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                var categories = await _context.Categories.ToListAsync();
                ViewData["CategoryId"] = new SelectList(categories, "Id", "Name");
                return View(note);
            }

           
           

        }

        // GET: Note/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var note = await _context.Notes.FindAsync(id);
            var userId = _userManager.GetUserId(User);



            if (note == null||note.UserId != userId )
            {
                return NotFound();
            }
            return View(note);
        }

        // POST: Note/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Content")] Note note)
        {
            if (id != note.Id)
            {
                return NotFound();
            }

            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var notes = await _context.Notes.FirstOrDefaultAsync
                    (note => note.Id == id && note.UserId == userId);
                notes.Id = note.Id;
                notes.Title = note.Title;
                notes.Content = note.Content;

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

            }
            catch
            {
                return View(note);
            }



           
        }

        // GET: Note/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var note = await _context.Notes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (note == null)
            {
                return NotFound();
            }

            return View(note);
        }

        // POST: Note/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var note = await _context.Notes.FindAsync(id);
            if (note != null)
            {
                _context.Notes.Remove(note);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NoteExists(int id)
        {
            return _context.Notes.Any(e => e.Id == id);
        }
    }
}
