using ChatUpdater.Extensions;
using ChatUpdater.Middlewares;
using ChatUpdater.SignalR;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
Log.Logger = new LoggerConfiguration()
    .WriteTo.File("Logs/Logs.txt", rollingInterval: RollingInterval.Day)
    .WriteTo.Console()
    .CreateLogger();

// Add services to the container.
builder.Services.AddSignalR();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder => builder
            .WithOrigins("https://localhost:4200")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});
builder.Services.AddHttpContextAccessor();


builder.Host.UseSerilog(Log.Logger);

builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseStaticFiles();



app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseCors(u => u.AllowAnyHeader().AllowAnyMethod()
.AllowCredentials()
.WithOrigins("https://localhost:4200"));

app.UseMiddleware<ErrorHandlerMiddleware>();
app.MapControllers();

app.MapHub<MessageHub>("hubs/messages");
app.MapFallbackToController("Index", "Fallback");
await app.Migrate();
app.Run();
