using System;
using System.Runtime.Serialization;

namespace CVA_Rep_Logging
{
    [Serializable]
    public class SequencerException : Exception
    {
        public SequencerException()
        {
        }

        public SequencerException(string message) : base(message)
        {
        }

        public SequencerException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public SequencerException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}