using MajhiPaithani.Application.Services;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace MajhiPaithani.API.Endpoint
{
    public class GetSellerDashboard
    {
        public static void Map(WebApplication app)
        {
            var dashboard = app.MapGroup("/api/dashboard");

            dashboard.MapGet("/GetSellerDashboard", async (int RequestedFor, int TaskId,
                int? sellerId,
                int? UserId,               
                [FromServices] GetSellerDashboardService service) =>
            {
                try
                {
                    var result = await service.GetSellerDashboardAsync(RequestedFor, TaskId, sellerId ?? 0, UserId ?? 0);

                    if (result == null || result.Tables.Count == 0 || result.Tables[0].Rows.Count == 0)
                    {
                        return Results.Ok(new
                        {
                            StatusCode = StatusCodes.Status204NoContent,
                            Message = "No data found",
                            Data = (object)null
                        });
                    }

                    var jsonResult = new List<Dictionary<string, object?>>();

                    foreach (DataRow row in result.Tables[0].Rows)
                    {
                        var dict = new Dictionary<string, object?>();

                        foreach (DataColumn col in result.Tables[0].Columns)
                        {
                            object? value = row[col] == DBNull.Value ? null : row[col];
                            dict[col.ColumnName] = value;
                        }

                        jsonResult.Add(dict);
                    }

                    return Results.Ok(new
                    {
                        StatusCode = StatusCodes.Status200OK,
                        Message = "Dashboard data fetched successfully",
                        Data = jsonResult
                    });
                }
                catch (Exception ex)
                {
                    return Results.Problem(detail: $"Error: {ex.Message}");
                }
            })
            .WithName("GetSellerDashboard")
            .WithTags("Dashboard");
        }
    }
}