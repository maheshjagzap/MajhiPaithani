using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MajhiPaithani.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialBaseline : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdminLogs",
                columns: table => new
                {
                    iLogId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    iAdminUserId = table.Column<int>(type: "int", nullable: true),
                    sAction = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    sDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    dCreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__AdminLog__BBE56A58B7D3C77D", x => x.iLogId);
                });

            migrationBuilder.CreateTable(
                name: "Cart",
                columns: table => new
                {
                    iCartId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    iCustomerId = table.Column<int>(type: "int", nullable: true),
                    dCreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(getdate())"),
                    dUpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Cart__7B40985777FFF8B8", x => x.iCartId);
                });

            migrationBuilder.CreateTable(
                name: "CartItems",
                columns: table => new
                {
                    iCartItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    iCartId = table.Column<int>(type: "int", nullable: true),
                    iProductId = table.Column<int>(type: "int", nullable: true),
                    iSellerId = table.Column<int>(type: "int", nullable: true),
                    iQuantity = table.Column<int>(type: "int", nullable: true),
                    dcPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    dCreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__CartItem__FFFAC7A81BD443CD", x => x.iCartItemId);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    iCategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    sCategoryName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    sDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    bIsActive = table.Column<bool>(type: "bit", nullable: true, defaultValue: true),
                    bIsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    dCreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(getdate())"),
                    dDeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Categori__342A080C79EA8501", x => x.iCategoryId);
                });

            migrationBuilder.CreateTable(
                name: "ChatMessages",
                columns: table => new
                {
                    iMessageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    iChatRoomId = table.Column<int>(type: "int", nullable: true),
                    iSenderUserId = table.Column<int>(type: "int", nullable: true),
                    sMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sAttachmentUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    dSentDate = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ChatMess__93BB44EB74366B16", x => x.iMessageId);
                });

            migrationBuilder.CreateTable(
                name: "ChatRooms",
                columns: table => new
                {
                    iChatRoomId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    iCustomerId = table.Column<int>(type: "int", nullable: true),
                    iSellerId = table.Column<int>(type: "int", nullable: true),
                    dCreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ChatRoom__DB1BA504B08C0155", x => x.iChatRoomId);
                });

            migrationBuilder.CreateTable(
                name: "Coupons",
                columns: table => new
                {
                    iCouponId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    sCouponCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    dcDiscountAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    dStartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    dEndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    bIsActive = table.Column<bool>(type: "bit", nullable: true, defaultValue: true),
                    bIsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    dCreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Coupons__FEF61934271E728B", x => x.iCouponId);
                });

            migrationBuilder.CreateTable(
                name: "CustomerAddresses",
                columns: table => new
                {
                    iAddressId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    iCustomerId = table.Column<int>(type: "int", nullable: true),
                    sFullName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    sPhoneNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    sAddressLine1 = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    sAddressLine2 = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    sCity = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    sState = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    sPincode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    bIsDefault = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    bIsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    dCreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(getdate())"),
                    dDeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Customer__C6E4D70D1247776F", x => x.iAddressId);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    iCustomerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    iUserId = table.Column<int>(type: "int", nullable: true),
                    bIsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    dCreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(getdate())"),
                    dDeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Customer__EA0B906C0EBFFEED", x => x.iCustomerId);
                });

            migrationBuilder.CreateTable(
                name: "CustomizationRequests",
                columns: table => new
                {
                    iCustomizationRequestId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    iCustomerId = table.Column<int>(type: "int", nullable: true),
                    iProductId = table.Column<int>(type: "int", nullable: true),
                    sCustomizationDetails = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sReferenceImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    sStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    dCreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Customiz__99534C10A0E6A198", x => x.iCustomizationRequestId);
                });

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    iLocationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    sCity = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    sDistrict = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    sState = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    sPincode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    bIsActive = table.Column<bool>(type: "bit", nullable: true, defaultValue: true),
                    bIsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    dCreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Location__1AC9AF763EAAB00E", x => x.iLocationId);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    iNotificationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    iUserId = table.Column<int>(type: "int", nullable: true),
                    sTitle = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    sMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    sNotificationType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    bIsRead = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    dCreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Notifica__84A3165B3A29EE20", x => x.iNotificationId);
                });

            migrationBuilder.CreateTable(
                name: "OrderCoupons",
                columns: table => new
                {
                    iOrderCouponId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    iOrderId = table.Column<int>(type: "int", nullable: true),
                    iCouponId = table.Column<int>(type: "int", nullable: true),
                    dcDiscountAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__OrderCou__120A39F357494FD6", x => x.iOrderCouponId);
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    iOrderItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    iOrderId = table.Column<int>(type: "int", nullable: true),
                    iProductId = table.Column<int>(type: "int", nullable: true),
                    iSellerId = table.Column<int>(type: "int", nullable: true),
                    iQuantity = table.Column<int>(type: "int", nullable: true),
                    dcPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    dcTotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__OrderIte__02504D79E65F42FB", x => x.iOrderItemId);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    iOrderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    iCustomerId = table.Column<int>(type: "int", nullable: true),
                    dcTotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    sOrderStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    sPaymentStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    dOrderDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    dDeliveryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    dCreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Orders__F354140FD000231A", x => x.iOrderId);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    iPaymentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    iOrderId = table.Column<int>(type: "int", nullable: true),
                    sPaymentMethod = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    dcAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    sTransactionId = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    sPaymentStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    dPaymentDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Payments__D6E7F12201069949", x => x.iPaymentId);
                });

            migrationBuilder.CreateTable(
                name: "Productdemos",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    StockAvailable = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Productdemos", x => x.ProductId);
                });

            migrationBuilder.CreateTable(
                name: "ProductImages",
                columns: table => new
                {
                    iImageId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    iProductId = table.Column<int>(type: "int", nullable: true),
                    sImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    bIsPrimary = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    dCreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ProductI__EEA8AE34B628B178", x => x.iImageId);
                });

            migrationBuilder.CreateTable(
                name: "ProductReviews",
                columns: table => new
                {
                    iReviewId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    iProductId = table.Column<int>(type: "int", nullable: true),
                    iCustomerId = table.Column<int>(type: "int", nullable: true),
                    iRating = table.Column<int>(type: "int", nullable: true),
                    sComment = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    dCreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ProductR__2339F98883394C5A", x => x.iReviewId);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    iProductId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    iSellerId = table.Column<int>(type: "int", nullable: true),
                    iCategoryId = table.Column<int>(type: "int", nullable: true),
                    sProductTitle = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    sDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    dcBasePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    sColor = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    sFabric = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    sDesignType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    bIsCustomizationAvailable = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    bIsActive = table.Column<bool>(type: "bit", nullable: true, defaultValue: true),
                    bIsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    dCreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(getdate())"),
                    dUpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    dDeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Products__2A611C8457938B60", x => x.iProductId);
                });

            migrationBuilder.CreateTable(
                name: "ProductTagMapping",
                columns: table => new
                {
                    iMappingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    iProductId = table.Column<int>(type: "int", nullable: true),
                    iTagId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ProductT__51ECBB1DBCEF2A24", x => x.iMappingId);
                });

            migrationBuilder.CreateTable(
                name: "ProductTags",
                columns: table => new
                {
                    iTagId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    sTagName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    bIsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    dCreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ProductT__40490F4C826BB1FE", x => x.iTagId);
                });

            migrationBuilder.CreateTable(
                name: "ProductViews",
                columns: table => new
                {
                    iViewId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    iUserId = table.Column<int>(type: "int", nullable: true),
                    iProductId = table.Column<int>(type: "int", nullable: true),
                    dViewedDate = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__ProductV__D937E72EF6D288E0", x => x.iViewId);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    iRoleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    sRoleName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    bIsActive = table.Column<bool>(type: "bit", nullable: true, defaultValue: true),
                    dCreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Roles__D69F8C9E6DE6E80B", x => x.iRoleId);
                });

            migrationBuilder.CreateTable(
                name: "SearchHistory",
                columns: table => new
                {
                    iSearchId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    iUserId = table.Column<int>(type: "int", nullable: true),
                    sSearchKeyword = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    dSearchDate = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__SearchHi__1D51F56AC4A9082D", x => x.iSearchId);
                });

            migrationBuilder.CreateTable(
                name: "SellerBankDetails",
                columns: table => new
                {
                    IBankDetailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ISellerId = table.Column<int>(type: "int", nullable: false),
                    SAccountHolderName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    SAccountNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SIFSCCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    SBankName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    DCreatedDate = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getutcdate())"),
                    DUpdatedDate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__SellerBa__AD3D267EE3DBF786", x => x.IBankDetailId);
                });

            migrationBuilder.CreateTable(
                name: "SellerDocuments",
                columns: table => new
                {
                    iDocumentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    iSellerId = table.Column<int>(type: "int", nullable: true),
                    sDocumentType = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    sDocumentUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    bIsVerified = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    dUploadedDate = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__SellerDo__336026B2D7BB52B8", x => x.iDocumentId);
                });

            migrationBuilder.CreateTable(
                name: "SellerReviews",
                columns: table => new
                {
                    iSellerReviewId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    iSellerId = table.Column<int>(type: "int", nullable: true),
                    iCustomerId = table.Column<int>(type: "int", nullable: true),
                    iRating = table.Column<int>(type: "int", nullable: true),
                    sComment = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    dCreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__SellerRe__95A25099EF6AD721", x => x.iSellerReviewId);
                });

            migrationBuilder.CreateTable(
                name: "Sellers",
                columns: table => new
                {
                    iSellerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    iUserId = table.Column<int>(type: "int", nullable: false),
                    sShopName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    sShopDescription = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    iLocationId = table.Column<int>(type: "int", nullable: false),
                    bIsVerified = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    bIsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    bIsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    dCreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(getdate())"),
                    dUpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    dDeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Sellers__99BF1FD4CE2D8F7E", x => x.iSellerId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    iUserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    sFirstName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    sLastName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    sEmail = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    sPhoneNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    sPasswordHash = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    iRoleId = table.Column<int>(type: "int", nullable: false),
                    bIsActive = table.Column<bool>(type: "bit", nullable: true, defaultValue: true),
                    bIsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    dCreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(getdate())"),
                    dUpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    dDeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Users__BA95FFB1C0146444", x => x.iUserId);
                });

            migrationBuilder.CreateTable(
                name: "Wishlist",
                columns: table => new
                {
                    iWishlistId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    iUserId = table.Column<int>(type: "int", nullable: true),
                    iProductId = table.Column<int>(type: "int", nullable: true),
                    dCreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Wishlist__AD0A7D289C37FC4F", x => x.iWishlistId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdminLogs");

            migrationBuilder.DropTable(
                name: "Cart");

            migrationBuilder.DropTable(
                name: "CartItems");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "ChatMessages");

            migrationBuilder.DropTable(
                name: "ChatRooms");

            migrationBuilder.DropTable(
                name: "Coupons");

            migrationBuilder.DropTable(
                name: "CustomerAddresses");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "CustomizationRequests");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "OrderCoupons");

            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "Productdemos");

            migrationBuilder.DropTable(
                name: "ProductImages");

            migrationBuilder.DropTable(
                name: "ProductReviews");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "ProductTagMapping");

            migrationBuilder.DropTable(
                name: "ProductTags");

            migrationBuilder.DropTable(
                name: "ProductViews");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "SearchHistory");

            migrationBuilder.DropTable(
                name: "SellerBankDetails");

            migrationBuilder.DropTable(
                name: "SellerDocuments");

            migrationBuilder.DropTable(
                name: "SellerReviews");

            migrationBuilder.DropTable(
                name: "Sellers");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Wishlist");
        }
    }
}
