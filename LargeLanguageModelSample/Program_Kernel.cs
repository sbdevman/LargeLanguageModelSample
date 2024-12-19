using LargeLanguageModelSample;
using Microsoft.SemanticKernel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

partial class Program
{
    private static Kernel GetKernel(Settings settings)
    {
        ArgumentNullException.ThrowIfNull(settings);

        IKernelBuilder kernelBuilder = Kernel.CreateBuilder();
        kernelBuilder.AddOpenAIChatCompletion(
            settings.ModelId, 
            settings.OpenAISecretKey);

        kernelBuilder.Services.AddLogging(c =>
        {
            c.AddConsole().SetMinimumLevel(LogLevel.Trace);
        });

        kernelBuilder.Services.ConfigureHttpClientDefaults(builder =>
        {
            builder.AddStandardResilienceHandler();
        });
        
        var kernel = kernelBuilder.Build();
        
        kernel.ImportPluginFromFunctions("",
        [
            kernel.CreateFunctionFromMethod(
                method: GetAuthorInfo,
               functionName: nameof(GetAuthorInfo),
               description: "Gets the author's biography.")
        ]);
        
        return kernel;
    }


    private static string GetAuthorInfo()
    {
        return string.Empty;
    }
}