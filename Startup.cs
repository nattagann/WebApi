using CShapTestWebApi.Entities;
using CShapTestWebApi.Services;
using Microsoft.OpenApi.Models;
using System.Reflection;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        // Service registrations
        services.AddControllers();

        // Initialize ProductService with initial products
        var initialProducts = new List<Product>
        {
            new Product { Id = 1, Name = "ProductA", Description = "This is product A", Price = 20.02 },
            new Product { Id = 2, Name = "ProductB", Description = "This is product B", Price = 11.11 },
        };

        services.AddScoped(provider => new ProductService(initialProducts));

        // Help documentation
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "C# Test Web API", Version = "v1" });

            // Comment with xml
            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseHttpsRedirection();

        // Add Swagger middleware
        app.UseSwagger();

        // Specify the Swagger UI endpoint
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "C# Test Web API V1");
        });

        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
