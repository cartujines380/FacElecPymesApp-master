using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FacturaExitoView : ContentPage
    {
        public FacturaExitoView()
        {
            InitializeComponent();

            CheckListo.ScaleTo(2, 2000, Easing.BounceOut);
        }
    }
}