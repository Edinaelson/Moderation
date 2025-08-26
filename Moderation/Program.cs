using Moderation.Adapter;
using Moderation.Interfaces;
using Moderation.Services;
using Moderation.Utils;
using OpenAI.Moderations;

namespace Moderation;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var apiKey = ApiKee.GetApiKey();
        
        builder.Services.AddSingleton(new ModerationClient(
            model: "omni-moderation-latest",
            apiKey: apiKey
        ));
        builder.Services.AddSingleton<IModerationClient, ModerationClientAdapter>();
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