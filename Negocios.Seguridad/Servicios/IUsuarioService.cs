using System;
using System.Threading.Tasks;

using Sipecom.FactElec.Pymes.Entidades.Seguridad;

namespace Sipecom.FactElec.Pymes.Negocios.Seguridad.Servicios
{
    public interface IUsuarioService : IDisposable
    {
        Usuario ObtenerUsuario(int usuarioId);

        Task<Usuario> ObtenerUsuarioAsync(int usuarioId);
    }
}
