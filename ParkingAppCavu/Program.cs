using Microsoft.EntityFrameworkCore;
using ParkingAppCavu.Data;
using ParkingAppCavu.Interfaces;
using ParkingAppCavu.Services;
using ParkingAppCavu.Utils;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<CarParkDbContext>(options =>
{
    // Replace with real connection to DB in real-world.
    options.UseInMemoryDatabase("LocalDb");
});
builder.Services.AddScoped<IBookingUtils, BookingUtils>();
builder.Services.AddScoped<ICarParkingService, CarParkingService>();

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
