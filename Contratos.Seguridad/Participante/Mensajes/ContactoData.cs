using System;

namespace Sipecom.FactElec.Pymes.Contratos.Seguridad.Participante.Mensajes
{
    public class ContactoData
    {
        public string Identificador { get; set; }

        public string Correo { get; set; }

        public string Direccion { get; set; }

        public string PaisId { get; set; }

        public string ProvinciaId { get; set; }

        public string CiudadId { get; set; }

        public string Telefono { get; set; }

        public string TelefonoAlternativo { get; set; }

        public string Comentario { get; set; }
    }
}
