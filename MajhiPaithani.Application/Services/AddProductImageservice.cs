
using MajhiPaithani.Application.DataAccess;
using MajhiPaithani.Application.Models.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MajhiPaithani.Application.Services
{
    public class AddProductImageservice
    {
        private readonly AddproductimagedataAccess _dataAccess;

        public AddProductImageservice(AddproductimagedataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public async Task<string> SaveProductImagesAsync(List<string> fileUrls, int productid, int userId)
        {

            return await _dataAccess.ExecuteProductImagesAsync(fileUrls, productid,userId);
        }
    }
}