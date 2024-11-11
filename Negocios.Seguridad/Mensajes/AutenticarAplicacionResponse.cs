using System;

using Sipecom.FactElec.Pymes.Negocios.Base.Mensajes;

namespace Sipecom.FactElec.Pymes.Negocios.Seguridad.Mensajes
{
    public class AutenticarAplicacionResponse : Response
    {
        public AplicacionData Aplicacion { get; set; }
    }
}
