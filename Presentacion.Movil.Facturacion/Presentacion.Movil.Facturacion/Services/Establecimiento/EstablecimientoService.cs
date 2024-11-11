using Sipecom.FactElec.Pymes.Presentacion.Movil.Base.Exceptions;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Base.Helpers;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Base.Services.RequestProvider;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Models;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Models.Comprobante;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Services.Setttings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Services.Establecimiento
{
    public class EstablecimientoService : IEstablecimientoService
    {
        #region Campos

        private readonly IRequestProvider m_requestProvider;
        private readonly ISettingsService m_settingsService;
        private const string URL_BASE = "api/establecimiento";

        #endregion

        #region Constructor

        public EstablecimientoService(
           IRequestProvider requestProvider,
           ISettingsService settingsService
       )
        {
            m_requestProvider = requestProvider ?? throw new ArgumentNullException(nameof(requestProvider));
            m_settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));
        }

        #endregion

        #region IEstablecimientoService

        public async Task<ObservableCollection<EstablecimientoModel>> ObtenerEstablecimientoCmb()
        {
            var retorno = new ObservableCollection<EstablecimientoModel>();

            try
            {
                var uri = UriHelper.CombineUri(m_settingsService.FacturacionEndpointBase, URL_BASE, "obtenerestablecimientocmb");

                var establecimiento = await m_requestProvider.GetAsync<IEnumerable<EstablecimientoModel>>(uri, m_settingsService.AuthAccessToken);

                retorno = new ObservableCollection<EstablecimientoModel>(establecimiento);

            }
            catch (HttpRequestExceptionEx hre)
            {
                Console.WriteLine(string.Format("Error http al obtener formas de pago. Mensaje: [{0}]", hre.Message));
            }
            catch (ServiceAuthenticationException sae)
            {
                Console.WriteLine(string.Format("Error auth al obtener formas de pago. Mensaje: [{0}]", sae.Message));
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Error general al obtener formas de pago. Mensaje: [{0}]", ex.Message));
            }

            return retorno;
        }

        public async Task<EstablecimientoModel> ObtenerEstablecimiento()
        {
            var retorno = new EstablecimientoModel();

            try
            {
                var uri = UriHelper.CombineUri(m_settingsService.FacturacionEndpointBase, URL_BASE, "obtenerestablecimiento");

                var establecimiento = await m_requestProvider.GetAsync<IEnumerable<EstablecimientoModel>>(uri, m_settingsService.AuthAccessToken);

                retorno = new ObservableCollection<EstablecimientoModel>(establecimiento).FirstOrDefault();

            }
            catch (HttpRequestExceptionEx hre)
            {
                Console.WriteLine(string.Format("Error http al obtener formas de pago. Mensaje: [{0}]", hre.Message));
            }
            catch (ServiceAuthenticationException sae)
            {
                Console.WriteLine(string.Format("Error auth al obtener formas de pago. Mensaje: [{0}]", sae.Message));
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Error general al obtener formas de pago. Mensaje: [{0}]", ex.Message));
            }

            return retorno;
        }

        public async Task<EstablecimientoModel> ObtenerPlan(string empresaRuc)
        {
            var retorno = new EstablecimientoModel();

            try
            {
                var uri = UriHelper.CombineUri(m_settingsService.FacturacionEndpointBase, URL_BASE, "obtenerplan", empresaRuc);

                retorno = await m_requestProvider.GetAsync<EstablecimientoModel>(uri, m_settingsService.AuthAccessToken);

            }
            catch (HttpRequestExceptionEx hre)
            {
                Console.WriteLine(string.Format("Error http al obtener formas de pago. Mensaje: [{0}]", hre.Message));
            }
            catch (ServiceAuthenticationException sae)
            {
                Console.WriteLine(string.Format("Error auth al obtener formas de pago. Mensaje: [{0}]", sae.Message));
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Error general al obtener formas de pago. Mensaje: [{0}]", ex.Message));
            }

            return retorno;
        }

        public async Task<ObservableCollection<EstablecimientoModel>> ObtenerDataEstablecimientosTransportitasPorUsuario()
        {
            var retorno = new ObservableCollection<EstablecimientoModel>();

            try
            {
                var uri = UriHelper.CombineUri(m_settingsService.FacturacionEndpointBase, URL_BASE, "obtenerestablecimientotransportista");

                var establecimiento = await m_requestProvider.GetAsync<IEnumerable<EstablecimientoModel>>(uri, m_settingsService.AuthAccessToken);

                retorno = new ObservableCollection<EstablecimientoModel>(establecimiento);

            }
            catch (HttpRequestExceptionEx hre)
            {
                Console.WriteLine(string.Format("Error http al obtener formas de pago. Mensaje: [{0}]", hre.Message));
            }
            catch (ServiceAuthenticationException sae)
            {
                Console.WriteLine(string.Format("Error auth al obtener formas de pago. Mensaje: [{0}]", sae.Message));
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Error general al obtener formas de pago. Mensaje: [{0}]", ex.Message));
            }

            return retorno;
        }

        public async Task<string> ObtenerDireccionSucursal(InfoComprobanteModel info)
        {
            var retorno = string.Empty;

            try
            {
                var uri = UriHelper.CombineUri(m_settingsService.FacturacionEndpointBase, URL_BASE, "obtenerdireccionsucursal");

                retorno = await m_requestProvider.PostAsync<InfoComprobanteModel, string>(uri, info, m_settingsService.AuthAccessToken);
            }
            catch (HttpRequestExceptionEx hre)
            {
                Console.WriteLine(string.Format("Error http al obtener formas de pago. Mensaje: [{0}]", hre.Message));
            }
            catch (ServiceAuthenticationException sae)
            {
                Console.WriteLine(string.Format("Error auth al obtener formas de pago. Mensaje: [{0}]", sae.Message));
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Error general al obtener formas de pago. Mensaje: [{0}]", ex.Message));
            }

            return retorno;
        }

        #endregion
    }
}
