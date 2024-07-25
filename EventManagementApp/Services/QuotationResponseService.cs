using EventManagementApp.Context;
using EventManagementApp.DTOs.QuotationRequest;
using EventManagementApp.Enums;
using EventManagementApp.Exceptions;
using EventManagementApp.Interfaces.Repository;
using EventManagementApp.Interfaces.Service;
using EventManagementApp.Models;
using EventManagementApp.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EventManagementApp.Services
{
    public class QuotationResponseService: IQuotationResponseService
    {
        private readonly IQuotationRequestRepository _quotationRequestRepository;


        public QuotationResponseService(IQuotationRequestRepository quotationRequestRepository,
            INotificationService notificationService,
            EventManagementDBContext context

            )
        {
            _quotationRequestRepository = quotationRequestRepository;

        }

        public async Task<int> CreateQuotationResponse(CreateQuotationResponseDTO createQuotationResponseDTO)
        {

            QuotationRequest quotationRequest = await _quotationRequestRepository.GetById((int)createQuotationResponseDTO.QuotationRequestId);

            if (quotationRequest == null)
            {
                throw new NoQuotationRequestFoundException();
            }

            if (quotationRequest.QuotationStatus == QuotationStatus.Responded)
            {
                throw new QuotationAlreadyRespondedException();
            }

            if (createQuotationResponseDTO.RequestStatus == RequestStatus.Accepted 
                && createQuotationResponseDTO.QuotedAmount == null)
            {
                throw new AmountNullException();
            }

            if (createQuotationResponseDTO.RequestStatus == RequestStatus.Accepted
                && createQuotationResponseDTO.Currency == null)
            {
                throw new CurrencyNullException();
            }

            if (createQuotationResponseDTO.RequestStatus == RequestStatus.Denied)
            {
                createQuotationResponseDTO.Currency = null;
                createQuotationResponseDTO.QuotedAmount = null;
            }


            QuotationResponse quotationResponse = new QuotationResponse();
            quotationResponse.QuotationRequestId = (int)createQuotationResponseDTO.QuotationRequestId;
            quotationResponse.QuotedAmount = createQuotationResponseDTO.QuotedAmount;
            quotationResponse.Currency = createQuotationResponseDTO.Currency;
            quotationResponse.ResponseMessage = createQuotationResponseDTO.ResponseMessage;
            quotationResponse.RequestStatus = createQuotationResponseDTO.RequestStatus;

            quotationRequest.QuotationStatus = QuotationStatus.Responded;
            quotationRequest.QuotationResponse = quotationResponse;


                    await _quotationRequestRepository.Update(quotationRequest);


                    return quotationResponse.QuotationResponseId;

            
        }
    }
}
