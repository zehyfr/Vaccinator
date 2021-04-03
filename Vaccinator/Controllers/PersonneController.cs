using System.Linq;
using System.Threading.Tasks;
using Vaccinator.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Vaccinator.Controllers
{
    public class PersonneController : Controller
    {
        private readonly ContextBDD _context = new ContextBDD();
       
        // GET: Personnes
        public async Task<IActionResult> Index()
        {
            return View(await _context.Personnes.ToListAsync());
        }
        
        // GET: Personne/Details/5
        public async Task<IActionResult> Details(string? uuid)
        {
            if (uuid == null)
            {
                return NotFound();
            }

            var personne = await _context.Personnes
                .FirstOrDefaultAsync(m => m.uuid == uuid);
            if (personne == null)
            {
                return NotFound();
            }

            return View(personne);
        }
        
        // GET: Categories/Create
        public IActionResult Create()
        {
            return View();
        }
        
        // POST: Personnes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("uuid")] Personne personne)
        {
            if (ModelState.IsValid)
            {
                _context.Add(personne);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(personne);
        }
        
        // GET: Personnes/Edit/5
        public async Task<IActionResult> Edit(string? uuid)
        {
            if (uuid == null)
            {
                return NotFound();
            }

            var personne = await _context.Personnes.FindAsync(uuid);
            if (personne == null)
            {
                return NotFound();
            }
            return View(personne);
        }
        
        // POST: Personnes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string uuid, [Bind("Id,Libelle,Description")] Personne personne)
        {
            if (uuid != personne.uuid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(personne);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PersonneExist(personne.uuid))
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
            return View(personne);
        }
        
        // GET: Personnes/Delete/5
        public async Task<IActionResult> Delete(string? uuid)
        {
            if (uuid == null)
            {
                return NotFound();
            }

            var personne = await _context.Personnes
                .FirstOrDefaultAsync(m => m.uuid == uuid);
            if (personne == null)
            {
                return NotFound();
            }

            return View(personne);
        }
        
        // POST: Personne/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string uuid)
        {
            var personne = await _context.Personnes.FindAsync(uuid);
            _context.Personnes.Remove(personne);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PersonneExist(string uuid)
        {
            return _context.Personnes.Any(e => e.uuid == uuid);
        }


    }
}