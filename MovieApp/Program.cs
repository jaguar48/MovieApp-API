using Microsoft.OpenApi.Models;
using MovieApp;
using MovieApp.Interface;
using MovieApp.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Movie App", Version = "v1" });
});


var configuration = builder.Configuration;
builder.Services.Configure<ApiSettings>(options =>
{
    options.OmdbApiKey = configuration["Api:OmdbApiKey"];
    options.BaseAddress = configuration["BaseAddress"];
});


builder.Services.AddHttpClient();

builder.Services.AddScoped<IOmdbService, OmdbService>();
builder.Services.AddSingleton<SearchHistoryService>();


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAnyOrigin",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});

var app = builder.Build();



if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
       
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Movie App v1");
    });
}


app.UseCors("AllowAnyOrigin");

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
