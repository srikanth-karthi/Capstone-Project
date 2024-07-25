using System.Runtime.Serialization;

namespace EventManagementApp.Exceptions
{
    [Serializable]
    internal class EventNotFoundExceptions : Exception
    {

        public int EventId { get; }

        public EventNotFoundExceptions()
        {
        }


        public EventNotFoundExceptions(int eventId)
            : base($"Event with ID {eventId} was not found.")
        {
            EventId = eventId;
        }
        public EventNotFoundExceptions(string? message) : base(message)
        {
        }

        public EventNotFoundExceptions(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected EventNotFoundExceptions(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}