﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Infraestructura.Transversal.eComp.PDF.Model
{
    public static class XElementExtensions
    {
        public static string GetElementValue(this XElement input, XName name)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            var elements = input.Elements(name);

            if (!elements.Any())
            {
                return null;
            }

            return elements.First().Value;
        }

        public static string GetAttributeValue(this XElement input, XName name)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            var attributes = input.Attributes(name);

            if (!attributes.Any())
            {
                return null;
            }

            return attributes.First().Value;
        }
    }
}
