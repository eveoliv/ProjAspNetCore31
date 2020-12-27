using System;
using System.Linq;
using Modelo.Docente;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ProjAspNetCore31.Data.DAL.Docente
{
    public class ProfessorDAL
    {
        private IESContext _context;

        public ProfessorDAL(IESContext context)
        {
            _context = context;
        }

        public IQueryable<Professor> ObterProfessoresClassificadosPorNome()
        {
            return _context.Professores.OrderBy(p => p.Nome);
        }

        public async Task<Professor> ObterProfessorPorId(long id)
        {
            return await _context.Professores.FindAsync(id);
        }

        public async Task<Professor> GravarProfessor(Professor professor)
        {
            if (professor.ProfessorID == null)
            {
                _context.Professores.Add(professor);
            }
            else
            {
                _context.Update(professor);
            }

            await _context.SaveChangesAsync();
            return professor;
        }

        public async Task<Professor> EliminarProfessorPorId(long id)
        {
            Professor professor = await ObterProfessorPorId(id);
            _context.Professores.Remove(professor);
            await _context.SaveChangesAsync();
            return professor;
        }
    }
}
