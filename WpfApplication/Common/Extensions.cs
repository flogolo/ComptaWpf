using System;
using System.Globalization;

namespace MaCompta.Common
{
    public static class Extensions
    {
        /// <summary>
        /// Raises the event safely by checking for null and avoiding race conditions.
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <see cref="http://stackoverflow.com/questions/248072/evil-use-of-extension-methods" />
        public static void Raise(this EventHandler handler, object sender, EventArgs e)
        {
            if (handler != null)
                handler(sender, e);
        }

        /// <summary>
        /// Raises the event safely by checking for null and avoiding race conditions.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="handler"></param>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <see cref="http://stackoverflow.com/questions/248072/evil-use-of-extension-methods" />
        public static void Raise<T>(this EventHandler<T> handler, object sender, T e) where T : EventArgs
        {
            if (handler != null)
                handler(sender, e);
        }

        public static DateTime? ToDateTime(this string source)
        {
            DateTime result;
            if (DateTime.TryParse(source, out result))
                return result;
            return null;
        }

        public static decimal? ToDecimal(this string source)
        {
            decimal result;
            if (decimal.TryParse(source, NumberStyles.Currency, CultureInfo.CurrentCulture, out result))
                return result;
            return 0;
        }

        public static double? ToDouble(this string source)
        {
            double result;
            if (double.TryParse(source, out result))
                return result;
            return 0;
        }
    }
}
