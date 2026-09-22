using TaskManager.Api.Services;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddSingleton<ITaskRepository, InMemoryTaskRepository>();

// Injecting Global exception handling services
builder.Services.AddExceptionHandler<TaskManger.Api.Exceptions.GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "Asset Management API ");
        options.RoutePrefix = "swagger"; 
        
    });
}

app.UseHttpsRedirection();

// NOTE!!
app.UseAuthorization();

app.UseExceptionHandler();

app.MapControllers();


app.Run();
