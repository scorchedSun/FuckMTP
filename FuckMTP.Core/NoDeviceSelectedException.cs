using System;
using System.Runtime.Serialization;

namespace FuckMTP.Core
{
    [Serializable]
    internal class NoDeviceSelectedException : Exception
    {
        public NoDeviceSelectedException()
        {
        }

        protected NoDeviceSelectedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}