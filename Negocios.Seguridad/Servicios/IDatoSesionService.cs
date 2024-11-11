using System;

using Sipecom.FactElec.Pymes.Contratos.Seguridad.Comun.Mensajes;

namespace Sipecom.FactElec.Pymes.Negocios.Seguridad.Servicios
{
    public interface IDatoSesionService : IDisposable
    {
        Sesion ObtenerSesionAgente();
    }
}
