var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) app.MapOpenApi();

app.UseHttpsRedirection();

app.MapPost("/upload", async (HttpRequest request) =>
{
    var chunkNumber = int.Parse(request.Headers["Chunk-Number"]!);
    var totalChunks = int.Parse(request.Headers["Total-Chunks"]!);
    var fileName = request.Headers["File-Name"];

    var tempFilePath = Path.Combine(Path.GetTempPath(), fileName);

    await using (var fileStream = new FileStream(tempFilePath, FileMode.Append, FileAccess.Write))
    {
        await request.Body.CopyToAsync(fileStream);
    }

    if (chunkNumber == totalChunks - 1)
    {
        var finalFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", fileName);
        if (File.Exists(finalFilePath)) File.Delete(finalFilePath);
        File.Move(tempFilePath, finalFilePath);
    }

    return Results.Ok();
});

app.Run();