using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Models.Catalogo;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Models.Articulo
{
    public class ArticuloRequest
    {
        public string RucEmpresa { get; set; }

        public ObservableCollection<CatalogoModel> Articulo { get; set; }
    }
}
