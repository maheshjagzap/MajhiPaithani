using MajhiPaithani.Application.Models.Request;
using MajhiPaithani.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace MajhiPaithani.API.Endpoint
{
    public class OrderEndpoint
    {
        public static void Map(WebApplication app)
        {
            var order = app.MapGroup("/api/Order");

            order.MapPost("/placeorder", async (
                [FromBody] PlaceOrderRequestWrapper req,
                OrderService service) =>
            {
                try
                {
                    if (req?.order == null || req.orderItems == null || req.orderItems.Count == 0)
                        return Results.BadRequest(new { StatusCode = 400, Message = "Invalid request. Order and order items are required." });

                    var (orderId, message) = await service.PlaceOrderAsync(req);

                    return orderId > 0
                        ? Results.Ok(new { StatusCode = 200, Message = message, OrderId = orderId })
                        : Results.BadRequest(new { StatusCode = 400, Message = message });
                }
                catch (Exception ex)
                {
                    return Results.Problem(detail: $"Error: {ex.Message}");
                }
            })
            .WithName("PlaceOrder")
            .WithTags("Orders");

            order.MapPut("/Orderstatus", async (
                [FromBody] UpdateOrderStatusRequest req,
                OrderService service) =>
            {
                try
                {
                    if (req == null || req.iOrderId <= 0)
                        return Results.BadRequest(new { StatusCode = 400, Message = "Invalid request. OrderId is required." });

                    var message = await service.UpdateOrderStatusAsync(req);

                    return Results.Ok(new { StatusCode = 200, Message = message, OrderId = req.iOrderId });
                }
                catch (Exception ex)
                {
                    return Results.Problem(detail: $"Error: {ex.Message}");
                }
            })
            .WithName("UpdateOrderStatus")
            .WithTags("Orders");

            order.MapGet("/orderlist", async (int? customerId, int? sellerId, OrderService service) =>
            {
                try
                {
                    if (customerId == null && sellerId == null)
                        return Results.BadRequest(new { StatusCode = 400, Message = "At least one of customerId or sellerId is required." });

                    var data = await service.GetOrdersAsync(customerId, sellerId);

                    return Results.Ok(new { StatusCode = 200, Message = "Orders fetched successfully.", Orders = data });
                }
                catch (Exception ex)
                {
                    return Results.Problem(detail: $"Error: {ex.Message}");
                }
            })
            .WithName("GetOrderslist")
            .WithTags("Orders");

            order.MapGet("/history/{orderId}", async (int orderId, OrderService service) =>
            {
                try
                {
                    if (orderId <= 0)
                        return Results.BadRequest(new { StatusCode = 400, Message = "Invalid OrderId." });

                    var data = await service.GetOrderHistoryAsync(orderId);

                    return Results.Ok(new { StatusCode = 200, Message = "Order history fetched successfully.", OrderId = orderId, History = data });
                }
                catch (Exception ex)
                {
                    return Results.Problem(detail: $"Error: {ex.Message}");
                }
            })
            .WithName("GetOrderHistory")
            .WithTags("Orders");

            order.MapGet("/track/{awb}", async (string awb, OrderService service) =>
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(awb))
                        return Results.BadRequest(new { StatusCode = 400, Message = "AWB tracking number is required." });

                    var data = await service.TrackShipmentAsync(awb);

                    return Results.Ok(new { StatusCode = 200, Message = "Tracking data fetched successfully.", TrackingData = data });
                }
                catch (Exception ex)
                {
                    return Results.Problem(detail: $"Error: {ex.Message}");
                }
            })
            .WithName("TrackShipment")
            .WithTags("Orders");
        }
    }
}
