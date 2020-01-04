using System;
using System.Runtime.Serialization;

namespace FuckMTP.Core.Contracts
{
    [Serializable]
    public class ExecutionFailedException : Exception
    {
        public ExecutionFailedException(string message) : base(message)
        {
        }

        protected ExecutionFailedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}