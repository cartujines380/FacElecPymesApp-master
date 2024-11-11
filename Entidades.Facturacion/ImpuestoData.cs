using System;
using System.Collections.Generic;
using System.Text;

namespace Sipecom.FactElec.Pymes.Entidades.Facturacion
{
    public class ImpuestoData
    {
        public const string IMPUESTO_CERO_PORCIENTO = "0";

        public const string IMPUESTO_DOCE_PORCIENTO = "2";

        public const string IMPUESTO_CATORCE_PORCIENTO = "3";

        public const string IMPUESTO_NO_OBJETO_IVA = "6";

        public const string IMPUESTO_EXCENTO_IVA = "7";

        public const string IMPUESTO_NO_GRABA_ICE = "0";

        public string Codigo { get; set; }

        public int ImpuestoRetener { get; set; }

        public string Descripcion { get; set; }

        public decimal? Valor { get; set; }

        public string ValorStr
        {
            get
            {
                return UtilFormato.ACadena(Valor);
            }
            set
            {
                Valor = UtilFormato.ADecimal(value);
            }
        }

        public bool EsCeroPorCiento()
        {
            return EsCeroPorCiento(this.Codigo);
        }

        public static bool EsCeroPorCiento(string codigo)
        {
            return (codigo == IMPUESTO_CERO_PORCIENTO);
        }

        public bool EsDocePorCiento()
        {
            return EsDocePorCiento(this.Codigo);
        }

        public static bool EsDocePorCiento(string codigo)
        {
            return (codigo == IMPUESTO_DOCE_PORCIENTO);
        }

        public bool EsCatorcePorCiento()
        {
            return EsCatorcePorCiento(this.Codigo);
        }

        public static bool EsCatorcePorCiento(string codigo)
        {
            return (codigo == IMPUESTO_CATORCE_PORCIENTO);
        }

        public bool EsNoObjetoIva()
        {
            return EsNoObjetoIva(this.Codigo);
        }

        public static bool EsNoObjetoIva(string codigo)
        {
            return (codigo == IMPUESTO_NO_OBJETO_IVA);
        }

        public bool EsExentoIva()
        {
            return EsExentoIva(this.Codigo);
        }

        public static bool EsExentoIva(string codigo)
        {
            return (codigo == IMPUESTO_EXCENTO_IVA);
        }

        public bool NoGrabaIce()
        {
            return NoGrabaIce(this.Codigo);
        }

        public static bool NoGrabaIce(string codigo)
        {
            return (codigo == IMPUESTO_NO_GRABA_ICE);
        }
    }

}
