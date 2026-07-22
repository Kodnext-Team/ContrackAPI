using ContrackAPI;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();

builder.Services.AddApplicationConfiguration();
builder.Services.Configure<AppSettings>(
    builder.Configuration.GetSection("AppSettings"));
var app = builder.Build();
Common.ImageBaseUrl = builder.Configuration["AppSettings:ImageBaseUrl"];

Common.HttpContextAccessor =
    app.Services.GetRequiredService<IHttpContextAccessor>();

app.UseApplicationConfiguration();

app.Run();