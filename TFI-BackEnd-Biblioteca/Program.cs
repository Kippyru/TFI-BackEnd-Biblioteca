using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using System.Text.Json.Serialization;
using TFI_BackEnd_Biblioteca.Data;
using TFI_BackEnd_Biblioteca.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Evitar referencias circulares
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        // Ignorar propiedades null en la respuesta JSON
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

// Configurar CORS desde appsettings
var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? new[] { "*" };
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        if (allowedOrigins.Contains("*"))
        {
            policy.AllowAnyOrigin()
           .AllowAnyMethod()
        .AllowAnyHeader();
        }
        else
        {
            policy.WithOrigins(allowedOrigins)
   .AllowAnyMethod()
  .AllowAnyHeader()
        .AllowCredentials();
        }
    });
});

// Configurar Entity Framework y SQL Server
builder.Services.AddDbContext<BibliotecaContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configurar OpenAPI/Swagger
builder.Services.AddOpenApi();

var app = builder.Build();

// Middleware de manejo global de excepciones (primero)
app.UseGlobalExceptionHandler();

// Aplicar migraciones automáticamente en desarrollo
if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<BibliotecaContext>();
        dbContext.Database.Migrate();
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Mapear OpenAPI
 app.MapOpenApi();
    
    // Configurar Scalar para la documentación interactiva
    app.MapScalarApiReference(options =>
    {
        options
  .WithTitle("Biblioteca API")
            .WithTheme(ScalarTheme.Purple)
   .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
    });
}

app.UseCors("CorsPolicy");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
