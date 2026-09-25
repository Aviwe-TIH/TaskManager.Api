using Microsoft.EntityFrameworkCore;
using TaskManager.Api.Services;


var builder = WebApplication.CreateBuilder(args);

// Add framework services
builder.Services.AddControllers();
builder.Services.AddOpenApi();

// Register application services and DbContext

builder.Services.AddScoped<ITaskRepository, TaskService>();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=taskDB")
);

// Global exception handling
builder.Services.AddExceptionHandler<TaskManger.Api.Exceptions.GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();


// Ensuring the data seeding happens
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

// 1. Exception handling must wrap the entire request pipeline
app.UseExceptionHandler();

// 2. Dev documentation UI
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "TaskManager API");
        options.RoutePrefix = "swagger";
    });
}

// 3. Security and routing
app.UseHttpsRedirection();
app.UseAuthorization();

// 4. Endpoints
app.MapControllers();

app.Run();