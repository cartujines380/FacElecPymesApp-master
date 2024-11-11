using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructura.Transversal.eComp.PDF.Model
{
    public class General
    {
        public InfoContrato LeerDataTableCliente(DataTable InfoCliente)
        {
            var objCliente = new InfoContrato();
            if (InfoCliente != null && InfoCliente.Rows.Count > 0)
            {
                foreach (DataRow row in InfoCliente.Rows)
                {
                    objCliente = new InfoContrato
                    {
                        RazonSocial = row["RazonSocial"].ToString(),
                        Ruc = row["Ruc"].ToString(),
                        Provincia = row["Provincia"].ToString(),
                        Ciudad = row["Ciudad"].ToString(),
                        Direccion = row["DirMatriz"].ToString(),
                        Celular = row["Celular"].ToString(),
                        CorreoAvisoCertificado = row["CorreoAvisoCertificado"].ToString(),
                        RepresentanteLegal = row["RepresentanteLegal"].ToString(),
                        IdentificacionRepresentante = row["IdentificacionRepresentante"].ToString(),
                        NombrePlan = row["NombrePlan"].ToString(),
                        Precio = row["Precio"].ToString(),
                        CantidadDocHasta = row["CantidadDocHasta"].ToString(),
                        FechaCompletaInicioPlan = row["FechaInicioPlan"].ToString(),
                        FechaInicioPlan = row["FechaInicioPlan"].ToString(),
                        FechaFinPlan = row["FechaFinPlan"].ToString()
                    };
                }
            }

            return objCliente;
        }

        public string ObtenerFechaDDMMYYYY(string fecha)
        {
            var cadenaFecha = "";
            if (fecha != null && fecha != "")
            {
                var split = fecha.Split('/');
                if (split.Length > 0)
                {
                    var mes = split[0].ToString().PadLeft(2, '0');
                    var dia = split[1].ToString().PadLeft(2, '0');
                    var anio = split[2];
                    cadenaFecha = dia + "/" + mes + "/" + anio;
                }
            }
            return cadenaFecha;
        }

        public string ObtenerFechaFormateada(string fechaInicioPlan)
        {
            var cadenaFecha = "";
            if (fechaInicioPlan != null && fechaInicioPlan != "")
            {
                var split = fechaInicioPlan.Split('/');
                if (split.Length > 0)
                {
                    var dia = split[0].ToString().PadLeft(2, '0');
                    var mes = split[1].ToString().PadLeft(2, '0');
                    var anio = split[2];
                    var mesLetra = ObtenerMesLetra(mes);
                    cadenaFecha = "Guayaquil, a los " + dia + " días del mes de " + mesLetra + " del " + anio + ".";
                }
            }
            return cadenaFecha;
        }

        public string ObtenerMesLetra(string mesLetra)
        {
            var mes = "";
            switch (mesLetra)
            {
                case "01":
                    mes = "Enero";
                    break;
                case "02":
                    mes = "Febrero";
                    break;
                case "03":
                    mes = "Marzo";
                    break;
                case "04":
                    mes = "Abril";
                    break;
                case "05":
                    mes = "Mayo";
                    break;
                case "06":
                    mes = "Junio";
                    break;
                case "07":
                    mes = "Julio";
                    break;
                case "08":
                    mes = "Agosto";
                    break;
                case "09":
                    mes = "Septiembre";
                    break;
                case "10":
                    mes = "Octubre";
                    break;
                case "11":
                    mes = "Noviembre";
                    break;
                case "12":
                    mes = "Diciembre";
                    break;
            }

            return mes;
        }
    }
}
