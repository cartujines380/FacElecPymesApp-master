using System;
using System.Collections.Generic;

using Sipecom.FactElec.Pymes.AccesoDatos.Base;
using Sipecom.FactElec.Pymes.Entidades.Facturacion;

namespace Sipecom.FactElec.Pymes.AccesoDatos.Facturacion
{
    public interface IEstablecimientoRepository : IRepository<Establecimiento, string>
    {
        IEnumerable<Establecimiento> ObtenerEstablecimientosPorUsuario(string usuarioId);

        IEnumerable<Establecimiento> ObtenerEstablecimientosPorUsuarioCmb(string usuarioId);

        IEnumerable<EstablecimientoData> ObtenerDataEstablecimientosPorUsuario(string usuarioId);

        IEnumerable<EstablecimientoData> ObtenerDataEstablecimientosTransportitasPorUsuario(string usuarioId);

        EstablecimientoData ObtenerDataEstablecimientoPorUsuarioEmpresa(string usuarioId, string empresaRuc);

        EstablecimientoData ObtenerDataEstablecimientoTransportistaPorUsuarioEmpresa(string usuarioId, string empresaRuc);

        IEnumerable<RegimenMicroempresaData> ObtenerRegimenMicroempresaData(string empresaRuc, DateTime fechaEmision);

        IEnumerable<RimpeData> ObtenerRimpeData(string empresaRuc);

        EstablecimientoData Obtenerplan(string empresaRuc);

        string ObtenerDireccionSucursal(InfoComprobanteModel info);
    }
}
