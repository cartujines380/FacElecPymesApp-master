using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Services.Cliente;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuView : FlyoutPage
    {
        public MenuView()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            var menuDetatilPage = ((NavigationPage)Detail).RootPage;
            var viewModel = menuDetatilPage.BindingContext as MenuDetailViewModel;

            await viewModel.InitializeAsync(null);
        }
    }
}