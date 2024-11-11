using System;
using System.Threading.Tasks;

using Sipecom.FactElec.Pymes.Contratos.Seguridad.Cliente.Mensajes;

namespace Sipecom.FactElec.Pymes.Contratos.Seguridad.Cliente.Servicios
{
    public interface IClienteAgenteService : IDisposable
    {
        ResetearClaveResponse ResetearClave(ResetearClaveRequest request);

        Task<ResetearClaveResponse> ResetearClaveAsync(ResetearClaveRequest request);
    }
}
