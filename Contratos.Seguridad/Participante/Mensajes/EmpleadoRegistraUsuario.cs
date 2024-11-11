using System;
using System.Collections.Generic;
using System.Text;

namespace Sipecom.FactElec.Pymes.Contratos.Seguridad.Participante.Mensajes
{
    public class EmpleadoRegistraUsuario
    {
        public string Estado;

        public DateTime FechaIngSeguro;

        public DateTime FechaNotEgreso;

        public bool HorasExtras;

        public int CargoId;

        public int EmpresaId;

        public int EmpresaPerteneceId;

        public int MonedaId;

        public int OficinaId;

        public int OrganigramaId;

        public int ParticipanteId;

        public int TipoEmpleadoId;

        public string LibretaSeguro;

        public string Operacion;

        public decimal Sueldo;
    }
}
