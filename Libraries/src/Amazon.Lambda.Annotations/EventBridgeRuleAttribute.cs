using System;
using System.Collections.Generic;
using System.Text;

namespace Amazon.Lambda.Annotations
{
    [AttributeUsage(AttributeTargets.Method)]
    public class EventBridgeRuleAttribute : Attribute, IEventBridgeRule
    {
        public EventBridgeRuleAttribute(string eventPattern)
        {
            this.EventPattern = eventPattern;
        }
        public string EventPattern { get; set; }
    }
}
