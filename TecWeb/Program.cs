using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using TecWeb.Core.Interfaces;
using TecWeb.Core.Services;
using TecWeb.Infrastructure.Data;
using TecWeb.Infrastructure.Filters;
using TecWeb.Infrastructure.Mappings;
using TecWeb.Infrastructure.Repositories;
using TecWeb.Infrastructure.Validators;

var builder = WebApplication.CreateBuilder(args);

// -------------------- DbContext --------------------
builder.Services.AddDbContext<GestionCulturalContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("connectionDB"))
);

// -------------------- AutoMapper --------------------
builder.Services.AddAutoMapper(cfg =>
{

}, typeof(MappingProfile));

// ====================
// Registrar Servicios
// ====================
// (línea solicitada por el instructivo)
//builder.Services.AddScoped<ICorrespondenciaService, CorrespondenciaService>();

// registrar controladores (parte 1 pide AddControllers aquí)
builder.Services.AddControllers();

// -------------------- FluentValidation (API moderna) --------------------
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();

builder.Services.AddValidatorsFromAssemblyContaining<EventoValidator>();

// -------------------- Repositorios (DI) --------------------
builder.Services.AddScoped<IEventoRepository, EventoRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IInscripcionRepository, InscripcionRepository>();

// -------------------- Servicios (DI) --------------------
builder.Services.AddScoped<IEventoService, EventoService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IInscripcionService, InscripcionService>();

// -------------------- Swagger / Otros --------------------
// Configurar Swagger (parte 1: metadata según tu instrucción)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Backend Social Media API",
        Version = "v1",
        Description = "Documentación de la API de Social Media - .NET 9",
        Contact = new OpenApiContact
        {
            Name = "Equipo de Desarrollo UCB",
            Email = "desarrollo@ucb.edu.bo"
        }
    });
});

// Se agrega configuración adicional del MVC con filtros y Newtonsoft (no modificar)
builder.Services.AddControllers(options =>
{
    options.Filters.Add<GlobalExceptionFilter>();
}).AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
}).ConfigureApiBehaviorOptions(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

// Registrar UnitOfWork, Dapper y factory
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();

builder.Services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();
builder.Services.AddScoped<IDapperContext, DapperContext>();

var app = builder.Build();

// Usar Swagger (parte 2: solo UseSwagger() dentro del if de Development)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
