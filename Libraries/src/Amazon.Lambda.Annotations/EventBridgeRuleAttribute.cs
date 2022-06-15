using System;
using System.Collections.Generic;
using System.Text;

namespace Amazon.Lambda.Annotations
{
    [AttributeUsage(AttributeTargets.Method)]
    public class EventBridgeRuleAttribute : Attribute, IEventBridgeRule
    {
        public string EventPattern { get; set; }
        public string[] EventPatternSources { get; set; } = new string[] { };
    }
}
