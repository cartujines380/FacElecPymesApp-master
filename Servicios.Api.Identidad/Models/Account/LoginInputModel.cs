using System;
using System.ComponentModel.DataAnnotations;

namespace Sipecom.FactElec.Pymes.Servicios.Api.Identidad.Models.Account
{
    public class LoginInputModel
    {
        [Required(ErrorMessage = "El campo usuario es requerido")]
        public string Username { get; set; }

        [Required(ErrorMessage = "El campo contraseña es requerido")]
        public string Password { get; set; }

        public bool RememberLogin { get; set; }

        public string ReturnUrl { get; set; }
    }
}