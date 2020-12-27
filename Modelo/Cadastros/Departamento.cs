using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Modelo.Cadastros
{
    public class Departamento
    {
        public long? DepartamentoID { get; set; }
        public string Nome { get; set; }
        public long? InstituicaoID { get; set; }
        public Instituicao Instituicao { get; set; }
        public virtual ICollection<Curso> Cursos { get; set; }

        //DepartamentoID(chave primária) e InstituicaoID(chave estrangeira)
        //Instituicao representa as associações com objetos de seus respectivos tipos
    }
}
