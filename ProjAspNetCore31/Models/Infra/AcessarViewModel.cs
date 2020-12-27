using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjAspNetCore31.Models.Infra
{
    public class AcessarViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Senha { get; set; }

        [Display(Name = "Lembrar de mim?")]
        public bool LembrarDeMim { get; set; }
    }
}
