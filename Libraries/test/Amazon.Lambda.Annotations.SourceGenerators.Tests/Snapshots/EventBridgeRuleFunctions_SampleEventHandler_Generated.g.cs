using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Amazon.Lambda.Core;

namespace TestServerlessApp
{
    public class EventBridgeRuleFunctions_SampleEventHandler_Generated
    {
        private readonly EventBridgeRuleFunctions eventBridgeRuleFunctions;

        public EventBridgeRuleFunctions_SampleEventHandler_Generated()
        {
            SetExecutionEnvironment();
            eventBridgeRuleFunctions = new EventBridgeRuleFunctions();
        }

        public async System.Threading.Tasks.Task<int> SampleEventHandler(Amazon.Lambda.CloudWatchEvents.CloudWatchEvent<TestServerlessApp.SampleEvent> input, Amazon.Lambda.Core.ILambdaContext __context__)
        {
            return await eventBridgeRuleFunctions.SampleEventHandler(input, __context__);
        }

        private static void SetExecutionEnvironment()
        {
            const string envName = "AWS_EXECUTION_ENV";

            var envValue = new StringBuilder();

            // If there is an existing execution environment variable add the annotations package as a suffix.
            if(!string.IsNullOrEmpty(Environment.GetEnvironmentVariable(envName)))
            {
                envValue.Append($"{Environment.GetEnvironmentVariable(envName)}_");
            }

            envValue.Append("amazon-lambda-annotations_0.6.0.0");

            Environment.SetEnvironmentVariable(envName, envValue.ToString());
        }
    }
}