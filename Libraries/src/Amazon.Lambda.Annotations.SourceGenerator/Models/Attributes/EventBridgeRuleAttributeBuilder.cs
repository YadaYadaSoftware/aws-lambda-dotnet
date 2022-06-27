using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Newtonsoft.Json.Linq;

namespace Amazon.Lambda.Annotations.SourceGenerator.Models.Attributes
{
    internal class EventBridgeRuleAttributeBuilder
    {
        private const string ValidJsonExceptionMessage = "Must be valid Json";

        public static EventBridgeRuleAttribute Build(AttributeData att)
        {
            var pattern = att.ConstructorArguments.First().Value.ToString();

            var data = new EventBridgeRuleAttribute(pattern);

            foreach (var attNamedArgument in att.NamedArguments)
            {
                switch (attNamedArgument.Key)
                {
                    default:
                        throw new NotSupportedException(attNamedArgument.Key);
                }
            }

            return data;
        }

    }
}
