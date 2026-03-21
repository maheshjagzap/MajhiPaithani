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
