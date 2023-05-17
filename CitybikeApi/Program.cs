using Microsoft.EntityFrameworkCore;
// Data -folder includes DBContext!
using CitybikeApi;
using CitybikeApi.Data;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// UseSqlServer
builder.Services.AddDbContext<CitybikeDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CitybikeDBContext") ??
    throw new InvalidOperationException("Connection string 'CitybikeDBContext' not found.")));

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

