using MajhiPaithani.Application.Models.Request;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajhiPaithani.Application.DataAccess
{

    public class AddproductimagedataAccess
    {
        private readonly string _connectionString;

        public AddproductimagedataAccess(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<string> ExecuteProductImagesAsync(List<string> fileUrls, int productid, int userId)
        {
            string message = "";

            try
            {
                using (var conn = new SqlConnection(_connectionString))
                using (var cmd = new SqlCommand("AddProductImageData", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    await conn.OpenAsync();

                    foreach (var url in fileUrls)
                    {
                        cmd.Parameters.Clear();

                        cmd.Parameters.AddWithValue("@taskid", 1);
                        cmd.Parameters.AddWithValue("@UserId", userId);
                        cmd.Parameters.AddWithValue("@iProductId", productid);
                        cmd.Parameters.AddWithValue("@sImageUrl", url);

                        using var reader = await cmd.ExecuteReaderAsync();

                        if (await reader.ReadAsync())
                        {
                            message = reader["Message"]?.ToString();
                        }

                        reader.Close();
                    }
                }
            }
            catch (SqlException ex)
            {
                message = $"SQL Error: {ex.Message}";
            }
            catch (Exception ex)
            {
                message = $"Error: {ex.Message}";
            }

            return message;
        }

        public async Task<List<ProductImageDto>> GetProductImagesAsync(int userId)
        {
            var list = new List<ProductImageDto>();

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("AddProductImageData", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@taskid", 0);
                cmd.Parameters.AddWithValue("@UserId", userId);

                await conn.OpenAsync();

                using var reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    list.Add(new ProductImageDto
                    {
                        iProductId = Convert.ToInt32(reader["iProductId"]),
                        iSellerId = Convert.ToInt32(reader["iSellerId"]),
                        iCategoryId = Convert.ToInt32(reader["iCategoryId"]),
                        sProductTitle = reader["sProductTitle"]?.ToString(),
                        sDescription = reader["sDescription"]?.ToString(),
                        dcBasePrice = Convert.ToDecimal(reader["dcBasePrice"]),
                        sColor = reader["sColor"]?.ToString(),
                        sFabric = reader["sFabric"]?.ToString(),
                        sDesignType = reader["sDesignType"]?.ToString(),
                        bIsCustomizationAvailable = Convert.ToBoolean(reader["bIsCustomizationAvailable"]),
                        bIsActive = Convert.ToBoolean(reader["bIsActive"]),
                        bIsDeleted = Convert.ToBoolean(reader["bIsDeleted"]),
                        ProductCreatedDate = Convert.ToDateTime(reader["ProductCreatedDate"]),
                        ProductUpdatedDate = reader["ProductUpdatedDate"] as DateTime?,

                        iImageId = reader["iImageId"] as int?,
                        sImageUrl = reader["sImageUrl"]?.ToString(),
                        bIsPrimary = reader["bIsPrimary"] as bool?,
                        ImageCreatedDate = reader["ImageCreatedDate"] as DateTime?
                    });
                }
            }

            return list;
        }

    }
}