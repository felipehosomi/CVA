using System;
using System.Runtime.Serialization;

namespace CVA_Rep_Exception
{
    [Serializable]
    public class GerenciadorException : Exception
    {
        public GerenciadorException() { }

        public GerenciadorException(string message) : base(message) { }

        public GerenciadorException(string message, Exception innerException) : base(message, innerException) { }

        protected GerenciadorException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}