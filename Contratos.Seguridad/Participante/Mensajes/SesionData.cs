using System;

namespace Sipecom.FactElec.Pymes.Contratos.Seguridad.Participante.Mensajes
{
    public class  SesionData
    {
        //public string Formulario { get; set; }

        public int AplicacionId { get; set; }

        public int EmpresaId { get; set; }

        //public int OrganizacionId { get; set; }

        public int SucursalId { get; set; }

        public string UsuarioId { get; set; }

        public char Login { get; set; }

        public string Maquina { get; set; }

        public string SitioToken { get; set; }

        public string SitioUsuario { get; set; }

        public string SitioMaquina { get; set; }

        //public string Token { get; set; }
    }
}
