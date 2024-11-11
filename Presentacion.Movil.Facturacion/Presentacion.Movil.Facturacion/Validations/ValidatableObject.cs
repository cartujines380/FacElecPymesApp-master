using System;
using System.Collections.Generic;
using System.Linq;
using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.ViewModels.Base;

namespace Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Validations
{
    public class ValidatableObject<T> : ExtendedBindableObject, IValidity
    {
        private readonly List<IValidationRule<T>> m_validations;
        private List<string> m_errors;
        private T m_value;
        private bool m_isValid;

        public List<IValidationRule<T>> Validations => m_validations;

        public List<string> Errors
        {
            get
            {
                return m_errors;
            }
            set
            {
                m_errors = value;
                RaisePropertyChanged(() => Errors);
            }
        }

        public T Value
        {
            get
            {
                return m_value;
            }
            set
            {
                m_value = value;
                RaisePropertyChanged(() => Value);
            }
        }

        public bool IsValid
        {
            get
            {
                return m_isValid;
            }
            set
            {
                m_isValid = value;
                RaisePropertyChanged(() => IsValid);
            }
        }

        public ValidatableObject()
        {
            m_isValid = true;
            m_errors = new List<string>();
            m_validations = new List<IValidationRule<T>>();
        }

        public bool Validate()
        {
            Errors.Clear();

            IEnumerable<string> errors = m_validations
                .Where(v => !v.Check(Value))
                .Select(v => v.ValidationMessage);

            Errors = errors.ToList();
            IsValid = !Errors.Any();

            return this.IsValid;
        }
    }
}
