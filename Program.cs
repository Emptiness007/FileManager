using FileManager.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<FileService>();

var app = builder.Build();

app.MapControllers();

app.Run();
