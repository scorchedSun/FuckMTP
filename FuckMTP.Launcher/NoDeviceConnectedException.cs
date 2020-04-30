using System;
using System.Runtime.Serialization;

namespace FuckMTP
{
    [Serializable]
    internal class NoDeviceConnectedException : Exception
    {
        public NoDeviceConnectedException()
        {
        }

        public NoDeviceConnectedException(string message) : base(message)
        {
        }

        public NoDeviceConnectedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NoDeviceConnectedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}