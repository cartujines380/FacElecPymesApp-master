using Plugin.Connectivity;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class IniciarSesionView : ContentPage
    {

        public IniciarSesionView()
        {
            InitializeComponent();

            //sinInternetView = new SinInternetView();
            //Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
        }


        protected override void OnAppearing()
        {
            base.OnAppearing();

            // Verificar la conectividad al mostrar la página
            if (!CrossConnectivity.Current.IsConnected)
            {
                ShowNoInternetView();
            }

            CrossConnectivity.Current.ConnectivityChanged += ConnectivityChanged;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            CrossConnectivity.Current.ConnectivityChanged -= ConnectivityChanged;
        }

        private void ConnectivityChanged(object sender, Plugin.Connectivity.Abstractions.ConnectivityChangedEventArgs e)
        {
            if (!e.IsConnected)
            {
                ShowNoInternetView();
            }
            //else
            //{
            //    HideNoInternetView();
            //}
        }

        private void ShowNoInternetView()
        {
            //SinConexion.IsVisible = true;
            Navigation.PushModalAsync(new SinInternetView());
        }

        private void HideNoInternetView()
        {
            //SinConexion.IsVisible = false;
            //IniciarSesionViewModel viewModel = new IniciarSesionViewModel(null, null, null);
            //BindingContext = viewModel;
            //viewModel.IniciarSesionCommand();
            if (BindingContext is IniciarSesionViewModel viewModel)
            {
                //if (viewModel.IniciarSesionCommand.CanExecute(null))
                //{
                    viewModel.IniciarSesionCommand.Execute(null);
                //}
            }
        }

        //protected override void OnAppearing()
        //{
        //    base.OnAppearing();

        //    // Verificar la conectividad al mostrar la página
        //    if (!CrossConnectivity.Current.IsConnected)
        //    {
        //        ShowNoInternetView();
        //    }

        //    CrossConnectivity.Current.ConnectivityChanged += ConnectivityChanged;
        //}

        //protected override void OnDisappearing()
        //{
        //    base.OnDisappearing();

        //    CrossConnectivity.Current.ConnectivityChanged -= ConnectivityChanged;
        //}

        //private void ConnectivityChanged(object sender, Plugin.Connectivity.Abstractions.ConnectivityChangedEventArgs e)
        //{
        //    if (!e.IsConnected)
        //    {
        //        ShowNoInternetView();
        //    }
        //    else
        //    {
        //        HideNoInternetView();
        //    }
        //}

        //private void ShowNoInternetView()
        //{
        //    if (!MainLayout.Children.Contains(sinInternetView))
        //    {
        //        ContentLayout.Children.Add(sinInternetView);
        //    }
        //}

        //private void HideNoInternetView()
        //{
        //    if (ContentLayout.Children.Contains(sinInternetView))
        //    {
        //        ContentLayout.Children.Remove(sinInternetView);
        //    }
        //}

        //protected override void OnAppearing()
        //{
        //    base.OnAppearing();

        //    // Verificar la conectividad al mostrar la página
        //    if (!CrossConnectivity.Current.IsConnected)
        //    {
        //        ShowNoInternetAlert();
        //    }

        //    CrossConnectivity.Current.ConnectivityChanged += ConnectivityChanged;
        //}

        //protected override void OnDisappearing()
        //{
        //    base.OnDisappearing();

        //    CrossConnectivity.Current.ConnectivityChanged -= ConnectivityChanged;
        //}

        //private void ConnectivityChanged(object sender, Plugin.Connectivity.Abstractions.ConnectivityChangedEventArgs e)
        //{
        //    if (!e.IsConnected)
        //    {
        //        ShowNoInternetAlert();
        //    }
        //}

        //private async void ShowNoInternetAlert()
        //{
        //    await DisplayAlert("Error", "No hay conexión a Internet", "Aceptar");
        //}
        //private void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        //{
        //    var currentNetworkAccess = e.NetworkAccess;

        //    if (currentNetworkAccess != NetworkAccess.Internet)
        //    {
        //        // No hay conexión a Internet
        //        // Mostrar pantalla personalizada o mensaje de error
        //        // o realizar cualquier otra acción necesaria

        //        // Por ejemplo, puedes mostrar una página o mostrar un mensaje utilizando DisplayAlert
        //        Device.BeginInvokeOnMainThread(async () =>
        //        {
        //            await DisplayAlert("Error", "No hay conexión a Internet", "Aceptar");
        //        });
        //    }
        //}

        //protected override void OnDisappearing()
        //{
        //    base.OnDisappearing();

        //    Connectivity.ConnectivityChanged -= Connectivity_ConnectivityChanged;
        //}



    }
}