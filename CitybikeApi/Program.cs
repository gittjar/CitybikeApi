using Microsoft.EntityFrameworkCore;
using CitybikeApi;
using CitybikeApi.Data;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add CORS policy for frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        builder => builder
            .WithOrigins("https://solitacitybike.azurewebsites.net")
            .AllowAnyHeader()
            .AllowAnyMethod());
});

builder.Services.AddDbContext<CitybikeDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CitybikeDBContext") ??
    throw new InvalidOperationException("Connection string 'CitybikeDBContext' not found.")));

builder.Services.AddDbContext<CitybiketripsMay2021DBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CitybikeDBContext") ??
    throw new InvalidOperationException("Connection string 'CitybikeDBContext' not found.")));

var app = builder.Build();

// Serve static files from the project's root directory
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)),
    RequestPath = "/static"
});

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Citybike API V1");
    c.InjectStylesheet("/static/swagger-custom.css"); // Inject custom CSS
    c.DocumentTitle = "Citybike API Documentation"; // Set custom page title
    c.RoutePrefix = string.Empty; // Set Swagger UI at the app's root
});

// Enable CORS for frontend
app.UseCors("AllowFrontend");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();