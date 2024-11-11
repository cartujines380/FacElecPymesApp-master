using System;
using System.ComponentModel.DataAnnotations;

namespace Sipecom.FactElec.Pymes.Servicios.Api.Identidad.Models.Account
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "El campo usuario es requerido")]
        [Display(Name = "Usuario")]
        public string Username { get; set; }

        [Required(ErrorMessage = "El campo Clave anterior es requerido")]
        [Display(Name = "Clave anterior")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "El campo Clave nueva es requerido")]
        [Display(Name = "Clave nueva")]
        public string NewPassword { get; set; }

        public string ReturnUrl { get; set; }
    }
}
