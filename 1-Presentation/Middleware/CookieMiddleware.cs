using Infrastructure;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Instagram_Api.Presentation.Middleware
{
    public class CookieMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly JwtSettings _jwtSettings;


        public CookieMiddleware(RequestDelegate next, IOptions<JwtSettings> jwtSettings)
        {
            _next = next;
            _jwtSettings = jwtSettings.Value;

    }

    public async Task Invoke(HttpContext context)
        {
            var token = context.Request.Cookies["IG_LOGIN"];

            if (context.Request.Headers["Authorization"].IsNullOrEmpty())
            {
                context.Request.Headers.Add("Authorization", $"Bearer {token}");
            }

            if (!string.IsNullOrEmpty(token))
            {

                if (context.Request.Cookies.TryGetValue("IG_LOGIN", out var jwtCookie))
                {
                    // Validate the JWT token
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var tokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = _jwtSettings.Issuer,
                        ValidAudience = _jwtSettings.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret))
                    };

                    try
                    {
                        var principal = tokenHandler.ValidateToken(jwtCookie, tokenValidationParameters, out _);
                        context.User = principal; // Set the principal to the context
                    }
                    catch
                    {
                        // Handle validation errors or unauthorized access
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        return;
                    }
                }

            }
            await _next(context);
        }
    }
}
