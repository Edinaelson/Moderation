using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Moderation.Data;
using Moderation.Interfaces;
using Moderation.Services;


namespace Moderation;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        var apiKey = builder.Configuration["Gemini:GOOGLE_API_KEY"]
                     ?? throw new Exception("API Key do Gemini não configurada!");
        
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
        
        builder.Services.AddEndpointsApiExplorer();
        
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo 
            { 
                Title = "API de Moderação", 
                Version = "v1" 
            });
    
            // Ativa os atributos de anotação
            c.EnableAnnotations();
        });
        
        builder.Services.AddHttpClient<IModerationClient, GeminiModerationService>();
        
        builder.Services.AddSingleton<ModerationIA>();
        builder.Services.AddAuthorization();
        builder.Services.AddControllers();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API de Moderação v1");
            });
           
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
        app.Run();
    }
}