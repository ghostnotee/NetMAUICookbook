using System.Text;
using OllamaSharp;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddSingleton(_ => new OllamaApiClient(new Uri("http://localhost:11434"), "llama3.2"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) app.MapOpenApi();

app.UseHttpsRedirection();

app.MapGet("/ask-question", async (string question, OllamaApiClient ollamaClient) =>
{
    var chatContentBuilder = new StringBuilder();
    await foreach (var stream in ollamaClient.GenerateAsync(question))
        if (stream?.Response != null)
            chatContentBuilder.Append(stream.Response);
    return Results.Content(chatContentBuilder.ToString());
});

app.Run();