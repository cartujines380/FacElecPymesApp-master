using System;

namespace Sipecom.FactElec.Pymes.Negocios.Seguridad.Configuraciones
{
    public interface IConfiguracionServicioSeguridad
    {
        void Initialize();

        string Maquina { get; set; }

        int EmpresaId { get; set; }

        int SucursalId { get; set; }

        int AplicacionId { get; set; }

        string Dominio { get; set; }

        char Perfil { get; set; }

        string Token { get; set; }

        string TokenSitio { get; set; }

        string UsuarioSitio { get; set; }

        string Formulario { get; set; }

        char Login { get; set; }

        string ServidorBaseDatosId { get; set; }

        string CorreoRemitente { get; set; }

        string CorreoPlantillaRuta { get; set; }
    }
}
