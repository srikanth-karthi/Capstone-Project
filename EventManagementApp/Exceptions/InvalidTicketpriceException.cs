using System.Runtime.Serialization;

namespace EventManagementApp.Exceptions
{
    [Serializable]
    internal class InvalidTicketpriceException : Exception
    {
        public InvalidTicketpriceException()
        {
        }

        public InvalidTicketpriceException(string? message) : base(message)
        {
        }

        public InvalidTicketpriceException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected InvalidTicketpriceException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}