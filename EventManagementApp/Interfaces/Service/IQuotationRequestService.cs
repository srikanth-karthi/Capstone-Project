using EventManagementApp.DTOs.QuotationRequest;
using EventManagementApp.Exceptions;
namespace EventManagementApp.Interfaces.Service
{
    public interface IQuotationRequestService
    {
        /// <exception cref="NoEventCategoryFoundException" />
        /// <exception cref="EventInActiveException" />
        public Task<int> CreateQuotationRequest(int UserId, CreateQuotationRequestDTO quotationRequestDTO);
    }
}
