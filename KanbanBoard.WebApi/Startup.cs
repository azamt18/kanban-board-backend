using KanbanBoard.Core.Settings;
using KanbanBoard.Database;
using KanbanBoard.EventConsumer;
using KanbanBoard.EventPublisher;
using KanbanBoard.WebApi.Helpers;
using Microsoft.EntityFrameworkCore;

namespace KanbanBoard.WebApi;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    private IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        LoadEnvironmentSettings(services);
        
        ConfigureDatabase(services);
        ConfigureMessageBroker(services);
        
        ConfigureControllers(services);
    }

    private void LoadEnvironmentSettings(IServiceCollection services)
    {
        services.Configure<EventBusSettings>(Configuration.GetSection("EventBusSettings"));
    }
    
    private void ConfigureDatabase(IServiceCollection services)
    {
        services.AddDbContext<DatabaseContext>(options =>
            options.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));
    }

    private void ConfigureMessageBroker(IServiceCollection services)
    {
        services.AddHostedService<RabbitMqConsumer>();
        services.AddScoped<RabbitMqPublisher>();
    }
    
    
    private void ConfigureControllers(IServiceCollection services)
    {
        services.AddControllers().AddNewtonsoftJson(options =>
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
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}