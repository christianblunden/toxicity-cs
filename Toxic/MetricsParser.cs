using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Metrics = System.Collections.Generic.Dictionary<string, string>;

namespace Toxic
{
    public class MetricsParser
    {
        public const string LinesPerMethod = "LinesPerMethod";
        public const string TypeName = "TypeName";
        public const string MethodCount = "MethodCount";
        public const string Name = "Name";
        public const string Value = "Value";
        public const string ClassCoupling = "ClassCoupling";
        public const string MaintainabilityIndex = "MaintainabilityIndex";
        public const string LinesOfCode = "LinesOfCode";
        public const string CyclomaticComplexity = "CyclomaticComplexity";
        public const string ComplexityPerMethod = "ComplexityPerMethod";
        public const string CouplingPerMethod = "CouplingPerMethod";
        public static string[] generatedFiles = new[] {"designer.cs", "reference.cs", "assemblyinfo.cs"};

        public static Dictionary<string, Threashold> Threasholds = new Dictionary<string, Threashold> 
        { 
            {LinesPerMethod, Is.GreaterThanOrEqualTo(1)},
            {CyclomaticComplexity, Is.GreaterThanOrEqualTo(1)},
            {ComplexityPerMethod, Is.GreaterThanOrEqualTo(1)},
            {CouplingPerMethod, Is.GreaterThanOrEqualTo(1)},
            {TypeName, value => false},
            {MethodCount, value => false},
        };

        public static Dictionary<string, double> threasholdLimits = new Dictionary<string, double> 
        { 
            {LinesPerMethod, 30},
            {CyclomaticComplexity, 10},
            {ClassCoupling, 30},
        };

        public IEnumerable<Metrics> Parse(XDocument inputDoc)
        {
            var data = inputDoc.Descendants("Type")
                .Select(Metrics)
                .Where(MetricIsAboveThreashold);
            return data;
        }

        public static bool MetricIsAboveThreashold(Metrics value)
        {
            return value.Any(metric => Threasholds[metric.Key](metric.Value));
        }

        public static Metrics Metrics(XElement typeNode)
        {
            var memberMetrics = typeNode.Descendants("Member")
                .Where(IsNotGenerated)
                .SelectMany(node => node.Descendants("Metric"));

            var metricsMetaData = new Metrics();
            metricsMetaData[TypeName] = typeNode.Get(Name);
            metricsMetaData[LinesPerMethod] = MemberToxicityFor(LinesOfCode, threasholdLimits[LinesPerMethod], memberMetrics).ToString();
            metricsMetaData[ComplexityPerMethod] = MemberToxicityFor(CyclomaticComplexity, threasholdLimits[CyclomaticComplexity], memberMetrics).ToString();
            metricsMetaData[CouplingPerMethod] = MemberToxicityFor(ClassCoupling, threasholdLimits[ClassCoupling], memberMetrics).ToString();

            return metricsMetaData.ToDictionary();
        }

        public static double MemberToxicityFor(string metricName, double threshold, IEnumerable<XElement> metrics)
        {
            var sum = metrics.Where(metric => metric.Get(Name) == metricName)
                .Sum(metric => Normalise( metric.Get(Value).ToDouble(), threshold));

            return sum;
        }

        public static double Normalise(double value, double threshold)
        {
            double raw = value / threshold;
            if(raw < 1) return 0;
            return raw;
        }

        public static bool IsNotGenerated(XElement node)
        {
            if (node.Attribute("File") == null) return true;
            return !generatedFiles.Contains(node.Get("File").ToLower());
        }
    }
}