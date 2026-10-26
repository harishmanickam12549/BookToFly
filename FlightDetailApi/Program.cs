using FlightDetailApi.Configurations;
using FlightDetailApi.Data;
using FlightDetailApi.MappingDTO;
using FlightDetailApi.Models;
using FlightDetailApi.Repositories;
using FlightDetailApi.Repositories.IRepository;
using FlightDetailApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Add configurations
builder.Services.AddSwaggerConfiguration();
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddAutoMapper(cfg => cfg.AddProfile<FlightMapper>()); // AutoMapper profile registration
builder.Services.AddDatabaseContexts(builder.Configuration); // Ensure this method correctly configures your DbContext
builder.Services.AddCorsPolicy();
builder.Host.ConfigureLogging();

// Registering repositories and services
builder.Services.AddTransient<AdminService>();
builder.Services.AddScoped<IFlightRepository, FlightRepository>();
builder.Services.AddSingleton<JWTTokenService>();

var app = builder.Build();

// Enable CORS
app.UseCors();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Middleware Order
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
