using ContrackAPI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class JwtMiddleware : CustomException
{
    private readonly RequestDelegate _next;
    private readonly string _secretKey;
    private readonly string _issuer;
    private readonly string _audience;

    public JwtMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        _next = next;
        _secretKey = configuration["Jwt:Key"] ?? "";
        _issuer = configuration["Jwt:Issuer"] ?? "";
        _audience = configuration["Jwt:Audience"] ?? "";
    }

    public async Task Invoke(HttpContext context)
    {
        if (context.GetEndpoint()?.Metadata?.GetMetadata<IAllowAnonymous>() != null)
        {
            await _next(context);
            return;
        }
        var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
        if (string.IsNullOrWhiteSpace(authHeader) || !authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        {
            await WriteUnauthorized(context, "Unauthorized User");
            return;
        }
        var token = authHeader.Substring("Bearer ".Length).Trim();

        try
        {
            context.User = ValidateJwtToken(token);
            await _next(context);
        }
        catch (SecurityTokenExpiredException ex)
        {
            RecordException(ex);
            await WriteUnauthorized(context, "Token expired");
        }
        catch (Exception ex)
        {
            RecordException(ex);
            await WriteUnauthorized(context, "Invalid token");
        }
    }

    private ClaimsPrincipal ValidateJwtToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_secretKey);

        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = true,
            ValidIssuer = _issuer,
            ValidateAudience = true,
            ValidAudience = _audience,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };

        return tokenHandler.ValidateToken(token, validationParameters, out _);
    }

    private static async Task WriteUnauthorized(HttpContext context, string message)
    {
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        context.Response.ContentType = "application/json";
        var json = System.Text.Json.JsonSerializer.Serialize(Common.ErrorMessage(message));
        await context.Response.WriteAsync(json);
    }
}