using MajhiPaithani.Application.Models.Request;
using MajhiPaithani.Application.Models.Response;
using MajhiPaithani.Infrastructure.Entities;
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
    public class SaveDataAccess
    {
        private readonly string _connectionString;

        public SaveDataAccess(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<string> ExecuteSellerAsync(SellerDto dto, int? UserId, int? RoleId)
        {
            string message = "";

            try
            {
                using (var conn = new SqlConnection(_connectionString))
                using (var cmd = new SqlCommand("Proc_SaveData", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    var json = System.Text.Json.JsonSerializer.Serialize(dto);

                    cmd.Parameters.AddWithValue("@JsonData", json);
                    cmd.Parameters.AddWithValue("@TaskID", dto.Taskid);
                    cmd.Parameters.AddWithValue("@RequestedFor", dto.RequestedFor);
                    cmd.Parameters.AddWithValue("@headerUserID", UserId);
                    cmd.Parameters.AddWithValue("@headerRoleid", RoleId);

                    await conn.OpenAsync();

                    using var reader = await cmd.ExecuteReaderAsync();

                    if (await reader.ReadAsync())
                    {
                        message = reader["Message"]?.ToString();
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

        public async Task<string> UpdatesellerBankdeatilsasync(BankDto dto, int? UserId, int? RoleId)
        {
            string message = "";

            try
            {
                using (var conn = new SqlConnection(_connectionString))
                using (var cmd = new SqlCommand("Proc_SaveData", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    var json = System.Text.Json.JsonSerializer.Serialize(dto);

                    cmd.Parameters.AddWithValue("@JsonData", json);
                    cmd.Parameters.AddWithValue("@TaskID", dto.Taskid);
                    cmd.Parameters.AddWithValue("@RequestedFor", dto.RequestedFor);
                    cmd.Parameters.AddWithValue("@headerUserID", UserId);
                    cmd.Parameters.AddWithValue("@headerRoleid", RoleId);

                    await conn.OpenAsync();

                    using var reader = await cmd.ExecuteReaderAsync();

                    if (await reader.ReadAsync())
                    {
                        message = reader["Message"]?.ToString();
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

        public async Task<string> UpdateProductInformation(UpdateProductDto dto)
        {
            string message = "";

            try
            {
                using var conn = new SqlConnection(_connectionString);
                var sql = @"UPDATE Products SET
                    iCategoryId = @iCategoryId,
                    sProductTitle = @sProductTitle,
                    sDescription = @sDescription,
                    dcBasePrice = @dcBasePrice,
                    sColor = @sColor,
                    sFabric = @sFabric,
                    sDesignType = @sDesignType,
                    bIsCustomizationAvailable = @bIsCustomizationAvailable,
                    bIsActive = 1,
                    bIsDeleted = 0,
                    dUpdatedDate = GETDATE()
                WHERE iProductId = @iProductId";

                using var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@iProductId", dto.iProductId);
                cmd.Parameters.AddWithValue("@iCategoryId", (object)dto.iCategoryId ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@sProductTitle", dto.sProductTitle);
                cmd.Parameters.AddWithValue("@sDescription", dto.sDescription);
                cmd.Parameters.AddWithValue("@dcBasePrice", (object)dto.dcBasePrice ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@sColor", (object)dto.sColor ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@sFabric", (object)dto.sFabric ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@sDesignType", (object)dto.sDesignType ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@bIsCustomizationAvailable", (object)dto.bIsCustomizationAvailable ?? DBNull.Value);

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();

                message = "product information updated successfully";
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

        public async Task<string> DeleteProductInformation(int productId)
        {
            string message = "";

            try
            {
                using var conn = new SqlConnection(_connectionString);
                using var cmd = new SqlCommand("UPDATE Products SET bIsDeleted = 1 ,dDeletedDate=GETDATE() WHERE iProductId = @iProductId", conn);
                cmd.Parameters.AddWithValue("@iProductId", productId);

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();

                message = "product information deleted successfully";
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

        public async Task<ProductAddresponce> AddPrductinformation(ProductDto dto, int? UserId, int? RoleId)
        {
            var response = new ProductAddresponce();

            try
            {
                using (var conn = new SqlConnection(_connectionString))
                using (var cmd = new SqlCommand("Proc_SaveData", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    var json = System.Text.Json.JsonSerializer.Serialize(dto);

                    cmd.Parameters.Add("@JsonData", SqlDbType.NVarChar).Value = json;
                    cmd.Parameters.Add("@TaskID", SqlDbType.Int).Value = dto.Taskid;
                    cmd.Parameters.Add("@RequestedFor", SqlDbType.Int).Value = dto.RequestedFor;
                    cmd.Parameters.Add("@headerUserID", SqlDbType.Int).Value = (object)UserId ?? DBNull.Value;
                    cmd.Parameters.Add("@headerRoleid", SqlDbType.Int).Value = (object)RoleId ?? DBNull.Value;

                    await conn.OpenAsync();

                    using var reader = await cmd.ExecuteReaderAsync();

                    if (await reader.ReadAsync())
                    {
                        response.Message = reader["Message"]?.ToString();

                        if (reader["ProductId"] != DBNull.Value)
                        {
                            response.ProductId = Convert.ToInt32(reader["ProductId"]);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                response.Message = $"SQL Error: {ex.Message}";
            }
            catch (Exception ex)
            {
                response.Message = $"Error: {ex.Message}";
            }

            return response;
        }


    }
}
