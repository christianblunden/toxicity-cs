using System.Linq;
using System.Xml.Linq;
using NUnit.Framework;
using Toxic;
using Is = NUnit.Framework.Is;

namespace Tests
{
    [TestFixture]
    public class MetricsParserTests
    {
        [Test]
        public void ShouldGetMetricsForSingleType()
        {
            const string data = 
@"<CodeMetricsReport Version=""10.0"">
  <Targets>
    <Target Name=""SomeAssembly.dll"">
      <Modules>
        <Module Name=""SomeAssembly.dll"" AssemblyVersion=""1.0.0.0"" FileVersion=""1.0.0.0"">
          <Metrics>
            <Metric Name=""MaintainabilityIndex"" Value=""65"" />
            <Metric Name=""CyclomaticComplexity"" Value=""31"" />
            <Metric Name=""ClassCoupling"" Value=""14"" />
            <Metric Name=""DepthOfInheritance"" Value=""7"" />
            <Metric Name=""LinesOfCode"" Value=""218"" />
          </Metrics>
          <Namespaces>
            <Namespace Name=""Some.Namespace"">
              <Metrics>
                <Metric Name=""MaintainabilityIndex"" Value=""65"" />
                <Metric Name=""CyclomaticComplexity"" Value=""31"" />
                <Metric Name=""ClassCoupling"" Value=""14"" />
                <Metric Name=""DepthOfInheritance"" Value=""7"" />
                <Metric Name=""LinesOfCode"" Value=""218"" />
              </Metrics>
              <Types>
                <Type Name=""SomeType"">
                  <Metrics>
                    <Metric Name=""MaintainabilityIndex"" Value=""65"" />
                    <Metric Name=""CyclomaticComplexity"" Value=""31"" />
                    <Metric Name=""ClassCoupling"" Value=""14"" />
                    <Metric Name=""DepthOfInheritance"" Value=""7"" />
                    <Metric Name=""LinesOfCode"" Value=""218"" />
                  </Metrics>
                  <Members>
                    <Member Name=""SomeMethod() : void"" File=""filename.cs"" Line=""15"">
                      <Metrics>
                        <Metric Name=""MaintainabilityIndex"" Value=""80"" />
                        <Metric Name=""CyclomaticComplexity"" Value=""9"" />
                        <Metric Name=""ClassCoupling"" Value=""29"" />
                        <Metric Name=""LinesOfCode"" Value=""30"" />
                      </Metrics>
                    </Member>
                    <Member Name=""AnotherMethod() : void"" File=""filename.cs"" Line=""15"">
                      <Metrics>
                        <Metric Name=""MaintainabilityIndex"" Value=""80"" />
                        <Metric Name=""CyclomaticComplexity"" Value=""10"" />
                        <Metric Name=""ClassCoupling"" Value=""29"" />
                        <Metric Name=""LinesOfCode"" Value=""29"" />
                      </Metrics>
                    </Member>
                    <Member Name=""YetAnotherMethod() : void"" File=""filename.cs"" Line=""15"">
                      <Metrics>
                        <Metric Name=""MaintainabilityIndex"" Value=""80"" />
                        <Metric Name=""CyclomaticComplexity"" Value=""9"" />
                        <Metric Name=""ClassCoupling"" Value=""30"" />
                        <Metric Name=""LinesOfCode"" Value=""29"" />
                      </Metrics>
                    </Member>
                  </Members>
                </Type>
              </Types>
            </Namespace>
          </Namespaces>
        </Module>
      </Modules>
    </Target>
  </Targets>
</CodeMetricsReport>";

            var results = new MetricsParser().Parse(XDocument.Parse(data));

            Assert.That(results.Count(), Is.EqualTo(1));
            var metrics = results.First();
            Assert.That(metrics[MetricsParser.ComplexityPerMethod], Is.EqualTo("1"));
            Assert.That(metrics[MetricsParser.CouplingPerMethod], Is.EqualTo("1"));
            Assert.That(metrics[MetricsParser.LinesPerMethod], Is.EqualTo("1"));
            Assert.That(metrics[MetricsParser.TypeName], Is.EqualTo("SomeType"));
        }

