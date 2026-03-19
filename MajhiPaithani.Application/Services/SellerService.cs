using MajhiPaithani.Application.DataAccess;
using MajhiPaithani.Application.Models.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajhiPaithani.Application.Services
{
    public class SaveSellerService
    {
        private readonly SaveDataAccess _dataAccess;

        public SaveSellerService(SaveDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public async Task<string> SaveSellerAsync(SellerDto dto)
        {
            return await _dataAccess.ExecuteSellerAsync(dto);
        }
    }
}
