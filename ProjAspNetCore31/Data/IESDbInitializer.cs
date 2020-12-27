using System;
using System.Linq;
using Modelo.Cadastros;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ProjAspNetCore31.Data
{
    public class IESDbInitializer
    {
        public static void Initialize(IESContext context)
        {
            //context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
               
            //-------------------------------------------------------------------------//
            if (context.Instituicoes.Any())            
                return;            

            var instituicoes = new Instituicao[]
            {
                new Instituicao { Nome = "UniParaná",      Endereco = "Paraná" },
                new Instituicao { Nome = "UniSanta",       Endereco = "Santa Catarina" },
                new Instituicao { Nome = "UniSãoPaulo",    Endereco = "São Paulo" },
                new Instituicao { Nome = "UniSulgrandense",Endereco = "Rio Grande do Sul" },
                new Instituicao { Nome = "UniCarioca",     Endereco = "Rio de Janeiro" }
            };

            foreach (Instituicao i in instituicoes)            
                context.Instituicoes.Add(i);

            context.SaveChanges();
            //-------------------------------------------------------------------------//

            if (context.Departamentos.Any())
                return;

            var departamentos = new Departamento[]
            {
                new Departamento{Nome = "Ciência da Computação", InstituicaoID = 1},
                new Departamento{Nome = "Ciência de Alimentos", InstituicaoID = 2}
            };

            foreach (Departamento d in departamentos)
                context.Departamentos.Add(d);


            context.SaveChanges();
        }
    }
}
