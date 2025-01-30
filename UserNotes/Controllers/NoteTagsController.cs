using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UserNotes.Data;
using UserNotes.Models;

namespace UserNotes.Controllers
{
    public class NoteTagsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public NoteTagsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: NoteTags
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.NoteTags.Include(n => n.Note).Include(n => n.Tag);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: NoteTags/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var noteTag = await _context.NoteTags
                .Include(n => n.Note)
                .Include(n => n.Tag)
                .FirstOrDefaultAsync(m => m.NoteId == id);
            if (noteTag == null)
            {
                return NotFound();
            }

            return View(noteTag);
        }

        // GET: NoteTags/Create
        public IActionResult Create()
        {
            ViewData["NoteId"] = new SelectList(_context.Notes, "Id", "Id");
            ViewData["TagId"] = new SelectList(_context.Tags, "Id", "Id");
            return View();
        }

        // POST: NoteTags/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NoteId,TagId")] NoteTag noteTag)
        {
            if (ModelState.IsValid)
            {
                _context.Add(noteTag);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["NoteId"] = new SelectList(_context.Notes, "Id", "Id", noteTag.NoteId);
            ViewData["TagId"] = new SelectList(_context.Tags, "Id", "Id", noteTag.TagId);
            return View(noteTag);
        }

        // GET: NoteTags/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var noteTag = await _context.NoteTags.FindAsync(id);
            if (noteTag == null)
            {
                return NotFound();
            }
            ViewData["NoteId"] = new SelectList(_context.Notes, "Id", "Id", noteTag.NoteId);
            ViewData["TagId"] = new SelectList(_context.Tags, "Id", "Id", noteTag.TagId);
            return View(noteTag);
        }

        // POST: NoteTags/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("NoteId,TagId")] NoteTag noteTag)
        {
            if (id != noteTag.NoteId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(noteTag);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NoteTagExists(noteTag.NoteId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["NoteId"] = new SelectList(_context.Notes, "Id", "Id", noteTag.NoteId);
            ViewData["TagId"] = new SelectList(_context.Tags, "Id", "Id", noteTag.TagId);
            return View(noteTag);
        }

        // GET: NoteTags/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var noteTag = await _context.NoteTags
                .Include(n => n.Note)
                .Include(n => n.Tag)
                .FirstOrDefaultAsync(m => m.NoteId == id);
            if (noteTag == null)
            {
                return NotFound();
            }

            return View(noteTag);
        }

        // POST: NoteTags/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var noteTag = await _context.NoteTags.FindAsync(id);
            if (noteTag != null)
            {
                _context.NoteTags.Remove(noteTag);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NoteTagExists(int id)
        {
            return _context.NoteTags.Any(e => e.NoteId == id);
        }
    }
}
