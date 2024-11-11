using System;
using System.Collections.Generic;
using System.Text;

namespace Sipecom.FactElec.Pymes.Contratos.Seguridad.Participante.Mensajes
{
    public class DatosClienteRegistraUsuario
    {
        public ClienteRegistraUsuario[] Clientes;

        public EmpleadoRegistraUsuario[] Empleados;

        public int ParticipanteId;

        public ProveedorRegistraUsuario[] Proveedores;

        public string TipoParticipante;
    }
}
