using System.Text.Json.Serialization;
using TradingDashboard.API.Middleware;
using TradingDashboard.API.Swagger;
using TradingDashboard.Application;
using TradingDashboard.Infrastructure;


var builder = WebApplication.CreateBuilder(args);


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

//builder.Host.UseSerilog((context, configuration) =>
//    configuration.ReadFrom.Configuration(context.Configuration));

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/openapi/v1.json", "Trading Dashboard API");
        c.RoutePrefix = string.Empty;
    });
    //using var scope = app.Services.CreateScope();
    //var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    //dbContext.Database.Migrate();   // creates DB if not exists + applies all pending migrations
}


app.UseHttpsRedirection();
app.MapControllers();


app.Run();

