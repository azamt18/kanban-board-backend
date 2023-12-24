using KanbanBoard.Core.Settings;
using KanbanBoard.Database;
using KanbanBoard.EventConsumer;
using KanbanBoard.EventPublisher;
using KanbanBoard.Service;
using KanbanBoard.Service.Card;
using KanbanBoard.Service.List;
using KanbanBoard.WebApi.Helpers;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var corsSettings = builder.Configuration.GetSection("CorsSettings").Get<CorsSettings>();

builder.Services.AddCors();
builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.InputFormatterMemoryBufferThreshold = 1024 * 1024 * 30;
    options.OutputFormatterMemoryBufferThreshold = 1024 * 1024 * 30;
}).AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new ApiDateTimeConverter(
        "yyyy-MM-dd HH:mm:ss",
        new []
        {
            "yyyy-MM-dd",
            "yyyy-MM-dd HH:mm:ss"
        }
    ));
});

// EventBus
builder.Services.Configure<EventBusSettings>(builder.Configuration.GetSection("EventBusSettings"));

// Database - SQLite
builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// EventBus
builder.Services.AddHostedService<RabbitMqConsumer>();
builder.Services.AddScoped<RabbitMqPublisher>();

// Services
builder.Services.AddScoped<ListService>();
builder.Services.AddScoped<CardService>();


var app = builder.Build();
app.UseRouting();
app.UseCors(policyBuilder =>
{
    policyBuilder.AllowAnyMethod();
    policyBuilder.AllowCredentials();
    policyBuilder.AllowAnyHeader();
    policyBuilder.WithOrigins(corsSettings.AllowedOrigins);
    policyBuilder.SetPreflightMaxAge(TimeSpan.FromDays(1));
});
app.UseDefaultFiles();
app.UseStaticFiles();
app.UseAuthorization();
app.UseAuthentication();
app.MapControllers();

app.Run();