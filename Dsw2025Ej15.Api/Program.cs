using Dsw2025Ej15.Application.Services;
using Dsw2025Ej15.Data;
using Dsw2025Ej15.Data.Repositories; // Agregado para encontrar PersistenceEf
using Dsw2025Ej15.Domain;
using Microsoft.EntityFrameworkCore; // Agregado para usar UseSqlServer

namespace Dsw2025Ej15.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // --- CONFIGURACIÓN DE BASE DE DATOS (EJERCICIO 16) ---

            // 1. Registramos el DbContext usando la conexión de appsettings.json
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // 2. Cambiamos la persistencia: de InMemory (Singleton) a PersistenceEf (Scoped)
            // builder.Services.AddSingleton<IPersistence, InMemory>(); // Línea vieja comentada
            builder.Services.AddScoped<IPersistence, PersistenceEf>();

            // -------------------------------------------------------

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddHealthChecks();

            // Mantenemos el servicio de gestión
            builder.Services.AddTransient<ProductsManagementService>();

            var app = builder.Build();

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