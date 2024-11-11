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
    public partial class FacturaView : ContentPage
    {
        public FacturaView()
        {
            InitializeComponent();
            //LblTipoFactura.Text = "";
        }

        //private async void MostrarModalTipoFactura(object sender, EventArgs e)
        //{
        //    var tipoFacturaPage = new ModalTipoFacturaView();

        //    await Navigation.PushModalAsync(tipoFacturaPage);
        //}

        //private async void MostrarModalEmpresa(object sender, EventArgs e)
        //{
        //    var empresaPage = new ModalEmpresaView();

        //    await Navigation.PushModalAsync(empresaPage);
        //}

        //private async void MostrarModalCliente(object sender, EventArgs e)
        //{
        //    var empresaPage = new ModalClienteView();

        //    await Navigation.PushModalAsync(empresaPage);
        //}
    }
}