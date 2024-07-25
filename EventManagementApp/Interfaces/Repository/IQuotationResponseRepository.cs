using EventManagementApp.Models;

namespace EventManagementApp.Interfaces.Repository
{
    public interface IQuotationResponseRepository: IRepository<QuotationResponse, int>
    {
        public Task<QuotationResponse> GetByIdWithQuotationRequestAndClientResponse(int id, int userId);
    }
}
