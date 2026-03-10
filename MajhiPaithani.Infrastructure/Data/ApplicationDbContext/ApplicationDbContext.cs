using System;
using System.Collections.Generic;
using MajhiPaithani.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace MajhiPaithani.Infrastructure.Data.ApplicationDbContext;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AdminLog> AdminLogs { get; set; }
    public virtual DbSet<Productdemo> Productdemos { get; set; }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<CartItem> CartItems { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<ChatMessage> ChatMessages { get; set; }

    public virtual DbSet<ChatRoom> ChatRooms { get; set; }

    public virtual DbSet<Coupon> Coupons { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<CustomerAddress> CustomerAddresses { get; set; }

    public virtual DbSet<CustomizationRequest> CustomizationRequests { get; set; }

    public virtual DbSet<Location> Locations { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderCoupon> OrderCoupons { get; set; }

    public virtual DbSet<OrderItem> OrderItems { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductImage> ProductImages { get; set; }

    public virtual DbSet<ProductReview> ProductReviews { get; set; }

    public virtual DbSet<ProductTag> ProductTags { get; set; }

    public virtual DbSet<ProductTagMapping> ProductTagMappings { get; set; }

    public virtual DbSet<ProductView> ProductViews { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<SearchHistory> SearchHistories { get; set; }

    public virtual DbSet<Seller> Sellers { get; set; }

    public virtual DbSet<SellerBankDetail> SellerBankDetails { get; set; }

    public virtual DbSet<SellerDocument> SellerDocuments { get; set; }

    public virtual DbSet<SellerReview> SellerReviews { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Wishlist> Wishlists { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=EIDTH;Database=MajhiPaithaniDBDEMO;User Id=sa;Password=infitech;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AdminLog>(entity =>
        {
            entity.HasKey(e => e.ILogId).HasName("PK__AdminLog__BBE56A58B7D3C77D");

            entity.Property(e => e.ILogId).HasColumnName("iLogId");
            entity.Property(e => e.DCreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("dCreatedDate");
            entity.Property(e => e.IAdminUserId).HasColumnName("iAdminUserId");
            entity.Property(e => e.SAction)
                .HasMaxLength(500)
                .HasColumnName("sAction");
            entity.Property(e => e.SDescription).HasColumnName("sDescription");
        });

        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasKey(e => e.ICartId).HasName("PK__Cart__7B40985777FFF8B8");

            entity.ToTable("Cart");

            entity.Property(e => e.ICartId).HasColumnName("iCartId");
            entity.Property(e => e.DCreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("dCreatedDate");
            entity.Property(e => e.DUpdatedDate).HasColumnName("dUpdatedDate");
            entity.Property(e => e.ICustomerId).HasColumnName("iCustomerId");
        });

        modelBuilder.Entity<CartItem>(entity =>
        {
            entity.HasKey(e => e.ICartItemId).HasName("PK__CartItem__FFFAC7A81BD443CD");

            entity.Property(e => e.ICartItemId).HasColumnName("iCartItemId");
            entity.Property(e => e.DCreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("dCreatedDate");
            entity.Property(e => e.DcPrice)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("dcPrice");
            entity.Property(e => e.ICartId).HasColumnName("iCartId");
            entity.Property(e => e.IProductId).HasColumnName("iProductId");
            entity.Property(e => e.IQuantity).HasColumnName("iQuantity");
            entity.Property(e => e.ISellerId).HasColumnName("iSellerId");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.ICategoryId).HasName("PK__Categori__342A080C79EA8501");

            entity.Property(e => e.ICategoryId).HasColumnName("iCategoryId");
            entity.Property(e => e.BIsActive)
                .HasDefaultValue(true)
                .HasColumnName("bIsActive");
            entity.Property(e => e.BIsDeleted)
                .HasDefaultValue(false)
                .HasColumnName("bIsDeleted");
            entity.Property(e => e.DCreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("dCreatedDate");
            entity.Property(e => e.DDeletedDate).HasColumnName("dDeletedDate");
            entity.Property(e => e.SCategoryName)
                .HasMaxLength(200)
                .HasColumnName("sCategoryName");
            entity.Property(e => e.SDescription)
                .HasMaxLength(500)
                .HasColumnName("sDescription");
        });

        modelBuilder.Entity<ChatMessage>(entity =>
        {
            entity.HasKey(e => e.IMessageId).HasName("PK__ChatMess__93BB44EB74366B16");

            entity.Property(e => e.IMessageId).HasColumnName("iMessageId");
            entity.Property(e => e.DSentDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("dSentDate");
            entity.Property(e => e.IChatRoomId).HasColumnName("iChatRoomId");
            entity.Property(e => e.ISenderUserId).HasColumnName("iSenderUserId");
            entity.Property(e => e.SAttachmentUrl)
                .HasMaxLength(500)
                .HasColumnName("sAttachmentUrl");
            entity.Property(e => e.SMessage).HasColumnName("sMessage");
        });

        modelBuilder.Entity<ChatRoom>(entity =>
        {
            entity.HasKey(e => e.IChatRoomId).HasName("PK__ChatRoom__DB1BA504B08C0155");

            entity.Property(e => e.IChatRoomId).HasColumnName("iChatRoomId");
            entity.Property(e => e.DCreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("dCreatedDate");
            entity.Property(e => e.ICustomerId).HasColumnName("iCustomerId");
            entity.Property(e => e.ISellerId).HasColumnName("iSellerId");
        });

        modelBuilder.Entity<Coupon>(entity =>
        {
            entity.HasKey(e => e.ICouponId).HasName("PK__Coupons__FEF61934271E728B");

            entity.Property(e => e.ICouponId).HasColumnName("iCouponId");
            entity.Property(e => e.BIsActive)
                .HasDefaultValue(true)
                .HasColumnName("bIsActive");
            entity.Property(e => e.BIsDeleted)
                .HasDefaultValue(false)
                .HasColumnName("bIsDeleted");
            entity.Property(e => e.DCreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("dCreatedDate");
            entity.Property(e => e.DEndDate).HasColumnName("dEndDate");
            entity.Property(e => e.DStartDate).HasColumnName("dStartDate");
            entity.Property(e => e.DcDiscountAmount)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("dcDiscountAmount");
            entity.Property(e => e.SCouponCode)
                .HasMaxLength(100)
                .HasColumnName("sCouponCode");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.ICustomerId).HasName("PK__Customer__EA0B906C0EBFFEED");

            entity.Property(e => e.ICustomerId).HasColumnName("iCustomerId");
            entity.Property(e => e.BIsDeleted)
                .HasDefaultValue(false)
                .HasColumnName("bIsDeleted");
            entity.Property(e => e.DCreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("dCreatedDate");
            entity.Property(e => e.DDeletedDate).HasColumnName("dDeletedDate");
            entity.Property(e => e.IUserId).HasColumnName("iUserId");
        });

        modelBuilder.Entity<CustomerAddress>(entity =>
        {
            entity.HasKey(e => e.IAddressId).HasName("PK__Customer__C6E4D70D1247776F");

            entity.Property(e => e.IAddressId).HasColumnName("iAddressId");
            entity.Property(e => e.BIsDefault)
                .HasDefaultValue(false)
                .HasColumnName("bIsDefault");
            entity.Property(e => e.BIsDeleted)
                .HasDefaultValue(false)
                .HasColumnName("bIsDeleted");
            entity.Property(e => e.DCreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("dCreatedDate");
            entity.Property(e => e.DDeletedDate).HasColumnName("dDeletedDate");
            entity.Property(e => e.ICustomerId).HasColumnName("iCustomerId");
            entity.Property(e => e.SAddressLine1)
                .HasMaxLength(500)
                .HasColumnName("sAddressLine1");
            entity.Property(e => e.SAddressLine2)
                .HasMaxLength(500)
                .HasColumnName("sAddressLine2");
            entity.Property(e => e.SCity)
                .HasMaxLength(150)
                .HasColumnName("sCity");
            entity.Property(e => e.SFullName)
                .HasMaxLength(200)
                .HasColumnName("sFullName");
            entity.Property(e => e.SPhoneNumber)
                .HasMaxLength(50)
                .HasColumnName("sPhoneNumber");
            entity.Property(e => e.SPincode)
                .HasMaxLength(20)
                .HasColumnName("sPincode");
            entity.Property(e => e.SState)
                .HasMaxLength(150)
                .HasColumnName("sState");
        });

        modelBuilder.Entity<CustomizationRequest>(entity =>
        {
            entity.HasKey(e => e.ICustomizationRequestId).HasName("PK__Customiz__99534C10A0E6A198");

            entity.Property(e => e.ICustomizationRequestId).HasColumnName("iCustomizationRequestId");
            entity.Property(e => e.DCreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("dCreatedDate");
            entity.Property(e => e.ICustomerId).HasColumnName("iCustomerId");
            entity.Property(e => e.IProductId).HasColumnName("iProductId");
            entity.Property(e => e.SCustomizationDetails).HasColumnName("sCustomizationDetails");
            entity.Property(e => e.SReferenceImageUrl)
                .HasMaxLength(500)
                .HasColumnName("sReferenceImageUrl");
            entity.Property(e => e.SStatus)
                .HasMaxLength(50)
                .HasColumnName("sStatus");
        });

        modelBuilder.Entity<Location>(entity =>
        {
            entity.HasKey(e => e.ILocationId).HasName("PK__Location__1AC9AF763EAAB00E");

            entity.Property(e => e.ILocationId).HasColumnName("iLocationId");
            entity.Property(e => e.BIsActive)
                .HasDefaultValue(true)
                .HasColumnName("bIsActive");
            entity.Property(e => e.BIsDeleted)
                .HasDefaultValue(false)
                .HasColumnName("bIsDeleted");
            entity.Property(e => e.DCreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("dCreatedDate");
            entity.Property(e => e.SCity)
                .HasMaxLength(150)
                .HasColumnName("sCity");
            entity.Property(e => e.SDistrict)
                .HasMaxLength(150)
                .HasColumnName("sDistrict");
            entity.Property(e => e.SPincode)
                .HasMaxLength(20)
                .HasColumnName("sPincode");
            entity.Property(e => e.SState)
                .HasMaxLength(150)
                .HasColumnName("sState");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.INotificationId).HasName("PK__Notifica__84A3165B3A29EE20");

            entity.Property(e => e.INotificationId).HasColumnName("iNotificationId");
            entity.Property(e => e.BIsRead)
                .HasDefaultValue(false)
                .HasColumnName("bIsRead");
            entity.Property(e => e.DCreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("dCreatedDate");
            entity.Property(e => e.IUserId).HasColumnName("iUserId");
            entity.Property(e => e.SMessage).HasColumnName("sMessage");
            entity.Property(e => e.SNotificationType)
                .HasMaxLength(100)
                .HasColumnName("sNotificationType");
            entity.Property(e => e.STitle)
                .HasMaxLength(300)
                .HasColumnName("sTitle");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.IOrderId).HasName("PK__Orders__F354140FD000231A");

            entity.Property(e => e.IOrderId).HasColumnName("iOrderId");
            entity.Property(e => e.DCreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("dCreatedDate");
            entity.Property(e => e.DDeliveryDate).HasColumnName("dDeliveryDate");
            entity.Property(e => e.DOrderDate).HasColumnName("dOrderDate");
            entity.Property(e => e.DcTotalAmount)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("dcTotalAmount");
            entity.Property(e => e.ICustomerId).HasColumnName("iCustomerId");
            entity.Property(e => e.SOrderStatus)
                .HasMaxLength(50)
                .HasColumnName("sOrderStatus");
            entity.Property(e => e.SPaymentStatus)
                .HasMaxLength(50)
                .HasColumnName("sPaymentStatus");
        });

        modelBuilder.Entity<OrderCoupon>(entity =>
        {
            entity.HasKey(e => e.IOrderCouponId).HasName("PK__OrderCou__120A39F357494FD6");

            entity.Property(e => e.IOrderCouponId).HasColumnName("iOrderCouponId");
            entity.Property(e => e.DcDiscountAmount)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("dcDiscountAmount");
            entity.Property(e => e.ICouponId).HasColumnName("iCouponId");
            entity.Property(e => e.IOrderId).HasColumnName("iOrderId");
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(e => e.IOrderItemId).HasName("PK__OrderIte__02504D79E65F42FB");

            entity.Property(e => e.IOrderItemId).HasColumnName("iOrderItemId");
            entity.Property(e => e.DcPrice)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("dcPrice");
            entity.Property(e => e.DcTotalPrice)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("dcTotalPrice");
            entity.Property(e => e.IOrderId).HasColumnName("iOrderId");
            entity.Property(e => e.IProductId).HasColumnName("iProductId");
            entity.Property(e => e.IQuantity).HasColumnName("iQuantity");
            entity.Property(e => e.ISellerId).HasColumnName("iSellerId");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.IPaymentId).HasName("PK__Payments__D6E7F12201069949");

            entity.Property(e => e.IPaymentId).HasColumnName("iPaymentId");
            entity.Property(e => e.DPaymentDate).HasColumnName("dPaymentDate");
            entity.Property(e => e.DcAmount)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("dcAmount");
            entity.Property(e => e.IOrderId).HasColumnName("iOrderId");
            entity.Property(e => e.SPaymentMethod)
                .HasMaxLength(100)
                .HasColumnName("sPaymentMethod");
            entity.Property(e => e.SPaymentStatus)
                .HasMaxLength(50)
                .HasColumnName("sPaymentStatus");
            entity.Property(e => e.STransactionId)
                .HasMaxLength(300)
                .HasColumnName("sTransactionId");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.IProductId).HasName("PK__Products__2A611C8457938B60");

            entity.Property(e => e.IProductId).HasColumnName("iProductId");
            entity.Property(e => e.BIsActive)
                .HasDefaultValue(true)
                .HasColumnName("bIsActive");
            entity.Property(e => e.BIsCustomizationAvailable)
                .HasDefaultValue(false)
                .HasColumnName("bIsCustomizationAvailable");
            entity.Property(e => e.BIsDeleted)
                .HasDefaultValue(false)
                .HasColumnName("bIsDeleted");
            entity.Property(e => e.DCreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("dCreatedDate");
            entity.Property(e => e.DDeletedDate).HasColumnName("dDeletedDate");
            entity.Property(e => e.DUpdatedDate).HasColumnName("dUpdatedDate");
            entity.Property(e => e.DcBasePrice)
                .HasColumnType("decimal(18, 2)")
                .HasColumnName("dcBasePrice");
            entity.Property(e => e.ICategoryId).HasColumnName("iCategoryId");
            entity.Property(e => e.ISellerId).HasColumnName("iSellerId");
            entity.Property(e => e.SColor)
                .HasMaxLength(100)
                .HasColumnName("sColor");
            entity.Property(e => e.SDescription).HasColumnName("sDescription");
            entity.Property(e => e.SDesignType)
                .HasMaxLength(100)
                .HasColumnName("sDesignType");
            entity.Property(e => e.SFabric)
                .HasMaxLength(100)
                .HasColumnName("sFabric");
            entity.Property(e => e.SProductTitle)
                .HasMaxLength(300)
                .HasColumnName("sProductTitle");
        });

        modelBuilder.Entity<ProductImage>(entity =>
        {
            entity.HasKey(e => e.IImageId).HasName("PK__ProductI__EEA8AE34B628B178");

            entity.Property(e => e.IImageId).HasColumnName("iImageId");
            entity.Property(e => e.BIsPrimary)
                .HasDefaultValue(false)
                .HasColumnName("bIsPrimary");
            entity.Property(e => e.DCreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("dCreatedDate");
            entity.Property(e => e.IProductId).HasColumnName("iProductId");
            entity.Property(e => e.SImageUrl)
                .HasMaxLength(500)
                .HasColumnName("sImageUrl");
        });

        modelBuilder.Entity<ProductReview>(entity =>
        {
            entity.HasKey(e => e.IReviewId).HasName("PK__ProductR__2339F98883394C5A");

            entity.Property(e => e.IReviewId).HasColumnName("iReviewId");
            entity.Property(e => e.DCreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("dCreatedDate");
            entity.Property(e => e.ICustomerId).HasColumnName("iCustomerId");
            entity.Property(e => e.IProductId).HasColumnName("iProductId");
            entity.Property(e => e.IRating).HasColumnName("iRating");
            entity.Property(e => e.SComment)
                .HasMaxLength(1000)
                .HasColumnName("sComment");
        });

        modelBuilder.Entity<ProductTag>(entity =>
        {
            entity.HasKey(e => e.ITagId).HasName("PK__ProductT__40490F4C826BB1FE");

            entity.Property(e => e.ITagId).HasColumnName("iTagId");
            entity.Property(e => e.BIsDeleted)
                .HasDefaultValue(false)
                .HasColumnName("bIsDeleted");
            entity.Property(e => e.DCreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("dCreatedDate");
            entity.Property(e => e.STagName)
                .HasMaxLength(150)
                .HasColumnName("sTagName");
        });

        modelBuilder.Entity<ProductTagMapping>(entity =>
        {
            entity.HasKey(e => e.IMappingId).HasName("PK__ProductT__51ECBB1DBCEF2A24");

            entity.ToTable("ProductTagMapping");

            entity.Property(e => e.IMappingId).HasColumnName("iMappingId");
            entity.Property(e => e.IProductId).HasColumnName("iProductId");
            entity.Property(e => e.ITagId).HasColumnName("iTagId");
        });

        modelBuilder.Entity<ProductView>(entity =>
        {
            entity.HasKey(e => e.IViewId).HasName("PK__ProductV__D937E72EF6D288E0");

            entity.Property(e => e.IViewId).HasColumnName("iViewId");
            entity.Property(e => e.DViewedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("dViewedDate");
            entity.Property(e => e.IProductId).HasColumnName("iProductId");
            entity.Property(e => e.IUserId).HasColumnName("iUserId");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.IRoleId).HasName("PK__Roles__D69F8C9E6DE6E80B");

            entity.Property(e => e.IRoleId).HasColumnName("iRoleId");
            entity.Property(e => e.BIsActive)
                .HasDefaultValue(true)
                .HasColumnName("bIsActive");
            entity.Property(e => e.DCreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("dCreatedDate");
            entity.Property(e => e.SRoleName)
                .HasMaxLength(100)
                .HasColumnName("sRoleName");
        });

        modelBuilder.Entity<SearchHistory>(entity =>
        {
            entity.HasKey(e => e.ISearchId).HasName("PK__SearchHi__1D51F56AC4A9082D");

            entity.ToTable("SearchHistory");

            entity.Property(e => e.ISearchId).HasColumnName("iSearchId");
            entity.Property(e => e.DSearchDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("dSearchDate");
            entity.Property(e => e.IUserId).HasColumnName("iUserId");
            entity.Property(e => e.SSearchKeyword)
                .HasMaxLength(300)
                .HasColumnName("sSearchKeyword");
        });

        modelBuilder.Entity<Seller>(entity =>
        {
            entity.HasKey(e => e.ISellerId).HasName("PK__Sellers__99BF1FD4CE2D8F7E");

            entity.Property(e => e.ISellerId).HasColumnName("iSellerId");
            entity.Property(e => e.BIsActive)
                .HasDefaultValue(true)
                .HasColumnName("bIsActive");
            entity.Property(e => e.BIsDeleted)
                .HasDefaultValue(false)
                .HasColumnName("bIsDeleted");
            entity.Property(e => e.BIsVerified)
                .HasDefaultValue(false)
                .HasColumnName("bIsVerified");
            entity.Property(e => e.DCreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("dCreatedDate");
            entity.Property(e => e.DDeletedDate).HasColumnName("dDeletedDate");
            entity.Property(e => e.DUpdatedDate).HasColumnName("dUpdatedDate");
            entity.Property(e => e.ILocationId).HasColumnName("iLocationId");
            entity.Property(e => e.IUserId).HasColumnName("iUserId");
            entity.Property(e => e.SShopDescription)
                .HasMaxLength(1000)
                .HasColumnName("sShopDescription");
            entity.Property(e => e.SShopName)
                .HasMaxLength(200)
                .HasColumnName("sShopName");
        });

        modelBuilder.Entity<SellerBankDetail>(entity =>
        {
            entity.HasKey(e => e.IbankDetailId).HasName("PK__SellerBa__AD3D267EE3DBF786");

            entity.Property(e => e.IbankDetailId).HasColumnName("IBankDetailId");
            entity.Property(e => e.DcreatedDate)
                .HasDefaultValueSql("(getutcdate())")
                .HasColumnType("datetime")
                .HasColumnName("DCreatedDate");
            entity.Property(e => e.DupdatedDate)
                .HasColumnType("datetime")
                .HasColumnName("DUpdatedDate");
            entity.Property(e => e.IsellerId).HasColumnName("ISellerId");
            entity.Property(e => e.SaccountHolderName)
                .HasMaxLength(150)
                .HasColumnName("SAccountHolderName");
            entity.Property(e => e.SaccountNumber)
                .HasMaxLength(50)
                .HasColumnName("SAccountNumber");
            entity.Property(e => e.SbankName)
                .HasMaxLength(150)
                .HasColumnName("SBankName");
            entity.Property(e => e.Sifsccode)
                .HasMaxLength(20)
                .HasColumnName("SIFSCCode");
        });

        modelBuilder.Entity<SellerDocument>(entity =>
        {
            entity.HasKey(e => e.IDocumentId).HasName("PK__SellerDo__336026B2D7BB52B8");

            entity.Property(e => e.IDocumentId).HasColumnName("iDocumentId");
            entity.Property(e => e.BIsVerified)
                .HasDefaultValue(false)
                .HasColumnName("bIsVerified");
            entity.Property(e => e.DUploadedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("dUploadedDate");
            entity.Property(e => e.ISellerId).HasColumnName("iSellerId");
            entity.Property(e => e.SDocumentType)
                .HasMaxLength(150)
                .HasColumnName("sDocumentType");
            entity.Property(e => e.SDocumentUrl)
                .HasMaxLength(500)
                .HasColumnName("sDocumentUrl");
        });

        modelBuilder.Entity<SellerReview>(entity =>
        {
            entity.HasKey(e => e.ISellerReviewId).HasName("PK__SellerRe__95A25099EF6AD721");

            entity.Property(e => e.ISellerReviewId).HasColumnName("iSellerReviewId");
            entity.Property(e => e.DCreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("dCreatedDate");
            entity.Property(e => e.ICustomerId).HasColumnName("iCustomerId");
            entity.Property(e => e.IRating).HasColumnName("iRating");
            entity.Property(e => e.ISellerId).HasColumnName("iSellerId");
            entity.Property(e => e.SComment)
                .HasMaxLength(1000)
                .HasColumnName("sComment");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.IUserId).HasName("PK__Users__BA95FFB1C0146444");

            entity.Property(e => e.IUserId).HasColumnName("iUserId");
            entity.Property(e => e.BIsActive)
                .HasDefaultValue(true)
                .HasColumnName("bIsActive");
            entity.Property(e => e.BIsDeleted)
                .HasDefaultValue(false)
                .HasColumnName("bIsDeleted");
            entity.Property(e => e.DCreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("dCreatedDate");
            entity.Property(e => e.DDeletedDate).HasColumnName("dDeletedDate");
            entity.Property(e => e.DUpdatedDate).HasColumnName("dUpdatedDate");
            entity.Property(e => e.IRoleId).HasColumnName("iRoleId");
            entity.Property(e => e.SEmail)
                .HasMaxLength(255)
                .HasColumnName("sEmail");
            entity.Property(e => e.SFirstName)
                .HasMaxLength(150)
                .HasColumnName("sFirstName");
            entity.Property(e => e.SLastName)
                .HasMaxLength(150)
                .HasColumnName("sLastName");
            entity.Property(e => e.SPasswordHash)
                .HasMaxLength(500)
                .HasColumnName("sPasswordHash");
            entity.Property(e => e.SPhoneNumber)
                .HasMaxLength(50)
                .HasColumnName("sPhoneNumber");
        });

        modelBuilder.Entity<Wishlist>(entity =>
        {
            entity.HasKey(e => e.IWishlistId).HasName("PK__Wishlist__AD0A7D289C37FC4F");

            entity.ToTable("Wishlist");

            entity.Property(e => e.IWishlistId).HasColumnName("iWishlistId");
            entity.Property(e => e.DCreatedDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnName("dCreatedDate");
            entity.Property(e => e.IProductId).HasColumnName("iProductId");
            entity.Property(e => e.IUserId).HasColumnName("iUserId");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
