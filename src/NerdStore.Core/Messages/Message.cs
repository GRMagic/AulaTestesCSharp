using System;
using System.Collections.Generic;
using System.Text;

namespace NerdStore.Core.Messages
{
    public abstract class Message
    {
        public string Type { get; protected set; }
        public Guid AggregateId { get; protected set; }
        protected Message()
        {
            Type = GetType().Name;
        }
    }
}
