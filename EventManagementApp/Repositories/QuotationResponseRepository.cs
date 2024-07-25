using EventManagementApp.Context;
using EventManagementApp.Interfaces.Repository;
using EventManagementApp.Models;
using Microsoft.EntityFrameworkCore;

namespace EventManagementApp.Repositories
{
    public class QuotationResponseRepository : Repository<QuotationResponse, int>, IQuotationResponseRepository
    {
        public QuotationResponseRepository(EventManagementDBContext _context) : base(_context)
        {
        }

        public async Task<QuotationResponse> GetByIdWithQuotationRequestAndClientResponse(int id, int userId)
        {
            QuotationResponse? quotationResponse = await _context.QuotationResponses
                .Include(q => q.QuotationRequest)
                .Include(q => q.ClientResponse)
                .FirstOrDefaultAsync(q => q.QuotationResponseId == id && q.QuotationRequest.UserId == userId);
            return quotationResponse;
        }
    }
}
