using Microsoft.EntityFrameworkCore;
using TFI_BackEnd_Biblioteca.Data;

var builder = WebApplication.CreateBuilder(args);

//Este codigo le "enseña"la aplicación cómo usar BibliotecaContext y de dónde sacar la cadena de conexión.
// 1. Obtener la cadena de conexión que escribiste en appsettings.json

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// 2. Registrar el DbContext (BibliotecaContext) en el sistema.
//    Esto se llama "Inyección de Dependencias".

builder.Services.AddDbContext<BibliotecaContext>(options =>
    options.UseSqlServer(connectionString));
// --- FIN DE LA CONFIGURACIÓN DE EF ---

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOpenApi();


// Configuración de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp",
        builder =>
        {
            builder.WithOrigins("http://localhost:4200") // ¡La URL de tu app Angular!
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Aquí le decimos a la app que use la política que definimos
app.UseCors("AllowAngularApp");

// La línea de CORS debe ir ANTES de UseAuthorization
app.UseAuthorization();

app.MapControllers();

app.Run();