        [Test]
        public void ShouldGetMetricsForMultipleTypes()
        {
            const string data =
@"<CodeMetricsReport Version=""10.0"">
  <Targets>
    <Target Name=""SomeAssembly.dll"">
      <Modules>
        <Module Name=""SomeAssembly.dll"" AssemblyVersion=""1.0.0.0"" FileVersion=""1.0.0.0"">
          <Metrics>
            <Metric Name=""MaintainabilityIndex"" Value=""65"" />
            <Metric Name=""CyclomaticComplexity"" Value=""31"" />
            <Metric Name=""ClassCoupling"" Value=""14"" />
            <Metric Name=""DepthOfInheritance"" Value=""7"" />
            <Metric Name=""LinesOfCode"" Value=""218"" />
          </Metrics>
          <Namespaces>
            <Namespace Name=""Some.Namespace"">
              <Metrics>
                <Metric Name=""MaintainabilityIndex"" Value=""65"" />
                <Metric Name=""CyclomaticComplexity"" Value=""31"" />
                <Metric Name=""ClassCoupling"" Value=""14"" />
                <Metric Name=""DepthOfInheritance"" Value=""7"" />
                <Metric Name=""LinesOfCode"" Value=""218"" />
              </Metrics>
              <Types>
                <Type Name=""SomeType"">
                  <Metrics>
                    <Metric Name=""MaintainabilityIndex"" Value=""65"" />
                    <Metric Name=""CyclomaticComplexity"" Value=""31"" />
                    <Metric Name=""ClassCoupling"" Value=""14"" />
                    <Metric Name=""DepthOfInheritance"" Value=""7"" />
                    <Metric Name=""LinesOfCode"" Value=""218"" />
                  </Metrics>
                  <Members>
                    <Member Name=""SomeMethod() : void"" File=""filename.cs"" Line=""15"">
                      <Metrics>
                        <Metric Name=""MaintainabilityIndex"" Value=""80"" />
                        <Metric Name=""CyclomaticComplexity"" Value=""9"" />
                        <Metric Name=""ClassCoupling"" Value=""29"" />
                        <Metric Name=""LinesOfCode"" Value=""30"" />
                      </Metrics>
                    </Member>
                    <Member Name=""AnotherMethod() : void"" File=""filename.cs"" Line=""15"">
                      <Metrics>
                        <Metric Name=""MaintainabilityIndex"" Value=""80"" />
                        <Metric Name=""CyclomaticComplexity"" Value=""10"" />
                        <Metric Name=""ClassCoupling"" Value=""29"" />
                        <Metric Name=""LinesOfCode"" Value=""29"" />
                      </Metrics>
                    </Member>
                    <Member Name=""YetAnotherMethod() : void"" File=""filename.cs"" Line=""15"">
                      <Metrics>
                        <Metric Name=""MaintainabilityIndex"" Value=""80"" />
                        <Metric Name=""CyclomaticComplexity"" Value=""9"" />
                        <Metric Name=""ClassCoupling"" Value=""30"" />
                        <Metric Name=""LinesOfCode"" Value=""29"" />
                      </Metrics>
                    </Member>
                  </Members>
                </Type>
                <Type Name=""AnotherType"">
                  <Metrics>
                    <Metric Name=""MaintainabilityIndex"" Value=""65"" />
                    <Metric Name=""CyclomaticComplexity"" Value=""31"" />
                    <Metric Name=""ClassCoupling"" Value=""14"" />
                    <Metric Name=""DepthOfInheritance"" Value=""7"" />
                    <Metric Name=""LinesOfCode"" Value=""218"" />
                  </Metrics>
                  <Members>
                    <Member Name=""SomeMethod() : void"" File=""another-filename.cs"" Line=""15"">
                      <Metrics>
                        <Metric Name=""MaintainabilityIndex"" Value=""80"" />
                        <Metric Name=""CyclomaticComplexity"" Value=""10"" />
                        <Metric Name=""ClassCoupling"" Value=""60"" />
                        <Metric Name=""LinesOfCode"" Value=""30"" />
                      </Metrics>
                    </Member>
                    <Member Name=""AnotherMethod() : void"" File=""filename.cs"" Line=""15"">
                      <Metrics>
                        <Metric Name=""MaintainabilityIndex"" Value=""80"" />
                        <Metric Name=""CyclomaticComplexity"" Value=""10"" />
                        <Metric Name=""ClassCoupling"" Value=""30"" />
                        <Metric Name=""LinesOfCode"" Value=""90"" />
                      </Metrics>
                    </Member>
                    <Member Name=""YetAnotherMethod() : void"" File=""filename.cs"" Line=""15"">
                      <Metrics>
                        <Metric Name=""MaintainabilityIndex"" Value=""80"" />
                        <Metric Name=""CyclomaticComplexity"" Value=""10"" />
                        <Metric Name=""ClassCoupling"" Value=""90"" />
                        <Metric Name=""LinesOfCode"" Value=""60"" />
                      </Metrics>
                    </Member>
                  </Members>
                </Type>
              </Types>
            </Namespace>
          </Namespaces>
        </Module>
      </Modules>
    </Target>
  </Targets>
</CodeMetricsReport>";

            var results = new MetricsParser().Parse(XDocument.Parse(data));

            Assert.That(results.Count(), Is.EqualTo(2));
            var metricsForSomeType = results.First();
            Assert.That(metricsForSomeType[MetricsParser.ComplexityPerMethod], Is.EqualTo("1"));
            Assert.That(metricsForSomeType[MetricsParser.CouplingPerMethod], Is.EqualTo("1"));
            Assert.That(metricsForSomeType[MetricsParser.LinesPerMethod], Is.EqualTo("1"));
            Assert.That(metricsForSomeType[MetricsParser.TypeName], Is.EqualTo("SomeType"));            
            
            var metricsForAnotherType = results.Last();
            Assert.That(metricsForAnotherType[MetricsParser.ComplexityPerMethod], Is.EqualTo("3"));
            Assert.That(metricsForAnotherType[MetricsParser.CouplingPerMethod], Is.EqualTo("6"));
            Assert.That(metricsForAnotherType[MetricsParser.LinesPerMethod], Is.EqualTo("6"));
            Assert.That(metricsForAnotherType[MetricsParser.TypeName], Is.EqualTo("AnotherType"));
        }
    }
}
