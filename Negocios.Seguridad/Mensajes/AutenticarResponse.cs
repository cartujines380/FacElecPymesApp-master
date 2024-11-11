using System;

using Sipecom.FactElec.Pymes.Negocios.Base.Mensajes;

namespace Sipecom.FactElec.Pymes.Negocios.Seguridad.Mensajes
{
    public class AutenticarResponse : Response
    {
        public UsuarioData Usuario { get; set; }
    }
}
