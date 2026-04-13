using ContrackAPI;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddApplicationConfiguration();
var app = builder.Build();
Common.HttpContextAccessor = app.Services.GetRequiredService<IHttpContextAccessor>();
app.UseApplicationConfiguration();
app.Run();