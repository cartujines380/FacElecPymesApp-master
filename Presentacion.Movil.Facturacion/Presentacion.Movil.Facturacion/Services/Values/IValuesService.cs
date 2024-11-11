using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Services.Values
{
    public interface IValuesService
    {
        Task<ObservableCollection<string>> GetValuesAsync();
    }
}
