using System;
using System.Linq;
using Modelo.Cadastros;
using ProjAspNetCore31.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProjAspNetCore31.Data.DAL.Cadastros;

namespace ProjAspNetCore31.Areas.Cadastros.Controllers
{
    [Area("Cadastros")]
    public class CursoController : Controller
    {
        private readonly CursoDAL cursoDAL;
        private readonly IESContext _context;
        private readonly DepartamentoDAL departamentoDAL;

        public CursoController(IESContext context)
        {
            _context = context;
            cursoDAL = new CursoDAL(context);
            departamentoDAL = new DepartamentoDAL(context);
        }

        public async Task<IActionResult> Index()
        {
            return View(await cursoDAL.ObterCursosClassificadosPorNome().ToListAsync());
        }

        public IActionResult Create()
        {
            var departamentos = departamentoDAL.ObterDepartamentosClassificadosPorNome().ToList();
            departamentos.Insert(0, new Departamento() { DepartamentoID = 0, Nome = "Selecione o Departamento" });
            ViewBag.Departamento = departamentos;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nome, DepartamentoID")] Curso curso)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await cursoDAL.GravarCurso(curso);
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Não foi possível inserir os dados.");
            }

            return View(curso);
        }

        public async Task<IActionResult> Edit(long? id)
        {
            ViewResult visaoCurso = (ViewResult)await ObterVisaoCursoPorId(id);

            Curso curso = (Curso)visaoCurso.Model;

            ViewBag.Departamento =
                new SelectList(departamentoDAL.ObterDepartamentosClassificadosPorNome(), "DepartamentoID", "Nome", curso.DepartamentoID);

            return visaoCurso;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long? id, [Bind("DepartamentoID, Nome")] Curso curso)
        {
            if (id != curso.CursoID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await cursoDAL.GravarCurso(curso);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await CursoExists(curso.CursoID))
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
            ViewBag.Departamentos =
                new SelectList(departamentoDAL.ObterDepartamentosClassificadosPorNome(), "DepartamentoID", "Nome", curso.DepartamentoID);
            return View(curso);
        }

        public async Task<IActionResult> Details(long? id)
        {
            return await ObterVisaoCursoPorId(id);
        }

        public async Task<IActionResult> Delete(long? id)
        {
            return await ObterVisaoCursoPorId(id);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long? id)
        {
            var curso = await cursoDAL.EliminarCursoPorId((long)id);
            TempData["Message"] = $"Curso {curso.Nome.ToUpper()} foi removido";
            return RedirectToAction(nameof(Index));
        }

        private async Task<IActionResult> ObterVisaoCursoPorId(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var curso = await cursoDAL.ObterCursoPorId((long)id);

            if (curso == null)
            {
                return NotFound();
            }

            return View(curso);
        }

        private async Task<bool> CursoExists(long? id)
        {
            return await cursoDAL.ObterCursoPorId((long)id) != null;
        }
    }
}