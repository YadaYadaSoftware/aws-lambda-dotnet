using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Amazon.Lambda.Annotations.SourceGenerator.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Testing.Verifiers;
using Microsoft.CodeAnalysis.Text;
using Xunit;
using Xunit.Sdk;
using VerifyCS = Amazon.Lambda.Annotations.SourceGenerators.Tests.CSharpSourceGeneratorVerifier<Amazon.Lambda.Annotations.SourceGenerator.Generator>;

namespace Amazon.Lambda.Annotations.SourceGenerators.Tests
{
    public partial class SourceGeneratorTests
    {
        [Fact(DisplayName = nameof(EventBridgeRuleTest))]
        public async Task EventBridgeRuleTest()
        {
            const string sampleFunctionClassFileName = "EventBridgeRuleFunctions.cs";
            const string generatedSampleEventGenerated = "EventBridgeRuleFunctions_SampleEventHandler_Generated.g.cs";

            var expectedTemplateContent = File.ReadAllText(Path.Combine("Snapshots", "ServerlessTemplates", "eventbridgerule.template")).ToEnvironmentLineEndings();
            var expectedgeneratedSampleEventGenerated = File.ReadAllText(Path.Combine("Snapshots", generatedSampleEventGenerated)).ToEnvironmentLineEndings();

            try
            {
                await new VerifyCS.Test
                {
                    TestState =
                    {
                        Sources =
                        {
                            (Path.Combine("TestServerlessApp", sampleFunctionClassFileName), File.ReadAllText(Path.Combine("TestServerlessApp", sampleFunctionClassFileName))),
                            (Path.Combine("TestServerlessApp", "SampleEvent.cs"), File.ReadAllText(Path.Combine("TestServerlessApp", "SampleEvent.cs"))),
                            (Path.Combine("Amazon.Lambda.Annotations", "LambdaFunctionAttribute.cs"), File.ReadAllText(Path.Combine("Amazon.Lambda.Annotations", "LambdaFunctionAttribute.cs"))),
                            (Path.Combine("Amazon.Lambda.Annotations", "LambdaStartupAttribute.cs"), File.ReadAllText(Path.Combine("Amazon.Lambda.Annotations", "LambdaStartupAttribute.cs"))),
                        },
                        GeneratedSources =
                        {
                            (
                                typeof(SourceGenerator.Generator),
                                generatedSampleEventGenerated,
                                SourceText.From(expectedgeneratedSampleEventGenerated, Encoding.UTF8, SourceHashAlgorithm.Sha256)
                            )
                        },
                        ExpectedDiagnostics =
                        {
                            new DiagnosticResult("AWSLambda0103", DiagnosticSeverity.Info).WithArguments(generatedSampleEventGenerated, expectedgeneratedSampleEventGenerated),
                            new DiagnosticResult("AWSLambda0103", DiagnosticSeverity.Info).WithArguments($"TestServerlessApp{Path.DirectorySeparatorChar}serverless.template", expectedTemplateContent)
                        }
                    }
                }.RunAsync();
            }
            finally
            {
                var actualTemplateContent = File.ReadAllText(Path.Combine("TestServerlessApp", "serverless.template"));
                Assert.Equal(expectedTemplateContent, actualTemplateContent);
            }
        }
    }
}

