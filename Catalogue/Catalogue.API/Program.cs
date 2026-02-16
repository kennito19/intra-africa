using Catalogue.API.Filter;
using Catalogue.API.Middleware;
using Catalogue.Application;
using Catalogue.Application.IRepositories;
using Catalogue.Application.IServices;
using Catalogue.Application.Services;
using Catalogue.Infrastructure.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

//Valid Project scopes
string[] Projectscopes = new string[] {"catelogue.read","catelogue.write","catelogue.update","catelogue.delete"};
string[] generalScope = new string[] { "general" };

//Encryption Loader is required to get token decrypted and validate
var EncLoader = new AuthHelper();

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

builder.Services.AddScoped<IColorRepository, ColorRepository>();
builder.Services.AddScoped<IColorService, ColorService>();

builder.Services.AddScoped<ISizeRepository, SizeRepository>();
builder.Services.AddScoped<ISizeService, SizeService>();

builder.Services.AddScoped<IHSNCodeRepository, HSNCodeRepository>();
builder.Services.AddScoped<IHSNCodeService, HSNCodeService>();

builder.Services.AddScoped<ITaxTypeRespository, TaxTypeRepository>();
builder.Services.AddScoped<ITaxTypeService, TaxTypeService>();

builder.Services.AddScoped<ITaxTypeValueRepository, TaxTypeValueRespository>();
builder.Services.AddScoped<ITaxTypeValueService, TaxTypeValueService>();

builder.Services.AddScoped<IWeightSlabRepository, WeightSlabRepository>();
builder.Services.AddScoped<IWeightSlabService, WeightSlabService>();

builder.Services.AddScoped<IAssignSpecToCategoryRepository, AssignSpecToCategoryRepository>();
builder.Services.AddScoped<IAssignSpecToCategoryService, AssignSpecToCategoryService>();

builder.Services.AddScoped<IAssignSizeValueToCategoryRepository, AssignSizeValueToCategoryRepository>();
builder.Services.AddScoped<IAssignSizeValueToCategoryService, AssignSizeValueToCategoryService>();

builder.Services.AddScoped<IProductRollbackRepository, ProductRollbackRepository>();
builder.Services.AddScoped<IProductRollbackService, ProductRollbackService>();

builder.Services.AddScoped<IProductsRepository, ProductsRepository>();
builder.Services.AddScoped<IProductsService, ProductsService>();

builder.Services.AddScoped<IProductPriceMasterRepository, ProductPriceMasterRepository>();
builder.Services.AddScoped<IProductPriceMasterService, ProductPriceMasterService>();

builder.Services.AddScoped<ISellerProductRepository, SellerProductRepository>();
builder.Services.AddScoped<ISellerProductService, SellerProductService>();

builder.Services.AddScoped<IProductWarehouseRepository, ProductWarehouseRespository>();
builder.Services.AddScoped<IProductWarehouseService, ProductWarehouseService>();


builder.Services.AddScoped<IProductsImagesRepository, ProductsImagesRepository>();
builder.Services.AddScoped<IProductsImagesService, ProductsImagesService>();

//builder.Services.AddScoped<IProductsVideoLinksRepository, ProductsVideoLinksRepository>();
//builder.Services.AddScoped<IProductsVideoLinksService, ProductsVideoLinksService>();

builder.Services.AddScoped<IProductsColorMappingRepository, ProductsColorMappingRepository>();
builder.Services.AddScoped<IProductsColorMappingService, ProductsColorMappingService>();

builder.Services.AddScoped<IReturnPolicyRepository, ReturnPolicyRepository>();
builder.Services.AddScoped<IReturnPolicyService, ReturnPolicyService>();

builder.Services.AddScoped<IReturnPolicyDetailRepository, ReturnPolicyDetailRepository>();
builder.Services.AddScoped<IReturnPolicyDetailService, ReturnPolicyDetailService>();

builder.Services.AddScoped<IAssignReturnPolicyToCatagoryRepository, AssignReturnPolicyToCatagoryRepository>();
builder.Services.AddScoped<IAssignReturnPolicyToCatagoryService, AssignReturnPolicyToCatagoryService>();

