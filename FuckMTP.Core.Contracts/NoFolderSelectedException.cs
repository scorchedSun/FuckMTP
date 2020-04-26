using System;
using System.Runtime.Serialization;

namespace FuckMTP.Core.Contracts
{
    [Serializable]
    public class NoFolderSelectedException : Exception
    {
        public NoFolderSelectedException()
        {
        }

        protected NoFolderSelectedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
