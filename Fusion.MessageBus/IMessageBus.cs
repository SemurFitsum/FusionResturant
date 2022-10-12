﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fusion.MessageBus
{
    public interface IMessageBus
    {
        Task PublisheMessage(BaseMessage message, string topicName);
    }
}
