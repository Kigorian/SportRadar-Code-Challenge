using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace NHL_API.Services
{
    public class CsvService
    {
        /// <summary>
        /// Generates a CSV string for the given list of objects. Headers are automatically
        /// assigned based on the object's property names.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objectlist"></param>
        /// <returns>A CSV file string, complete with headers.</returns>
        public static string ToCsv<T>(IEnumerable<T> objectlist)
        {
            Type t = typeof(T);
            var properties = t.GetProperties();

            string header = string.Join(',', properties.Select(f => f.Name).ToArray());

            StringBuilder csvdata = new StringBuilder();
            csvdata.AppendLine(header);

            foreach (var o in objectlist)
            {
                csvdata.AppendLine(ToCsvFields(properties, o));
            }

            return csvdata.ToString();
        }

        /// <summary>
        /// Generates a row of CSV data from the given properties.
        /// </summary>
        /// <param name="properties"></param>
        /// <param name="o"></param>
        /// <returns>A string for a single row of CSV data.</returns>
        private static string ToCsvFields(PropertyInfo[] properties, object o)
        {
            StringBuilder csv = new StringBuilder();

            foreach (var property in properties)
            {
                if (csv.Length > 0)
                {
                    csv.Append(',');
                }

                var x = property.GetValue(o);

                if (x != null)
                {
                    csv.Append(x.ToString());
                }
            }

            return csv.ToString();
        }
    }
}
