namespace MajhiPaithani.Application.Models.Request
{
    public class PlaceOrderRequest
    {
        public int iCustomerId { get; set; }
        public int sellerId { get; set; }
        public string? sFullName { get; set; }
        public string? sPhoneNumber { get; set; }
        public string? sAddress { get; set; }
        public string? sCity { get; set; }
        public string? sPincode { get; set; }
        public string? sState { get; set; }
        public string? sLandmark { get; set; }
        public decimal dcTotalAmount { get; set; }
        public string? sOrderStatus { get; set; }
        public string? sPaymentStatus { get; set; }
    }

    public class OrderItemRequest
    {
        public int iProductId { get; set; }
        public int iQuantity { get; set; }
        public decimal dcPrice { get; set; }
        public decimal dcTotalPrice { get; set; }
        public string? sProductName { get; set; }
        public string? sSku { get; set; }
    }

    public class PlaceOrderRequestWrapper
    {
        public PlaceOrderRequest order { get; set; }
        public List<OrderItemRequest> orderItems { get; set; }
    }

    public class UpdateOrderStatusRequest
    {
        public int iOrderId { get; set; }
        public int iStatusId { get; set; }
        public string? sRemarks { get; set; }
        public int iUpdatedBy { get; set; }
    }

    public class OrderSummaryDto
    {
        public int iOrderId { get; set; }
        public int iCustomerId { get; set; }
        public decimal dcTotalAmount { get; set; }
        public string? sOrderStatus { get; set; }
        public string? sPaymentStatus { get; set; }
        public DateTime dOrderDate { get; set; }
        public DateTime? dDeliveryDate { get; set; }
        public string? sTrackingNumber { get; set; }
        public string? sShiprocketOrderId { get; set; }
        public string? sShipmentId { get; set; }
        public string? sCourierName { get; set; }
        public string? sLabelUrl { get; set; }
        public string? sInvoiceUrl { get; set; }
        public List<OrderItemDto> Items { get; set; } = new();
    }

    public class OrderItemDto
    {
        public int iOrderItemId { get; set; }
        public int iProductId { get; set; }
        public int iSellerId { get; set; }
        public int iQuantity { get; set; }
        public decimal dcPrice { get; set; }
        public decimal dcTotalPrice { get; set; }
    }

    public class OrderStatusHistoryDto
    {
        public int iHistoryId { get; set; }
        public int iOrderId { get; set; }
        public int iStatusId { get; set; }
        public string? sStatusName { get; set; }
        public string? sRemarks { get; set; }
        public int iUpdatedBy { get; set; }
        public DateTime dCreatedDate { get; set; }
    }
}
