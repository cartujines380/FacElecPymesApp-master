using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Infraestructura.Transversal.eComp.PDF
{
    public class ReadyCatalogo
    {
        private const string archivo = "catalogos.xml";
        private static string ruta = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase.ToString()).Remove(0, 6);

        public static string _ruta { get { return ruta; } }
        /// <summary>
        /// Consulta de Catalogos
        /// </summary>
        /// <param name="catalogo">Nombre del Catalgoo</param>
        /// <param name="idTabla">Identificador del detalle del catalogo</param>
        /// <returns></returns>
        public static Catalogo getCatalogo(string catalogo, string idTabla)
        {
            Catalogo cat = new Catalogo();
            XmlDocument XmlCatalogo = new XmlDocument();
            XmlNode XmlNode = null;
            try
            {
                if (File.Exists(Path.Combine(ruta, archivo)))
                {
                    XmlCatalogo.Load(Path.Combine(ruta, archivo));
                    XmlNode = XmlCatalogo.SelectSingleNode(string.Format("/catalogos/catalogo[@name='{0}']/tabla[@codigo='{1}']", catalogo, idTabla));
                    if (!Object.ReferenceEquals(XmlNode, null))
                    {
                        cat.IdTabla = Object.ReferenceEquals(XmlNode.Attributes["idtabla"], null) ? 0 : Convert.ToInt32(XmlNode.Attributes["idtabla"].InnerText);
                        cat.Codigo = Object.ReferenceEquals(XmlNode.Attributes["codigo"], null) ? "" : XmlNode.Attributes["codigo"].InnerText;
                        cat.Descripcion = Object.ReferenceEquals(XmlNode.Attributes["descripcion"], null) ? "" : XmlNode.Attributes["descripcion"].InnerText;
                        cat.DescAlterno = Object.ReferenceEquals(XmlNode.Attributes["descalterno"], null) ? "" : XmlNode.Attributes["descalterno"].InnerText;
                        cat.Estado = Object.ReferenceEquals(XmlNode.Attributes["estado"], null) ? "" : XmlNode.Attributes["estado"].InnerText;
                        cat.Necesario = Object.ReferenceEquals(XmlNode.Attributes["necesarion"], null) ? "" : XmlNode.Attributes["necesarion"].InnerText;
                        cat.Relacion1 = Object.ReferenceEquals(XmlNode.Attributes["relacion1"], null) ? "" : XmlNode.Attributes["relacion1"].InnerText;
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return cat;
        }
    }

    public class Catalogo
    {
        public int IdTabla { get; set; }
        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public string DescAlterno { get; set; }
        public string Estado { get; set; }
        public string Necesario { get; set; }
        public string Relacion1 { get; set; }
    }
}
