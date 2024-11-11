using System;
using System.Data;
using System.IO;
using System.Xml;
using System.Xml.Linq;

using Sipecom.FactElec.Pymes.Agentes.Framework.Comun.Mensajes;

namespace Sipecom.FactElec.Pymes.Agentes.Framework.Comun
{
    internal static class Convertidor
    {
		internal static ArrayOfXElement ToArrayOfXElement(DataSet dataSet)
		{
			if (dataSet == null) throw new ArgumentNullException(nameof(dataSet));

			var arrayOfXElement = new ArrayOfXElement();

			using (var schemaStream = new MemoryStream())
			{
				using (var writer = XmlWriter.Create(schemaStream))
				{
					dataSet.WriteXmlSchema(writer);
				}
				schemaStream.Position = 0;
				arrayOfXElement.Nodes.Add(XElement.Load(schemaStream));
			}

			using (var dataStream = new MemoryStream())
			{
				using (var writer = XmlWriter.Create(dataStream))
				{
					dataSet.WriteXml(writer, XmlWriteMode.DiffGram);
				}
				dataStream.Position = 0;
				arrayOfXElement.Nodes.Add(XElement.Load(dataStream));
			}

			return arrayOfXElement;
		}

		internal static DataSet ToDataSet(ArrayOfXElement arrayOfXElement)
		{
			if (arrayOfXElement == null) throw new ArgumentNullException(nameof(arrayOfXElement));
			if (arrayOfXElement.Nodes.Count != 2) throw new ArgumentException($"{nameof(arrayOfXElement)} should have 2 {nameof(arrayOfXElement.Nodes)}.", nameof(arrayOfXElement));

			var dataSet = new DataSet();

			using (var schemaStream = new MemoryStream())
			{
				using (var writer = XmlWriter.Create(schemaStream))
				{
					arrayOfXElement.Nodes[0].WriteTo(writer);
				}
				schemaStream.Position = 0;
				dataSet.ReadXmlSchema(schemaStream);
			}

			using (var dataStream = new MemoryStream())
			{
				using (var writer = XmlWriter.Create(dataStream))
				{
					arrayOfXElement.Nodes[1].WriteTo(writer);
				}
				dataStream.Position = 0;
				dataSet.ReadXml(dataStream, XmlReadMode.DiffGram);
			}

			return dataSet;
		}
	}
}
