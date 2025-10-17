using Microsoft.EntityFrameworkCore;
using TecWeb.Mappings;
using TecWeb.Models;
using TecWeb.Validators;
using TecWeb.Services;
using FluentValidation;
using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// --------------------- Configuración de la Base de Datos ---------------------
builder.Services.AddDbContext<GestionCulturalContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("connectionDB"))
);

// --------------------- Controladores y JSON ---------------------
builder.Services.AddControllers()
    .AddJsonOptions(opts =>
    {
        opts.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

// --------------------- AutoMapper ---------------------
builder.Services.AddAutoMapper(typeof(MappingProfile));

// --------------------- FluentValidation ---------------------
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<UsuarioValidator>();

// --------------------- Inyección de Servicios ---------------------
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IEventoService, EventoService>();
builder.Services.AddScoped<IInscripcionService, InscripcionService>();

// --------------------- OpenAPI / Swagger ---------------------
builder.Services.AddOpenApi();

var app = builder.Build();

// --------------------- Configuración de la App ---------------------
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();