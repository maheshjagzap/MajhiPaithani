using MajhiPaithani.Application.DataAccess;
using System.Data;
using System.Threading.Tasks;

namespace MajhiPaithani.Application.Services
{
    public class GetSellerDashboardService
    {
        private readonly GetSellerDashboardDataAccess _dataAccess;

        public GetSellerDashboardService(GetSellerDashboardDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public async Task<DataSet> GetSellerDashboardAsync(int RequestedFor, int TaskId,int sellerId,int UserId)
        {
            return await _dataAccess.GetSellerDashboardAsync(RequestedFor,TaskId, sellerId,UserId);
        }
    }
}