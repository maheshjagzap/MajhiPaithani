using MajhiPaithani.Application.DataAccess;
using MajhiPaithani.Application.Models.Request;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MajhiPaithani.Application.Services
{
    public class OrderService
    {
        private readonly OrderDataAccess _dataAccess;

        public OrderService(OrderDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public Task<(int orderId, string message)> PlaceOrderAsync(PlaceOrderRequestWrapper req)
            => _dataAccess.PlaceOrderAsync(req);

        public Task<string> UpdateOrderStatusAsync(UpdateOrderStatusRequest req)
            => _dataAccess.UpdateOrderStatusAsync(req);

        public Task<List<OrderSummaryDto>> GetOrdersAsync(int? customerId, int? sellerId)
            => _dataAccess.GetOrdersAsync(customerId, sellerId);

        public Task<List<OrderStatusHistoryDto>> GetOrderHistoryAsync(int orderId)
            => _dataAccess.GetOrderHistoryAsync(orderId);

        public Task<object> TrackShipmentAsync(string awb)
            => _dataAccess.TrackShipmentAsync(awb);
    }
}
