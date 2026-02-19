using IDServer.Domain.Entity;
using IDServer.Infrastructure.Auth;
using IDServer.Infrastructure.Data;
using IDServer.Infrastructure.Repositories;
using IDServerApplication.IRepositories;
using IDServerApplication.IServices;
using IDServerApplication.Services;
using JWTTokenProvider.API.Filter;
using JWTTokenProvider.API.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

//Valid Project scopes
string[] generalScope = new string[] { "general" };

// Add services to the container.
var EncLoader = new EncryptionLoader();
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AspNetIdentityDBContext>(options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString), b => b.MigrationsAssembly("JWTTokenProvider.API")));


//Change this settings to true, once we integrate email and SMS capability
builder.Services.AddIdentity<Users, IdentityRole>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.SignIn.RequireConfirmedPhoneNumber = false;
    options.SignIn.RequireConfirmedEmail = false;
}).AddDefaultTokenProviders()
.AddUserManager<TokenProvider<Users>>()
    //.AddTokenProvider<SixDigitTokenProvider>("SixDigitTokenProvider")
    .AddEntityFrameworkStores<AspNetIdentityDBContext>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
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

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<IPageRoleRepository, PageRoleRepository>();
builder.Services.AddScoped<IPageRolesServices, PageRolesServices>();

builder.Services.AddSingleton<ITokenManagerRepository, TokenManagerRepository>();
builder.Services.AddSingleton<ITokenManagerService, TokenManagerService>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidAudience = builder.Configuration["JWT:ValidAudience"],
        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"])),
        TokenDecryptionKey = EncLoader.GetDecryptionKey()
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("General", policy => policy.RequireClaim("Scope", generalScope));
});

var app = builder.Build();
    
    

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction() || app.Environment.IsStaging())
{
    app.UseDeveloperExceptionPage();
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

//app.UseHttpsRedirection();

app.UseAuthentication();
app.UseTokenInterceptMiddleware();
app.UseAuthorization();

app.MapControllers();

app.Run();
