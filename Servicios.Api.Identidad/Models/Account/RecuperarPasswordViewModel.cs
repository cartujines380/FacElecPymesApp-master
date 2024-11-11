using System.ComponentModel.DataAnnotations;

namespace Sipecom.FactElec.Pymes.Servicios.Api.Identidad.Models.Account
{
    public class RecuperarPasswordViewModel
    {
        [Required(ErrorMessage = "El campo usuario es requerido")]
        [Display(Name = "Usuario")]
        public string Username { get; set; }

        public string ReturnUrl { get; set; }

        public string Accion { get; set; }
    }
}
