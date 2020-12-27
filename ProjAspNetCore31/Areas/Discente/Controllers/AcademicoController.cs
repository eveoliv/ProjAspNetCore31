using System;
using System.Linq;
using Modelo.Discente;
using ProjAspNetCore31.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using ProjAspNetCore31.Data.DAL.Discente;

namespace ProjAspNetCore31.Areas.Discente.Controllers
{
    [Area("Discente")]
    public class AcademicoController : Controller
    {
        private readonly IESContext _context;
        private readonly AcademicoDAL academicoDAL;

        public AcademicoController(IESContext context)
        {
            _context = context;
            academicoDAL = new AcademicoDAL(context);
        }

        public async Task<IActionResult> Index()
        {
            return View(await academicoDAL.ObterAcademicosClassificadosPorNome().ToListAsync());
        }

        public async Task<IActionResult> ObterVisalAcademicoPorId(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var academico = await academicoDAL.ObterAcademicoPorId((long)id);

            if (academico == null)
            {
                return NotFound();
            }

            return View(academico);
        }

        public async Task<IActionResult> Details(long? id)
        {
            return await ObterVisalAcademicoPorId(id);
        }

        public async Task<IActionResult> Delete(long? id)
        {
            return await ObterVisalAcademicoPorId(id);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nome, RegistroAcademico, Nascimento")]Academico academico)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await academicoDAL.GravarAcademico(academico);
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Não foi possível inserir os dados.");
            }

            return View(academico);
        }

        public async Task<IActionResult> Edit(long? id)
        {
            return await ObterVisalAcademicoPorId(id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long? id, [Bind("AcademidoID, Nome, RegistroAcademico, Nascimento")]Academico academico)
        {
            if (id != academico.AcademicoID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await academicoDAL.GravarAcademico(academico);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await AcademicoExists(academico.AcademicoID))
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
            return View(academico);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long? id)
        {
            var academico = await academicoDAL.EliminarAcademicoPorId((long)id);
            TempData["Message"] = $"Acadêmico {academico.Nome.ToUpper()} foi removido(a).";
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> AcademicoExists(long? id)
        {
            return await academicoDAL.ObterAcademicoPorId((long)id) != null;
        }
    }
}