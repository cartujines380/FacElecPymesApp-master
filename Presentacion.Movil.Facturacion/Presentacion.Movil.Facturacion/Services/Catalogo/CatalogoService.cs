using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Base.Exceptions;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Base.Helpers;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Base.Services.RequestProvider;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Models.Catalogo;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Services.Setttings;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Models;

namespace Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Services.Catalogo
{
    public class CatalogoService : ICatalogoService
    {
        #region Campos

        private readonly IRequestProvider m_requestProvider;
        private readonly ISettingsService m_settingsService;
        private const string URL_BASE = "api/catalogo";

        #endregion

        #region Constructor

        public CatalogoService(
            IRequestProvider requestProvider,
            ISettingsService settingsService
        )
        {
            m_requestProvider = requestProvider ?? throw new ArgumentNullException(nameof(requestProvider));
            m_settingsService = settingsService ?? throw new ArgumentNullException(nameof(settingsService));
        }

        #endregion

        #region ICatalogoService

        public async Task<ObservableCollection<CatalogoModel>> ObtenerCatalogo(string nombreTabla)
        {
            var retorno = new ObservableCollection<CatalogoModel>();

            try
            { 
                var uri = UriHelper.CombineUri(m_settingsService.FacturacionEndpointBase, URL_BASE, "obtener", nombreTabla);

                var catalogos = await m_requestProvider.GetAsync<IEnumerable<CatalogoModel>>(uri, m_settingsService.AuthAccessToken);

                retorno = new ObservableCollection<CatalogoModel>(catalogos);

            }
            catch(HttpRequestExceptionEx hre)
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

        public async Task<ObservableCollection<CatalogoModel>> ObtenerCatalogoGeneral(int codigo)
        {
            var retorno = new ObservableCollection<CatalogoModel>();

            try
            {
                var uri = UriHelper.CombineUri(m_settingsService.FacturacionEndpointBase, URL_BASE, "obtenergeneral", codigo.ToString());

                var catalogos = await m_requestProvider.GetAsync<IEnumerable<CatalogoModel>>(uri, m_settingsService.AuthAccessToken);

                retorno = new ObservableCollection<CatalogoModel>(catalogos);

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

        public async Task<ObservableCollection<CatalogoModel>> ObtenerFormaPago()
        {
            var retorno = new ObservableCollection<CatalogoModel>();

            try
            {
                var uri = UriHelper.CombineUri(m_settingsService.FacturacionEndpointBase, URL_BASE, "obtenerformapago");

                var formaPago = await m_requestProvider.GetAsync<IEnumerable<CatalogoModel>>(uri, m_settingsService.AuthAccessToken);

                retorno = new ObservableCollection<CatalogoModel>(formaPago);

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

        public async Task<ObservableCollection<ImpuestoModel>> ObtenerImpuestoPorCodigo(string tipo)
        {
            var retorno = new ObservableCollection<ImpuestoModel>();

            try
            {
                var uri = UriHelper.CombineUri(m_settingsService.FacturacionEndpointBase, URL_BASE, "obtenerimpuestoportipo", tipo);

                var impuestos = await m_requestProvider.GetAsync<IEnumerable<ImpuestoModel>>(uri, m_settingsService.AuthAccessToken);

                retorno = new ObservableCollection<ImpuestoModel>(impuestos);

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

        public async Task<ObservableCollection<CatalogoModel>> ObtenerCatalogoPais(int codigo)
        {
            var retorno = new ObservableCollection<CatalogoModel>();

            try
            {
                var uri = UriHelper.CombineUri(m_settingsService.FacturacionEndpointBase, URL_BASE, "obtenerpais", codigo.ToString());

                var pais = await m_requestProvider.GetAsync<IEnumerable<CatalogoModel>>(uri, m_settingsService.AuthAccessToken);

                retorno = new ObservableCollection<CatalogoModel>(pais);

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

        public async Task<ObservableCollection<CatalogoModel>> ObtenerCatalogoProvinciaCiudad(int codigo, string descAlterno)
        {
            var retorno = new ObservableCollection<CatalogoModel>();

            try
            {
                var uri = UriHelper.CombineUri(m_settingsService.FacturacionEndpointBase, URL_BASE, "obtenerprovinciaciudad", codigo.ToString(), descAlterno);

                var info = await m_requestProvider.GetAsync<IEnumerable<CatalogoModel>>(uri, m_settingsService.AuthAccessToken);

                retorno = new ObservableCollection<CatalogoModel>(info);

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
