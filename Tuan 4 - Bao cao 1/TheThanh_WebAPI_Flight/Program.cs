using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using TheThanh_WebAPI_Flight.Authorization;
using TheThanh_WebAPI_Flight.Data;
using TheThanh_WebAPI_Flight.Mapper;
using TheThanh_WebAPI_Flight.Repository;
using TheThanh_WebAPI_Flight.Services;
using TheThanh_WebAPI_Flight.Validation;

namespace TheThanh_WebAPI_Flight
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // kiem tra va xac thuc token nguoi dung
            string secretKey = builder.Configuration["Jwt:Key"]; // Doc cau hinh tu appsettings.json
            byte[] secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opt =>
                {
                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        //tu cap token 
                        ValidateIssuer = false,
                        ValidateAudience = false,

                        //ky vao token
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(secretKeyBytes),

                        ClockSkew = TimeSpan.Zero
                    };
                });

            // Dang ky Database
            IConfigurationRoot cf = new ConfigurationBuilder().SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true).Build();

            builder.Services.AddDbContext<MyDBContext>(opt => opt.UseSqlServer(cf.GetConnectionString("MyDB")));


            // Dang ky interface respository
            builder.Services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();

            // Dang ky interface Services
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IRoleService, RoleService>();
            builder.Services.AddScoped<IPermissionService, PermissionService>();
            builder.Services.AddScoped<IRoleUserService, RoleUserService>();
            builder.Services.AddScoped<IRolePermissionService, RolePermissionService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IFlightService, FlightService>();
            builder.Services.AddScoped<IDocumentService, DocumentService>();
            builder.Services.AddScoped<IDocumentTypeService, DocumentTypeService>();

            // Dang ky Mapper
            builder.Services.AddAutoMapper(typeof(MappingUser));
            builder.Services.AddAutoMapper(typeof(MappingFlight));
            builder.Services.AddAutoMapper(typeof(MappingDocument));
            builder.Services.AddAutoMapper(typeof(MappingDocType));


            // Dang ky Fluent Validation
            builder.Services.AddControllers().AddFluentValidation();
            builder.Services.AddValidatorsFromAssemblyContaining<UserValidator>();

            // Dang ky JwtSecurityTokenHandler
            builder.Services.AddSingleton<JwtSecurityTokenHandler>();
            builder.Services.AddSingleton<TokenValidationParameters>(provider =>
            {
                IConfiguration configuration = provider.GetRequiredService<IConfiguration>();
                byte[] key = Encoding.UTF8.GetBytes(configuration["Jwt:Key"]);
                return new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ClockSkew = TimeSpan.Zero,
                    ValidateLifetime = false
                };
            });

            // Dang ky phan quen
            builder.Services.AddScoped<IUserPermission, UserPermission>();
            builder.Services.AddScoped<IAuthorizationHandler, PermissionHandler>();


            WebApplication app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication(); // Middleware xac thuc

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
