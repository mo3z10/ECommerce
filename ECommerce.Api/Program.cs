using System.Security.Claims;
using System.Text;
using System.Threading.RateLimiting;
using ECommerce.Api.Middleware;
using ECommerce.Api.SeedData;
using ECommerce.BIL.Services.AuthenicationServices;
using ECommerce.BIL.Services.CacheService;
using ECommerce.BIL.Services.CartJob;
using ECommerce.BIL.Services.CartService;
using ECommerce.BIL.Services.CustomerService;
using ECommerce.BIL.Services.EmailServices;
using ECommerce.BIL.Services.InventoryJob;
using ECommerce.BIL.Services.JobSercvices;
using ECommerce.BIL.Services.NotificationHubService;
using ECommerce.BIL.Services.OrderService;
using ECommerce.BIL.Services.PaymentServices;
using ECommerce.BIL.Services.ProductService;
using ECommerce.DAL.Database;
using ECommerce.DAL.IUnitOfWork;
using ECommerce.DAL.Models;
using ECommerce.DAL.Reposatories.CartItemsRepo;
using ECommerce.DAL.Reposatories.CartRepo;
using ECommerce.DAL.Reposatories.CustomerRepo;
using ECommerce.DAL.Reposatories.GenericRepo;
using ECommerce.DAL.Reposatories.ProductRepo;
using ECommerce.Shared.HubService;
using Hangfire;
using Hangfire.Dashboard.BasicAuthorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NETCore.MailKit.Core;
using NETCore.MailKit.Extensions;
using NETCore.MailKit.Infrastructure.Internal;
using Stripe;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IProductService, ProductSerivce>();
builder.Services.AddScoped<IProductRepo, ProductRepo>();
builder.Services.AddScoped<ICustomerService, ECommerce.BIL.Services.CustomerService.CustomerService>();
builder.Services.AddScoped<ICustomerRepo, CustomerRepo>();
builder.Services.AddScoped<ICartItemstRepo,CartItemstRepo>();
builder.Services.AddScoped<ICartRepo, CartRepo>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ICacheService, CacheService>();
builder.Services.AddScoped<IJobService, HangfireJopServic>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IDefinedMailService,DefinedMailService>();
builder.Services.AddScoped<ICartJob, CartJob>();
builder.Services.AddScoped<IInventoryJob,InventoryJob>();
builder.Services.AddScoped<IPaymentService,PaymentService>();
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
builder.Services.AddDbContext<ECommerceContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("cs"),
        sql =>

    options.UseLazyLoadingProxies());
}); builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
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

    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];
            var path = context.HttpContext.Request.Path;

            if (!string.IsNullOrEmpty(accessToken) &&
                path.StartsWithSegments("/notificationHub"))
            {
                context.Token = accessToken;
            }

            return Task.CompletedTask;
        }
    };
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("Allow",policy=>policy.WithOrigins("http://127.0.0.1:5500", "http://localhost:5500")
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowCredentials());


}
);
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    options.AddFixedWindowLimiter("Fixed", opt =>
    {
        opt.PermitLimit = 5;
        opt.Window = TimeSpan.FromMinutes(1);

        opt.QueueLimit = 2;
        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
    });

    options.AddSlidingWindowLimiter("Sliding", opt =>
    {
        opt.PermitLimit = 5;
        opt.Window = TimeSpan.FromMinutes(1);
        opt.SegmentsPerWindow = 6;

        opt.QueueLimit = 0;
    });

    options.AddTokenBucketLimiter("Token", opt =>
    {
        opt.TokenLimit = 20;
        opt.TokensPerPeriod = 5;
        opt.ReplenishmentPeriod = TimeSpan.FromSeconds(10);

        opt.AutoReplenishment = true;

        opt.QueueLimit = 0;
    });

    options.AddConcurrencyLimiter("Concurrency", opt =>
    {
        opt.PermitLimit = 3;

        opt.QueueLimit = 2;

        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
    });

    options.OnRejected = async (context, token) =>
    {
        context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;

        await context.HttpContext.Response.WriteAsync(
            "Too many requests. Please try again later.",
            token);
    };
});

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.InstanceName = "Redis";
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
}
);
builder.Services.AddHangfire(x=>x.UseSqlServerStorage(builder.Configuration.GetConnectionString("cs")));
builder.Services.AddHangfireServer();
builder.Services.AddSignalR();
StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];
var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var service = scope.ServiceProvider;
    var context = service.GetRequiredService<ECommerceContext>();

    await context.Database.MigrateAsync();
    var UserManager = service.GetRequiredService<UserManager<ApplicationUser>>();
    var RoleManager = service.GetRequiredService<RoleManager<IdentityRole>>();
    var Configration = service.GetRequiredService < IConfiguration>();
    await SeedData.SeedAdmin(UserManager, RoleManager,Configration);
}
app.MapHub<NotificationHub>("/notificationHub");

RecurringJob.AddOrUpdate<IJobService>("Clean Up Unused Carts",x => x.CleanupCarts(), cronExpression: Cron.Daily);
RecurringJob.AddOrUpdate<IJobService>("Low Stock Alert", x => x.LowStockMail(), Cron.Daily);
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();

}
app.UseSwagger();
app.UseSwaggerUI();
app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseCors("Allow");
app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    Authorization = new[]
    {
        new BasicAuthAuthorizationFilter(
            new BasicAuthAuthorizationFilterOptions
            {
                RequireSsl = false,
                LoginCaseSensitive = true,
                Users = new[]
                {
                   new BasicAuthAuthorizationUser
                   {
                     Login = builder.Configuration["Hangfire:UserName"],
                     PasswordClear = builder.Configuration["Hangfire:Password"]
                    }
                }
            })
    }
});

app.UseRateLimiter();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();
app.Run();
