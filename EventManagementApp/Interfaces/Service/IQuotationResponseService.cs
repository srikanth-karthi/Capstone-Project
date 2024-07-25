using EventManagementApp.DTOs.QuotationRequest;

namespace EventManagementApp.Interfaces.Service
{
    public interface IQuotationResponseService
    {
        /// <exception cref="NoQuotationRequestFoundException"></exception>
        /// <exception cref="QuotationAlreadyRespondedException"></exception>
        /// <exception cref="AmountNullException"></exception>
        public Task<int> CreateQuotationResponse(CreateQuotationResponseDTO createQuotationResponseDTO);
    }
}