builder.Services.AddScoped<IManageLayoutsRepository, ManageLayoutsRepository>();
builder.Services.AddScoped<IManageLayoutsService, ManageLayoutsService>();

builder.Services.AddScoped<IManageLayoutTypesRepository, ManageLayoutTypesRepository>();
builder.Services.AddScoped<IManageLayoutTypesService, ManageLayoutTypesService>();

builder.Services.AddScoped<IManageLayoutTypesDetailsRepository, ManageLayoutTypesDetailsRepository>();
builder.Services.AddScoped<IManageLayoutTypesDetailsService, ManageLayoutTypesDetailsService>();

builder.Services.AddScoped<IManageHomePageSectionsRepository, ManageHomePageSectionsRepository>();
builder.Services.AddScoped<IManageHomePageSectionsService, ManageHomePageSectionsService>();

builder.Services.AddScoped<IManageHomePageDetailsRepository, ManageHomePageDetailsRepository>();
builder.Services.AddScoped<IManageHomePageDetailsService, ManageHomePageDetailsService>();

builder.Services.AddScoped<IManageStaticPagesRepository, ManageStaticPagesRepository>();
builder.Services.AddScoped<IManageStaticPagesService, ManageStaticPagesService>();


builder.Services.AddScoped<IManageHeaderMenuRepository, ManageHeaderMenuRepository>();
builder.Services.AddScoped<IManageHeaderMenuService, ManageHeaderMenuService>();

builder.Services.AddScoped<IManageSubMenuRepository, ManageSubMenuRepository>();
builder.Services.AddScoped<IManageSubMenuService, ManageSubMenuService>();

builder.Services.AddScoped<IManageConfigKeyRepository, ManageConfigKeyRepository>();
builder.Services.AddScoped<IManageConfigKeyService, ManageConfigKeyService>();

builder.Services.AddScoped<IManageConfigValueRepository, ManageConfigValueRepository>();
builder.Services.AddScoped<IManageConfigValueService, ManageConfigValueService>();

builder.Services.AddScoped<IManageLendingPageRepository, ManageLendingPageRepository>();
builder.Services.AddScoped<IManageLendingPageService, ManageLendingPageService>();

builder.Services.AddScoped<IManageLendingPageSectionsRepository, ManageLendingPageSectionsRepository>();
builder.Services.AddScoped<IManageLendingPageSectionsService, ManageLendingPageSectionsService>();

builder.Services.AddScoped<IManageLendingPageSectionDetailsRepository, ManageLendingPageSectionDetailRepository>();
builder.Services.AddScoped<IManageLendingPageSectionDetailsService, ManageLendingPageSectionDetailService>();

builder.Services.AddScoped<IManageCollectionRepository, ManageCollectionRepository>();
builder.Services.AddScoped<IManageCollectionService, ManageCollectionService>();

builder.Services.AddScoped<IManageCollectionMappingRepository, ManageCollectionMappingRepository>();
builder.Services.AddScoped<IManageCollectionMappingService, ManageCollectionMappingService>();

builder.Services.AddScoped<IManageFlashSalePriceMasterRepository,ManageFlashSalePriceMasterRepository>();
builder.Services.AddScoped<IManageFlashSalePriceMasterService, ManageFlashSalePriceMasterService>();

builder.Services.AddScoped<IManageOffersRepository,ManageOffersRepository>();
builder.Services.AddScoped<IManageOffersService, ManageOffersService>();

builder.Services.AddScoped<IManageOffersMappingRepository,ManageOffersMappingRepository>();
builder.Services.AddScoped<IManageOffersMappingService, ManageOffersMappingService>();

builder.Services.AddScoped<IProductListRepository, ProductListRepository>();
builder.Services.AddScoped<IProductListService, ProductListService>();

builder.Services.AddScoped<IUserProductsRepository, UserProductsRepository>();
builder.Services.AddScoped<IUserProductService, UserProductService>();

builder.Services.AddScoped<ISpecificationRepository, SpecificationRepository>();
builder.Services.AddScoped<ISpecificationService, SpecificationService>();

