using System;
using System.Collections.Generic;
using System.Text;

namespace Amazon.Lambda.Annotations
{
    public interface IS3Event
    {
        /*

        AWS::Serverless::Function - Event Data For S3

            Bucket: String
            Events: String | List
            Filter: NotificationFilter

         */

        string EventBucket { get; set; }
        string[] EventEvents { get; set; }
        string[] EventFilterRules { get; set; }

    }
    public interface IS3EventSerializable : IS3Event
    {

    }
}
