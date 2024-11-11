using System;
using Sipecom.FactElec.Pymes.Contratos.Seguridad.Comun.Mensajes;

namespace Sipecom.FactElec.Pymes.Contratos.Seguridad.Administracion.Mensajes
{
    public class ConsRolTransaccionRequest
    {
        public int idRol { get; set; }
        public Sesion Sesion { get; set; }
        public string sucursal0 { get; set; }
    }
}
