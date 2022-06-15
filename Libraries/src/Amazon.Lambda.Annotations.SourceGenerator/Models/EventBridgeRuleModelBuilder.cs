using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace Amazon.Lambda.Annotations.SourceGenerator.Models
{
    internal class EventBridgeRuleModelBuilder
    {
        public static EventBridgeRuleModel Build(ILambdaFunctionSerializable lambdaFunction, EventBridgeRuleAttribute data)
        {
            return new EventBridgeRuleModel()
            {
                EventPattern = JObject.Parse(data.EventPattern)
            };
        }
    }
}
