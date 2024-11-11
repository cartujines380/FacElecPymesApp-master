using System;

namespace Sipecom.FactElec.Pymes.Contratos.Seguridad.Seguridad.Mensajes
{
    public class RegistraUsuarioRequest
    {
        public string UsuarioId { get; set; }

        public string Clave { get; set; }

        //public string Token { get; set; }

        public string Maquina { get; set; }

        public string SitioUsuario { get; set; }

        public string SitioToken { get; set; }

        public string SitioMaquina { get; set; }

        public int EmpresaId { get; set; }

        public int SucursalId { get; set; }

        public int AplicacionId { get; set; }

        //public string Formulario { get; set; }

        //public string Login { get; set; }

        public string Dominio { get; set; }

        public char Perfil { get; set; }
    }
}
