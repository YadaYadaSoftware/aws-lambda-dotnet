﻿using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Amazon.Lambda.Annotations;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;

namespace TestServerlessApp
{
    public class Messaging
    {
        /// <summary>
        /// This function will use a preexisting queue, whether the queue is defined manually in the template or external to the template.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        [LambdaFunction]
        [SqsMessage(EventQueueARN = "arn:aws:sqs:us-east-1:968993296699:app-deploy-blue-LAVETRYB3JKX-SomeQueueName", EventBatchSize = 11)]
        public async Task<int> MessageHandlerForPreExistingQueue(SQSEvent.SQSMessage message, ILambdaContext context)
        {
            LambdaLogger.Log($"Message Received: {message.MessageId}");
            // the Task<int> is a hack to work with the current generator
            return 0;
        }

        /// <summary>
        /// This function will add the AWS::SQS::Queue to the template and use it for the function.
        /// All Serverless Event attributes start with the word Event.
        /// All others are the AWS::SQS::Queue properties.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        [LambdaFunction]
        [SqsMessage(
            EventBatchSize = 12,
            EventFilterCriteria = new string[] { "Filter1", "Filter2" },
            EventMaximumBatchingWindowInSeconds = 31,
            VisibilityTimeout = 100, 
            ContentBasedDeduplication = true, 
            DeduplicationScope = "queue", 
            DelaySeconds = 5, 
            FifoQueue = true,
            FifoThroughputLimit = "perQueue",
            KmsDataKeyReusePeriodSeconds = 299,
            KmsMasterKeyId = "alias/aws/sqs",
            MaximumMessageSize = 1024,
            MessageRetentionPeriod = 60,
            QueueName = "thisismyqueuename.fifo",
            ReceiveMessageWaitTimeSeconds =5,
            RedriveAllowPolicy = "{ 'redrivePermission' : 'denyAll' }",
            RedrivePolicy = "{ 'deadLetterTargetArn': 'arn:somewhere', 'maxReceiveCount': 5 }",
            Tags = new string[]{ "keyname1=value1", "keyname2=value2" })]
        public async Task<int> MessageHandlerForNewQueue(SQSEvent.SQSMessage message, ILambdaContext context)
        {
            LambdaLogger.Log($"Message Received: {message.MessageId}");
            // the Task<int> is a hack to work with the current generator
            return 0;
        }

        [LambdaFunction]
        [SqsMessage(
            FifoQueue = true,
            QueueName = "{ 'Fn::Sub' : '${AWS::Stack}MyFifoQueueWithStackEmbedded.fifo' }")]
        public async Task<int> MessageHandlerForNewFifoQueueUsingFnSubForQueueName(SQSEvent.SQSMessage message, ILambdaContext context)
        {
            LambdaLogger.Log($"Message Received: {message.MessageId}");
            // the Task<int> is a hack to work with the current generator
            return 0;
        }

        /// <summary>
        /// This demonstrates (and tests) the ability to create
        /// a Fifo queue without specifying a QueueName
        /// </summary>
        /// <param name="message"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        [LambdaFunction]
        [SqsMessage(FifoQueue = true)]
        public async Task<int> MessageHandlerForNewFifoQueueWithoutAQueueName(SQSEvent.SQSMessage message, ILambdaContext context)
        {
            LambdaLogger.Log($"Message Received: {message.MessageId}");
            // the Task<int> is a hack to work with the current generator
            return 0;
        }

    }
}