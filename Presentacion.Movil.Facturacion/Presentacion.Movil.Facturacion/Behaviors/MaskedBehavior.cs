using System;
using System.Collections.Generic;
using Xamarin.Forms;

using Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Behaviors.Base;

namespace Sipecom.FactElec.Pymes.Presentacion.Movil.Facturacion.Behaviors
{
    public class MaskedBehavior : BindableBehavior<Entry>
    {
        private string m_mask = string.Empty;
        IDictionary<int, char> m_positions;

        public string Mask
        {
            get
            {
                return m_mask;
            }
            set
            {
                m_mask = value;
                SetPositions();
            }
        }

        protected override void OnAttachedTo(Entry visualElement)
        {
            visualElement.TextChanged += OnEntryTextChanged;

            base.OnAttachedTo(visualElement);
        }

        protected override void OnDetachingFrom(Entry view)
        {
            view.TextChanged -= OnEntryTextChanged;

            base.OnDetachingFrom(view);
        }

        private void SetPositions()
        {
            if (string.IsNullOrEmpty(Mask))
            {
                m_positions = null;
                return;
            }

            var list = new Dictionary<int, char>();

            for (var i = 0; i < Mask.Length; i++)
            {
                if (Mask[i] != 'X')
                {
                    list.Add(i, Mask[i]);
                }
            }

            m_positions = list;
        }

        private void OnEntryTextChanged(object sender, TextChangedEventArgs args)
        {
            var entry = sender as Entry;

            var text = entry.Text;

            if (string.IsNullOrWhiteSpace(text) || (m_positions == null))
            {
                return;
            }

            if (text.Length > m_mask.Length)
            {
                entry.Text = text.Remove(text.Length - 1);
                return;
            }

            foreach (var position in m_positions)
            {
                if (text.Length < (position.Key + 1))
                {
                    continue;
                }

                var value = position.Value.ToString();

                if (text.Substring(position.Key, 1) != value)
                {
                    text = text.Insert(position.Key, value);
                }
            }

            if (entry.Text != text)
            {
                entry.Text = text;
            }
        }
    }
}
