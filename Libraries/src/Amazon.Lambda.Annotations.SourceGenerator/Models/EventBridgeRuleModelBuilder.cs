using System;
using System.Collections.Generic;
using System.Text;

namespace Amazon.Lambda.Annotations.SourceGenerator.Models
{
    internal class EventBridgeRuleModelBuilder
    {
        public static EventBridgeRuleModel Build(ILambdaFunctionSerializable lambdaFunction, EventBridgeRuleAttribute data)
        {
            return new EventBridgeRuleModel()
            {
                EventPattern = data.EventPattern
            };
        }
    }
}
