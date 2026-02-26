using Microsoft.EntityFrameworkCore;
using CitybikeApi;
using CitybikeApi.Data;
using Microsoft.Extensions.FileProviders;

// Load environment variables from .env file
var envFilePath = Path.Combine(Directory.GetCurrentDirectory(), ".env");
if (File.Exists(envFilePath))
{
    foreach (var line in File.ReadAllLines(envFilePath))
    {
        if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#"))
            continue;
            
        var parts = line.Split('=', 2, StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length == 2)
        {
            Environment.SetEnvironmentVariable(parts[0].Trim(), parts[1].Trim());
        }
    }
}

var builder = WebApplication.CreateBuilder(args);

// Build connection string from environment variables
var dbServer = Environment.GetEnvironmentVariable("DB_SERVER") ?? "tcp:stone900.database.windows.net,1433";
var dbName = Environment.GetEnvironmentVariable("DB_NAME") ?? "GreenlizardDb";
var dbUser = Environment.GetEnvironmentVariable("DB_USER") ?? "kingdat4";
var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? "";

var connectionString = $"Server={dbServer};Initial Catalog={dbName};Persist Security Info=False;User ID={dbUser};Password={dbPassword};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

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
    options.UseSqlServer(connectionString));

builder.Services.AddDbContext<CitybiketripsMay2021DBContext>(options =>
    options.UseSqlServer(connectionString));

var app = builder.Build();

// Serve static files from the project's root directory
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) ?? Directory.GetCurrentDirectory()),
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