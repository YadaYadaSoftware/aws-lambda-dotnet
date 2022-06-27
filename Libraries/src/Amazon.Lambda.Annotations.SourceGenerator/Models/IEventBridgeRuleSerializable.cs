using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace Amazon.Lambda.Annotations.SourceGenerator.Models
{
    public interface IEventBridgeRuleSerializable
    {
        JObject EventPattern { get; set; }
    }
}
