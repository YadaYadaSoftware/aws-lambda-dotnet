using System;
using System.Collections.Generic;
using System.Text;

namespace Amazon.Lambda.Annotations.SourceGenerator.Models
{
    public class EventBridgeRuleModel : IEventBridgeRuleSerializable
    {
        public string EventPattern { get; set; }
        public string[] EventPatternSources { get; set; }
    }
}
