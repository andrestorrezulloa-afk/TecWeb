using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using TecWeb.Core.Interfaces;
using TecWeb.Core.Services;
using TecWeb.Infrastructure.Data;
using TecWeb.Infrastructure.Filters;
using TecWeb.Infrastructure.Mappings;
using TecWeb.Infrastructure.Repositories;
using TecWeb.Infrastructure.Validators;

var builder = WebApplication.CreateBuilder(args);

// Configurar User Secrets en desarrollo
if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();
}

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
builder.Services.AddControllers();

// API Versioning
builder.Services.AddApiVersioning(options =>
{
    options.ReportApiVersions = true;
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ApiVersionReader = ApiVersionReader.Combine(
        new UrlSegmentApiVersionReader(),
        new HeaderApiVersionReader("x-api-version"),
        new QueryStringApiVersionReader("api-version")
    );
});

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

// -------------------- Swagger --------------------
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

// -------------------- Configuración adicional del MVC --------------------
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

// ========================
// JWT AUTHENTICATION
// ========================
var secretKey = builder.Configuration["Authentication:SecretKey"]
    ?? throw new InvalidOperationException("SecretKey no configurada");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Authentication:Issuer"],
        ValidAudience = builder.Configuration["Authentication:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(secretKey))
    };
});

// Registrar UnitOfWork, Dapper y factory
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
builder.Services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();
builder.Services.AddScoped<IDapperContext, DapperContext>();

// Registrar UserSecurityService
builder.Services.AddTransient<IUserSecurityService, UserSecurityService>();

var app = builder.Build();

// ===========================
// Configuración Swagger UI
// ===========================
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Backend Social Media API v1");
        options.RoutePrefix = string.Empty;
    });
}

// ===========================
// Middlewares habituales
// ===========================
app.UseHttpsRedirection();

// ¡IMPORTANTE: En este orden!
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();