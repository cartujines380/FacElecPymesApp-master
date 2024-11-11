using WebKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Controls;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.iOS;

[assembly: ExportRenderer(typeof(CustomWebView), typeof(IOSWebViewRenderer))]
namespace Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.iOS
{
    public class IOSWebViewRenderer : WkWebViewRenderer
    {
        public IOSWebViewRenderer() : this(new WKWebViewConfiguration())
        {
        }

        public IOSWebViewRenderer(WKWebViewConfiguration config) : base(config)
        {
        }

        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            //Aqui deberia dejarlo pasar
        }
    }
}
