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
        
        builder.Services.AddHttpClient<IModerationClient, GeminiModerationService>();
        
        builder.Services.AddSingleton<ModerationIA>();
        builder.Services.AddAuthorization();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddControllers();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
        app.Run();
    }
}