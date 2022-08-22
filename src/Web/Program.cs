using Microsoft.EntityFrameworkCore;
using RToora.DemoApi.Web.DB;
using RToora.DemoApi.Web.Repository;
using RToora.DemoApi.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//builder.Services.AddScoped<ITodoItemRepository, TodoItemRepository>();
//builder.Services.AddScoped<ITodoItemService, TodoItemService>();

//builder.Services.AddDbContext<TodoContext>(options => options.UseInMemoryDatabase("TodoList"));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => {
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Version = "v1.0",
        Title = "ToDo API",
        Description = "An ASP.NET Core Web API for managing ToDo items",
        TermsOfService = new Uri("https://example.com/terms"),
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Example Contact",
            Email = "example.contact@example.com",
            Url = new Uri("https://contact.example.com")
        },
        License = new Microsoft.OpenApi.Models.OpenApiLicense
        {
            Name = "Example license",
            Url = new Uri("https://license.example.com")
        }
    });

    // using System.Reflection;
    var xmlFilename = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

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

        options.InjectStylesheet("/css/swagger-ui/custom.css");
    });
    
    app.UseDeveloperExceptionPage();  // app.UseExceptionHandler("/error");  // when running on non-dev environments. https://docs.microsoft.com/en-us/aspnet/core/web-api/handle-errors?view=aspnetcore-6.0    
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseStaticFiles();

app.MapControllers();

app.Run();
