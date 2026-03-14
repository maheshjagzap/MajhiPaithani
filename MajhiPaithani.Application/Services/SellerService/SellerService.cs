using MajhiPaithani.Application.Interfaces.ISellerInserface;
using MajhiPaithani.Application.Models.Request;
using MajhiPaithani.Application.Models.Response;
using MajhiPaithani.Application.Models.Response.UpdateBankDetailsResponse;
using MajhiPaithani.Infrastructure.Data.ApplicationDbContext;
using MajhiPaithani.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;


namespace MajhiPaithani.Infrastructure.Services
{
    public class SellerService : ISellerService
    {
        private readonly ApplicationDbContext _context;

        public SellerService(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<RegisterSellerResponse> RegisterSellerAsync(RegisterSellerRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (string.IsNullOrWhiteSpace(request.ShopName))
                throw new Exception("Shop name is required");

            var existingSeller = await _context.Sellers
                .FirstOrDefaultAsync(x => x.IUserId == request.UserId);

            if (existingSeller != null)
                throw new InvalidOperationException("User is already registered as seller");

            var seller = new Seller
            {
                IUserId = request.UserId,
                SShopName = request.ShopName,
                SShopDescription = request.Description,
                //ILocationId = request.LocationId,
                BIsVerified = false,
                BIsActive = true,
                DCreatedDate = DateTime.UtcNow,
                DUpdatedDate = DateTime.UtcNow
            };

            _context.Sellers.Add(seller);
            await _context.SaveChangesAsync();

            return new RegisterSellerResponse
            {
                iSellerId = seller.ISellerId,
                iUserId = seller.IUserId,
                sShopName = seller.SShopName,
                //bIsVerified = seller.BIsVerified,
                bIsActive = seller.BIsActive,
                sMessage = "Seller registered successfully. Waiting for admin approval."
            };
        }
        public async Task<GetSellerProfileResponse> GetSellerProfileAsync(int sellerId)
        {
            var seller = await _context.Sellers
                .FirstOrDefaultAsync(x => x.ISellerId == sellerId);

            if (seller == null)
                throw new Exception("Seller not found");

            return new GetSellerProfileResponse
            {
                iSellerId = seller.ISellerId,
                iUserId = seller.IUserId,
                sShopName = seller.SShopName,
                sShopDescription = seller.SShopDescription,
                iLocationId = seller.ILocationId,
                bIsVerified = seller.BIsVerified,
                bIsActive = seller.BIsActive
            };
        }
        public async Task<string> UpdateSellerProfileAsync(int sellerId, UpdateSellerProfileRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var seller = await _context.Sellers
                .FirstOrDefaultAsync(x => x.ISellerId == sellerId);

            if (seller == null)
                throw new Exception("Seller not found");

            if (!seller.BIsActive)
                throw new Exception("Seller account is inactive");

            if (string.IsNullOrWhiteSpace(request.sShopName))
                throw new Exception("Shop name cannot be empty");

            seller.SShopName = request.sShopName;
            seller.SShopDescription = request.sShopDescription;
            seller.ILocationId = request.iLocationId;
            seller.DUpdatedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return "Seller profile updated successfully";
        }
        public async Task<AddSellerBankDetailsResponse> AddSellerBankDetailsAsync(AddSellerBankDetailsRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var seller = await _context.Sellers
                .FirstOrDefaultAsync(x => x.ISellerId == request.iSellerId);

            if (seller == null)
                throw new Exception("Seller not found");

            var existingBank = await _context.SellerBankDetails
                .FirstOrDefaultAsync(x => x.IsellerId == request.iSellerId);

            if (existingBank != null)
                throw new Exception("Bank details already added for this seller");

            if (string.IsNullOrWhiteSpace(request.sAccountNumber))
                throw new Exception("Account number is required");

            var bank = new SellerBankDetail
            {
                IsellerId = request.iSellerId,
                SaccountHolderName = request.sAccountHolderName,
                SaccountNumber = request.sAccountNumber,
                Sifsccode = request.sIFSCCode,
                SbankName = request.sBankName,
                DcreatedDate = DateTime.UtcNow
            };

            _context.SellerBankDetails.Add(bank);

            await _context.SaveChangesAsync();

            return new AddSellerBankDetailsResponse
            {
                iBankDetailId = bank.IbankDetailId,
                Message = "Bank details added successfully"
            };
        }
        public async Task<GetSellerBankDetailsResponse> GetSellerBankDetailsAsync(int sellerId)
        {
            var seller = await _context.Sellers
                .FirstOrDefaultAsync(x => x.ISellerId == sellerId);

            if (seller == null)
                throw new Exception("Seller not found");

            var bank = await _context.SellerBankDetails
                .FirstOrDefaultAsync(x => x.IsellerId == sellerId);

            if (bank == null)
                throw new Exception("Bank details not found");

            var maskedAccount = bank.SaccountNumber.Length > 4
                ? "XXXXXX" + bank.SaccountNumber.Substring(bank.SaccountNumber.Length - 4)
                : bank.SaccountNumber;

            return new GetSellerBankDetailsResponse
            {
                BankDetailId = bank.IbankDetailId,
                SellerId = bank.IsellerId,
                AccountHolderName = bank.SaccountHolderName,
                BankName = bank.SbankName,
                IFSCCode = bank.Sifsccode,
                AccountNumber = maskedAccount
            };
        }
        public async Task<UpdateBankDetailsResponse> UpdateSellerBankDetailsAsync(UpdateBankDetailsRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var bank = await _context.SellerBankDetails
                .FirstOrDefaultAsync(x => x.IbankDetailId == request.BankDetailId);

            if (bank == null)
                throw new Exception("Bank details not found");

            if (string.IsNullOrWhiteSpace(request.AccountNumber))
                throw new Exception("Account number cannot be empty");

            bank.SaccountHolderName = request.AccountHolderName;
            bank.SbankName = request.BankName;
            bank.SaccountNumber = request.AccountNumber;
            bank.Sifsccode = request.IFSCCode;
            //bank.SupiId = request.UPIId;
            bank.DupdatedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return new UpdateBankDetailsResponse
            {
                BankDetailId = bank.IbankDetailId,
                UpdatedDate = bank.DupdatedDate.Value,
                Message = "Bank details updated successfully"
            };
        }
        public async Task<UploadSellerProfileImageResponse> UploadSellerProfileImageAsync(UploadSellerProfileImageRequest request)
        {
            if (request == null || request.ImageFile == null)
                throw new Exception("Image file is required");

            var seller = await _context.Sellers
                .FirstOrDefaultAsync(x => x.ISellerId == request.SellerId);

            if (seller == null)
                throw new Exception("Seller not found");

            // Validate image size (max 5MB)
            if (request.ImageFile.Length > 5 * 1024 * 1024)
                throw new Exception("Image size cannot exceed 5MB");

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "sellers", request.SellerId.ToString());

            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var fileExtension = Path.GetExtension(request.ImageFile.FileName);

            var fileName = "profile" + fileExtension;

            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await request.ImageFile.CopyToAsync(stream);
            }

