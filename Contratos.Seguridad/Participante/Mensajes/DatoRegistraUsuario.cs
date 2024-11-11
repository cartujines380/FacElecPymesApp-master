using System;
using System.Collections.Generic;
using System.Text;

namespace Sipecom.FactElec.Pymes.Contratos.Seguridad.Participante.Mensajes
{
    public class DatoRegistraUsuario
    {
        public ContactoRegistraUsuario[] Contactos;

        public DireccionRegistraUsuario[] Direcciones;

        public EmpresaRegistraUsuario Empresa;

        public ParticipanteRegistraUsuario Participante;

        public PersonaRegistraUsuario Persona;

        public RegistroClienteRegistraUsuario RegistroCliente;
    }
}
