using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using User.API.Filter;
using User.API.Middleware;
using User.Application.Helper;
using User.Application.IRepositories;
using User.Application.IServices;
using User.Application.Services;
using User.Infrastructure.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
string[] Projectscopes = new string[] { "user.read", "user.write", "user.update", "user.delete" };
string[] generalScope = new string[] { "general" };
var EncLoader = new AuthHelper();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ICountryRepository, CountryRepository>();
builder.Services.AddScoped<ICountryService, CountryService>();

builder.Services.AddScoped<IStateRepository, StateRepository>();
builder.Services.AddScoped<IStateService, StateService>();

builder.Services.AddScoped<ICityRepository, CityRepository>();
builder.Services.AddScoped<ICityService, CityService>();

builder.Services.AddScoped<IDeliveryRepository, DeliveryRepository>();
builder.Services.AddScoped<IDeliveryService, DeliveryService>();

builder.Services.AddScoped<IGSTInfoRepository, GSTInfoRepository>();
builder.Services.AddScoped<IGSTInfoService, GSTInfoService>();

builder.Services.AddScoped<IKYCDetailsRepository, KYCDetailsRepository>();
builder.Services.AddScoped<IKYCDetailsService, KYCDetailsService>();

builder.Services.AddScoped<IWarehouseRepository, WarehouseRepository>();
builder.Services.AddScoped<IWarehouseService, WarehouseService>();

builder.Services.AddScoped<IAddressRepository, AddressRepository>();
builder.Services.AddScoped<IAddressService, AddressService>();

builder.Services.AddScoped<IBrandRepository, BrandRepository>();
builder.Services.AddScoped<IBrandService, BrandService>();

builder.Services.AddScoped<IAssignBrandToSellerRepository, AssignBrandToSellerRepository>();
builder.Services.AddScoped<IAssignBrandToSellerService, AssignBrandToSellerService>();

builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<ICartService, CartService>();

builder.Services.AddScoped<IWishlistRepository, WishlistRepository>();
builder.Services.AddScoped<IWishlistService, WishlistService>();

builder.Services.AddScoped<IKycCountRepository, KycCountRepository>();
builder.Services.AddScoped<IKycCountService, KycCountService>();

builder.Services.AddScoped<IBrandCountRepository, BrandCountRepository>();
builder.Services.AddScoped<IBrandCountService, BrandCountService>();

builder.Services.AddScoped<IReportsRepository, ReportsRepository>();
builder.Services.AddScoped<IReportsService, ReportsService>();

builder.Services.AddScoped<IUserDetailsRepository, UserDetailsRepository>();
builder.Services.AddScoped<IUserDetailsServices, UserDetailsServices>();

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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction() || app.Environment.IsStaging())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "StaticFiles")),
    RequestPath = "/StaticFiles"
});

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseTokenInterceptMiddleware();
app.UseAuthorization();

app.MapControllers();

app.Run();
