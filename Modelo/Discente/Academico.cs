using System;
using System.Text;
using System.ComponentModel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Modelo.Discente
{
    public class Academico
    {
        [DisplayName("ID")]
        public long? AcademicoID { get; set; }

        [Required]
        [DisplayName("RA")]
        [RegularExpression("([0-9{10}])")]
        [StringLength(10, MinimumLength = 10)]
        public string RegistroAcademico { get; set; }

        [Required]
        public string Nome { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime? Nascimento { get; set; }
    }
}
