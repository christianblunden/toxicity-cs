using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Metrics = System.Collections.Generic.Dictionary<string, string>;

namespace Toxic
{
    public delegate bool Threashold(string value);

    public class Program
    {
        public static void Main(string[] args)
        {
            if(args.Count() == 0)   
            {
                Console.WriteLine("Please provide an input filename");
                return;
            }

            var inputFilename = args[0];

            var inputDoc = XDocument.Load(inputFilename);

            var data = new MetricsParser().Parse(inputDoc);

            File.WriteAllText("scores.csv", ToCSV(data));
        }

        private static string ToCSV(IEnumerable<Metrics> data)
        {
            var output = new StringBuilder();
            output.AppendLine(data.First().Keys.Join());
            data.ToList().ForEach(row => output.AppendLine(row.Values.Join()));
            return output.ToString();
        }
    }
}
    