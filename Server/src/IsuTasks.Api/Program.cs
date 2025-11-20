using IsuTasks.Api;

var builder = WebApplication.CreateBuilder(args);
builder.Services.RegisterDependencies(builder.Configuration);

var app = builder.Build();
app.ConfigurePipeline();

app.Run();
