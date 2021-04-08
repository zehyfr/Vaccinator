using System;
using System.Linq;
using System.Threading.Tasks;
using Vaccinator.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Vaccinator.Controllers
{
    public class InjectionController : Controller
    {
        private readonly ContextBDD _context = new ContextBDD();
       
        // GET: Injections
        public async Task<IActionResult> Index()
        {
            return View(await _context.Injections.ToListAsync());
        }
        
        // GET: Injections/Details/5
        public async Task<IActionResult> Details(string? uuid)
        {
            if (uuid == null)
            {
                return NotFound();
            }

            var injection = await _context.Injections
                .FirstOrDefaultAsync(m => m.uuid == uuid);
            if (injection == null)
            {
                return NotFound();
            }

            return View(injection);
        }
        
        // GET: Injections/Create
        public IActionResult Create(string uuid)
        {
            Personne personne = _context.Personnes.Where(p=>p.uuid==uuid).First();
            ViewData["Personne"] = personne;
            return View();
        }
        
        // POST: Injections/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Maladie,Marque,NumLot,DatePrise,DateRappel,StatusRappel")] Injection injection,string Personne, bool isRappel)
        {
            var newUuid = Guid.NewGuid().ToString();
            injection.uuid = newUuid;
            injection.StatusRappel = false;
            var newPersonne = await _context.Personnes.FindAsync(Personne);
            injection.Personne = newPersonne;
            
            Console.WriteLine(isRappel);
            Console.WriteLine(newPersonne.uuid);
            //Force la réévaluation de modelState
            ModelState.Clear();
            TryValidateModel(injection);

            if (isRappel)
            {
                Console.WriteLine("c'est un rappel");
                Injection oldInjection = _context.Injections
                    .Where(i => i.Maladie == injection.Maladie && i.StatusRappel == false).OrderBy(i => i.DatePrise).Last();
                if (oldInjection != null)
                {
                    oldInjection.StatusRappel = true;
                    _context.Update(oldInjection);
                    await _context.SaveChangesAsync();
                }
            }

            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(injection);
                    await _context.SaveChangesAsync();  
                    return RedirectToAction("InjectionPerPersonne","Personne" ,new{uuid=newPersonne.uuid});
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return RedirectToAction("InjectionPerPersonne","Personne" ,new{uuid=newPersonne.uuid});
            
        }
        
        // GET: Injections/Edit/5
        public async Task<IActionResult> Edit(string? uuid)
        {
            if (uuid == null)
            {
                return NotFound();
            }

            var injection = await _context.Injections.FindAsync(uuid);
            if (injection == null)
            {
                return NotFound();
            }
            ViewData["listePersonnes"]= new SelectList(_context.Personnes,"uuid","nom");
            return View(injection);
        }
        
        // POST: Injections/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string uuid, [Bind("uuid")] Injection injection)
        {
            if (uuid != injection.uuid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(injection);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InjectionExist(injection.uuid))
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
            return View(injection);
        }
        
        // GET: Injections/Delete/5
        public async Task<IActionResult> Delete(string? uuid)
        {
            if (uuid == null)
            {
                return NotFound();
            }

            var injection = await _context.Injections
                .FirstOrDefaultAsync(m => m.uuid == uuid);
            if (injection == null)
            {
                return NotFound();
            }

            return View(injection);
        }
        
        // POST: Injection/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string uuid)
        {
            var injection = await _context.Injections.FindAsync(uuid);
            _context.Injections.Remove(injection);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InjectionExist(string uuid)
        {
            return _context.Injections.Any(e => e.uuid == uuid);
        }


    }
}