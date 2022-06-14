using System;
using System.Collections.Generic;
using System.Text;

namespace Amazon.Lambda.Annotations
{
    public class S3EventAttribute : Attribute, IS3Event
    {
        public string EventBucket { get; set; }
        public string[] EventEvents { get; set; }
        public string[] EventFilterRules { get; set; }
    }
}
