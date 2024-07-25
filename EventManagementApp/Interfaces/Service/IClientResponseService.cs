using EventManagementApp.DTOs.ClientResponse;
using EventManagementApp.Exceptions;
using EventManagementApp.Models;

namespace EventManagementApp.Interfaces.Service
{
    public interface IClientResponseService
    {
        /// <exception cref="RequestNotApprovedException"></exception>
        /// <exception cref="NoQuotationResponseFoundException"></exception>
        /// <exception cref="ClientAlreadyRespondedException"></exception>
        public Task<ClientResponse> CreateClientResponse(ClientResponseDTO clientResponseDTO, int userId);

    }
}
