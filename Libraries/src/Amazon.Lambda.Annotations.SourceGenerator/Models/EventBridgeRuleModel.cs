using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace Amazon.Lambda.Annotations.SourceGenerator.Models
{
    public class EventBridgeRuleModel : IEventBridgeRuleSerializable
    {
        public JObject EventPattern { get; set; }
    }
}
