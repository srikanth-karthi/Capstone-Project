
using EventManagementApp.DTOs.ClientResponse;
using EventManagementApp.Enums;
using EventManagementApp.Exceptions;
using EventManagementApp.Interfaces.Repository;
using EventManagementApp.Interfaces.Service;
using EventManagementApp.Models;
using EventManagementApp.Repositories;
using EventManagementApp.Services;

namespace EventManagementTest
{
    class ClientResponseTest
    {

        private TestDBContext _context;
        private IClientResponseService _clientResponseService;

        [SetUp]
        public void Setup()
        {

            _context = new TestDBContext(TestDBContext.GetInMemoryOptions());
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
            IQuotationResponseRepository _quotationRequestRepo = new QuotationResponseRepository(_context);

            _clientResponseService = new ClientResponseService(_quotationRequestRepo);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public async Task CreateClientResponse()
        {
            // Arrange 
            var clientResponseDTO = new ClientResponseDTO
            {
                ClientDecision = ClientDecision.Accepted,
                QuotationResponseId = 1
            };

            // Act
            ClientResponse response = await _clientResponseService.CreateClientResponse(clientResponseDTO, 2);

            // Assert
            Assert.That(response.ClientDecision, Is.EqualTo(ClientDecision.Accepted));

        }


        [Test]
        public void CreateClientResponseFail1()
        {
            // Arrange 
            var clientResponseDTO = new ClientResponseDTO
            {
                ClientDecision = ClientDecision.Accepted,
                QuotationResponseId = 2
            };

            // Assert
            Assert.ThrowsAsync<RequestNotApprovedException>(async () => {
                ClientResponse response = await _clientResponseService.CreateClientResponse(clientResponseDTO, 2);
            });
        }


        [Test]
        public void CreateClientResponseFail2()
        {
            // Arrange 
            var clientResponseDTO = new ClientResponseDTO
            {
                ClientDecision = ClientDecision.Accepted,
                QuotationResponseId = 9999
            };

            // Assert
            Assert.ThrowsAsync<NoQuotationResponseFoundException>(async () => {
                ClientResponse response = await _clientResponseService.CreateClientResponse(clientResponseDTO, 2);
            });
        }

        [Test]
        public void CreateClientResponseFail3()
        {
            // Arrange 
            var clientResponseDTO = new ClientResponseDTO
            {
                ClientDecision = ClientDecision.Accepted,
                QuotationResponseId = 3
            };

            // Assert
            Assert.ThrowsAsync<ClientAlreadyRespondedException>(async () => {
                ClientResponse response = await _clientResponseService.CreateClientResponse(clientResponseDTO, 2);
            });
        }

    }
}