using DotNetEnv;
using Microsoft.OpenApi.Models;
using System.Net;
using Microsoft.AspNetCore.RateLimiting;
using Polly;
using Polly.Extensions.Http;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Http;
using Yarp.ReverseProxy.Transforms;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using Microsoft.Extensions.Http;

var builder = WebApplication.CreateBuilder(args);

// Load .env file
Env.Load();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Configure rate limiting
builder.Services.AddRateLimiter(options =>
{
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: context.User.Identity?.Name ?? context.Request.Headers.Host.ToString(),
            factory: partition => new FixedWindowRateLimiterOptions
            {
                AutoReplenishment = true,
                PermitLimit = 100,
                Window = TimeSpan.FromMinutes(1)
            }));
});

// Configure circuit breaker
builder.Services.AddHttpClient("DefaultClient")
    .AddTransientHttpErrorPolicy(builder => builder
        .CircuitBreakerAsync(
            handledEventsAllowedBeforeBreaking: 5,
            durationOfBreak: TimeSpan.FromSeconds(30)
        ));

// Cấu hình Swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Tasco Gateway API",
        Version = "v1",
        Description = "Gateway API for Tasco System"
    });

    // Thêm các route từ các service
    c.AddServer(new OpenApiServer
    {
        Url = "https://localhost:7059",
        Description = "Gateway Server"
    });

    // Thêm các route từ UserAuthService
    c.SwaggerGeneratorOptions.SwaggerDocs.Add("auth", new OpenApiInfo
    {
        Title = "Authentication API",
        Version = "v1",
        Description = "Authentication endpoints"
    });

    // Thêm các route từ TaskService
    c.SwaggerGeneratorOptions.SwaggerDocs.Add("tasks", new OpenApiInfo
    {
        Title = "Task Management API",
        Version = "v1",
        Description = "Task management endpoints"
    });
});

// Add CORS with more restrictive policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins", policy =>
    {
        policy.WithOrigins(
                "https://localhost:3000", // React Native app
                "https://your-production-domain.com"
            )
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

// Configure reverse proxy with timeout
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"))
    .AddTransforms(transformBuilderContext =>
    {
        transformBuilderContext.AddRequestTransform(context =>
        {
            // Set timeout using HttpClient.Timeout
            context.ProxyRequest.Options.Set(new HttpRequestOptionsKey<TimeSpan>("RequestTimeout"), TimeSpan.FromSeconds(30));
            return default;
        });
    });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Tasco Gateway API V1");
        c.SwaggerEndpoint("/swagger/auth/swagger.json", "Authentication API");
        c.SwaggerEndpoint("/swagger/tasks/swagger.json", "Task Management API");
    });
}

app.UseHttpsRedirection();
app.UseCors("AllowSpecificOrigins");
app.UseRateLimiter();

app.MapControllers();
app.MapReverseProxy();

app.Run();