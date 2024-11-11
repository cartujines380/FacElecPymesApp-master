using System.Collections.Generic;

using Sipecom.FactElec.Pymes.Negocios.Seguridad.Mensajes;
using Sipecom.FactElec.Pymes.Servicios.Api.Identidad.Services;

namespace Sipecom.FactElec.Pymes.Servicios.Api.Identidad.Models.Account
{
    public class UserViewModel
    {
        public UserViewModel()
        { }

        public UserViewModel(UsuarioData usuario)
        {
            SubjectId = Converter.ToString(usuario.Id);
            Name = usuario.Nombre;
            Username = usuario.NombreUsuario;
        }

        public string SubjectId { get; set; }

        public string Username { get; set; }

        public string Name { get; set; }
    }
}
