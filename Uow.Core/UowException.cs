using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Uow.Core
{
    /// <summary>
    /// Base exception type for those are thrown by WebPay system for WebPay specific exceptions.
    /// </summary>
    [Serializable]
    public class UowException : Exception
    {
        /// <summary>
        /// Creates a new <see cref="WebPayException"/> object.
        /// </summary>
        public UowException()
        {

        }

        /// <summary>
        /// Creates a new <see cref="WebPayException"/> object.
        /// </summary>
        public UowException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context)
        {

        }

        /// <summary>
        /// Creates a new <see cref="WebPayException"/> object.
        /// </summary>
        /// <param name="message">Exception message</param>
        public UowException(string message)
            : base(message)
        {

        }

        /// <summary>
        /// Creates a new <see cref="WebPayException"/> object.
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="innerException">Inner exception</param>
        public UowException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
