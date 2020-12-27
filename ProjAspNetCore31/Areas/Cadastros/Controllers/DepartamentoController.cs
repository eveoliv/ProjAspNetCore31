using System;
using System.Linq;
using Modelo.Cadastros;
using ProjAspNetCore31.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProjAspNetCore31.Data.DAL.Cadastros;

namespace ProjAspNetCore31.Areas.Cadastros.Controllers
{    
    [Area("Cadastros")]
    public class DepartamentoController : Controller
    {
        private readonly IESContext _context;
        private readonly DepartamentoDAL departamentoDAL;
        private readonly InstituicaoDAL instituicaoDAL;

        public DepartamentoController(IESContext context)
        {
            _context = context;
            departamentoDAL = new DepartamentoDAL(context);
            instituicaoDAL = new InstituicaoDAL(context);
        }

        public async Task<IActionResult> Index()
        {
            return View(await departamentoDAL.ObterDepartamentosClassificadosPorNome().ToListAsync());
        }

        public IActionResult Create()
        {
            var instituicoes = instituicaoDAL.ObterInstituicoesClassificadasPorNome().ToList();
            instituicoes.Insert(0, new Instituicao() { InstituicaoID = 0, Nome = "Selecione a Instituição" });
            ViewBag.Instituicoes = instituicoes;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nome, InstituicaoID")] Departamento departamento)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await departamentoDAL.GravarDepartamento(departamento);
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Não foi possível inserir os dados.");
            }

            return View(departamento);
        }

        public async Task<IActionResult> Edit(long? id)
        {
            ViewResult visaoDepartamento = (ViewResult)await ObterVisaoDepartamentoPorId(id);

            Departamento departamento = (Departamento)visaoDepartamento.Model;           

            ViewBag.Instituicoes = 
                new SelectList(instituicaoDAL.ObterInstituicoesClassificadasPorNome(), "InstituicaoID", "Nome", departamento.InstituicaoID);

            return visaoDepartamento;
        }
    
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long? id, [Bind("DepartamentoID, Nome, InstituicaoID")] Departamento departamento)
        {
            if (id != departamento.DepartamentoID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await departamentoDAL.GravarDepartamento(departamento);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await DepartamentoExists(departamento.DepartamentoID))
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
            ViewBag.Instituicoes = 
                new SelectList(instituicaoDAL.ObterInstituicoesClassificadasPorNome(), "InstituicaoID", "Nome", departamento.InstituicaoID);
            return View(departamento);
        }
     
        public async Task<IActionResult> Details(long? id)
        {
            return await ObterVisaoDepartamentoPorId(id);
        }
   
        public async Task<IActionResult> Delete(long? id)
        {
            return await ObterVisaoDepartamentoPorId(id);
        }
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long? id)
        {
            var departamento = await departamentoDAL.EliminarDepartamentoPorId((long)id);
            TempData["Message"] = $"Departamento {departamento.Nome.ToUpper()} foi removido";            
            return RedirectToAction(nameof(Index));
        }

        private async Task<IActionResult> ObterVisaoDepartamentoPorId(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var departamento = await departamentoDAL.ObterDepartamentoPorId((long)id);

            if (departamento == null)
            {
                return NotFound();
            }

            return View(departamento);
        }

        private async Task<bool> DepartamentoExists(long? id)
        {
            return await departamentoDAL.ObterDepartamentoPorId((long)id)!=null;
        }
    }
}