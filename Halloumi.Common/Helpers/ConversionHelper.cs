using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Halloumi.Common.Helpers
{
    public static class ConversionHelper
    {
        /// <summary>
        /// Converts a string to an int
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>
        /// The value as an int.
        /// </returns>
        public static int ToInt(object value, int defaultValue)
        {
            int result = defaultValue;
            int.TryParse(value.ToString(), out result);
            return result;
        }

        /// <summary>
        /// Converts a string to an int
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The value as an int.
        /// </returns>
        public static int ToInt(object value)
        {
            return ToInt(value, 0);
        }

        /// <summary>
        /// Converts a string to a decimal
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>
        /// The value as a decimal.
        /// </returns>
        public static decimal ToDecimal(object value, decimal defaultValue)
        {
            decimal result = defaultValue;
            decimal.TryParse(value.ToString(), out result);
            return result;
        }

        /// <summary>
        /// Converts a string to a decimal
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The value as a decimal.
        /// </returns>
        public static decimal ToDecimal(object value)
        {
            return ToDecimal(value, 0);
        }

        /// <summary>
        /// Converts a string to a double
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>
        /// The value as a double.
        /// </returns>
        public static double ToDouble(object value, double defaultValue)
        {
            double result = defaultValue;
            double.TryParse(value.ToString(), out result);
            return result;
        }

        /// <summary>
        /// Converts a string to a double
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The value as a double.
        /// </returns>
        public static double ToDouble(object value)
        {
            return ToDouble(value, 0);
        }

        /// <summary>
        /// Converts a string to a float
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>
        /// The value as a float.
        /// </returns>
        public static float ToFloat(object value, float defaultValue)
        {
            float result = defaultValue;
            float.TryParse(value.ToString(), out result);
            return result;
        }

        /// <summary>
        /// Converts a string to a float
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The value as a float.
        /// </returns>
        public static float ToFloat(object value)
        {
            return ToFloat(value, 0);
        }


        /// <summary>
        /// Converts a string to a boolean
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>
        /// The value as a bool.
        /// </returns>
        public static bool ToBoolean(object value, bool defaultValue)
        {
            bool result = defaultValue;
            var text = value.ToString().ToLower().Trim();
            if (text == "1" || text == "true" || text == "yes") return true;
            if (text == "0" || text == "false" || text == "no") return false;

            bool.TryParse(value.ToString(), out result);
            return result;
        }

        /// <summary>
        /// Converts a string to a boolean
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The value as a bool.
        /// </returns>
        public static bool ToBoolean(object value)
        {
            return ToBoolean(value, false);
        }


        /// <summary>
        /// Converts a string to a long
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>
        /// The value as a long.
        /// </returns>
        public static long ToLong(object value, long defaultValue)
        {
            long result = defaultValue;
            long.TryParse(value.ToString(), out result);
            return result;
        }

        /// <summary>
        /// Converts a string to a long
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The value as a long.
        /// </returns>
        public static long ToLong(object value)
        {
            return ToLong(value, 0);
        }
    }
}
