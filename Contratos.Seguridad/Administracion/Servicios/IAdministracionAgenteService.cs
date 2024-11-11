using System;
using System.Threading.Tasks;
using Sipecom.FactElec.Pymes.Contratos.Seguridad.Administracion.Mensajes;

namespace Sipecom.FactElec.Pymes.Contratos.Seguridad.Administracion.Servicios
{
    public interface IAdministracionAgenteService : IDisposable
    {
        ConsultaUsuarioRolResponse ConsultaUsuarioRol(ConsultaUsuarioRolRequest1 request);

        Task<ConsultaUsuarioRolResponse> ConsultaUsuarioRolAsync(ConsultaUsuarioRolRequest1 request);

        ConsRolTransaccionResponse ConsultaRolTransaccion(ConsRolTransaccionRequest1 request);

        Task<ConsRolTransaccionResponse> ConsultaRolTransaccionAsync(ConsRolTransaccionRequest1 request);
    }
}
