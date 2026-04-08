using ContrackAPI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddApplicationConfiguration();
var app = builder.Build();
Common.HttpContextAccessor = app.Services.GetRequiredService<IHttpContextAccessor>();
app.UseApplicationConfiguration();
app.Run();