using System.Diagnostics;
using Microsoft.SemanticKernel.Connectors.Redis;
using OllamaSharp;
using OllamaSharp.Models;
using OllamaSharp.Models.Chat;
using Spectre.Console;

string port = "11434";
Uri uri = new Uri($"http://localhost:{port}");
OllamaApiClient Ollama = new OllamaApiClient(uri);

Table table = new();
table.AddColumn("Name");
table.AddColumn("Size");

IEnumerable<Model> models = await Ollama.ListLocalModelsAsync(CancellationToken.None);

foreach (var model in models)
{
    table.AddRow(model.Name, model.Size.ToString("N0"));
}

AnsiConsole.Write(table);

string modelName = "llama3:latest";
Console.WriteLine();

Console.WriteLine($"Selected model: {modelName}");
Ollama.SelectedModel = modelName;

Console.Write("Please enter your prompt: ");

string? prompt = Console.ReadLine();

if (string.IsNullOrWhiteSpace(prompt))
{
    Console.WriteLine("Prompt is required. Exiting the app.");
    return;
}

Stopwatch timer = Stopwatch.StartNew();

ConversationContext context = new ConversationContext([]);

var chat = new Chat(Ollama);
while (true)
{
    var message = Console.ReadLine()!;
    await foreach (var answerToken in chat.SendAsync(message))
        Console.Write(answerToken);
}


// await foreach(var msg = Ollama.GenerateAsync(new GenerateRequest(){Context}))
//
// await foreach (var msg in Ollama.ChatAsync(new ChatRequest(), CancellationToken.None))
// {
//     Console.WriteLine(msg?.Message);
// }
// timer.Stop();
// Console.WriteLine();
// Console.WriteLine();
// Console.WriteLine($"Elapsed time: {timer.ElapsedMilliseconds}");

// partial class Program
// {
//     public static void Main(string[] args)
//     {


// Settings? settings = GetSetttings();
// if (settings is null)
// {
//     Console.WriteLine("Settings not found or not valid.");
//     return;
// }
//
// Kernel kernel = GetKernel(settings);
//
// KernelFunction function =
//     kernel.CreateFunctionFromPrompt(
//         "Author biography: {{ author.getAuthorBiography }}" +
//         "{{$question}}");
// KernelArguments arguments = new();
//
// IChatCompletionService completion = kernel.GetRequiredService<IChatCompletionService>();
//
// ChatHistory history = new ChatHistory(systemMessage: "You are an");
// OpenAIPromptExecutionSettings options = new() { ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions};
//
// StringBuilder builder = new StringBuilder();
// ConsoleKey key = ConsoleKey.A;
// while (key is not ConsoleKey.X)
// {
//     Console.WriteLine("Enter your question: ");
//     string question = Console.ReadLine()!;
//
//     arguments["question"] = question;
//
//     var response = await function.InvokeAsync(kernel, arguments);
//     
//     Console.WriteLine(response);
//     
//     history.AddUserMessage(question);
//
//     await foreach (StreamingChatMessageContent message in completion.GetStreamingChatMessageContentsAsync(history))
//     {
//         Console.WriteLine(message.Content);
//         builder.Append(message.Content);
//     }
//     
//     history.AddAssistantMessage(builder.ToString());
//     ChatMessageContent answer = await completion.GetChatMessageContentAsync(history);
//     history.AddAssistantMessage(answer.Content!);
//
//     Console.WriteLine(answer.Content);
//     
//     Console.WriteLine();
//
//     Console.WriteLine("Press X to exit ");
//
//     key = Console.ReadKey(intercept: true).Key;
//
// }
//     }
// }