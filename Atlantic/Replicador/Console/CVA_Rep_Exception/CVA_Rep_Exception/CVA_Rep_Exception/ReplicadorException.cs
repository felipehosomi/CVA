using System;
using System.Runtime.Serialization;

namespace CVA_Rep_Exception
{
    [Serializable]
    public class ReplicadorException : Exception
    {
        public readonly int? BASE;
        public readonly int? REGISTRO;

        public ReplicadorException() { }

        public ReplicadorException(string message) : base(message)
        {
            BASE = null;
            REGISTRO = null;
        }

        public ReplicadorException(string message, Exception innerException) : base(message, innerException)
        {
            BASE = null;
            REGISTRO = null;
        }

        protected ReplicadorException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            BASE = null;
            REGISTRO = null;
        }

        public ReplicadorException(string message, int _base, int _registro) : base(message)
        {
            BASE = _base;
            REGISTRO = _registro;
        }

        public ReplicadorException(string message, int _base, int _registro, Exception innerException) : base(message, innerException)
        {
            BASE = _base;
            REGISTRO = _registro;
        }

        protected ReplicadorException(SerializationInfo info, StreamingContext context, int _base, int _registro) : base(info, context)
        {
            BASE = _base;
            REGISTRO = _registro;
        }
    }
}