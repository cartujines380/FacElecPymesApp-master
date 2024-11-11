using System;
using System.Threading.Tasks;

using Sipecom.FactElec.Pymes.Negocios.Seguridad.Mensajes;

namespace Sipecom.FactElec.Pymes.Negocios.Seguridad.Servicios
{
    public interface IAutenticacionService : IDisposable
    {
        void IniciarSesion(UsuarioData usuario, bool persistente);

        void IniciarSesion(string esquemaAutenticacion, UsuarioData usuario, bool persistente);

        Task IniciarSesionAsync(UsuarioData usuario, bool persistente);

        Task IniciarSesionAsync(string esquemaAutenticacion, UsuarioData usuario, bool persistente);

        void CerrarSesion();

        void CerrarSesion(string esquemaAutenticacion);

        Task CerrarSesionAsync();

        Task CerrarSesionAsync(string esquemaAutenticacion);

        UsuarioData ObtenerUsuarioDataAutenticado();

        Task<UsuarioData> ObtenerUsuarioDataAutenticadoAsync();

        void EstablecerUsuarioDataAutenticado(UsuarioData usuario);
    }
}
