using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.Lambda.Annotations;
using Amazon.Lambda.S3Events;

namespace TestServerlessApp
{
    public class S3Functions
    {
        [LambdaFunction]
        [S3Event]
        public Task BaseS3EventHandler(Amazon.Lambda.S3Events.S3Event.)
    }
}
