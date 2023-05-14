using FlightNavigatorApi.DAL;
using Microsoft.EntityFrameworkCore;
using FlightNavigatorApi.BusinessLogic;
using FlightNavigatorApi.Hubs;
using NLog.Web;

// Add services to the container.
try
{
    var builder = WebApplication.CreateBuilder(args);
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("CorsPolicy", builder => builder
            .WithOrigins("http://localhost:4200")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
    });
    builder.Services.AddDbContext<DbData>(Options => Options.UseSqlServer(builder.Configuration.GetConnectionString("Flight")));
 
    builder.Services.AddHostedService<FlightsLogic>();
    builder.Services.AddSignalR();

    builder.Services.AddControllers();
    builder.Logging.ClearProviders();
    builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Debug);
    builder.Host.UseNLog();
    //builder.Services.AddEndpointsApiExplorer();

    //builder.Services.AddSwaggerGen();
    var app = builder.Build();

    // Configure the HTTP request pipeline.

    app.UseHttpsRedirection();

    app.UseCors("CorsPolicy");

    app.UseAuthorization();
    //if (app.Environment.IsDevelopment())
    //{
    //    app.UseSwagger();
    //    app.UseSwaggerUI();
    //}
    app.MapControllers();
    app.MapHub<TerminalHub>("/board");

    app.Run();
}
catch
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
        

