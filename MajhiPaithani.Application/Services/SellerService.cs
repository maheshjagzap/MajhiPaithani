using MajhiPaithani.Application.DataAccess;
using MajhiPaithani.Application.Models.Request;
using MajhiPaithani.Application.Models.Response;
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

        public async Task<string> SaveSellerAsync(SellerDto dto, int? UserId, int? RoleId)
        {
            return await _dataAccess.ExecuteSellerAsync(dto,UserId,RoleId);
        }

        
        public async Task<string> updatebankdeatils(BankDto dto, int? UserId, int? RoleId)
        {
            return await _dataAccess.UpdatesellerBankdeatilsasync(dto,UserId,RoleId);
        }

        public async Task<ProductAddresponce> AddProdudctinfoasync(ProductDto dto, int? UserId, int? RoleId)
        {
            return await _dataAccess.AddPrductinformation(dto, UserId, RoleId);
        }
    }
}
