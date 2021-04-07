using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vaccinator.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Vaccinator.Controllers
{
    public class PersonneController : Controller
    {
        private readonly ContextBDD _context = new ContextBDD();
       
        // GET: Personnes
        public async Task<IActionResult> Index(string sortOrder,string searchString,string submitFilter,string choosenMaladie,string noRappelSubmit,string date)
        {
            ViewData["NomSortParam"] = String.IsNullOrEmpty(sortOrder) ? "nom_desc" : "";
            ViewData["PrenomSortParam"] = sortOrder == "Prenom" ? "prenom_desc" : "Prenom";
            ViewData["CurrentFilter"] = searchString;
            
            
            var personnes = from p in _context.Personnes select p;
            if (noRappelSubmit != null)
            {
                personnes = personnes.Where(p => p.injections.Contains(_context.Injections.Where(i=>i.DateRappel<DateTime.Now).First()) );
            }
            
            Console.WriteLine(date);
            //todo not the first injections but all injections for this maladie
            if (submitFilter == "Vaccinés")
            {
                if (date != "Selectionnez une date")
                {
                    //select * from personne inner join (injections uuid) where ...
                    var listInjection = _context.Injections.Where(i =>
                        i.Maladie == choosenMaladie && i.DatePrise.Year == int.Parse(date));
                    
                    //personnes =  personnes.Where(p => p.injections.Contains(_context.Injections.Where(i=>i.Maladie==choosenMaladie && i.DatePrise.Year == int.Parse(date))) );

                    var listPersonne = from p in _context.Personnes
                        join i in _context.Injections on p.uuid equals i.Personne.uuid into personnesInjection
                        select new
                        {
                            p,
                            NewPersonnes = personnesInjection.Where(i =>
                                i.Maladie == choosenMaladie && i.DatePrise.Year == int.Parse(date))
                        };
                }
                else
                {
                    var listPersonne = from p in _context.Personnes
                        join i in _context.Injections on p.uuid equals i.Personne.uuid into personnesInjection
                        select new
                        {
                            p,
                            NewPersonnes = personnesInjection.Where(i =>
                                i.Maladie == choosenMaladie)
                        };
                }
            }else if (submitFilter == "Non Vaccinés")
            {
                if (date != "Selectionnez une date")
                {
                    personnes = personnes.Where(p => !p.injections.Contains(_context.Injections.Where(i=>i.Maladie==choosenMaladie && i.DatePrise.Year == int.Parse(date)).First()));
  
                }
                else
                {
                    personnes = personnes.Where(p => !p.injections.Contains(_context.Injections.Where(i=>i.Maladie==choosenMaladie).First()));
  
                }
            }

            if (!String.IsNullOrEmpty(searchString))
            {
                personnes = personnes.Where(p => p.nom.Contains(searchString)||
                                                 p.prenom.Contains(searchString));
            }
            
            switch (sortOrder)
            {
                case "nom_desc":
                    personnes = personnes.OrderByDescending(p => p.nom);
                    break;
                case "prenom_desc" :
                    personnes = personnes.OrderByDescending(p => p.prenom);
                    break;
                default:
                    personnes = personnes.OrderBy(p => p.nom);
                    break;
            }

            List<string> listYears = new List<string>();
            listYears.Add("Selectionnez une date");
            for (int i = DateTime.Today.Year; i > DateTime.Today.Year - 50; i--)
            {
                listYears.Add(i.ToString());
            }
            ViewData["listeYears"] = listYears;
            
            List<string> ListMaladies = new List<string>();
            foreach (var injection in _context.Injections)
            {
                if (!ListMaladies.Contains(injection.Maladie))
                {
                    ListMaladies.Add(injection.Maladie);
                }
            }

            ViewData["listeMaladies"]= ListMaladies;
            
            return View(await personnes.AsNoTracking().ToListAsync());
        }
        
        // GET: Personne/Injection
        public async Task<IActionResult> InjectionPerPersonne(string? uuid)
        {
            
            if (uuid == null)
            {
                return NotFound();
            }

            Personne personne = _context.Personnes.Where(p=>p.uuid==uuid).First();
            ViewData["Personne"] = personne;
            
            var listInjectionsPersonne = await _context.Injections
                .Where(injection => injection.Personne.uuid == uuid).ToListAsync();

            if (listInjectionsPersonne.Count == 0)
            {
                ViewBag.Message = "Cette personne n'a aucune injection";
            }
            
            return View(listInjectionsPersonne);
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
        public async Task<IActionResult> Create([Bind("nom,prenom,ddn,sexe,role")] Personne personne)
        {
           
            var new_uuid = Guid.NewGuid().ToString();
            personne.uuid = new_uuid;
            //Force la réévaluation de modelState
            ModelState.Clear();
            TryValidateModel(personne);
            
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