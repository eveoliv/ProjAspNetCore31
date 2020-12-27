using System;
using System.Linq;
using Modelo.Docente;
using Modelo.Cadastros;
using ProjAspNetCore31.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using ProjAspNetCore31.Data.DAL.Docente;
using ProjAspNetCore31.Data.DAL.Cadastros;
using ProjAspNetCore31.Areas.Docente.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ProjAspNetCore31.Areas.Docente.Controllers
{
    [Area("Docente")]
    public class ProfessorController : Controller
    {
        private readonly CursoDAL cursoDAL;
        private readonly IESContext _context;
        private readonly ProfessorDAL professorDAL;
        private readonly InstituicaoDAL instituicaoDAL;
        private readonly DepartamentoDAL departamentoDAL;

        public ProfessorController(IESContext context)
        {
            _context = context;
            cursoDAL = new CursoDAL(context);
            professorDAL = new ProfessorDAL(context);
            instituicaoDAL = new InstituicaoDAL(context);
            departamentoDAL = new DepartamentoDAL(context);
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AdicionarProfessor()
        {
            PrepararViewBags(
                instituicaoDAL.ObterInstituicoesClassificadasPorNome().ToList(),
                new List<Departamento>().ToList(),
                new List<Curso>().ToList(),
                new List<Professor>().ToList());

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AdicionarProfessor([Bind("InstituicaoID, DepartamentoID, CursoID, ProfessorID")] AdicionarProfessorViewModel model)
        {
            if (model.InstituicaoID == 0 || model.DepartamentoID == 0 || model.CursoID == 0 || model.ProfessorID == 0)
            {
                ModelState.AddModelError("", "É preciso selecionar todos os dados.");
            }
            else
            {
                cursoDAL.RegistrarProfessor((long)model.CursoID, (long)model.ProfessorID);

                PrepararViewBags(
                    instituicaoDAL.ObterInstituicoesClassificadasPorNome().ToList(),
                    departamentoDAL.ObterDepartamentosPorInstituicao((long)model.InstituicaoID).ToList(),
                    cursoDAL.ObterCursosPorDepartamento((long)model.DepartamentoID).ToList(),
                    cursoDAL.ObterProfessoresForaDoCurso((long)model.CursoID).ToList());
            }
            return View(model);
        }

        public void PrepararViewBags(List<Instituicao> instituicoes, 
            List<Departamento> departamentos, List<Curso> cursos, List<Professor> professores)
        {
            instituicoes.Insert(0, new Instituicao() { InstituicaoID = 0, Nome = "Selecione a Instituição" });
            ViewBag.Instituicoes = instituicoes;

            departamentos.Insert(0, new Departamento() { DepartamentoID = 0, Nome = "Selecione o Departamento" });
            ViewBag.Departamentos = departamentos;

            cursos.Insert(0, new Curso() {  CursoID = 0, Nome = "Selecionar o Curso"});
            ViewBag.Cursos = cursos;

            professores.Insert(0, new Professor() { ProfessorID = 0, Nome = "Selecionar o Professor"});
            ViewBag.Professores = professores;
        }

        public JsonResult ObterDepartamentosPorInstituicao(long actionID)
        {
            var departamentos = departamentoDAL.ObterDepartamentosPorInstituicao(actionID).ToList();
            return Json(new SelectList(departamentos, "DepartamentoID", "Nome"));
}
        public JsonResult ObterCursosPorDepartamento(long actionID)
        {
            var cursos = cursoDAL.ObterCursosPorDepartamento(actionID).ToList();
            return Json(new SelectList(cursos, "CursoID", "Nome"));
        }
        public JsonResult ObterProfessoresForaDoCurso(long actionID)
        {
            var professores = cursoDAL.ObterProfessoresForaDoCurso(actionID).ToList();
            return Json(new SelectList(professores, "ProfessorID", "Nome"));
        }

    }
}