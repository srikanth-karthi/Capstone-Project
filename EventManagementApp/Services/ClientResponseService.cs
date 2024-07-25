using EventManagementApp.DTOs.ClientResponse;
using EventManagementApp.Enums;
using EventManagementApp.Exceptions;
using EventManagementApp.Interfaces.Repository;
using EventManagementApp.Interfaces.Service;
using EventManagementApp.Models;

namespace EventManagementApp.Services
{
    public class ClientResponseService : IClientResponseService
    {
        private readonly IQuotationResponseRepository _quotationResponseRepository;

        public ClientResponseService(IQuotationResponseRepository quotationResponseRepository)
        {
            _quotationResponseRepository = quotationResponseRepository;
        }

        public async Task<ClientResponse> CreateClientResponse(ClientResponseDTO clientResponseDTO, int userId)
        {
            QuotationResponse quotationResponse = await _quotationResponseRepository.GetByIdWithQuotationRequestAndClientResponse(clientResponseDTO.QuotationResponseId, userId);

            if (quotationResponse == null)
            {
                throw new NoQuotationResponseFoundException();
            }

            if (quotationResponse.RequestStatus == RequestStatus.Denied)
            {
                throw new RequestNotApprovedException();
            }

            if (quotationResponse.ClientResponse != null)
            {
                throw new ClientAlreadyRespondedException();
            }


            ClientResponse clientResponse = new ClientResponse();
            clientResponse.ClientDecision = clientResponseDTO.ClientDecision;

            if (clientResponse.ClientDecision == ClientDecision.Accepted)
            {
                Order order = new Order
                {
                    EventCategoryId = quotationResponse.QuotationRequest.EventCategoryId,
                    UserId = quotationResponse.QuotationRequest.UserId,
                    TotalAmount = (double) quotationResponse.QuotedAmount,
                    Currency = (Currency)quotationResponse.Currency,
                    OrderStatus = OrderStatus.Pending

                };
                clientResponse.Order = order;
            }

            quotationResponse.ClientResponse = clientResponse;
            await _quotationResponseRepository.Update(quotationResponse);
            return clientResponse;
        }
    }
}
