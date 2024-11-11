using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Models.Articulo;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Models
{
    public class RequestModel
    {
        public string RucEmpresa { get; set; }

        public ObservableCollection<ArticuloModel> Articulo { get; set; }

        public ObservableCollection<NumeroFacturaModel> ListCodEstablecimiento { get; set; }
    }
}
