using System;
using System.Threading.Tasks;

using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.ViewModels.Base;

namespace Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Services.Navigation
{
    public interface INavigationService
    {
        ViewModelBase PreviousPageViewModel { get; }

        Task InitializeAsync();

        Task NavigateToAsync<TViewModel>() where TViewModel : ViewModelBase;

        Task NavigateToAsync<TViewModel>(object parameter) where TViewModel : ViewModelBase;

        Task NavigateToModalAsync<TViewModel>() where TViewModel : ViewModelBase;

        Task NavigateToModalAsync<TViewModel>(object parameter) where TViewModel : ViewModelBase;

        Task NavigateBackModalAsync();

        Task NavigateBackModalAsync(object parameter);

        Task NavigateBackAsync();

        Task RemoveLastFromBackStackAsync();

        Task RemoveBackStackAsync();
    }
}
