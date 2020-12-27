using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ProjAspNetCore31.Areas.Docente.Models
{
    public class AdicionarProfessorViewModel
    {
        public long? CursoID { get; set; }
        public long? ProfessorID { get; set; }
        public long? InstituicaoID { get; set; }
        public long? DepartamentoID { get; set; }
    }
}
