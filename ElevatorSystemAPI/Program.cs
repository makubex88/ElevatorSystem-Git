using ElevatorSystemAPI;
using ElevatorSystemAPI.Application.Interfaces;
using ElevatorSystemAPI.Application.Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);


// Configure Serilog before building the app
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()          // keep console logging
    .WriteTo.File("Logs/elevatorlog-.txt", rollingInterval: RollingInterval.Day)  // daily rolling log files
    .CreateLogger();

builder.Host.UseSerilog(); // Use Serilog for logging

// 1. Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactWithCredentials", policy =>
    {
        policy
            .WithOrigins("http://localhost:5173")  // your frontend origin
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();   // <- important to allow credentials
    });
});



// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSignalR();

// Register elevator system option to eliminate primitive dependency 
builder.Services.Configure<ElevatorSystemOptions>(options =>
{
    options.ElevatorCount = 4;
});

// dependency injection for the service and scheduler
builder.Services.AddSingleton<IElevatorService, ElevatorService>();
builder.Services.AddSingleton<IElevatorScheduler, ElevatorScheduler>();

// simulation of the elevator queues.
builder.Services.AddHostedService<SimulationBackgroundService>();


var app = builder.Build();

app.MapHub<ElevatorHub>("/elevatorHub");


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

// 2. Use CORS policy
app.UseCors("AllowReactWithCredentials");

app.MapControllers();

app.Run();