            var imageUrl = $"/uploads/sellers/{request.SellerId}/{fileName}";

            seller.SProfileImageUrl = imageUrl;
            seller.DUpdatedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return new UploadSellerProfileImageResponse
            {
                SellerId = request.SellerId,
                ProfileImageUrl = imageUrl,
                UploadedDate = DateTime.UtcNow,
                Message = "Profile image uploaded successfully"
            };
        }
        public async Task<UpdateShopDetailsResponse> UpdateSellerShopDetailsAsync(UpdateShopDetailsRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var seller = await _context.Sellers
                .FirstOrDefaultAsync(x => x.ISellerId == request.SellerId);

            if (seller == null)
                throw new Exception("Seller not found");

            if (string.IsNullOrWhiteSpace(request.ShopName))
                throw new Exception("Shop name cannot be empty");

            seller.SShopName = request.ShopName;
            seller.SShopAddress = request.ShopAddress;
            seller.SCity = request.City;
            seller.SState = request.State;
            seller.SPincode = request.Pincode;
            seller.SBusinessDescription = request.BusinessDescription;
            seller.DUpdatedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return new UpdateShopDetailsResponse
            {
                SellerId = seller.ISellerId,
                ShopName = seller.SShopName,
                UpdatedDate = seller.DUpdatedDate.Value,
                Message = "Shop details updated successfully"
            };
        }
        public async Task<AddDesignResponse> AddDesignAsync(AddDesignRequest request)
        {
            if (request == null || request.ImageFile == null)
                throw new Exception("Design image is required");

            var seller = await _context.Sellers
                .FirstOrDefaultAsync(x => x.ISellerId == request.SellerId);

            if (seller == null)
                throw new Exception("Seller not found");

            var design = new Design
            {
                ISellerId = request.SellerId,
                SDesignName = request.DesignName,
                SDesignType = request.DesignType,
                SDescription = request.Description,
                DCreatedDate = DateTime.UtcNow
            };

            _context.Designs.Add(design);

            await _context.SaveChangesAsync();

            // Create folder
            var uploadsFolder = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "uploads",
                "designs",
                request.SellerId.ToString());

            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var extension = Path.GetExtension(request.ImageFile.FileName);

