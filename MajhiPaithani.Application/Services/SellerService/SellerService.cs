using MajhiPaithani.Application.Interfaces.ISellerInserface;
using MajhiPaithani.Application.Models.Request;
using MajhiPaithani.Application.Models.Response;
using MajhiPaithani.Infrastructure.Data.ApplicationDbContext;
using MajhiPaithani.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

public class SellerService : ISellerService
{
    private readonly ApplicationDbContext _context;

    public SellerService(ApplicationDbContext context)
    {
        _context = context;
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task<RegisterSellerResponse> RegisterSellerAsync(RegisterSellerRequest request)
    {
        var existingSeller = await _context.Sellers
            .FirstOrDefaultAsync(x => x.IUserId == request.UserId);

        if (existingSeller != null)
        {
            throw new Exception("User is already registered as seller");
        }

        var seller = new Seller
        {
            IUserId = (int)request.UserId,
            SShopName = request.ShopName,
            SShopDescription = request.Description,
            ILocationId = (int)request.UserId,
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
            bIsVerified = (bool)seller.BIsVerified,
            bIsActive = seller.BIsActive,
            sMessage = "Seller registered successfully. Waiting for admin approval."
        };
    }



    /// <summary>
    /// 
    /// </summary>
    /// <param name="sellerId"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task<GetSellerProfileResponse> GetSellerProfileAsync(int sellerId)
    {
        var seller = await _context.Sellers
            .FirstOrDefaultAsync(x => x.ISellerId == sellerId);

        if (seller == null)
        {
            throw new Exception("Seller not found");
        }

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



    /// <summary>
    /// 
    /// </summary>
    /// <param name="sellerId"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task<string> UpdateSellerProfileAsync(int sellerId, UpdateSellerProfileRequest request)
    {
        var seller = await _context.Sellers
            .FirstOrDefaultAsync(x => x.ISellerId == sellerId);

        if (seller == null)
        {
            throw new Exception("Seller not found");
        }

        seller.SShopName = request.sShopName;
        seller.SShopDescription = request.sShopDescription;
        seller.ILocationId = request.iLocationId;
        seller.DUpdatedDate = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return "Seller profile updated successfully";
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task<AddSellerBankDetailsResponse> AddSellerBankDetailsAsync(AddSellerBankDetailsRequest request)
    {
        var seller = await _context.Sellers
            .FirstOrDefaultAsync(x => x.ISellerId == request.iSellerId);

        if (seller == null)
        {
            throw new Exception("Seller not found");
        }

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
}