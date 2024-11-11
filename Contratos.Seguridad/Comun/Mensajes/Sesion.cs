using System;

namespace Sipecom.FactElec.Pymes.Contratos.Seguridad.Comun.Mensajes
{
    public class Sesion
    {
        public string Formulario { get; set; }

        public int AplicacionId { get; set; }

        public int EmpresaId { get; set; }

        public int OrganizacionId { get; set; }

        public int SucursalId { get; set; }

        public string UsuarioId { get; set; }

        public char Login { get; set; }

        public string MaqSitio { get; set; }

        public string Maquina { get; set; }

        public string Token { get; set; }

        public string TokenSitio { get; set; }

        public string UsrSitio { get; set; }
    }
}
