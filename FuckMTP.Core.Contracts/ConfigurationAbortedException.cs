using System;
using System.Runtime.Serialization;

namespace FuckMTP.Core
{
    [Serializable]
    public class ConfigurationAbortedException : Exception
    {
        public ConfigurationAbortedException()
        {
        }

        protected ConfigurationAbortedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
