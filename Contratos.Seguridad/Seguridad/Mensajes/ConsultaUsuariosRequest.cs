using System;

using Sipecom.FactElec.Pymes.Contratos.Seguridad.Comun.Mensajes;

namespace Sipecom.FactElec.Pymes.Contratos.Seguridad.Seguridad.Mensajes
{
    public class ConsultaUsuariosRequest<TProperty>
    {
        public int PageIndex { get; set; }

        public int PageCount { get; set; }

        public bool Ascending { get; set; }

        public Sesion Sesion { get; set; }
    }
}
