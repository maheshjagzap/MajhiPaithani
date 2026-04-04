using MajhiPaithani.Application.Models.Request;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MajhiPaithani.Application.DataAccess
{
    public class OrderDataAccess
    {
        private readonly string _connectionString;
        private readonly IConfiguration _config;
        private readonly IHttpClientFactory _httpClientFactory;

        public OrderDataAccess(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _config = configuration;
            _httpClientFactory = httpClientFactory;
        }

        // ── 1. Place Order ────────────────────────────────────────────────────────
        public async Task<(int orderId, string message)> PlaceOrderAsync(PlaceOrderRequestWrapper wrapper)
        {
            int orderId = 0;
            string message = "";
            var req = wrapper.order;
            var items = wrapper.orderItems;

            try
            {
                using var conn = new SqlConnection(_connectionString);
                await conn.OpenAsync();

                var sellerAddr = await GetSellerAddressAsync(conn, req.sellerId);

                var orderSql = @"
                    INSERT INTO Orders (iCustomerId, sellerId, sFullName, sPhoneNumber, sAddress, sCity, sPincode, sState, sLandmark, dcTotalAmount, sOrderStatus, sPaymentStatus, dOrderDate, dCreatedDate)
                    OUTPUT INSERTED.iOrderId
                    VALUES (@iCustomerId, @sellerId, @sFullName, @sPhoneNumber, @sAddress, @sCity, @sPincode, @sState, @sLandmark, @dcTotalAmount, @sOrderStatus, @sPaymentStatus, GETDATE(), GETDATE())";

                using (var cmd = new SqlCommand(orderSql, conn))
                {
                    cmd.Parameters.AddWithValue("@iCustomerId", req.iCustomerId);
                    cmd.Parameters.AddWithValue("@sellerId", req.sellerId);
                    cmd.Parameters.AddWithValue("@sFullName", req.sFullName ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@sPhoneNumber", req.sPhoneNumber ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@sAddress", req.sAddress ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@sCity", req.sCity ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@sPincode", req.sPincode ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@sState", req.sState ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@sLandmark", req.sLandmark ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@dcTotalAmount", req.dcTotalAmount);
                    cmd.Parameters.AddWithValue("@sOrderStatus", req.sOrderStatus ?? "Pending");
                    cmd.Parameters.AddWithValue("@sPaymentStatus", req.sPaymentStatus ?? "Pending");
                    var result = await cmd.ExecuteScalarAsync();
                    orderId = Convert.ToInt32(result);
                }

                foreach (var item in items)
                {
                    using var itemCmd = new SqlCommand(@"
                        INSERT INTO OrderItems (iOrderId, iProductId, iSellerId, iQuantity, dcPrice, dcTotalPrice)
                        VALUES (@iOrderId, @iProductId, @iSellerId, @iQuantity, @dcPrice, @dcTotalPrice)", conn);
                    itemCmd.Parameters.AddWithValue("@iOrderId", orderId);
                    itemCmd.Parameters.AddWithValue("@iProductId", item.iProductId);
                    itemCmd.Parameters.AddWithValue("@iSellerId", req.sellerId);
                    itemCmd.Parameters.AddWithValue("@iQuantity", item.iQuantity);
                    itemCmd.Parameters.AddWithValue("@dcPrice", item.dcPrice);
                    itemCmd.Parameters.AddWithValue("@dcTotalPrice", item.dcTotalPrice);
                    await itemCmd.ExecuteNonQueryAsync();
                }

                await InsertStatusHistoryAsync(conn, orderId, statusId: 1, "Order placed", req.iCustomerId);

                // Fire Shiprocket flow in background — does not block order response
                _ = Task.Run(async () =>
                {
                    try
                    {
                        var token = await GetShiprocketTokenAsync();
                        if (string.IsNullOrEmpty(token)) return;

                        // Step 1: Create Shiprocket Order
                        var (srOrderId, shipmentId, srError) = await CreateShiprocketOrderAsync(token, req, items, sellerAddr, orderId);
                        if (!string.IsNullOrEmpty(srError) || string.IsNullOrEmpty(shipmentId)) return;

                        // Always save srOrderId + shipmentId — even in DevMode
                        using var bgConn = new SqlConnection(_connectionString);
                        await bgConn.OpenAsync();
                        using var saveCmd = new SqlCommand(
                            "UPDATE Orders SET sShiprocketOrderId = @srOrderId, sShipmentId = @shipmentId WHERE iOrderId = @orderId", bgConn);
                        saveCmd.Parameters.AddWithValue("@srOrderId", srOrderId ?? (object)DBNull.Value);
                        saveCmd.Parameters.AddWithValue("@shipmentId", shipmentId ?? (object)DBNull.Value);
                        saveCmd.Parameters.AddWithValue("@orderId", orderId);
                        await saveCmd.ExecuteNonQueryAsync();

                        // Stop here in DevMode — Steps 2-5 skipped
                        if (_config.GetValue<bool>("Shiprocket:DevMode")) return;

                        // Step 2: Get Courier List — pick cheapest courier
                        var (courierId, courierName, courierError) = await GetBestCourierAsync(token, shipmentId, sellerAddr.pincode, req.sPincode, req.dcTotalAmount);
                        if (!string.IsNullOrEmpty(courierError) || string.IsNullOrEmpty(courierId)) return;

                        // Step 3: Assign AWB
                        var (awb, awbError) = await AssignAwbAsync(token, shipmentId, courierId);
                        if (!string.IsNullOrEmpty(awbError)) return;

                        // Step 4: Generate Label
                        var labelUrl = await GenerateLabelAsync(token, shipmentId);

                        // Step 5: Generate Invoice
                        var invoiceUrl = await GenerateInvoiceAsync(token, srOrderId);

                        // Save all remaining fields to DB
                        using var bgConn2 = new SqlConnection(_connectionString);
                        await bgConn2.OpenAsync();
                        using var upCmd = new SqlCommand(@"
                            UPDATE Orders SET
                                sTrackingNumber = @awb,
                                sCourierName    = @courierName,
                                sLabelUrl       = @labelUrl,
                                sInvoiceUrl     = @invoiceUrl,
                                sOrderStatus    = 'Confirmed'
                            WHERE iOrderId = @orderId", bgConn2);
                        upCmd.Parameters.AddWithValue("@awb", awb ?? (object)DBNull.Value);
                        upCmd.Parameters.AddWithValue("@courierName", courierName ?? (object)DBNull.Value);
                        upCmd.Parameters.AddWithValue("@labelUrl", labelUrl ?? (object)DBNull.Value);
                        upCmd.Parameters.AddWithValue("@invoiceUrl", invoiceUrl ?? (object)DBNull.Value);
                        upCmd.Parameters.AddWithValue("@orderId", orderId);
                        await upCmd.ExecuteNonQueryAsync();
                    }
                    catch { }
                });

                message = "Order placed successfully.";
            }
            catch (Exception ex)
            {
                message = $"Error: {ex.Message}";
            }

            return (orderId, message);
        }

        // ── 2. Update Order Status ────────────────────────────────────────────────
        public async Task<string> UpdateOrderStatusAsync(UpdateOrderStatusRequest req)
        {
            try
            {
                using var conn = new SqlConnection(_connectionString);
                await conn.OpenAsync();

                string statusName = "";
                using (var sCmd = new SqlCommand("SELECT sStatusName FROM OrderStatusMaster WHERE iStatusId = @id", conn))
                {
                    sCmd.Parameters.AddWithValue("@id", req.iStatusId);
                    statusName = (await sCmd.ExecuteScalarAsync())?.ToString() ?? "";
                }

                using (var cmd = new SqlCommand("UPDATE Orders SET sOrderStatus = @status WHERE iOrderId = @orderId", conn))
                {
                    cmd.Parameters.AddWithValue("@status", statusName);
                    cmd.Parameters.AddWithValue("@orderId", req.iOrderId);
                    await cmd.ExecuteNonQueryAsync();
                }

                await InsertStatusHistoryAsync(conn, req.iOrderId, req.iStatusId, req.sRemarks, req.iUpdatedBy);
                return "Order status updated successfully";
            }
            catch (Exception ex) { return $"Error: {ex.Message}"; }
        }

        // ── 3. Get Orders ─────────────────────────────────────────────────────────
        public async Task<List<OrderSummaryDto>> GetOrdersAsync(int? customerId, int? sellerId)
        {
            var orders = new Dictionary<int, OrderSummaryDto>();
            try
            {
                using var conn = new SqlConnection(_connectionString);
                await conn.OpenAsync();

                var sql = @"
                    SELECT o.iOrderId, o.iCustomerId, o.dcTotalAmount, o.sOrderStatus,
                           o.sPaymentStatus, o.dOrderDate, o.dDeliveryDate,
                           o.sTrackingNumber, o.sShiprocketOrderId, o.sShipmentId,
                           o.sCourierName, o.sLabelUrl, o.sInvoiceUrl,
                           oi.iOrderItemId, oi.iProductId, oi.iSellerId,
                           oi.iQuantity, oi.dcPrice, oi.dcTotalPrice
                    FROM Orders o
                    LEFT JOIN OrderItems oi ON o.iOrderId = oi.iOrderId
                    WHERE (@customerId IS NULL OR o.iCustomerId = @customerId)
                      AND (@sellerId IS NULL OR oi.iSellerId = @sellerId)
                    ORDER BY o.dOrderDate DESC";

                using var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@customerId", (object)customerId ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@sellerId", (object)sellerId ?? DBNull.Value);

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    int orderId = Convert.ToInt32(reader["iOrderId"]);
                    if (!orders.ContainsKey(orderId))
                    {
                        orders[orderId] = new OrderSummaryDto
                        {
                            iOrderId = orderId,
                            iCustomerId = Convert.ToInt32(reader["iCustomerId"]),
                            dcTotalAmount = Convert.ToDecimal(reader["dcTotalAmount"]),
                            sOrderStatus = reader["sOrderStatus"]?.ToString(),
                            sPaymentStatus = reader["sPaymentStatus"]?.ToString(),
                            dOrderDate = Convert.ToDateTime(reader["dOrderDate"]),
                            dDeliveryDate = reader["dDeliveryDate"] == DBNull.Value ? null : Convert.ToDateTime(reader["dDeliveryDate"]),
                            sTrackingNumber = reader["sTrackingNumber"]?.ToString(),
                            sShiprocketOrderId = reader["sShiprocketOrderId"]?.ToString(),
                            sShipmentId = reader["sShipmentId"]?.ToString(),
                            sCourierName = reader["sCourierName"]?.ToString(),
                            sLabelUrl = reader["sLabelUrl"]?.ToString(),
                            sInvoiceUrl = reader["sInvoiceUrl"]?.ToString(),
                            Items = new List<OrderItemDto>()
                        };
                    }

                    if (reader["iOrderItemId"] != DBNull.Value)
                    {
                        orders[orderId].Items.Add(new OrderItemDto
                        {
                            iOrderItemId = Convert.ToInt32(reader["iOrderItemId"]),
                            iProductId = Convert.ToInt32(reader["iProductId"]),
                            iSellerId = Convert.ToInt32(reader["iSellerId"]),
                            iQuantity = Convert.ToInt32(reader["iQuantity"]),
                            dcPrice = Convert.ToDecimal(reader["dcPrice"]),
                            dcTotalPrice = Convert.ToDecimal(reader["dcTotalPrice"])
                        });
                    }
                }
            }
            catch { }
            return new List<OrderSummaryDto>(orders.Values);
        }

        // ── 4. Get Order History ──────────────────────────────────────────────────
        public async Task<List<OrderStatusHistoryDto>> GetOrderHistoryAsync(int orderId)
        {
            var list = new List<OrderStatusHistoryDto>();
            try
            {
                using var conn = new SqlConnection(_connectionString);
                await conn.OpenAsync();

                using var cmd = new SqlCommand(@"
                    SELECT h.iHistoryId, h.iOrderId, h.iStatusId, m.sStatusName,
                           h.sRemarks, h.iUpdatedBy, h.dCreatedDate
                    FROM OrderStatusHistory h
                    LEFT JOIN OrderStatusMaster m ON h.iStatusId = m.iStatusId
                    WHERE h.iOrderId = @orderId
                    ORDER BY h.dCreatedDate ASC", conn);
                cmd.Parameters.AddWithValue("@orderId", orderId);

                using var reader = await cmd.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    list.Add(new OrderStatusHistoryDto
                    {
                        iHistoryId = Convert.ToInt32(reader["iHistoryId"]),
                        iOrderId = Convert.ToInt32(reader["iOrderId"]),
                        iStatusId = Convert.ToInt32(reader["iStatusId"]),
                        sStatusName = reader["sStatusName"]?.ToString(),
                        sRemarks = reader["sRemarks"]?.ToString(),
                        iUpdatedBy = Convert.ToInt32(reader["iUpdatedBy"]),
                        dCreatedDate = Convert.ToDateTime(reader["dCreatedDate"])
                    });
                }
            }
            catch { }
            return list;
        }

        // ── 5. Track Shipment ─────────────────────────────────────────────────────
        public async Task<object> TrackShipmentAsync(string awb)
        {
            try
            {
                var token = await GetShiprocketTokenAsync();
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await client.GetAsync($"{_config["Shiprocket:TrackOrderUrl"]}{awb}");
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<object>(json);
            }
            catch (Exception ex) { return new { error = ex.Message }; }
        }

        // ── Private: Get Shiprocket Token ─────────────────────────────────────────
        private async Task<string> GetShiprocketTokenAsync()
        {
            var client = _httpClientFactory.CreateClient();
            var payload = JsonSerializer.Serialize(new
            {
                email = _config["Shiprocket:Email"],
                password = _config["Shiprocket:Password"]
            });
            var response = await client.PostAsync(
                _config["Shiprocket:LoginUrl"],
                new StringContent(payload, Encoding.UTF8, "application/json"));
            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);
            return doc.RootElement.TryGetProperty("token", out var t) ? t.GetString() : null;
        }

        // ── Private: Step 1 — Create Shiprocket Order ─────────────────────────────
        private async Task<(string srOrderId, string shipmentId, string error)> CreateShiprocketOrderAsync(
            string token,
            PlaceOrderRequest req,
            List<OrderItemRequest> items,
            (string shopName, string address, string city, string state, string pincode, string phone, string email) seller,
            int orderId)
        {
            try
            {
                var nameParts = (req.sFullName ?? "Customer").Split(' ', 2);
                var firstName = nameParts[0];
                var lastName = nameParts.Length > 1 ? nameParts[1] : ".";

                var payload = new
                {
                    order_id = orderId.ToString(),
                    order_date = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm"),
                    pickup_location = seller.shopName ?? "Primary",
                    billing_customer_name = firstName,
                    billing_last_name = lastName,
                    billing_address = req.sAddress,
                    billing_address_2 = req.sLandmark ?? "",
                    billing_city = req.sCity,
                    billing_pincode = req.sPincode,
                    billing_state = req.sState,
                    billing_country = "India",
                    billing_phone = req.sPhoneNumber,
                    billing_email = seller.email ?? "noreply@majhipaithani.com",
                    shipping_customer_name = firstName,
                    shipping_last_name = lastName,
                    shipping_address = req.sAddress,
                    shipping_address_2 = req.sLandmark ?? "",
                    shipping_city = req.sCity,
                    shipping_pincode = req.sPincode,
                    shipping_state = req.sState,
                    shipping_country = "India",
                    shipping_phone = req.sPhoneNumber,
                    shipping_email = seller.email ?? "noreply@majhipaithani.com",
                    payment_method = req.sPaymentStatus == "Paid" ? "Prepaid" : "COD",
                    sub_total = req.dcTotalAmount,
                    length = 10,
                    breadth = 10,
                    height = 10,
                    weight = 0.5,
                    order_items = items.Select(i => new
                    {
                        name = i.sProductName ?? $"Product-{i.iProductId}",
                        sku = i.sSku ?? $"SKU-{i.iProductId}",
                        units = i.iQuantity,
                        selling_price = i.dcPrice
                    }).ToList()
                };

                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await client.PostAsync(
                    _config["Shiprocket:CreateOrderUrl"],
                    new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json"));
                var json = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    return (null, null, $"CreateOrder Error [{response.StatusCode}]: {json}");

                using var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;
                var srOrderId = root.TryGetProperty("order_id", out var oid) ? oid.GetRawText() : null;
                var shipmentId = root.TryGetProperty("shipment_id", out var sid) ? sid.GetRawText() : null;

                return (srOrderId, shipmentId, null);
            }
            catch (Exception ex) { return (null, null, ex.Message); }
        }

        // ── Private: Step 2 — Get Best Courier ────────────────────────────────────
        private async Task<(string courierId, string courierName, string error)> GetBestCourierAsync(
            string token, string shipmentId, string pickupPincode, string deliveryPincode, decimal totalAmount)
        {
            try
            {
                var isCod = 1; // always pass 1 — Shiprocket filters by COD availability
                var url = $"{_config["Shiprocket:CourierListUrl"]}?pickup_postcode={pickupPincode}&delivery_postcode={deliveryPincode}&cod={isCod}&weight=0.5&shipment_id={shipmentId}";

                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await client.GetAsync(url);
                var json = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    return (null, null, $"CourierList Error: {json}");

                using var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;

                if (root.TryGetProperty("data", out var data) &&
                    data.TryGetProperty("available_courier_companies", out var couriers) &&
                    couriers.GetArrayLength() > 0)
                {
                    // Pick courier with lowest rate
                    var best = couriers.EnumerateArray()
                        .OrderBy(c => c.TryGetProperty("rate", out var r) ? r.GetDecimal() : decimal.MaxValue)
                        .First();

                    var courierId = best.TryGetProperty("courier_company_id", out var cid) ? cid.GetRawText() : null;
                    var courierName = best.TryGetProperty("courier_name", out var cname) ? cname.GetString() : null;
                    return (courierId, courierName, null);
                }

                return (null, null, "No couriers available for this route");
            }
            catch (Exception ex) { return (null, null, ex.Message); }
        }

        // ── Private: Step 3 — Assign AWB ──────────────────────────────────────────
        private async Task<(string awb, string error)> AssignAwbAsync(string token, string shipmentId, string courierId)
        {
            try
            {
                var payload = JsonSerializer.Serialize(new { shipment_id = shipmentId, courier_id = courierId });
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await client.PostAsync(
                    _config["Shiprocket:AssignAwbUrl"],
                    new StringContent(payload, Encoding.UTF8, "application/json"));
                var json = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    return (null, $"AssignAWB Error: {json}");

                using var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;
                var awb = root.TryGetProperty("response", out var resp) &&
                          resp.TryGetProperty("data", out var d) &&
                          d.TryGetProperty("awb_code", out var awbProp)
                          ? awbProp.GetString() : null;

                return (awb, null);
            }
            catch (Exception ex) { return (null, ex.Message); }
        }

        // ── Private: Step 4 — Generate Label ──────────────────────────────────────
        private async Task<string> GenerateLabelAsync(string token, string shipmentId)
        {
            try
            {
                var payload = JsonSerializer.Serialize(new { shipment_id = new[] { shipmentId } });
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await client.PostAsync(
                    _config["Shiprocket:GenerateLabelUrl"],
                    new StringContent(payload, Encoding.UTF8, "application/json"));
                var json = await response.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(json);
                return doc.RootElement.TryGetProperty("label_url", out var url) ? url.GetString() : null;
            }
            catch { return null; }
        }

        // ── Private: Step 5 — Generate Invoice ────────────────────────────────────
        private async Task<string> GenerateInvoiceAsync(string token, string srOrderId)
        {
            try
            {
                var payload = JsonSerializer.Serialize(new { ids = new[] { srOrderId } });
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await client.PostAsync(
                    _config["Shiprocket:GenerateInvoiceUrl"],
                    new StringContent(payload, Encoding.UTF8, "application/json"));
                var json = await response.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(json);
                return doc.RootElement.TryGetProperty("invoice_url", out var url) ? url.GetString() : null;
            }
            catch { return null; }
        }

        // ── Private: Insert Status History ────────────────────────────────────────
        private async Task InsertStatusHistoryAsync(SqlConnection conn, int orderId, int statusId, string remarks, int updatedBy)
        {
            using var cmd = new SqlCommand(@"
                INSERT INTO OrderStatusHistory (iOrderId, iStatusId, sRemarks, iUpdatedBy, dCreatedDate)
                VALUES (@orderId, @statusId, @remarks, @updatedBy, GETDATE())", conn);
            cmd.Parameters.AddWithValue("@orderId", orderId);
            cmd.Parameters.AddWithValue("@statusId", statusId);
            cmd.Parameters.AddWithValue("@remarks", remarks ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@updatedBy", updatedBy);
            await cmd.ExecuteNonQueryAsync();
        }

        // ── Private: Get Seller Address ───────────────────────────────────────────
        private async Task<(string shopName, string address, string city, string state, string pincode, string phone, string email)>
            GetSellerAddressAsync(SqlConnection conn, int sellerId)
        {
            using var cmd = new SqlCommand(@"
                SELECT TOP 1 s.sShopName, s.sShopAddress, s.sCity, s.sState, s.sPincode,
                             u.sPhoneNumber, u.sEmail
                FROM Sellers s
                INNER JOIN Users u ON u.iUserId = s.iUserId
                WHERE s.iSellerId = @sellerId AND ISNULL(s.bIsDeleted, 0) = 0", conn);
            cmd.Parameters.AddWithValue("@sellerId", sellerId);

            using var reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return (
                    reader["sShopName"]?.ToString(),
                    reader["sShopAddress"]?.ToString(),
                    reader["sCity"]?.ToString(),
                    reader["sState"]?.ToString(),
                    reader["sPincode"]?.ToString(),
                    reader["sPhoneNumber"]?.ToString(),
                    reader["sEmail"]?.ToString()
                );
            }
            return (null, null, null, null, null, null, null);
        }
    }
}
