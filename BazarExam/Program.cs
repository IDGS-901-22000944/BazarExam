using Microsoft.EntityFrameworkCore;
using BazarExam.Data;

var builder = WebApplication.CreateBuilder(args);

// =======================================================
// 🔧 CONFIGURAR BASE DE DATOS (PostgreSQL desde Render o local)
// =======================================================
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection")));

// =======================================================
// 🌐 CONFIGURAR CORS (Netlify, Render y Localhost)
// =======================================================
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy
            .WithOrigins(
                "http://localhost:5173",                 // React local (Vite)
                "https://tu-netlify.netlify.app",        // 🔹 Cambia por tu dominio real de Netlify
                "https://bazar-backend.onrender.com"     // 🔹 Cambia por tu dominio real de Render
            )
            .AllowAnyHeader()
            .AllowAnyMethod());
});

// =======================================================
// 🚀 CONFIGURAR SERVICIOS
// =======================================================
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// =======================================================
// ⚙️ CONFIGURAR MIDDLEWARES
// =======================================================

// Render usa puerto 8080, así que configuramos las URLs explícitamente
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
app.Urls.Add($"http://*:{port}");

// Redirección HTTPS solo si está en entorno local
if (!app.Environment.IsProduction())
{
    app.UseHttpsRedirection();
}

// Aplicar CORS antes de los controladores
app.UseCors("AllowAll");

// Swagger siempre habilitado (útil en Render)
app.UseSwagger();
app.UseSwaggerUI();

// Mapear controladores
app.MapControllers();

// =======================================================
// 🧩 SEMILLA DE DATOS: CARGAR products.json AUTOMÁTICAMENTE
// =======================================================
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();

    // Aplicar migraciones pendientes
    context.Database.Migrate();

    // Cargar productos desde el archivo JSON
    await SeedData.LoadProducts(context);
}

// =======================================================
// ▶️ EJECUTAR APLICACIÓN
// =======================================================
app.Run();
