using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GestioanireDesStages.Data;
using GestioanireDesStages.Models;
using Microsoft.AspNetCore.Authorization;

namespace GestioanireDesStages.Controllers
{
    public class SuperviseursController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SuperviseursController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Superviseurs
        [Authorize(Roles ="Administrateur")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Personnes.Where(p => p.Superviseur).ToListAsync());
        }

        // GET: Superviseurs/Details/5
        [Authorize(Roles = "Administrateur")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var personne = await _context.Personnes
                .SingleOrDefaultAsync(m => m.PersonneId == id);
            if (personne == null)
            {
                return NotFound();
            }

            return View(personne);
        }

        // GET: Superviseurs/Create
        [Authorize(Roles = "Administrateur")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Superviseurs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrateur")]
        public async Task<IActionResult> Create([Bind("PersonneId,Nom,Prenom,Telephone,Courriel,Administrateur,Superviseur,Stagiaire")] Personne personne)
        {
            if (ModelState.IsValid)
            {
                _context.Add(personne);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(personne);
        }

        // GET: Superviseurs/Edit/5
        [Authorize(Roles = "Administrateur")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var personne = await _context.Personnes.SingleOrDefaultAsync(m => m.PersonneId == id);
            if (personne == null)
            {
                return NotFound();
            }
            return View(personne);
        }

        // POST: Superviseurs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrateur")]
        public async Task<IActionResult> Edit(int id, [Bind("PersonneId,Nom,Prenom,Telephone,Courriel,Administrateur,Superviseur,Stagiaire")] Personne personne)
        {
            if (id != personne.PersonneId)
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
                    if (!PersonneExists(personne.PersonneId))
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

        // GET: Superviseurs/Delete/5
        [Authorize(Roles = "Administrateur")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var personne = await _context.Personnes
                .SingleOrDefaultAsync(m => m.PersonneId == id);
            if (personne == null)
            {
                return NotFound();
            }

            return View(personne);
        }

        // POST: Superviseurs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrateur")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var personne = await _context.Personnes.SingleOrDefaultAsync(m => m.PersonneId == id);
            _context.Personnes.Remove(personne);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PersonneExists(int id)
        {
            return _context.Personnes.Any(e => e.PersonneId == id);
        }
    }
}
