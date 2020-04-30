using System;
using System.Runtime.Serialization;

namespace FuckMTP
{
    [Serializable]
    internal class NoDeviceSelectedException : Exception
    {
        public NoDeviceSelectedException()
        {
        }

        public NoDeviceSelectedException(string message) : base(message)
        {
        }

        public NoDeviceSelectedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NoDeviceSelectedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}