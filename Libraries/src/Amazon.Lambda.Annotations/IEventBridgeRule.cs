using System;
using System.Collections.Generic;
using System.Text;

namespace Amazon.Lambda.Annotations
{
    public interface IEventBridgeRule
    {

        /*
         From: https://docs.aws.amazon.com/serverless-application-model/latest/developerguide/sam-property-function-eventbridgerule.html
          DeadLetterConfig: DeadLetterConfig
          EventBusName: String
          Input: String
          InputPath: String
          Pattern: EventPattern
          RetryPolicy: RetryPolicy
          Target: Target
         */
        /// <summary>
        /// Important: Specify as JSON
        /// Describes which events are routed to the specified target. For more information, see Events and Event Patterns in EventBridge in the Amazon EventBridge User Guide.
        /// Type: EventPattern
        /// Required: Yes
        /// AWS CloudFormation compatibility: This property is passed directly to the EventPattern property of an AWS::Events::Rule resource.
        /// </summary>
        string EventPattern { get; set; }
    }
}
