using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.Json.Serialization;
using EventManagementApp.Context;
using EventManagementApp.Interfaces.Repository;
using EventManagementApp.Interfaces.Service;
using EventManagementApp.Repositories;
using EventManagementApp.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace EventManagementApp
{
    [ExcludeFromCodeCoverage]
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers(options =>
            {
                options.Filters.Add(new AuthorizeFilter());
            })
            .ConfigureApiBehaviorOptions(options =>
                options.SuppressModelStateInvalidFilter = true
            )
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["TokenKey:JWT"]))
                    };
                });

            builder.Services.AddDbContext<EventManagementDBContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("default"))
            );

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin", builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });

            #region Services

            builder.Services.AddScoped<IAdminService, AdminService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IClientResponseService, ClientResponseService>();
            builder.Services.AddScoped<IEventCategoryService, EventCategoryService>();
            builder.Services.AddScoped<IEventService, EventService>();
            builder.Services.AddScoped<INotificationService, NotificationService>();
            builder.Services.AddScoped<IPaymentService, PaymentService>();
            builder.Services.AddScoped<IQuotationRequestService, QuotationRequestService>();
            builder.Services.AddScoped<IQuotationResponseService, QuotationResponseService>();
            builder.Services.AddScoped<IScheduledEventService, SchedulesEventService>();
            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddScoped<ITicketService, TicketService>();
            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<BlobService, BlobService>();

            #endregion

            #region Repositories

            builder.Services.AddScoped<IEventCategoryRepository, EventCategoryRepository>();
            builder.Services.AddScoped<IEventRepository, EventRepository>();
            builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
            builder.Services.AddScoped<IOrderRepository, OrderRepository>();
            builder.Services.AddScoped<IQuotationRequestRepository, QuotationRequestRepository>();
            builder.Services.AddScoped<IQuotationResponseRepository, QuotationResponseRepository>();
            builder.Services.AddScoped<IScheduledEventRepository, ScheduledEventRepository>();
            builder.Services.AddScoped<ITicketRepository, TicketRepository>();
            builder.Services.AddScoped<IUserRepository,UserRespository>();

            #endregion

            builder.Services.AddHostedService<TicketProcessingService>();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("AllowSpecificOrigin");
            app.UseAuthentication(); 
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
