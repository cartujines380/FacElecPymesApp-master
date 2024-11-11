using System;
using System.Threading.Tasks;

using Sipecom.FactElec.Pymes.Negocios.Seguridad.Mensajes;

namespace Sipecom.FactElec.Pymes.Negocios.Seguridad.Servicios
{
    public interface IRegistroUsuarioService : IDisposable
    {
        AutenticarResponse Autenticar(string usuario, string contrasena);

        Task<AutenticarResponse> AutenticarAsync(string usuario, string contrasena);

        CambioClaveResponse CambiarClave(string usuario, string claveAnt, string claveNueva, string confirmarClave);

        Task<CambioClaveResponse> CambiarClaveAsync(string usuario, string claveAnt, string claveNueva, string confirmarClave);

        ResetearClaveResponse ResetearClave(string usuario);

        Task<ResetearClaveResponse> ResetearClaveAsync(string usuario);
    }
}
