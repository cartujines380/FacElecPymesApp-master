using System;
using System.Collections.Generic;
using System.Text;

using Sipecom.FactElec.Pymes.Contratos.Seguridad.Comun.Mensajes;

namespace Sipecom.FactElec.Pymes.Contratos.Seguridad.Participante.Mensajes
{
    public class RegistraUsuarioRequest
    {
        public DatoRegistraUsuario Datos;

        public DatosClienteRegistraUsuario DatosCliente;

        public Sesion Sesion;
    }
}
