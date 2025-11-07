using Microsoft.EntityFrameworkCore;
using BazarExam.Data;

var builder = WebApplication.CreateBuilder(args);

// =======================================================
// 🗄️ CONFIGURAR BASE DE DATOS (PostgreSQL local o Render)
// =======================================================
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection")));

// =======================================================
// 🌐 CONFIGURAR CORS (para Render, Netlify y localhost)
// =======================================================
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy
            .WithOrigins(
                "http://localhost:5173",                 // React local (Vite)
                "https://tu-netlify.netlify.app",        // 🔹 cambia por tu dominio de Netlify
                "https://bazar-backend.onrender.com"     // 🔹 cambia por tu dominio de Render
            )
            .AllowAnyHeader()
            .AllowAnyMethod());
});

// =======================================================
// 🚀 CONFIGURAR SERVICIOS GENERALES
// =======================================================
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// =======================================================
// ⚙️ MIDDLEWARES
// =======================================================

// ✅ Solo usar HTTPS Redirection en desarrollo (no en Render)
if (app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

// ✅ CORS
app.UseCors("AllowAll");

// ✅ Swagger habilitado siempre (Render y local)
app.UseSwagger();
app.UseSwaggerUI();

// ✅ URLs: Render usa 8080, local usa lo de launchSettings.json
if (app.Environment.IsProduction())
{
    var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
    app.Urls.Add($"http://*:{port}");
}

// ✅ Mapear controladores
app.MapControllers();

// =======================================================
// 🧩 CARGAR PRODUCTOS AUTOMÁTICAMENTE DESDE products.json
// =======================================================
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();

    // Ejecutar migraciones pendientes
    context.Database.Migrate();

    // Cargar datos iniciales
    await SeedData.LoadProducts(context);
}

// =======================================================
// ▶️ EJECUTAR APLICACIÓN
// =======================================================
app.Run();
