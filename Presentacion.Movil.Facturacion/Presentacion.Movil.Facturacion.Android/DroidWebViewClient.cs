using Android.Net.Http;
using Android.Webkit;
using System;

using Xamarin.Forms.Platform.Android;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Infrastructure;

namespace Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Android
{
    public class DroidWebViewClient : FormsWebViewClient
    {
        private DroidWebViewRenderer m_renderer;

        public DroidWebViewClient(DroidWebViewRenderer renderer) : base(renderer)
        {
            m_renderer = renderer;
        }

        public override void OnReceivedSslError(WebView view, SslErrorHandler handler, SslError error)
        {
            if (!Bootstrapper.Instance.ValidateSslCertificate)
            {
                handler.Proceed();
                return;
            }

            base.OnReceivedSslError(view, handler, error);
        }
    }
}