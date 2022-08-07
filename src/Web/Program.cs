using Microsoft.EntityFrameworkCore;
using RToora.DemoApi.Web.DB;
using RToora.DemoApi.Web.Repository;
using RToora.DemoApi.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<ITodoItemRepository, TodoItemRepository>();
builder.Services.AddScoped<ITodoItemService, TodoItemService>();

builder.Services.AddDbContext<TodoContext>(options => options.UseInMemoryDatabase("TodoList"));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // https://docs.microsoft.com/en-us/aspnet/core/tutorials/web-api-help-pages-using-swagger?view=aspnetcore-6.0#openapi-specification-openapijson
    // https://docs.microsoft.com/en-us/aspnet/core/tutorials/getting-started-with-swashbuckle?view=aspnetcore-6.0&tabs=visual-studio
    // To review OpenAPI specification navigate to: /swagger/v1/swagger.json
    // to review Swagger UI navigate to: /swagger
    app.UseSwagger(options =>
    {
        // Will generate and expose swagger JSON in v2 instead of the default v3
        //options.SerializeAsV2 = true;
    });
    app.UseSwaggerUI(options =>
    {
        // This will server Swagger UI at app's root
        //options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        //options.RoutePrefix = string.Empty;
    });
    
    app.UseDeveloperExceptionPage();  // app.UseExceptionHandler("/error");  // when running on non-dev environments. https://docs.microsoft.com/en-us/aspnet/core/web-api/handle-errors?view=aspnetcore-6.0    
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
