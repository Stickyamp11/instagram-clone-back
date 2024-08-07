using System.IdentityModel.Tokens.Jwt;

namespace Instagram_Api.Presentation.Middleware
{
    public class UserGuidMiddleware
    {
        private readonly RequestDelegate _next;

        public UserGuidMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var pathSegments = context.Request.Path.Value.Split('/');
            var userGuidFromRoute = pathSegments[^1];

            if (!string.IsNullOrEmpty(userGuidFromRoute) && Guid.TryParse(userGuidFromRoute, out _))
            {
                var jwtCookie = context.Request.Cookies["IG_LOGIN"];

                if (!string.IsNullOrEmpty(jwtCookie))
                {
                    var handler = new JwtSecurityTokenHandler();
                    var jwtToken = handler.ReadJwtToken(jwtCookie);

                    var userGuidFromJwt = jwtToken.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;

                    if (userGuidFromJwt != userGuidFromRoute)
                    {
                        context.Response.StatusCode = 403; 
                        return;
                    }
                }
                else
                {
                    context.Response.StatusCode = 401;
                    return;
                }
            }
            await _next(context);
        }
    }
}
