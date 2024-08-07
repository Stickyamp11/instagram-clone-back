using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

public class CustomCookieAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public CustomCookieAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock)
        : base(options, logger, encoder, clock)
    {
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Cookies.TryGetValue("IG_LOGIN", out var cookieValue))
        {
            return AuthenticateResult.Fail("No cookie found");
        }

        // Validate and process the cookie value to create claims
        var claims = new[] {
            new Claim(ClaimTypes.Name, "user"), // Replace with actual user details
            new Claim(ClaimTypes.NameIdentifier, "userId") // Replace with actual user details
        };

        var identity = new ClaimsIdentity(claims, "CustomCookie");
        var principal = new ClaimsPrincipal(identity);

        var ticket = new AuthenticationTicket(principal, "CustomCookie");

        return AuthenticateResult.Success(ticket);
    }
}