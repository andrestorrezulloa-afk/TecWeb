using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using TecWeb.Core.Interfaces;
using TecWeb.Core.Services;
using TecWeb.Infrastructure.Data;
using TecWeb.Infrastructure.Repositories;
using TecWeb.Infrastructure.Validators;
using TecWeb.Mappings;

var builder = WebApplication.CreateBuilder(args);

// -------------------- DbContext --------------------
builder.Services.AddDbContext<GestionCulturalContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("connectionDB"))
);

// -------------------- AutoMapper --------------------

builder.Services.AddAutoMapper(cfg =>
{
    
}, typeof(MappingProfile));


// -------------------- FluentValidation (API moderna) --------------------
builder.Services.AddControllers();


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
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "TecWeb API", Version = "v1" });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "TecWeb API V1");
        c.RoutePrefix = "swagger"; 
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
