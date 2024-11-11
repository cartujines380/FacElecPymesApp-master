using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Models.Comprobante;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Services.Comprobante
{
    public interface IComprobanteService
    {
        Task<ObservableCollection<ComprobanteModel>> ConsultarComprobante(FiltroModel cliente);

        Task<ComprobanteModel> ConsultarComprobanteXML(InfoComprobanteModel cliente);
    }
}
