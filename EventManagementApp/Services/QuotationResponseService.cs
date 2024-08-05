using EventManagementApp.Context;
using EventManagementApp.DTOs.QuotationRequest;
using EventManagementApp.Enums;
using EventManagementApp.Exceptions;
using EventManagementApp.Interfaces.Repository;
using EventManagementApp.Interfaces.Service;
using EventManagementApp.Models;
using EventManagementApp.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace EventManagementApp.Services
{
    public class QuotationResponseService: IQuotationResponseService
    {
        private readonly IQuotationRequestRepository _quotationRequestRepository;
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;

        public QuotationResponseService(IQuotationRequestRepository quotationRequestRepository,
            INotificationService notificationService,
            EventManagementDBContext context,
            IUserRepository userRepository,
            IEmailService emailService

            )
        {
            _quotationRequestRepository = quotationRequestRepository;
            _userRepository = userRepository;
            _emailService = emailService;
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

            if (createQuotationResponseDTO.RequestStatus == RequestStatus.Accepted)
            {
                if (createQuotationResponseDTO.QuotedAmount == null)
                {
                    throw new AmountNullException();
                }

                if (createQuotationResponseDTO.Currency == null)
                {
                    throw new CurrencyNullException();
                }
            }

            if (createQuotationResponseDTO.RequestStatus == RequestStatus.Denied)
            {
                createQuotationResponseDTO.Currency = null;
                createQuotationResponseDTO.QuotedAmount = null;
            }

            QuotationResponse quotationResponse = new QuotationResponse
            {
                QuotationRequestId = (int)createQuotationResponseDTO.QuotationRequestId,
                QuotedAmount = createQuotationResponseDTO.QuotedAmount,
                Currency = createQuotationResponseDTO.Currency,
                ResponseMessage = createQuotationResponseDTO.ResponseMessage,
                RequestStatus = createQuotationResponseDTO.RequestStatus
            };

            quotationRequest.QuotationStatus = QuotationStatus.Responded;
            quotationRequest.QuotationResponse = quotationResponse;

            await _quotationRequestRepository.Update(quotationRequest);

            User user = await _userRepository.GetById(quotationRequest.UserId);
            string emailSubject = "Quotation Response Update";

            string emailBody;

            if (createQuotationResponseDTO.RequestStatus == RequestStatus.Accepted)
            {
                emailBody = $@"
<!DOCTYPE html>
<html>
<head>
    <style>
        body {{ font-family: Arial, sans-serif; color: #333; }}
        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; border: 1px solid #ddd; border-radius: 5px; }}
        h1 {{ color: #0056b3; }}
        .details {{ margin-top: 20px; }}
        .details p {{ margin: 5px 0; }}
        .footer {{ margin-top: 20px; font-size: 0.9em; color: #666; }}
    </style>
</head>
<body>
    <div class='container'>
        <h1>Quotation Accepted</h1>
        <p>Dear {user.FullName},</p>
        <p>We are pleased to inform you that your quotation request has been accepted.</p>
        <div class='details'>
            <p><strong>Quotation Request ID:</strong> {quotationRequest.QuotationRequestId}</p>
            <p><strong>Quoted Amount:</strong> {createQuotationResponseDTO.QuotedAmount} INR</p>
            <p><strong>Response Message:</strong> {createQuotationResponseDTO.ResponseMessage}</p>
            <p><strong>Status:</strong> Accepted</p>
        </div>
        <p>If you have any questions or need further assistance, please feel free to reach out to us.</p>
        <p>Thank you for your patience.</p>
        <div class='footer'>
            <p>Best regards,<br>Eventify<br>Admin@eventify.com</p>
        </div>
    </div>
</body>
</html>
";
            }
            else if (createQuotationResponseDTO.RequestStatus == RequestStatus.Denied)
            {
                emailBody = $@"
<!DOCTYPE html>
<html>
<head>
    <style>
        body {{ font-family: Arial, sans-serif; color: #333; }}
        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; border: 1px solid #ddd; border-radius: 5px; }}
        h1 {{ color: #d9534f; }}
        .details {{ margin-top: 20px; }}
        .details p {{ margin: 5px 0; }}
        .footer {{ margin-top: 20px; font-size: 0.9em; color: #666; }}
    </style>
</head>
<body>
    <div class='container'>
        <h1>Quotation Declined</h1>
        <p>Dear {user.FullName},</p>
        <p>We regret to inform you that your quotation request has been declined.</p>
        <div class='details'>
            <p><strong>Quotation Request ID:</strong> {quotationRequest.QuotationRequestId}</p>
            <p><strong>Response Message:</strong> {createQuotationResponseDTO.ResponseMessage}</p>
            <p><strong>Status:</strong> Declined</p>
        </div>
        <p>If you have any questions or need further clarification, please do not hesitate to contact us.</p>
        <p>Thank you for your understanding.</p>
        <div class='footer'>
            <p>Best regards,<br>Eventify<br>Admin@eventify.com</p>
        </div>
    </div>
</body>
</html>
";
            }
            else
            {
                emailBody = $@"
<!DOCTYPE html>
<html>
<head>
    <style>
        body {{ font-family: Arial, sans-serif; color: #333; }}
        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; border: 1px solid #ddd; border-radius: 5px; }}
        h1 {{ color: #6c757d; }}
        .details {{ margin-top: 20px; }}
        .details p {{ margin: 5px 0; }}
        .footer {{ margin-top: 20px; font-size: 0.9em; color: #666; }}
    </style>
</head>
<body>
    <div class='container'>
        <h1>Quotation Status Update</h1>
        <p>Dear {user.FullName},</p>
        <p>Your quotation request has been updated with the following status:</p>
        <div class='details'>
            <p><strong>Quotation Request ID:</strong> {quotationRequest.QuotationRequestId}</p>
            <p><strong>Response Message:</strong> {createQuotationResponseDTO.ResponseMessage}</p>
            <p><strong>Status:</strong> {createQuotationResponseDTO.RequestStatus}</p>
        </div>
        <p>If you have any questions or need further assistance, please feel free to reach out to us.</p>
        <div class='footer'>
            <p>Best regards,<br>Eventify<br>Admin@eventify.com</p>
        </div>
    </div>
</body>
</html>
";
            }

            await _emailService.SendEmail(user.Email, emailSubject, emailBody);

            return quotationResponse.QuotationResponseId;

        }

    }
}
