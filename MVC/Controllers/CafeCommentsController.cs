using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Core.Models;
using Data;
using Microsoft.AspNetCore.Authorization;

namespace MVC.Controllers
{
    [Authorize]
    public class CafeCommentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CafeCommentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CafeComments
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.CafeComments.Include(c => c.Cafes);
            return View(await applicationDbContext.ToListAsync());
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(CafeCommentViewModel vm)
        {
            var comment = vm.Comment;
            var cafeId = vm.CafesId;
            var rating = vm.Rating;

            CafeComment cafcomment = new CafeComment()
            {
                CafesId = cafeId,
                Comments = comment,
                Rating = rating,
                PublishedDate = DateTime.Now
            };

            _context.CafeComments.Add(cafcomment);
            _context.SaveChanges();

            return RedirectToAction("Details", "Cafes", new { id = cafeId });

        }


        // GET: CafeComments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cafeComment = await _context.CafeComments
                .Include(c => c.Cafes)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cafeComment == null)
            {
                return NotFound();
            }

            return View(cafeComment);
        }

        // GET: CafeComments/Create
        public IActionResult Create()
        {
            ViewData["CafesId"] = new SelectList(_context.Cafe, "Id", "Impressoes");
            return View();
        }

        // POST: CafeComments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Comments,PublishedDate,CafesId,Rating")] CafeComment cafeComment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cafeComment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CafesId"] = new SelectList(_context.Cafe, "Id", "Impressoes", cafeComment.CafesId);
            return View(cafeComment);
        }

        // GET: CafeComments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cafeComment = await _context.CafeComments.FindAsync(id);
            if (cafeComment == null)
            {
                return NotFound();
            }
            ViewData["CafesId"] = new SelectList(_context.Cafe, "Id", "Impressoes", cafeComment.CafesId);
            return View(cafeComment);
        }

        // POST: CafeComments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Comments,PublishedDate,CafesId,Rating")] CafeComment cafeComment)
        {
            if (id != cafeComment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(cafeComment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CafeCommentExists(cafeComment.Id))
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
            ViewData["CafesId"] = new SelectList(_context.Cafe, "Id", "Impressoes", cafeComment.CafesId);
            return View(cafeComment);
        }

        // GET: CafeComments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cafeComment = await _context.CafeComments
                .Include(c => c.Cafes)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cafeComment == null)
            {
                return NotFound();
            }

            return View(cafeComment);
        }

        // POST: CafeComments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cafeComment = await _context.CafeComments.FindAsync(id);
            _context.CafeComments.Remove(cafeComment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CafeCommentExists(int id)
        {
            return _context.CafeComments.Any(e => e.Id == id);
        }
    }
}
