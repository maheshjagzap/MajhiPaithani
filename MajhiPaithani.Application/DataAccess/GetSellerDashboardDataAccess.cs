using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Threading.Tasks;

namespace MajhiPaithani.Application.DataAccess
{
    public class GetSellerDashboardDataAccess
    {
        private readonly string _connectionString;

        public GetSellerDashboardDataAccess(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public async Task<DataSet> GetSellerDashboardAsync(int RequestedFor,int TaskId,int sellerId,int UserId)
        {
            DataSet ds = new DataSet();

            try
            {
                using (var conn = new SqlConnection(_connectionString))
                using (var cmd = new SqlCommand("Proc_GetSellerDashboard", conn)) 
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@RequestedFor", RequestedFor);
                    cmd.Parameters.AddWithValue("@TaskId", TaskId);
                    cmd.Parameters.AddWithValue("@SellerId", sellerId);
                    cmd.Parameters.AddWithValue("@UserId", UserId);

                    await conn.OpenAsync();

                    using (var adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(ds);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception($"SQL Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Error: {ex.Message}");
            }

            return ds;
        }
    }
}