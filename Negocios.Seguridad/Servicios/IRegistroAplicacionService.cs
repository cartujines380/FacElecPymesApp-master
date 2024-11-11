using System;
using System.Threading.Tasks;

using Sipecom.FactElec.Pymes.Negocios.Seguridad.Mensajes;

namespace Sipecom.FactElec.Pymes.Negocios.Seguridad.Servicios
{
    public interface IRegistroAplicacionService : IDisposable
    {
        AutenticarAplicacionResponse AutenticarAplicacion();

        Task<AutenticarAplicacionResponse> AutenticarAplicacionAsync();
    }
}
