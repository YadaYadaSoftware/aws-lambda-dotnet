﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Amazon.Lambda.Annotations.SourceGenerator.Models
{
    public interface IQueueSerializable
    {
        string Name { get; set; }
        string QueueName { get; set; }
    }
}
