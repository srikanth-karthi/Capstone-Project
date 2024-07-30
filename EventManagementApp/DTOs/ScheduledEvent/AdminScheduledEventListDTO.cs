using EventManagementApp.Enums;

namespace EventManagementApp.DTOs.ScheduledEvent
{
    public class AdminScheduledEventListDTO:BasicScheduledEventListDTO
    {

        public string ClientEmail { get; set; }
        public string ClientName { get; set; }


    }
}
