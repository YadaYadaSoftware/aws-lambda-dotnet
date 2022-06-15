using System;
using System.Collections.Generic;
using System.Text;
using Amazon.Lambda.Annotations.SourceGenerator.Models;
using Moq;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Amazon.Lambda.Annotations.SourceGenerators.Tests.Models
{
    public class EventBridgeRuleModelBuilderTest
    {
        [Fact]
        public void EventPatternFnSubReplacementTest()
        {
            var eventBridgeRuleAttribute = new EventBridgeRuleAttribute("{ 'detail-type': ['Object Created'], 'detail': { 'bucket': { 'name': ['${Bucket}'] }}, 'source': ['aws.s3'] }");
            var mockLambdaFunction = new Mock<ILambdaFunctionSerializable>();
            var model = EventBridgeRuleModelBuilder.Build(mockLambdaFunction.Object, eventBridgeRuleAttribute);
            JArray name = model.EventPattern["detail"]["bucket"]["name"].Value<JArray>();
            var expectedBucketMacroObject = name.First as JObject;
            var value = expectedBucketMacroObject.Property("Fn::Sub");
            Assert.Equal("${Bucket}", value.Value.ToString());
        }
    }
}
