using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Views
{
    public class MenuViewFlyoutMenuItem
    {
        public MenuViewFlyoutMenuItem()
        {
            TargetType = typeof(MenuViewFlyoutMenuItem);
        }
        public int Id { get; set; }
        public string Title { get; set; }

        public Type TargetType { get; set; }
    }
}