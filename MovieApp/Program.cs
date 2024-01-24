using Microsoft.OpenApi.Models;
using MovieApp;
using MovieApp.Interface;
using MovieApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    // Configure Swagger to include API descriptions and other settings
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Movie App", Version = "v1" });
});

// Add configuration for API-related settings (e.g., API key)
var configuration = builder.Configuration;
builder.Services.Configure<ApiSettings>(options =>
{
    options.OmdbApiKey = configuration["Api:OmdbApiKey"];
    options.BaseAddress = configuration["BaseAddress"];
});

// Add HttpClient registration
builder.Services.AddHttpClient();

builder.Services.AddScoped<IOmdbService, OmdbService>();
builder.Services.AddSingleton<SearchHistoryService>();

// Add CORS configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAnyOrigin",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        // Configure Swagger UI settings
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Movie App v1");
    });
}

// Enable CORS middleware
app.UseCors("AllowAnyOrigin");

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
