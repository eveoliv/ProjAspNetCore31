using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjAspNetCore31.Models.Infra
{
    public class RegistrarNovoUsuarioViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Senha")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "A {0} precisa ter ao menos {2} e no máximo {1} caracteres de cumprimento.", MinimumLength = 6)]
        public string Password { get; set; }
       
        [Display(Name = "Confirmar Senha")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Os valores informados para SENHA e CONFIRMAÇÃO não são iguais.")]
        public string ConfirmPassword { get; set; }
    }
}
