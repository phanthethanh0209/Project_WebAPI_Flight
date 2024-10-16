using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
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


            // Dang ky Database
            IConfigurationRoot cf = new ConfigurationBuilder().SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true).Build();

            builder.Services.AddDbContext<MyDBContext>(opt => opt.UseSqlServer(cf.GetConnectionString("MyDB")));


            // Dang ky interface respository
            builder.Services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();

            // Dang ky interface Services
            builder.Services.AddScoped<IUserService, UserService>();

            // Dang ky Mapper
            builder.Services.AddAutoMapper(typeof(MappingUser));


            // Dang ky Fluent Validation
            builder.Services.AddControllers().AddFluentValidation();
            builder.Services.AddValidatorsFromAssemblyContaining<UserValidator>();


            WebApplication app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
