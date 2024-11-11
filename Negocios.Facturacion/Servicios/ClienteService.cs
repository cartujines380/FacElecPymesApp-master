using Sipecom.FactElec.Pymes.AccesoDatos.Facturacion;
using Sipecom.FactElec.Pymes.AccesoDatos.Facturacion.Model;
using Sipecom.FactElec.Pymes.Entidades.Facturacion;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sipecom.FactElec.Pymes.Negocios.Facturacion.Servicios
{
    public class ClienteService : IClienteService
    {
        #region Campos

        private readonly IClienteRepository m_clienteRepository;

        private bool m_desechado = false;

        #endregion

        #region Constructores 

        public ClienteService(
            IClienteRepository clienteRepository
        )
        {
            m_clienteRepository = clienteRepository ?? throw new ArgumentNullException(nameof(clienteRepository));
        }

        #endregion

        #region IClienteService

        public IEnumerable<Cliente> ObtenerClientesPorEmpresa(ObtenerClientesPorEmpresaRequest request)
        {
            var result = m_clienteRepository.ObtenerClientesPorEmpresa(request);

            return result;
        }

        public bool EsTransportista(string idUsuario)
        {
            var result = m_clienteRepository.EsTransportista(idUsuario);

            return result;
        }

        public Cliente Add(Cliente entity)
        {
            var result = m_clienteRepository.Add(entity);

            return result;
        }

        public string ObtenerDireccionCliente(InfoComprobanteModel info)
        {
            var result = m_clienteRepository.ObtenerDireccionCliente(info);

            return result;
        }

        #endregion

        #region IDispose

        protected virtual void Dispose(bool desechando)
        {
            if (m_desechado)
            {
                return;
            }

            if (desechando)
            {
                m_clienteRepository.Dispose();
            }

            m_desechado = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~ClienteService()
        {
            Dispose(false);
        }

        #endregion


    }
}
