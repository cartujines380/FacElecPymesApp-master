using Moq;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Services.Comprobante;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.ViewModels;
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
    public partial class ConsultaComprobanteView : ContentPage
    {
        private ConsultaComprobanteViewModel viewModel;

        public ConsultaComprobanteView()
        {
            try
            {
                InitializeComponent();
                Filtros.IsVisible = true;
                Filtros.FadeTo(1, 1000);

                viewModel = (ConsultaComprobanteViewModel)BindingContext;
            }
            catch (Exception ex)
            {

            }
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            bool visible = Filtros.IsVisible;
            if (visible)
            {
                BtnFiltro.Text = "\uf06e;\uf0b0";
                Filtros.IsVisible = false;
                Filtros.FadeTo(0, 1000);
            }
            else
            {
                BtnFiltro.Text = "\uf070;\uf0b0";
                Filtros.IsVisible = true;
                Filtros.FadeTo(1, 1000);
            }

        }

        private async void ListView_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            if (viewModel != null)
            {
                var items = viewModel.InfoComprobante;
                if (items != null && e.Item == items[items.Count - 1])
                {
                    await viewModel.ConsultarComprobanteInterno();
                }
            }
        }
    }
}