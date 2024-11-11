using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sipecom.FactElec.Pymes.Presentacion.Movil.Base.Models
{
    public class Response
    {
        private List<string> m_validaciones;

        public Response()
        {
            m_validaciones = new List<string>();
        }

        public IEnumerable<string> Validaciones
        {
            get
            {
                return m_validaciones;
            }
        }

        public bool Exito
        {
            get
            {
                return (!m_validaciones.Any());
            }
        }

        public void AgregarValidacion(string validacion)
        {
            m_validaciones.Add(validacion);
        }
    }
}
