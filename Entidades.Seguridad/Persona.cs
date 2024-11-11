using System;
using System.Collections.Generic;

namespace Sipecom.FactElec.Pymes.Entidades.Seguridad
{
    public class Persona : Usuario
    {
        #region Constantes

        public const string TIPO_USUARIO = "P";

        #endregion

        #region Propiedades

        public string PrimerApellido { get; set; }

        public string SegundoApellido { get; set; }

        public string PrimerNombre { get; set; }

        public string SegundoNombre { get; set; }

        //public string TituloCodigo { get; set; }

        public string GeneroCodigo { get; set; }

        public DateTime? FechaNacimiento { get; set; }

        public string EstadoCivilCodigo { get; set; }

        #endregion

        #region Usuario overrides

        public override string Tipo
        {
            get
            {
                return TIPO_USUARIO;
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException(nameof(value));
                }

                if (value != TIPO_USUARIO)
                {
                    throw new InvalidOperationException("Tipo usuario invalido");
                }

                //No se establece nada porque siempre tiene que ser el mismo tipo
            }
        }

        public override string NombreCompleto
        {
            get
            {
                var nombres = new List<string>();

                if (!string.IsNullOrEmpty(PrimerNombre))
                {
                    nombres.Add(PrimerNombre.Trim());
                }

                if (!string.IsNullOrEmpty(SegundoNombre))
                {
                    nombres.Add(SegundoNombre.Trim());
                }

                if (!string.IsNullOrEmpty(PrimerApellido))
                {
                    nombres.Add(PrimerApellido.Trim());
                }

                if (!string.IsNullOrEmpty(SegundoApellido))
                {
                    nombres.Add(SegundoApellido.Trim());
                }

                return string.Join(' ', nombres.ToArray());
            }
        }

        #endregion
    }
}
