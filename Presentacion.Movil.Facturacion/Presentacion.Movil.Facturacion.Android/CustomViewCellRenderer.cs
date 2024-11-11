using Android.Content;
using Android.Graphics.Drawables;
using view = Android.Views;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Android;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Controls;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomViewCell), typeof(CustomViewCellRenderer))]
namespace Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Android
{
    public class CustomViewCellRenderer : ViewCellRenderer
    {
        private view.View _cellCore;
        private Drawable _unselectedBackground;
        private bool _selected;

        protected override view.View GetCellCore(Xamarin.Forms.Cell item, view.View convertView, view.ViewGroup parent, Context context)
        {
            _cellCore = base.GetCellCore(item, convertView, parent, context);

            _selected = false;
            _unselectedBackground = _cellCore.Background;

            return _cellCore;
        }

        protected override void OnCellPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnCellPropertyChanged(sender, e);

            if (e.PropertyName == "IsSelected")
            {
                _selected = !_selected;

                if (_selected)
                {
                    var extendedViewCell = sender as CustomViewCell;
                    _cellCore.SetBackgroundColor(extendedViewCell.SelectedItemBackgroundColor.ToAndroid());
                }
                else
                {
                    _cellCore.SetBackground(_unselectedBackground);
                }
            }
        }
    }
}