﻿using System;
using System.Linq;
using Modelo.Cadastros;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ProjAspNetCore31.Data.DAL.Cadastros
{
    public class DepartamentoDAL
    {
        private readonly IESContext _context;

        public DepartamentoDAL(IESContext context)
        {
            _context = context;
        }
        public IQueryable<Departamento> ObterDepartamentosClassificadosPorNome()
        {
            return _context.Departamentos.Include(i => i.Instituicao).OrderBy(d => d.Nome);
        }
        public async Task<Departamento> ObterDepartamentoPorId(long id)
        {
            var departamento = await _context.Departamentos.SingleOrDefaultAsync(d => d.DepartamentoID == id);
            _context.Instituicoes.Where(i => departamento.InstituicaoID == i.InstituicaoID).Load();
            return departamento;
        }
        public async Task<Departamento> GravarDepartamento(Departamento departamento)
        {
            if (departamento.InstituicaoID == null)
            {
                _context.Departamentos.Add(departamento);
            }
            else
            {
                _context.Update(departamento);
            }

            await _context.SaveChangesAsync();
            return departamento;
        }
        public async Task<Departamento> EliminarDepartamentoPorId(long id)
        {
            Departamento departamento = await ObterDepartamentoPorId(id);
            _context.Departamentos.Remove(departamento);
            await _context.SaveChangesAsync();
            return departamento;
        }

        public IQueryable<Departamento> ObterDepartamentosPorInstituicao(long instituicaoID)
        {
            var departamentos = _context.Departamentos.Where(d => d.InstituicaoID == instituicaoID).OrderBy(d => d.Nome);
            return departamentos;
        }
    }
}
