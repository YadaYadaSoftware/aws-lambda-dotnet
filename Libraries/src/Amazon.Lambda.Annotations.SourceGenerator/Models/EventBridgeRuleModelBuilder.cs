using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;

namespace Amazon.Lambda.Annotations.SourceGenerator.Models
{
    public class EventBridgeRuleModelBuilder
    {
        public static EventBridgeRuleModel Build(ILambdaFunctionSerializable lambdaFunction, EventBridgeRuleAttribute data)
        {
            var pattern = JObject.Parse(data.EventPattern); 
            UpdatePatternWithFnSubs(pattern);

            return new EventBridgeRuleModel()
            {
                EventPattern = pattern
            };
        }
        //private static JObject UpdatePatternWithFnSubs(JObject pattern)
        //{
        //    switch (pattern.Type)
        //    {
        //        case JTokenType.Object:
        //            foreach (JProperty jProperty in pattern.Properties())
        //            {
        //                UpdatePatternWithFnSubs(jProperty);
        //            }
        //        case JTokenType.Array:
        //            foreach (var jArray in pattern.Values<JArray>())
        //            {
                        
        //            }
        //    }
        //    return pattern;
        //}

        static void UpdatePatternWithFnSubs(JToken node)
        {
            Debug.WriteLine(node.Path);
            if (node.Type == JTokenType.Object)
            {
                foreach (JProperty child in node.Children<JProperty>().ToList())
                {
                    UpdatePatternWithFnSubs(child.Value);
                }
            }
            else if (node.Type == JTokenType.Array)
            {
                foreach (JToken child in node.Children().ToList())
                {
                    UpdatePatternWithFnSubs(child);
                }
            }
            else if (node.Type == JTokenType.String)
            {
                if (node.Value<string>().Contains("${"))
                {
                    node.Replace(new JObject(new JProperty("Fn::Sub", node.Value<string>())));
                    //node.Parent.Add(new JObject(new JProperty("Fn::Sub", node.Value<string>())));
                    //node.Parent.Parent.Remove();
                }
            }
            
        }

    }
}
