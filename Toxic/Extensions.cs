using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Toxic
{
    public static class Extensions
    {
        public static string Get(this XElement element, string name)
        {
            return element.Attribute(name).Value.Replace(",","").Trim();
        }

        public static double ToDouble(this string value)
        {
            double result = 0;
			Double.TryParse(value, out result);
            return result;
        }

        public static string Join(this IEnumerable<string> items, string delimiter = ",")
        {
            return items.Skip(1).Aggregate(items.First(), (total, next) => String.Concat(total, delimiter, next));
        }

        public static Dictionary<string,string> ToDictionary(this IEnumerable<KeyValuePair<string,string>> pairs)
        {
            return pairs.ToDictionary(pair => pair.Key, pair => pair.Value);
        }
    }
}