using System;
using System.Linq;
using Modelo.Discente;
using Modelo.Cadastros;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using ProjAspNetCore31.Models.Infra;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace ProjAspNetCore31.Data
{
    public class IESContext : IdentityDbContext<UsuarioDaAplicacao>
    {
        public IESContext(DbContextOptions<IESContext> options) : base(options) { }

        public DbSet<Curso> Cursos { get; set; }
        public DbSet<Academico> Academicos { get; set; }
        public DbSet<Disciplina> Disciplinas { get; set; }
        public DbSet<Instituicao> Instituicoes { get; set; }
        public DbSet<Departamento> Departamentos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) 
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Curso>().ToTable("Curso");
            modelBuilder.Entity<Academico>().ToTable("Academico");
            modelBuilder.Entity<Disciplina>().ToTable("Disciplina");
            modelBuilder.Entity<Instituicao>().ToTable("Instituicao");
            modelBuilder.Entity<Departamento>().ToTable("Departamento");

            modelBuilder.Entity<CursoDisciplina>()
                .HasKey(cd => new { cd.CursoID, cd.DisciplinaID });
            
            modelBuilder.Entity<CursoDisciplina>()
                .HasOne(c => c.Curso).WithMany(cd => cd.CursosDisciplinas).HasForeignKey(c => c.CursoID);

            modelBuilder.Entity<CursoDisciplina>()
                .HasOne(d => d.Disciplina).WithMany(cd => cd.CursosDisciplinas).HasForeignKey(d => d.DisciplinaID);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //codigo opcional para consumo do servico sqlserver
            //optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database = ProjAspNetCore31; Trusted_Connection = True; MultipleActiveResultSets = true");
        }
    }
}
