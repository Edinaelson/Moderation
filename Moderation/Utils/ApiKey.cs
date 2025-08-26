
namespace Moderation.Utils;

public static class ApiKee
{
    
    public const string ApiKeyFilePath = @"C:\dev\dotnet\280825\Moderation\API_KEE.txt";
    
    public static string GetApiKey()
    {
        var fromEnv = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
        var key = !string.IsNullOrWhiteSpace(fromEnv) ? fromEnv : null;

        if (string.IsNullOrWhiteSpace(key))
        {
            if (!File.Exists(ApiKeyFilePath))
            {
                throw new InvalidOperationException(
                    $"Chave da API não encontrada. Defina a variável de ambiente 'OPENAI_API_KEY' " +
                    $"ou crie o arquivo '{ApiKeyFilePath}' contendo somente a chave."
                );
            }

            key = File.ReadAllText(ApiKeyFilePath).Trim();
        }

        if (string.IsNullOrWhiteSpace(key))
        {
            throw new InvalidOperationException(
                "A chave da API está vazia. Forneça uma chave válida via variável de ambiente 'OPENAI_API_KEY' " +
                "ou no arquivo configurado."
            );
        }

        return key;
    }
}
