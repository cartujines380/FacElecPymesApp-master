using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuViewFlyout : ContentPage
    {
        public ListView ListView;

        public MenuViewFlyout()
        {
            InitializeComponent();

            BindingContext = new MenuViewFlyoutViewModel();
            ListView = MenuItemsListView;
        }

        class MenuViewFlyoutViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<MenuViewFlyoutMenuItem> MenuItems { get; set; }

            public MenuViewFlyoutViewModel()
            {
                MenuItems = new ObservableCollection<MenuViewFlyoutMenuItem>(new[]
                {
                    new MenuViewFlyoutMenuItem { Id = 0, Title = "Page 1" },
                    new MenuViewFlyoutMenuItem { Id = 1, Title = "Page 2" },
                    new MenuViewFlyoutMenuItem { Id = 2, Title = "Page 3" },
                    new MenuViewFlyoutMenuItem { Id = 3, Title = "Page 4" },
                    new MenuViewFlyoutMenuItem { Id = 4, Title = "Page 5" },
                });
            }

            #region INotifyPropertyChanged Implementation
            public event PropertyChangedEventHandler PropertyChanged;
            void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                if (PropertyChanged == null)
                    return;

                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            #endregion
        }
    }
}