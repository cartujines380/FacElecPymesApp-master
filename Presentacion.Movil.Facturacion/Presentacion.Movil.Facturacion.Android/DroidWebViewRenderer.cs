using Android.Content;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Controls;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Android;

[assembly: ExportRenderer(typeof(CustomWebView), typeof(DroidWebViewRenderer))]
namespace Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Android
{
    public class DroidWebViewRenderer : WebViewRenderer
    {
        private Context m_context;

        public DroidWebViewRenderer(Context context) : base(context)
        {
            m_context = context;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<WebView> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement == null)
            {
                Control.SetWebViewClient(new DroidWebViewClient(this));
            }
        }
    }
}