using BillingAssistant.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BillingAssistant.ExtensionMethods
{
    public static class StringExtensionMethods
    {
        public static bool IsNotNullOrEmpty(this string value)
        {
            return !String.IsNullOrEmpty(value);
        }
        public static bool IsNullOrEmpty(this string value)
        {
            return String.IsNullOrEmpty(value);
        }

        public static DateTime ToDate(this string value)
        {
            return DateTime.Parse(value, CommonController.DateTimeFormatInfo);
        }
    }

    public static class NumericExtensionMethods
    {

        public static string ToHanaFormat(this double value)
        {
            return value.ToString("###0.000000").Replace(",", ".");
        }
    }

    public static class DateTimeExtensionMethods
    {
        public static string ToHanaFormat(this DateTime value)
        {
            return value.ToString("yyyy-MM-dd");
        }
    }

    public static class ObjectExtensionMethods
    {
        public static int ToInt32(this object value)
        {
            return Convert.ToInt32(value);
        }

        public static double ToDouble(this object value)
        {
            return Convert.ToDouble(value);
        }

        public static DateTime ToDateTime(this object value)
        {
            return DateTime.Parse(value.ToString(), CommonController.DateTimeFormatInfo);
        }

        public static TResult IfNotNull<T, TResult>(this T target, Func<T, TResult> getValue) where T : class
        {
            if (target == null)
            {
                return default(TResult);
            }
            return getValue(target);
        }

        public static void ExecuteIfNotNull(this object target, Action action)
        {
            switch (target.IfNotNull(x => x.GetType().Name))
            {
                case "System.Int32":
                    if (((int)target) != default(int))
                        action();
                    break;
            }

            if (target != null)
                action();
        }

        public static void Kill(this object o)
        {
            o.ExecuteIfNotNull(() =>
            {
                GC.WaitForPendingFinalizers();
                Marshal.ReleaseComObject(o); o = null;

                GC.Collect();
                GC.WaitForPendingFinalizers();
            });
        }
    }

    public static class EnumerableExtensionMethods
    {
        public static void Each<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (T item in enumerable)
            {
                action(item);
            }
        }

        public static string ToCsv<T>(this IEnumerable<T> instance, char separator)
        {
            StringBuilder csv;
            if (instance != null)
            {
                csv = new StringBuilder();
                instance.Each(value => csv.AppendFormat("{0}{1}", value, separator));
                return csv.ToString(0, csv.Length - 1);
            }
            return null;
        }

        public static string ToCsv<T>(this IEnumerable<T> instance)
        {
            StringBuilder csv;
            if (instance != null)
            {
                csv = new StringBuilder();
                instance.Each(v => csv.AppendFormat("{0},", v));
                return csv.ToString(0, csv.Length - 1);
            }
            return null;
        }
    }
}
