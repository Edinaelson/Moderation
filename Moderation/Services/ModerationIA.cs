using System.ClientModel;

namespace Moderation.Services;

using OpenAI.Moderations;

public class ModerationIA
{
    
    string filePath = "C:/dev/dotnet/280825/Moderation/API_KEE.txt";
    
    public void Moderate()
    {
        ModerationClient client = new(
            model: "omni-moderation-latest",
            apiKey: Environment.GetEnvironmentVariable(filePath)
        );

        ClientResult<ModerationResult> moderation = client.ClassifyText("I want to kill them.");    
    }
    

}