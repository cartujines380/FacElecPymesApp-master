using Foundation;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Controls;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.iOS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomViewCell), typeof(CustomViewCellRenderer))]
namespace Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.iOS
{
    public class CustomViewCellRenderer : ViewCellRenderer
    {
        public override UITableViewCell GetCell(Cell item, UITableViewCell reusableCell, UITableView tv)
        {
            var cell =  base.GetCell(item, reusableCell, tv);
            var view = item as CustomViewCell;
            cell.SelectedBackgroundView = new UIView
            {
                BackgroundColor = view.SelectedItemBackgroundColor.ToUIColor()
            };

            return cell;
        }
    }
}