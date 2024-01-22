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
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "c", Version = "v1" });
});

// Add configuration for API-related settings (e.g., API key)
builder.Configuration.Bind("Api", new ApiSettings());
builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection("Api"));

// Add HttpClient registration
builder.Services.AddHttpClient();

// Register OmdbService with IOmdbService interface
builder.Services.AddScoped<IOmdbService, OmdbService>();
builder.Services.AddSingleton<SearchHistoryService>();




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

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
