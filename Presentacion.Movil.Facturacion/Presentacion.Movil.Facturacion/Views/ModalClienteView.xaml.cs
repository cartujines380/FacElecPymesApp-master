using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ModalClienteView : ContentPage
    {
        public ModalClienteView()
        {
            InitializeComponent();

            Color grayWithOpacity = Color.FromRgba(128, 128, 128, 0.7); // Gris con 50% de opacidad
            BackgroundColor = grayWithOpacity;
        }
    }
}