using Microsoft.EntityFrameworkCore;
using PostService.BusinessLogic;
using Serilog;
using PostService.Data;
using PostService.Logging;
using Polly;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
Log.Logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();
builder.Host.UseSerilog();

builder.Services.AddControllers();

builder.Services.AddDbContext<PostDbContext>(options =>
    options.UseInMemoryDatabase("PostDb"));

builder.Services.AddScoped<IPostHandler, PostHandler>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<GlobalExceptionHandler>();

builder.Services.AddHttpClient("FeedService")
    .AddTransientHttpErrorPolicy(policyBuilder => policyBuilder
        .CircuitBreakerAsync(
            handledEventsAllowedBeforeBreaking: 3,
            durationOfBreak: TimeSpan.FromSeconds(30)
        ));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<GlobalExceptionHandler>();
app.UseAuthorization();

app.MapControllers();

app.Run();
