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
        _secretKey = configuration["Jwt:Key"];
        _issuer = configuration["Jwt:Issuer"];
        _audience = configuration["Jwt:Audience"];
    }

    public async Task Invoke(HttpContext context)
    {
        var endpoint = context.GetEndpoint();

        if (endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() != null)
        {
            await _next(context);
            return;
        }
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        if (string.IsNullOrEmpty(token))
        {
            await WriteResponse(context, Common.ErrorMessage("Unauthorized User"));
            return;
        }
        try
        {
            var claimsPrincipal = ValidateJwtToken(token);

            if (claimsPrincipal != null)
            {
                context.User = claimsPrincipal;
            }
        }
        catch (SecurityTokenExpiredException ex)
        {
            RecordException(ex);
            await WriteResponse(context, Common.ErrorMessage("Token expired"));
            return;
        }
        catch (Exception ex)
        {
            RecordException(ex);
            await WriteResponse(context, Common.ErrorMessage("Invalid token"));
            return;
        }
        await _next(context);
    }

    private ClaimsPrincipal ValidateJwtToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_secretKey);

        var parameters = new TokenValidationParameters
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

        return tokenHandler.ValidateToken(token, parameters, out _);
    }

    private async Task WriteResponse(HttpContext context, Result result)
    {
        context.Response.StatusCode = 401;
        context.Response.ContentType = "application/json";
        var json = System.Text.Json.JsonSerializer.Serialize(result);
        await context.Response.WriteAsync(json);
    }
}