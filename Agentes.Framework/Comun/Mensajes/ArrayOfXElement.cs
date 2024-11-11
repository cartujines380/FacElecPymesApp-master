using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace Sipecom.FactElec.Pymes.Agentes.Framework.Comun.Mensajes
{
    [XmlSchemaProvider(null, IsAny = true)]
    public partial class ArrayOfXElement : object, IXmlSerializable
    {

        private List<XElement> nodesList = new List<XElement>();

        public ArrayOfXElement()
        {
        }

        public virtual List<XElement> Nodes
        {
            get
            {
                return this.nodesList;
            }
        }

        public virtual XmlSchema GetSchema()
        {
            throw new NotImplementedException();
        }

        public virtual void WriteXml(XmlWriter writer)
        {
            IEnumerator<XElement> e = nodesList.GetEnumerator();
            for (
                ; e.MoveNext();
            )
            {
                ((IXmlSerializable)(e.Current)).WriteXml(writer);
            }
        }

        public virtual void ReadXml(XmlReader reader)
        {
            for (
                ; (reader.NodeType != XmlNodeType.EndElement);
            )
            {
                if ((reader.NodeType == XmlNodeType.Element))
                {
                    XElement elem = new XElement("default");
                    ((IXmlSerializable)(elem)).ReadXml(reader);
                    Nodes.Add(elem);
                }
                else
                {
                    reader.Skip();
                }
            }
        }
    }
}
