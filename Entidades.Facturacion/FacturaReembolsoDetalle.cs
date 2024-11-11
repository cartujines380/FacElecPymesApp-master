using System;
using Sipecom.FactElec.Pymes.Entidades.Base;

namespace Sipecom.FactElec.Pymes.Entidades.Facturacion
{
    public class FacturaReembolsoDetalle : Entity
    {
        public string Detalle { get; set; }

        public Proveedor Proveedor { get; set; }

        public Documento Documento { get; set; }

        public ImpuestoReembolso Impuesto { get; set; }

        public FacturaReembolsoDetalle()
        {
            Proveedor = new Proveedor();
            Documento = new Documento();
            Impuesto = new ImpuestoReembolso();
        }

        public void EstablecerProveedor(Proveedor proveedor)
        {
            if (proveedor == null)
                throw new ArgumentNullException(nameof(proveedor));

            this.Proveedor.TipoIdentificacionCodigo = proveedor.TipoIdentificacionCodigo;
            this.Proveedor.Identificacion = proveedor.Identificacion;
            this.Proveedor.RazonSocial = proveedor.RazonSocial;
            this.Proveedor.PaisPagoCodigo = proveedor.PaisPagoCodigo;
            this.Proveedor.Tipo = proveedor.Tipo;
        }

        public void EstablecerDocumento(Documento documento)
        {
            if (documento == null)
                throw new ArgumentNullException(nameof(documento));

            this.Documento.Codigo = documento.Codigo;
            this.Documento.EstablecimientoCodigo = documento.EstablecimientoCodigo;
            this.Documento.PuntoEmisionCodigo = documento.PuntoEmisionCodigo;
            this.Documento.Secuencial = documento.Secuencial;
            this.Documento.FechaEmision = documento.FechaEmision;
            this.Documento.NumeroAutorizacion = documento.NumeroAutorizacion;
        }

        public void EstablecerImpuesto(ImpuestoReembolso impuesto)
        {
            if (impuesto == null)
                throw new ArgumentNullException(nameof(impuesto));

            this.Impuesto.Codigo = impuesto.Codigo;
            this.Impuesto.PorcentajeCodigo = impuesto.PorcentajeCodigo;
            this.Impuesto.PorcentajeValor = impuesto.PorcentajeValor;
            this.Impuesto.BaseImponible = impuesto.BaseImponible;
        }

        public ResultadoValidacion Validar()
        {
            var resultado = new ResultadoValidacion();

            var validacionProveedor = Proveedor.Validar();

            if (!validacionProveedor.EsValido)
            {
                foreach (var errorProveedor in validacionProveedor.Errores)
                    resultado.AnadirError(errorProveedor);
            }
            // Agrego Validacion de Identificacion

            var validacionDocumento = Documento.Validar();

            if (!validacionDocumento.EsValido)
            {
                foreach (var errorDocumento in validacionDocumento.Errores)
                    resultado.AnadirError(errorDocumento);
            }

            var validacionImpuesto = Impuesto.Validar();

            if (!validacionImpuesto.EsValido)
            {
                foreach (var errorImpuesto in validacionImpuesto.Errores)
                    resultado.AnadirError(errorImpuesto);
            }

            return resultado;
        }
    }

}
