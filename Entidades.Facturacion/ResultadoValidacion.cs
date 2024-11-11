using System;
using System.Collections.Generic;
using System.Linq;

namespace Sipecom.FactElec.Pymes.Entidades.Facturacion
{
    public class ResultadoValidacion
    {
        private List<string> m_errores;

        public ResultadoValidacion()
        {
            m_errores = new List<string>();
        }

        public void AnadirError(string mensajeError)
        {
            m_errores.Add(mensajeError);
        }

        public bool EsValido
        {
            get
            {
                return (!m_errores.Any());
            }
        }

        public IEnumerable<string> Errores
        {
            get
            {
                return m_errores.AsReadOnly();
            }
        }
    }

}