builder.Services.AddScoped<IAssignSpecValuesToCategoryRepository, AssignSpecValuesToCategoryRepository>();
builder.Services.AddScoped<IAssignSpecValuesToCategoryService, AssignSpecValuesToCategoryService>();

builder.Services.AddScoped<IProductSpecificationMappingRepository, ProductSpecificationMappingRepository>();
builder.Services.AddScoped<IProductSpecificationMappingService, ProductSpecificationMappingService>();

builder.Services.AddScoped<IExtraChargesRepository, ExtraChargesRepository>();
builder.Services.AddScoped<IExtraChargesService, ExtraChargesService>();

builder.Services.AddScoped<IChargesPaidByRepository, ChargesPaidByRepository>();
builder.Services.AddScoped<IChargesPaidByService, ChargesPaidByService>();

builder.Services.AddScoped<IProductsCountRepository, ProductsCountRepository>();
builder.Services.AddScoped<IProductsCountService, ProductsCountService>();

builder.Services.AddScoped<ICommissionChargesRepository, CommissionChargesRepository>();
builder.Services.AddScoped<ICommissionChargesService, CommissionChargesService>();

builder.Services.AddScoped<ICollectionProductsListRepository, CollectionProductsListRepository>();
builder.Services.AddScoped<ICollectionProductsListService, CollectionProductsListService>();

builder.Services.AddScoped<ITaxRateToHSNCodeRepository, AssignTaxRateToHSNCodeRepository>();
builder.Services.AddScoped<ITaxRateToHSNCodeService, AssignTaxRateToHSNCodeService>();

builder.Services.AddScoped<ICheckAssignSpecsToCategoryRepository, CheckAssignSpecsToCategoryRepository>();
builder.Services.AddScoped<ICheckAssignSpecsToCategoryService, CheckAssignSpecsToCategoryService>();


builder.Services.AddScoped<IArchiveProductListRepository, ArchiveProductListRepository>();
builder.Services.AddScoped<IArchiveProductListService, ArchiveProductListService>();

builder.Services.AddScoped<IManageLayoutOptionRepository, ManageLayoutOptionRepository>();
builder.Services.AddScoped<IManageLayoutOptionService, ManageLayoutOptionService>();

builder.Services.AddScoped<IReportsRepository, ReportsRepository>();
builder.Services.AddScoped<IReportsService, ReportsService>();

builder.Services.AddScoped<IManageAppConfigRepository, ManageAppConfigRepository>();
builder.Services.AddScoped<IManageAppConfigService, ManageAppConfigService>();

builder.Services.AddScoped<IProductViewRepository, ProductViewRepository>();
builder.Services.AddScoped<IProductViewService, ProductViewService>();

builder.Services.AddScoped<IGetCartListRepository, GetCartListRepository>();
builder.Services.AddScoped<IGetCartListService, GetCartListService>();

builder.Services.AddScoped<ITaxMappingRespository, TaxMappingRespository>();
builder.Services.AddScoped<ITaxMappingService, TaxMappingService>();

IdentityModelEventSource.ShowPII = true;

JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
// any domain
builder.Services.AddSwaggerGen(options => {
    options.OperationFilter<DeviceIdHeaderParameterFilter>();
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme."
    });
    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme {
                    Reference = new Microsoft.OpenApi.Models.OpenApiReference {
                        Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                            Id = "Bearer"
                    }
                },
                new string[] {}
        }
    });
});

JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear(); // => remove default claims
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        //ValidateIssuer = true,
        //ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:ValidAudience"],
        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"])),
        TokenDecryptionKey = EncLoader.GetDecryptionKey()
    };
});
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Accessable", policy => policy.RequireClaim("Scope", Projectscopes));
    options.AddPolicy("General", policy => policy.RequireClaim("Scope", generalScope));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction() || app.Environment.IsStaging())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    
}

var staticFilesPath = Path.Combine(Directory.GetCurrentDirectory(), "StaticFiles");
if (!Directory.Exists(staticFilesPath))
    Directory.CreateDirectory(staticFilesPath);

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(staticFilesPath),
    RequestPath = "/StaticFiles"
});

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseTokenInterceptMiddleware();
app.UseAuthorization();

app.MapControllers();

app.Run();
