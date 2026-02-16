using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Order.API.Filter;
using Order.API.Middleware;
using Order.Application.Helper;
using Order.Application.IRepositories;
using Order.Application.IServices;
using Order.Application.Services;
using Order.Infrastructure.Repository;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

//Valid Project scopes
string[] Projectscopes = new string[] { "order.read", "order.write", "order.update", "order.delete" };
string[] generalScope = new string[] { "general" };
//This is used to decode Token 
var EncLoader = new AuthHelper();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IOrderStatusLibraryRepository, OrderStatusLibraryRepository>();
builder.Services.AddScoped<IOrderStatusLibraryService, OrderStatusLibraryService>();

builder.Services.AddScoped<IRejectionTypeService, RejectionTypeService>();
builder.Services.AddScoped<IRejectionTypeRepository, RejectionTypeRepository>();

builder.Services.AddScoped<IIssueTypeService, IssueTypeService>();
builder.Services.AddScoped<IIssueTypeRepository, IssueTypeRepository>();

builder.Services.AddScoped<IIssueReasonService, IssueReasonService>();
builder.Services.AddScoped<IIssueReasonRepository, IssueReasonRepository>();

builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

builder.Services.AddScoped<IOrderItemsService, OrderItemsService>();
builder.Services.AddScoped<IOrderItemsRepository, OrderItemsRepository>();

builder.Services.AddScoped<IOrderTaxInfoService, OrderTaxInfoService>();
builder.Services.AddScoped<IOrderTaxInfoRepository, OrderTaxInfoRepository>();

builder.Services.AddScoped<IOrderTrackDetailsService, OrderTrackDetailsService>();
builder.Services.AddScoped<IOrderTrackDetailsRepository, OrderTrackDetailsRepository>();

builder.Services.AddScoped<IOrderProductVariantMappingService, OrderProductVariantMappingService>();
builder.Services.AddScoped<IOrderProductVariantMappingRepository, OrderProductVariantMappingRepository>();

builder.Services.AddScoped<IOrderPackageService, OrderPackageService>();
builder.Services.AddScoped<IOrderPackageRepository, OrderPackageRepository>();

builder.Services.AddScoped<IOrderShipmentInfoService, OrderShipmentInfoService>();
builder.Services.AddScoped<IOrderShipmentInfoRepository, OrderShipmentInfoRepository>();

builder.Services.AddScoped<IOrderInvoiceService, OrderInvoiceService>();
builder.Services.AddScoped<IOrderInvoiceRepository, OrderInvoiceRepository>();

builder.Services.AddScoped<IOrderCancelReturnService, OrderCancelReturnService>();
builder.Services.AddScoped<IOrderCancelReturnRepository, OrderCancelReturnRepository>();

builder.Services.AddScoped<IOrderReturnActionService, OrderReturnActionService>();
builder.Services.AddScoped<IOrderReturnActionRepository, OrderReturnActionRepository>();

builder.Services.AddScoped<IOrderRefundService, OrderRefundService>();
builder.Services.AddScoped<IOrderRefundRepository, OrderRefundRepository>();

builder.Services.AddScoped<IReturnShipmentInfoService, ReturnShipmentInfoService>();
builder.Services.AddScoped<IReturnShipmentInfoRepository, ReturnShipmentInfoRepository>();

builder.Services.AddScoped<IOrderWiseExtraChargesService, OrderWiseExtraChargesService>();
builder.Services.AddScoped<IOrderWiseExtraChargesRepository, OrderWiseExtraChargesRepository>();

builder.Services.AddScoped<IOrderWiseProductSeriesNoService, OrderWiseProductSeriesNoService>();
builder.Services.AddScoped<IOrderWiseProductSeriesNoRepository, OrderWiseProductSeriesNoRepository>();


builder.Services.AddScoped<IOrderTaxInfoService, OrderTaxInfoService>();
builder.Services.AddScoped<IOrderTaxInfoRepository, OrderTaxInfoRepository>();

builder.Services.AddScoped<IOrderCountService, OrderCountService>();
builder.Services.AddScoped<IOrderCountRepository, OrderCountRepository>();

builder.Services.AddScoped<IReportsRepository, ReportsRepository>();
builder.Services.AddScoped<IReportsService, ReportsService>();

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
        TokenDecryptionKey = EncLoader.GetDecryptionKey(),
        //Scope = 
        //RoleClaimType = IdentityModel.JwtClaimTypes.Role,
        //NameClaimType = IdentityModel.JwtClaimTypes.Name
    };
});
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Accessable", policy => policy.RequireClaim("Scope", Projectscopes));
    options.AddPolicy("General", policy => policy.RequireClaim("Scope", generalScope));
});



var app = builder.Build();

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
