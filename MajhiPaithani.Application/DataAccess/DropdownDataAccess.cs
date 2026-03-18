using MajhiPaithani.Application.Models.Response;
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
    public class DropdownDataAccess
    {
        private readonly string _connectionString;

        public DropdownDataAccess(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<List<DropdownDto>> GetDropdownListAsync(int taskId)
        {
            var list = new List<DropdownDto>();

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("getdropdownlit", conn);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@TaskId", taskId);

            await conn.OpenAsync();

            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                list.Add(new DropdownDto
                {
                    Id = reader["Id"] != DBNull.Value ? Convert.ToInt32(reader["Id"]) : 0,
                    Name = reader["Name"]?.ToString()
                });
            }

            return list;
        }
    }
}
