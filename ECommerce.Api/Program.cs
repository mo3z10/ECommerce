using System.Data;
using System.Security.Claims;
using System.Text;
using ECommerce.Api.SeedData;
using ECommerce.BIL.Services.AuthenicationServices;
using ECommerce.BIL.Services.CacheService;
using ECommerce.BIL.Services.CartService;
using ECommerce.BIL.Services.CustomerService;
using ECommerce.BIL.Services.OrderService;
using ECommerce.BIL.Services.ProductService;
using ECommerce.DAL.Database;
using ECommerce.DAL.IUnitOfWork;
using ECommerce.DAL.Models;
using ECommerce.DAL.Reposatories.CartItemsRepo;
using ECommerce.DAL.Reposatories.CartRepo;
using ECommerce.DAL.Reposatories.CustomerRepo;
using ECommerce.DAL.Reposatories.GenericRepo;
using ECommerce.DAL.Reposatories.ProductRepo;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NETCore.MailKit.Core;
using NETCore.MailKit.Extensions;
using NETCore.MailKit.Infrastructure.Internal;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IProductService, ProductSerivce>();
builder.Services.AddScoped<IProductRepo, ProductRepo>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<ICustomerRepo, CustomerRepo>();
builder.Services.AddScoped<ICartItemstRepo,CartItemstRepo>();
builder.Services.AddScoped<ICartRepo, CartRepo>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ICacheService, CacheService>();
builder.Services.AddScoped(typeof(IGenericRepo<>), typeof(GenericRepo<>));
builder.Services.AddMailKit(option =>
{
    option.UseMailKit(new MailKitOptions
    {
        Server = "smtp.gmail.com",
        Port = 587,

        SenderName = "ECommerce",

        SenderEmail = "moazyasser983@gmail.com",

        Account = "moazyasser983@gmail.com",

        Password = "xtkxxlyxhlrwuwjj",

        Security = true
    });
});
builder.Services.AddDbContext<ECommerceContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("cs")).UseLazyLoadingProxies());
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequiredLength = 8;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireDigit = true;
    options.Password.RequireNonAlphanumeric = true;
    options.User.RequireUniqueEmail = true;
}).AddEntityFrameworkStores<ECommerceContext>().AddDefaultTokenProviders();
builder.Services.AddAuthorization();
builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateIssuerSigningKey = true,
        RoleClaimType = ClaimTypes.Role,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]))
    };
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("Allow",policy=>policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());


}
);
builder.Services.AddStackExchangeRedisCache(options => {
    options.InstanceName = "Redis";
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
    }
);
var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var service = scope.ServiceProvider;
    var UserManager = service.GetRequiredService<UserManager<ApplicationUser>>();
    var RoleManager = service.GetRequiredService<RoleManager<IdentityRole>>();
    await SeedData.SeedAdmin(UserManager, RoleManager);
}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("Allow");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