            var fileName = design.IDesignId + extension;

            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await request.ImageFile.CopyToAsync(stream);
            }

            var imageUrl = $"/uploads/designs/{request.SellerId}/{fileName}";

            design.SImageUrl = imageUrl;

            await _context.SaveChangesAsync();

            return new AddDesignResponse
            {
                DesignId = design.IDesignId,
                SellerId = design.ISellerId,
                DesignName = design.SDesignName,
                CreatedDate = design.DCreatedDate.Value,
                Message = "Design uploaded successfully"
            };
        }
        public async Task<List<GetSellerDesignsResponse>> GetSellerDesignsAsync(int sellerId)
        {
            var seller = await _context.Sellers
                .FirstOrDefaultAsync(x => x.ISellerId == sellerId);

            if (seller == null)
                throw new Exception("Seller not found");

            var designs =   await _context.Designs
                .Where(x => x.ISellerId == sellerId && x.BIsDeleted != true)
                .Select(x => new GetSellerDesignsResponse
                {
                    DesignId = x.IDesignId,
                    SellerId = x.ISellerId,
                    DesignName = x.SDesignName,
                    DesignType = x.SDesignType,
                    Description = x.SDescription,
                    ImageUrl = x.SImageUrl,
                    CreatedDate = x.DCreatedDate.Value
                })
                .ToListAsync();

            return designs;
        }
        public async Task<UpdateDesignResponse> UpdateDesignAsync(int designId, UpdateDesignRequest request)
        {
            var design = await _context.Designs
                .FirstOrDefaultAsync(x => x.IDesignId == designId);

            if (design == null)
                throw new Exception("Design not found");

            if (!string.IsNullOrWhiteSpace(request.DesignName))
                design.SDesignName = request.DesignName;

            if (!string.IsNullOrWhiteSpace(request.Description))
                design.SDescription = request.Description;

            if (request.ImageFile != null)
            {
                var uploadsFolder = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot",
                    "uploads",
                    "designs",
                    design.ISellerId.ToString());

                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var extension = Path.GetExtension(request.ImageFile.FileName);

                var fileName = design.IDesignId + extension;

                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await request.ImageFile.CopyToAsync(stream);
                }

                design.SImageUrl = $"/uploads/designs/{design.ISellerId}/{fileName}";
            }

            design.DUpdatedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return new UpdateDesignResponse
            {
                DesignId = design.IDesignId,
                UpdatedDate = design.DUpdatedDate.Value,
                Message = "Design updated successfully"
            };
        }
        public async Task<DeleteDesignResponse> DeleteDesignAsync(int designId)
        {
            var design = await _context.Designs
                .FirstOrDefaultAsync(x => x.IDesignId == designId);

            if (design == null)
                throw new Exception("Design not found");

            design.BIsDeleted = true;
            design.DUpdatedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return new DeleteDesignResponse
            {
                DesignId = design.IDesignId,
                DeletedDate = design.DUpdatedDate.Value,
                Message = "Design deleted successfully"
            };
        }
        public async Task<DesignDetailsResponse> GetDesignDetailsAsync(int designId)
        {
            var design = await _context.Designs
                .Where(x => x.IDesignId == designId && x.BIsDeleted != true)
                .Select(x => new DesignDetailsResponse
                {
                    DesignId = x.IDesignId,
                    SellerId = x.ISellerId,
                    DesignName = x.SDesignName,
                    DesignType = x.SDesignType,
                    Description = x.SDescription,
                    ImageUrl = x.SImageUrl,
                    CreatedDate = x.DCreatedDate.Value
                })
                .FirstOrDefaultAsync();

            if (design == null)
                throw new Exception("Design not found");

            return design;
        }
        public async Task<AddProductResponse> AddProductAsync(AddProductRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var seller = await _context.Sellers
                .FirstOrDefaultAsync(x => x.ISellerId == request.SellerId);

            if (seller == null)
                throw new Exception("Seller not found");

            var product = new Product
            {
                ISellerId = request.SellerId,
                ICategoryId = request.CategoryId,
                SProductTitle = request.ProductTitle,
                SDescription = request.Description,
                DcBasePrice = request.BasePrice,
                SColor = request.Color,
                SFabric = request.Fabric,
                SDesignType = request.DesignType,
                BIsCustomizationAvailable = request.IsCustomizationAvailable,
                BIsActive = true,
                BIsDeleted = false,
                DCreatedDate = DateTime.UtcNow
            };

            _context.Products.Add(product);

            await _context.SaveChangesAsync();

            return new AddProductResponse
            {
                ProductId = product.IProductId,
                SellerId = product.ISellerId.Value,
                ProductName = product.SProductTitle,
                CreatedDate = product.DCreatedDate.Value,
                Message = "Product created successfully"
            };
        }
        public async Task<UpdateProductResponse> UpdateProductAsync(int productId, UpdateProductRequest request)
        {
            var product = await _context.Products
                .FirstOrDefaultAsync(x => x.IProductId == productId && x.BIsDeleted != true);

            if (product == null)
                throw new Exception("Product not found");

            if (!string.IsNullOrWhiteSpace(request.ProductTitle))
                product.SProductTitle = request.ProductTitle;

            if (!string.IsNullOrWhiteSpace(request.Description))
                product.SDescription = request.Description;

            if (request.BasePrice > 0)
                product.DcBasePrice = request.BasePrice;

            product.SColor = request.Color;
            product.SFabric = request.Fabric;
            product.SDesignType = request.DesignType;
            product.BIsCustomizationAvailable = request.IsCustomizationAvailable;

            product.DUpdatedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return new UpdateProductResponse
            {
                ProductId = product.IProductId,
                UpdatedDate = product.DUpdatedDate.Value,
                Message = "Product updated successfully"
            };
        }

        public async Task<DeleteProductResponse> DeleteProductAsync(int productId)
        {
            var product = await _context.Products
                .FirstOrDefaultAsync(x => x.IProductId == productId && x.BIsDeleted != true);

            if (product == null)
                throw new Exception("Product not found");

            product.BIsDeleted = true;
            product.BIsActive = false;
            product.DDeletedDate = DateTime.UtcNow;
            product.DUpdatedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return new DeleteProductResponse
            {
                ProductId = product.IProductId,
                DeletedDate = product.DDeletedDate.Value,
                Message = "Product deleted successfully"
            };
        }
    }
}