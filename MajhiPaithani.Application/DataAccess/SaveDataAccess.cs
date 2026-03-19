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
    public class SaveDataAccess
    {
        private readonly string _connectionString;

        public SaveDataAccess(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<string> ExecuteSellerAsync(SellerDto dto)
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


    }
}
