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
using TecWeb.Core.CustomEntities;

var builder = WebApplication.CreateBuilder(args);

// ============================================
// CONFIGURACIÓN BASE PARA DESARROLLO/PRODUCCIÓN
// ============================================
builder.Configuration.Sources.Clear();
builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true);

if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();
    Console.WriteLine("? User Secrets habilitados para Desarrollo");
}

builder.Configuration.AddEnvironmentVariables();

Console.WriteLine($"?? Ambiente: {builder.Environment.EnvironmentName}");
Console.WriteLine($"?? Entorno: {builder.Environment.EnvironmentName}");

// -------------------- DbContext --------------------
builder.Services.AddDbContext<GestionCulturalContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("connectionDB"))
);

// -------------------- AutoMapper --------------------
builder.Services.AddAutoMapper(cfg =>
{

}, typeof(MappingProfile));

// Configurar PasswordOptions desde appsettings
builder.Services.Configure<PasswordOptions>(
    builder.Configuration.GetSection("PasswordOptions"));

// ====================
// Registrar Servicios
// ====================
builder.Services.AddControllers();

builder.Services.AddTransient<IPasswordService, PasswordService>();
builder.Services.AddTransient<IUserSecurityService, UserSecurityService>();

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

// ============================================
// CONFIGURACIÓN SWAGGER (SOLO DESARROLLO)
// ============================================

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "Gestión Cultural API",
            Version = "v1",
            Description = "API para gestión de eventos culturales - .NET 9",
            Contact = new OpenApiContact
            {
                Name = "Equipo de Desarrollo",
                Email = "desarrollo@ucb.edu.bo"
            }
        });

        // Configurar seguridad JWT para Swagger
        options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
        {
            Description = "JWT Authorization header usando el esquema Bearer. Ejemplo: \"Authorization: Bearer {token}\"",
            Name = "Authorization",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        });

        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }
        });

        // Incluir comentarios XML si los tienes
        var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        if (File.Exists(xmlPath))
        {
            options.IncludeXmlComments(xmlPath);
        }
    });
}

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

// Registrar UserSecurityService (ya está arriba, pero por si acaso)
builder.Services.AddTransient<IUserSecurityService, UserSecurityService>();

var app = builder.Build();

// ============================================
// SWAGGER UI (SOLO EN DESARROLLO)
// ============================================
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Gestión Cultural API v1");
        options.RoutePrefix = "swagger";  // Acceder en /swagger
        options.DocumentTitle = "Gestión Cultural - Documentación API";
    });
}
else
{
    // En producción: redirigir root a página de información
    app.MapGet("/", () =>
    {
        return Results.Redirect("/api/token/config");
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