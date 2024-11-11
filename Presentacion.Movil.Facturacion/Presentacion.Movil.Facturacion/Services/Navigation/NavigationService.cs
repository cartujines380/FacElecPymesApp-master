using System;
using System.Globalization;
using System.Reflection;
using System.Threading.Tasks;
using Xamarin.Forms;

using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.ViewModels;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.ViewModels.Base;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Views;
using Xamarin.Essentials;

namespace Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Services.Navigation
{
    public class NavigationService : INavigationService
    {
        #region Constructores

        public NavigationService()
        {

        }

        #endregion

        #region INavigationService

        public ViewModelBase PreviousPageViewModel
        {
            get
            {
                var mainPage = Application.Current.MainPage as NavigationView;
                var viewModel = mainPage.Navigation.NavigationStack[mainPage.Navigation.NavigationStack.Count - 2].BindingContext;
                return viewModel as ViewModelBase;
            }
        }

        public Task InitializeAsync()
        {
            //if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            //{
            //    return NavigateToAsync<SinInternetViewModel>();
            //}
            //else
            //{
            return NavigateToAsync<IniciarSesionViewModel>();
            //}
        }

        public Task NavigateToAsync<TViewModel>() where TViewModel : ViewModelBase
        {
            return InternalNavigateToAsync(typeof(TViewModel), null);
        }

        public Task NavigateToAsync<TViewModel>(object parameter) where TViewModel : ViewModelBase
        {
            return InternalNavigateToAsync(typeof(TViewModel), parameter);
        }

        public Task NavigateToModalAsync<TViewModel>() where TViewModel : ViewModelBase
        {
            return InternalNavigateToModalAsync(typeof(TViewModel), null);
        }

        public Task NavigateToModalAsync<TViewModel>(object parameter) where TViewModel : ViewModelBase
        {
            return InternalNavigateToModalAsync(typeof(TViewModel), parameter);
        }

        public Task NavigateBackModalAsync()
        {
            return InternalNavigateBackModalAsync(null);
        }

        public Task NavigateBackModalAsync(object parameter)
        {
            return InternalNavigateBackModalAsync(parameter);
        }

        public Task NavigateBackAsync()
        {
            return InternalNavigateBackAsync(null);
        }

        public Task RemoveLastFromBackStackAsync()
        {
            var mainPage = Application.Current.MainPage as NavigationView;

            if (mainPage != null)
            {
                mainPage.Navigation.RemovePage(mainPage.Navigation.NavigationStack[mainPage.Navigation.NavigationStack.Count - 2]);
            }

            return Task.FromResult(true);
        }

        public Task RemoveBackStackAsync()
        {
            var mainPage = Application.Current.MainPage as NavigationView;

            if (mainPage != null)
            {
                for (int i = 0; i < mainPage.Navigation.NavigationStack.Count - 1; i++)
                {
                    var page = mainPage.Navigation.NavigationStack[i];
                    mainPage.Navigation.RemovePage(page);
                }
            }

            return Task.FromResult(true);
        }

        #endregion

        #region Metodos privados

        private async Task InternalNavigateToAsync(Type viewModelType, object parameter)
        {
            try
            {
                Page page = CreatePage(viewModelType, parameter);

                if (page is IniciarSesionView)
                {
                    Application.Current.MainPage = new NavigationView(page);
                }
                else
                {
                    var navigationPage = Application.Current.MainPage as NavigationView;

                    if (navigationPage != null)
                    {
                        await navigationPage.PushAsync(page);
                    }
                    else
                    {
                        Application.Current.MainPage = new NavigationView(page);
                    }
                }

                await (page.BindingContext as ViewModelBase).InitializeAsync(parameter);
            }
            catch (Exception ex)
            {

            }

        }

        private async Task InternalNavigateToModalAsync(Type viewModelType, object parameter)
        {
            try
            {
                Page modalPage = CreatePage(viewModelType, parameter);

                var navigationPage = Application.Current.MainPage as NavigationView;

                if (navigationPage == null)
                {
                    return;
                }

                var page = navigationPage.CurrentPage;

                await page.Navigation.PushModalAsync(modalPage);

                await (modalPage.BindingContext as ViewModelBase).InitializeAsync(parameter);
            }
            catch (Exception ex)
            {

            }
        }

        private async Task InternalNavigateBackModalAsync(object parameter)
        {
            try
            {
                var navigationPage = Application.Current.MainPage as NavigationView;

                if (navigationPage == null)
                {
                    return;
                }

                var page = navigationPage.CurrentPage;

                var modalPage = await page.Navigation.PopModalAsync();

                var nombrePage = modalPage.GetType().Name;

                await (page.BindingContext as ViewModelBase).OnPopModalAsync(parameter, nombrePage);

            }
            catch (Exception ex)
            {

            }
        }

        private async Task InternalNavigateBackAsync(object parameter)
        {
            try
            {
                var navigationPage = Application.Current.MainPage as NavigationView;

                if (navigationPage == null)
                {
                    return;
                }

                var page = navigationPage.CurrentPage;

                var modalPage = await page.Navigation.PopAsync();

                var nombrePage = modalPage.GetType().Name;

                await (page.BindingContext as ViewModelBase).OnPopModalAsync(parameter, nombrePage);
            }
            catch (Exception ex)
            {

            }
        }

        private Type GetPageTypeForViewModel(Type viewModelType)
        {
            var viewName = viewModelType.FullName.Replace("Model", string.Empty);
            var viewModelAssemblyName = viewModelType.GetTypeInfo().Assembly.FullName;
            var viewAssemblyName = string.Format(CultureInfo.InvariantCulture, "{0}, {1}", viewName, viewModelAssemblyName);
            var viewType = Type.GetType(viewAssemblyName);
            return viewType;
        }

        private Page CreatePage(Type viewModelType, object parameter)
        {
            Type pageType = GetPageTypeForViewModel(viewModelType);

            if (pageType == null)
            {
                throw new Exception($"Vista modelo {viewModelType} no existe");
            }

            Page page = Activator.CreateInstance(pageType) as Page;

            return page;
        }

        #endregion   
    }
}
