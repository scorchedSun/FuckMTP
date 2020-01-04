using System;
using System.Runtime.Serialization;

namespace FuckMTP.Core
{
    [Serializable]
    internal class NoDeviceConnectedException : Exception
    {
        public NoDeviceConnectedException()
        {
        }

        protected NoDeviceConnectedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}