
using Dsw2025Ej15.Application.Services;
using Dsw2025Ej15.Data;
using Dsw2025Ej15.Domain;

namespace Dsw2025Ej15.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddHealthChecks();
            builder.Services.AddSingleton<IRepository, InMemory>();
            builder.Services.AddTransient<ProductsManagementService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();
            
            app.MapHealthChecks("/health-check");

            app.Run();
        }
    }
}
