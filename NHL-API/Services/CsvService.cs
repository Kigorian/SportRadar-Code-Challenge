using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace NHL_API.Services
{
    public class CsvService
    {
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
