using System.Runtime.Serialization;

namespace EventManagementApp.Exceptions
{
    [Serializable]
    internal class NotEnoughTicketsException : Exception
    {
  

        public NotEnoughTicketsException(string? message) : base(message)
        {
        }

        public NotEnoughTicketsException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected NotEnoughTicketsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}