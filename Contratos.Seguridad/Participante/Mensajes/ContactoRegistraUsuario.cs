using System;
using System.Collections.Generic;
using System.Text;

namespace Sipecom.FactElec.Pymes.Contratos.Seguridad.Participante.Mensajes
{
    public class ContactoRegistraUsuario
    {
        public int DireccionId;

        public int MedioContactoId;

        public int ParticipanteId;

        public int TipoMedioContactoId;

        public string Operacion;

        public string Valor;

        public string ValorAlt;
    }
}
