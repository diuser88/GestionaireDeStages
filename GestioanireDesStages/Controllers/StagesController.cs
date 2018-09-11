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
    public class StagesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StagesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Stages
        [Authorize(Roles ="Administrateur, Superviseur, Stagiaire")]
        [Route("Stages/Index/{PersonneId}")]
        public async Task<IActionResult> Index(int? PersonneId)
        {
            if (User.IsInRole("Stagiaire"))
            {
                var test = _context.Personnes.FirstOrDefault(p => p.Courriel == User.Identity.Name);
                return View(await _context.Stages.Where(stage => stage.Stagiaire == test.PersonneId).ToListAsync());
            }
            return View(await _context.Stages.Where(stage => stage.Stagiaire == PersonneId).ToListAsync());
        }

        // GET: Stages/Details/5
        [Authorize(Roles = "Administrateur, Superviseur, Stagiaire")]
        [Route("Stages/Details/{stageid}")]
        public async Task<IActionResult> Details(int? stageid)
        {
            
            if (stageid == null)
            {
                return NotFound();
            }

            var stage = await _context.Stages
                .SingleOrDefaultAsync(m => m.StageId == stageid);
            if (stage == null)
            {
                return NotFound();
            }

            return View(stage);
        }

        // GET: Stages/Create
        [Authorize(Roles = "Administrateur, Superviseur")]
        [Route("Stages/Create/{PersonneId}")]
        public IActionResult Create(int PersonneId)
        {
            return View();
        }

        // POST: Stages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrateur, Superviseur")]
        [Route("Stages/Create/{PersonneId}")]
        public async Task<IActionResult> Create([Bind("StageId,Titre,Debut,Fin,Commentaire")] Stage stage, int PersonneId )
        {

            stage.Stagiaire = PersonneId;

            if (ModelState.IsValid)
            {
                _context.Add(stage);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(stage);
        }

        // GET: Stages/Edit/5
        [Authorize(Roles = "Administrateur, Superviseur")]
        [Route("Stages/Edit/{PersonneId}/{id}")]
        public async Task<IActionResult> Edit(int? id, int PersonneId)
        {

            if (id == null)
            {
                return NotFound();
            }

            var stage = await _context.Stages.SingleOrDefaultAsync(m => m.StageId == id);
            if (stage == null)
            {
                return NotFound();
            }
            return View(stage);
        }

        // POST: Stages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrateur, Superviseur")]
        [Route("Stages/Edit/{PersonneId}/{id}")]
        public async Task<IActionResult> Edit(int id, [Bind("StageId,Titre,Debut,Fin,Commentaire,Stagiaire")]
        Stage stage, int PersonneId)
        {
            if (id != stage.StageId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(stage);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StageExists(stage.StageId))
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
            return View(stage);
        }

        // GET: Stages/Delete/5
        [Authorize(Roles = "Administrateur, Superviseur")]
        [Route("Delete/{id}/{PersonneId}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var stage = await _context.Stages
                .SingleOrDefaultAsync(m => m.StageId == id);
            if (stage == null)
            {
                return NotFound();
            }

            return View(stage);
        }

        // POST: Stages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Route("Delete/{id}/{PersonneId}")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var stage = await _context.Stages.SingleOrDefaultAsync(m => m.StageId == id);
            _context.Stages.Remove(stage);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StageExists(int id)
        {
            return _context.Stages.Any(e => e.StageId == id);
        }
    }
}
