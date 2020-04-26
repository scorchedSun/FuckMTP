using System;
using System.Runtime.Serialization;

namespace FuckMTP.Core.Contracts
{

    [Serializable]
    public class FileSelectionAbortedException : Exception
    {
        public FileSelectionAbortedException()
        {
        }

        protected FileSelectionAbortedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
