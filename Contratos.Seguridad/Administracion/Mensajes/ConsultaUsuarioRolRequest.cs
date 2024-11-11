using System;
using Sipecom.FactElec.Pymes.Contratos.Seguridad.Comun.Mensajes;

namespace Sipecom.FactElec.Pymes.Contratos.Seguridad.Administracion.Mensajes
{
    public class ConsultaUsuarioRolRequest
    {
       public string IdUsuario { get; set; }
       public Sesion Sesion { get; set; }
    }
}
