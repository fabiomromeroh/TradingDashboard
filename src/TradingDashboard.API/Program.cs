using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;
using TradingDashboard.API.Middleware;
using TradingDashboard.Application;
using TradingDashboard.Application.Common.Configurations;
using TradingDashboard.Infrastructure;
using TradingDashboard.Infrastructure.Persistence.Seed;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
 .AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"]!))
    };
});

if (builder.Environment.IsDevelopment())
{
    builder.Services.AddAuthorization(options =>
    {
        options.DefaultPolicy = new AuthorizationPolicyBuilder()
            .RequireAssertion(_ => true) // always allow
            .Build();
    });
}
else
{
    builder.Services.AddAuthorization(); // normal policies
}

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:3000", "https://tradingdashboard.azurewebsites.net")
            .AllowCredentials()   // required for cookies to be sent cross-origin
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Services.AddControllers()
    .AddJsonOptions(options => AppJsonOptions.Configure(options.JsonSerializerOptions));


// Add services to the container.
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

//Global exception handler.
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<ExceptionHandlingMiddleware>();

var appInsightsConnectionString = builder.Configuration["ApplicationInsights:ConnectionString"];

if (!string.IsNullOrWhiteSpace(appInsightsConnectionString))
{
    builder.Services.AddApplicationInsightsTelemetry();
}
builder.Host.UseSerilog((context, services, configuration) =>
    configuration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(services));   // enables DI-based enrichers

builder.Logging.AddFilter("LuckyPennySoftware.AutoMapper.License", LogLevel.None);
builder.Logging.AddFilter("LuckyPennySoftware.MediatR.License", LogLevel.None);

var app = builder.Build();


app.UseAuthentication();
app.UseAuthorization();



app.UseDefaultFiles();   // serves index.html on "/"
app.UseStaticFiles();    // serves JS/CSS/assets from wwwroot

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

app.UseCors("AllowFrontend");

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/openapi/v1.json", "Trading Dashboard API");
        c.RoutePrefix = "swagger";
    });

}


app.UseHttpsRedirection();
app.MapControllers();

app.MapFallbackToFile("index.html");   // React Router fallback

if (!app.Environment.IsDevelopment())
{
    app.Services.ApplyMigrationsAndSeed(app.Configuration);

}

app.Run();

// Required for WebApplicationFactory<TEntryPoint>
public partial class Program { }