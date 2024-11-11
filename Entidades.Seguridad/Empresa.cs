using System;

namespace Sipecom.FactElec.Pymes.Entidades.Seguridad
{
    public class Empresa : Usuario
    {
        #region Constantes

        public const string TIPO_USUARIO = "E";

        #endregion

        #region Propiedades

        public string NombreEmpresa { get; set; }

        //public int CategoriaId { get; set; }

        //public int Nivel { get; set; }

        //public int EmpresaPadreId { get; set; }

        //public string Licencia { get; set; }

        //public string Marca { get; set; }

        //public string NumeroPatronal { get; set; }

        //public string ZonaId { get; set; }

        //public string RazonSocialId { get; set; }

        //public DateTime? FechaConstitucion { get; set; }

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
                return NombreEmpresa;
            }
        }

        #endregion
    }
}
