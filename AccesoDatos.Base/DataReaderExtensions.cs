using System;
using System.Data;

namespace Sipecom.FactElec.Pymes.AccesoDatos.Base
{
    public static class DataReaderExtensions
    {
        public static string ColumnToString(this IDataReader input, int columnOrdinal)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            if (input.IsDBNull(columnOrdinal))
            {
                return (string)null;
            }

            return input[columnOrdinal] as string;
        }

        public static string ColumnToString(this IDataReader input, string columnName)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            var columnOrdinal = input.GetOrdinal(columnName);

            return ColumnToString(input, columnOrdinal);
        }

        public static short? ColumnToInt16(this IDataReader input, int columnOrdinal)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            if (input.IsDBNull(columnOrdinal))
            {
                return (short?)null;
            }

            return Convert.ToInt16(input[columnOrdinal]);
        }

        public static short? ColumnToInt16(this IDataReader input, string columnName)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            var columnOrdinal = input.GetOrdinal(columnName);

            return ColumnToInt16(input, columnOrdinal);
        }

        public static int? ColumnToInt32(this IDataReader input, int columnOrdinal)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            if (input.IsDBNull(columnOrdinal))
            {
                return (int?)null;
            }

            return Convert.ToInt32(input[columnOrdinal]);
        }

        public static int? ColumnToInt32(this IDataReader input, string columnName)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            var columnOrdinal = input.GetOrdinal(columnName);

            return ColumnToInt32(input, columnOrdinal);
        }

        public static long? ColumnToInt64(this IDataReader input, int columnOrdinal)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            if (input.IsDBNull(columnOrdinal))
            {
                return (long?)null;
            }

            return Convert.ToInt64(input[columnOrdinal]);
        }

        public static long? ColumnToInt64(this IDataReader input, string columnName)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            var columnOrdinal = input.GetOrdinal(columnName);

            return ColumnToInt64(input, columnOrdinal);
        }

        public static DateTime? ColumnToDateTime(this IDataReader input, int columnOrdinal)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            if (input.IsDBNull(columnOrdinal))
            {
                return (DateTime?)null;
            }

            return Convert.ToDateTime(input[columnOrdinal]);
        }

        public static DateTime? ColumnToDateTime(this IDataReader input, string columnName)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            var columnOrdinal = input.GetOrdinal(columnName);

            return ColumnToDateTime(input, columnOrdinal);
        }

        public static bool? ColumnToBoolean(this IDataReader input, int columnOrdinal)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            if (input.IsDBNull(columnOrdinal))
            {
                return (bool?)null;
            }

            return Convert.ToBoolean(input[columnOrdinal]);
        }

        public static bool? ColumnToBoolean(this IDataReader input, string columnName)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            var columnOrdinal = input.GetOrdinal(columnName);

            return ColumnToBoolean(input, columnOrdinal);
        }

        public static decimal? ColumnToDecimal(this IDataReader input, int columnOrdinal)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            if (input.IsDBNull(columnOrdinal))
            {
                return (decimal?)null;
            }

            return Convert.ToDecimal(input[columnOrdinal]);
        }

        public static decimal? ColumnToDecimal(this IDataReader input, string columnName)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            var columnOrdinal = input.GetOrdinal(columnName);

            return ColumnToDecimal(input, columnOrdinal);
        }

        public static byte? ColumnToByte(this IDataReader input, int columnOrdinal)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            if (input.IsDBNull(columnOrdinal))
            {
                return (byte?)null;
            }

            return Convert.ToByte(input[columnOrdinal]);
        }

        public static byte? ColumnToByte(this IDataReader input, string columnName)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            var columnOrdinal = input.GetOrdinal(columnName);

            return ColumnToByte(input, columnOrdinal);
        }

        public static double? ColumnToDouble(this IDataReader input, int columnOrdinal)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            if (input.IsDBNull(columnOrdinal))
            {
                return (double?)null;
            }

            return Convert.ToDouble(input[columnOrdinal]);
        }

        public static double? ColumnToDouble(this IDataReader input, string columnName)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            var columnOrdinal = input.GetOrdinal(columnName);

            return ColumnToDouble(input, columnOrdinal);
        }
    }
}
