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
    public partial class ModalEmpresaView : ContentPage
    {
        public ModalEmpresaView()
        {
            InitializeComponent();

            Color grayWithOpacity = Color.FromRgba(128, 128, 128, 0.7); // Gris con 50% de opacidad
            BackgroundColor = grayWithOpacity;
        }
    }
}