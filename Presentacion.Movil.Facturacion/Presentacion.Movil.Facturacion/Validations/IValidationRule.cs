using System;

namespace Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Validations
{
    public interface IValidationRule<T>
    {
        string ValidationMessage { get; set; }

        bool Check(T value);
    }
}
