using Serilog;
using System.Text.Json.Serialization;
using TradingDashboard.API.Middleware;
using TradingDashboard.API.Swagger;
using TradingDashboard.Application;
using TradingDashboard.Infrastructure;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Configure JSON serializer to use string representation for enums
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// Add services to the container.
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen(options =>
{
    // Converts enums to strings + shows per-value XML descriptions in Swagger
    options.SchemaFilter<EnumSchemaFilter>();

    // Load XML comments from all projects
    foreach (var xmlFile in Directory.GetFiles(AppContext.BaseDirectory, "TradingDashboard.*.xml"))
    {
        options.IncludeXmlComments(xmlFile);
    }
});


builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

//Global exception handler.
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<ExceptionHandlingMiddleware>();

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

var app = builder.Build();

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
        c.RoutePrefix = string.Empty;
    });

}


app.UseHttpsRedirection();
app.MapControllers();


app.Run();

