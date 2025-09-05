using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace B1.WFN.API.Infrastructure
{

    [Serializable]
    public class DIAPIException : Exception
    {
        public readonly int Code;
        public DIAPIException() { }
        public DIAPIException(int code, string message) : base($"{code} - {message}") {
            this.Code = code;
        }
        public DIAPIException(string message) : base(message) { }
        public DIAPIException(string message, Exception inner) : base(message, inner) { }
        protected DIAPIException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
