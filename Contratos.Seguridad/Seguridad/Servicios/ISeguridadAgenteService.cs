using System;
using System.Threading.Tasks;

using Sipecom.FactElec.Pymes.Contratos.Seguridad.Seguridad.Mensajes;

namespace Sipecom.FactElec.Pymes.Contratos.Seguridad.Seguridad.Servicios
{
    public interface ISeguridadAgenteService : IDisposable
    {
        RegistrarAplicacionResponse RegistrarAplicacion(RegistrarAplicacionRequest request);

        Task<RegistrarAplicacionResponse> RegistrarAplicacionAsync(RegistrarAplicacionRequest request);

        RegistraUsuarioResponse RegistrarUsuario(RegistraUsuarioRequest request);

        Task<RegistraUsuarioResponse> RegistrarUsuarioAsync(RegistraUsuarioRequest request);

        CambioClaveResponse CambiarClave(CambioClaveRequest request);

        Task<CambioClaveResponse> CambiarClaveAsync(CambioClaveRequest request);

        EncriptarResponse Encriptar(EncriptarRequest request);

        Task<EncriptarResponse> EncriptarAsync(EncriptarRequest request);
    }
}
