using OCR.Api.ExceptionHandlers;
using OCR.Services.Extensions;

namespace OCR;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        // Allow cors
        var myAllowSpecificOrigins = "_myAllowSpecificOrigins";
        builder.Services.AddCors(options =>
        {
            options.AddPolicy(myAllowSpecificOrigins,
                corsPolicyBuilder =>
                {
                    corsPolicyBuilder.WithOrigins("*")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
        });

        // Add services to the container.
        builder.Services.AddAuthorization();
        
        builder.Services.AddExceptionHandler<ArgumentExceptionHandler>();
        builder.Services.AddExceptionHandler<ValidationExceptionHandler>();
        builder.Services.AddExceptionHandler<ExceptionHandler>();
        
        builder.Services.AddControllers();
        
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();
        builder.Services.UseOcrServices();
        
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseExceptionHandler(options => { });

        app.UseHttpsRedirection();

        // Needs to be before UseAuthorization
        app.UseCors(myAllowSpecificOrigins);
        
        app.UseAuthorization();

        app.Run();
    }
}