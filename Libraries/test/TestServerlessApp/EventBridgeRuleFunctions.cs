using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.Lambda.Annotations;
using Amazon.Lambda.CloudWatchEvents;
using Amazon.Lambda.Core;

namespace TestServerlessApp
{
    public class EventBridgeRuleFunctions
    {

        [LambdaFunction]
        [EventBridgeRule("{'source': ['aws.ec2', 'aws.fargate']}")]
        public Task SampleEventHandler(CloudWatchEvent<SampleEvent> input, ILambdaContext context)
        {
            return Task.CompletedTask;
        }
    }
}
