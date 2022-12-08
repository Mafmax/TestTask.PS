using Mafmax.FileConverter.BackgroundServices;
using Mafmax.FileConverter.BusinessLogic.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
builder.Services.SetupBusinessLayer(builder.Configuration);
builder.Services.AddHostedService<OldFilesRemover>();
var app = builder.Build();
app.MapGet("/", () => "Hello World!");

app.Run();