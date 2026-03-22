
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

        public async Task<string> UpdateProductImageAsync(int imageId, string newFilePath)
        {
            return await _dataAccess.UpdateProductImageAsync(imageId, newFilePath);
        }
        public async Task<string> deleteProductImageAsync(int imageId)
        {
            return await _dataAccess.deleteProductImageAsync(imageId);
        }

        public async Task<List<ProductImageDto>> GetProductImagesAsync(int userId)
        {
            return await _dataAccess.GetProductImagesAsync(userId);
        }
    }
}