using System;

namespace Sipecom.FactElec.Pymes.Entidades.Seguridad
{
    public static class UsuarioFactory
    {
        #region Metodos privados

        private static Persona CrearPersona()
        {
            var retorno = new Persona();

            retorno.Activar();

            return retorno;
        }

        private static Empresa CrearEmpresa()
        {
            var retorno = new Empresa();

            retorno.Activar();

            return retorno;
        }

        #endregion

        #region Metodos publicos

        public static Usuario CrearUsuario(string tipo)
        {
            if (string.IsNullOrEmpty(tipo))
            {
                throw new ArgumentNullException(nameof(tipo));
            }

            if (tipo == Persona.TIPO_USUARIO)
            {
                return CrearPersona();
            }

            if (tipo == Empresa.TIPO_USUARIO)
            {
                return CrearEmpresa();
            }

            return null;
        }

        #endregion
    }
}
