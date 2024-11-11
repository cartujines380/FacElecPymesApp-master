using Sipecom.FactElec.Pymes.Entidades.Facturacion;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sipecom.FactElec.Pymes.Negocios.Facturacion.Servicios
{
    public interface IEstablecimientoService : IDisposable
    {
        IEnumerable<Establecimiento> ObtenerEstablecimientosPorUsuarioCmb(string usuarioId);

        IEnumerable<Establecimiento> ObtenerEstablecimientosPorUsuario(string usuarioId);

        EstablecimientoData Obtenerplan(string empresaRuc);

        IEnumerable<EstablecimientoData> ObtenerDataEstablecimientosTransportitasPorUsuario(string usuarioId);

        string ObtenerDireccionSucursal(InfoComprobanteModel info);
    }
}
