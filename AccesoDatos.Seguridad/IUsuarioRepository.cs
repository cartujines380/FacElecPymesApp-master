using System;

using Sipecom.FactElec.Pymes.AccesoDatos.Base;
using Sipecom.FactElec.Pymes.Entidades.Seguridad;

namespace Sipecom.FactElec.Pymes.AccesoDatos.Seguridad
{
    public interface IUsuarioRepository : IRepository<Usuario>
    {
        void ResetearClave(string usuarioId, string clave);

        void CambiarClave(string usuarioId, string claveAnt, string claveNueva);
    }
}
