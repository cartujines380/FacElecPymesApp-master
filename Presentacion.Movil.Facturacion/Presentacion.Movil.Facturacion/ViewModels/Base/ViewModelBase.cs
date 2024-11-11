using System;
using System.Threading.Tasks;

using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Infrastructure;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Services.Navigation;

namespace Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.ViewModels.Base
{
    public abstract class ViewModelBase : ExtendedBindableObject
    {
        protected readonly INavigationService m_navigationService;

        private bool m_isBusy;
        private bool m_isVisible;
        private bool m_isRunning;

        public bool IsBusy
        {
            get
            {
                return m_isBusy;
            }

            set
            {
                m_isBusy = value;
                RaisePropertyChanged(() => IsBusy);
            }
        }

        public bool IsVisible
        {
            get
            {
                return m_isVisible;
            }
            set
            {
                m_isVisible = value;
                RaisePropertyChanged(() => IsVisible);
            }
        }

        public bool IsRunning
        {
            get
            {
                return m_isRunning;
            }
            set
            {
                m_isRunning = value;
                RaisePropertyChanged(() => IsRunning);
            }
        }

        public ViewModelBase()
        {
            m_navigationService = Bootstrapper.Instance.Container.Resolve<INavigationService>();
        }

        public virtual Task InitializeAsync(object navigationData)
        {
            return Task.FromResult(false);
        }


        public virtual Task OnPopModalAsync(object navigationData, string nombrePage)
        {
            return Task.FromResult(false);
        }

        public void ShowLoading()
        {
            IsVisible = true;
            IsRunning = true;
        }

        public void HideLoading()
        {

            IsVisible = false;
            IsRunning = false;
        }
    }
}
