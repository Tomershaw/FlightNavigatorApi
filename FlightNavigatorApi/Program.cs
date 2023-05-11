using FlightNavigatorApi.DAL;
using NLog;
using NLog.Web;
using Microsoft.EntityFrameworkCore;
using FlightNavigatorApi.BusinessLogic;

//var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
//logger.Debug("init main");

// Add services to the container.

 try
{
    var MyAllowSpecificOrigins = "https://localhost:7088/api/ApiControllerFlights";

    var builder = WebApplication.CreateBuilder(args);
    builder.Services.AddDbContext<DbData>(Options => Options.UseSqlServer(builder.Configuration.GetConnectionString("Flight")));
    builder.Services.AddScoped<DbData>();
    builder.Services.AddScoped<IFlightsLogic,FlightsLogic>();

    // Add services to the container.

    builder.Services.AddControllers();
    builder.Logging.ClearProviders();
    builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Debug);
    builder.Host.UseNLog();
    builder.Services.AddEndpointsApiExplorer();

    builder.Services.AddSwaggerGen();

    builder.Services.AddCors(options => {
        options.AddPolicy(
            "aaa",
            policy => {
                policy.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
            });
    });


    var app = builder.Build();

    // Configure the HTTP request pipeline.

    app.UseHttpsRedirection();

    app.UseCors("aaa");

    app.UseAuthorization();
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.MapControllers();

    app.Run();
}
catch (Exception e)
{
    // NLog: catch setup errors
    //logger.Error(e, "Stopped program because of exception");
    throw;
}
finally
{
    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
    NLog.LogManager.Shutdown();
}
        

