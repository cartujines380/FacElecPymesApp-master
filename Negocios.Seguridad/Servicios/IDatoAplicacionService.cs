using System;

using Sipecom.FactElec.Pymes.Negocios.Seguridad.Mensajes;

namespace Sipecom.FactElec.Pymes.Negocios.Seguridad.Servicios
{
    public interface IDatoAplicacionService
    {
        AplicacionData ObtenerAplicacionData();

        void EstablecerAplicacionData(AplicacionData aplicacion);
    }
}
