using EventManagementApp.DTOs.EventCategory;
using EventManagementApp.DTOs.QuotationRequest;
using EventManagementApp.DTOs.ScheduledEvent;
using EventManagementApp.Exceptions;
using EventManagementApp.Models;

namespace EventManagementApp.Interfaces.Service
{
    public interface IAdminService
    {
        public Task<EventCategory> CreateEventCategory(CreateEventCategoryDTO eventCategoryDTO);
        public Task<List<AdminScheduledEventListDTO>> GetScheduledEvents();

        public Task<EventCategory> UpdateEventDetails(int id, UpdateEventCategoryDTO updateEventCategoryDTO);
        public Task<List<AdminBaseEventCategoryDTO>> GetAllEventCategories();
        Task<List<BasicQuotationRequestDTO>> GetQuotations(bool isNew);

        public Task<BasicQuotationRequestDTO> GetQuotationsByid(int id);
    }
}
