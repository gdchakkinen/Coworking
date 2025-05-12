using Coworking.Domain.Interfaces;
using Coworking.Infra;
using Coworking.Infra.Data;
using Coworking.Infra.Repositorios;
using Coworking.Application;
using Microsoft.EntityFrameworkCore;
using Coworking.API;
using Microsoft.Extensions.Configuration;
using Coworking.Domain.Entidades;
using Coworking.Application.Interfaces;
using Coworking.Application.Servicos;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddInfrastructure(builder.Configuration)
    .AddApplication()
    .AddWeb();

// Add services to the container.

builder.Services.Configure<SmtpSettings>(builder.Configuration
                                         .GetSection("SmtpSettings"));
builder.Services.AddSingleton<IEmailService, EmailService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<ISalaService, SalaService>();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

//DI
builder.Services.AddScoped<IReservaRepository, ReservaRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<ISalaRepository, SalaRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddHttpClient();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
