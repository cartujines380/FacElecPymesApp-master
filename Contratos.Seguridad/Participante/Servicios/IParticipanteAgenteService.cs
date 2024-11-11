using System;
using System.Threading.Tasks;

using Sipecom.FactElec.Pymes.Contratos.Seguridad.Participante.Mensajes;

namespace Sipecom.FactElec.Pymes.Contratos.Seguridad.Participante.Servicios
{
    public interface IParticipanteAgenteService : IDisposable
    {
        ObtenerParticipanteResponse ObtenerParticipante(ObtenerParticipanteRequest request);

        Task<ObtenerParticipanteResponse> ObtenerParticipanteAsync(ObtenerParticipanteRequest request);
    }
}
