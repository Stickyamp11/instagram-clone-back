using Application;
using Infrastructure;
using Infrastructure.Persistence;
using Instagram_Api.Application.Hubs;
using Instagram_Api.Presentation.Middleware;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddApplication();
    builder.Services.AddInfrastructure(builder.Configuration);

    builder.Services.AddAuthentication(defaultScheme: JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration.GetSection(JwtSettings.SectionName)["Issuer"],
            ValidAudience = builder.Configuration.GetSection(JwtSettings.SectionName)["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection(JwtSettings.SectionName)["Secret"]))
        });
    builder.Services.AddAuthentication(options =>
        {
            options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
        })
        .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
        {
            options.Events.OnValidatePrincipal = async context =>
            {
                var jwtCookie = context.Request.Cookies["YourJwtCookieName"];
                if (!string.IsNullOrEmpty(jwtCookie))
                {
                    // Extract and validate JWT
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var principal = tokenHandler.ValidateToken(jwtCookie, new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = builder.Configuration.GetSection(JwtSettings.SectionName)["Issuer"],
                        ValidAudience = builder.Configuration.GetSection(JwtSettings.SectionName)["Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection(JwtSettings.SectionName)["Secret"]))
                    }, out _);

                    context.Principal = principal;
                }
            };
        })
        .AddGoogle(GoogleDefaults.AuthenticationScheme, googleOptions =>
            {
                googleOptions.Events.OnCreatingTicket = context =>
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, context.Principal.FindFirstValue(ClaimTypes.NameIdentifier)),
                        new Claim(ClaimTypes.Name, context.Principal.FindFirstValue(ClaimTypes.Name)),
                    };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection(JwtSettings.SectionName)["Secret"]));
                    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                    var token = new JwtSecurityToken(
                        issuer: "localhost",
                        audience: "localhost",
                        claims: claims,
                        expires: DateTime.Now.AddMinutes(30),
                        signingCredentials: creds);

                    var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

                    // Add JWT token to the response
                    context.Response.Headers.Add("Authorization", "Bearer " + jwtToken);

                    return Task.CompletedTask;
                };
            });


    //builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    //.AddCookie(options =>
    //{
    //    options.Events.OnValidatePrincipal = async context =>
    //    {
    //        var jwtCookie = context.Request.Cookies["YourJwtCookieName"];
    //        if (!string.IsNullOrEmpty(jwtCookie))
    //        {
    //            var tokenHandler = new JwtSecurityTokenHandler();
    //            try
    //            {
    //                var tokenValidationParameters = new TokenValidationParameters
    //                {
    //                    ValidateIssuer = true,
    //                    ValidateAudience = true,
    //                    ValidateLifetime = true,
    //                    ValidateIssuerSigningKey = true,
    //                    ValidIssuer = "yourIssuer",
    //                    ValidAudience = "yourAudience",
    //                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("yourSecret"))
    //                };

    //                var principal = tokenHandler.ValidateToken(jwtCookie, tokenValidationParameters, out _);
    //                context.Principal = principal;
    //            }
    //            catch (Exception)
    //            {
    //                context.RejectPrincipal(); // Reject the principal if validation fails
    //            }
    //        }
    //    };
    //});




    //.AddCookie("CustomCookie", options =>
    // {
    //     options.Cookie.Name = "CUSTOM_AUTH_COOKIE";
    //     options.Events.OnValidatePrincipal = async context =>
    //     {
    //         // Custom validation logic here
    //     };
    // })


    //builder.Services.AddAuthentication(options =>
    //{
    //    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    //})
    //.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
    //{
    //    options.Cookie.HttpOnly = true;
    //    options.Cookie.SameSite = SameSiteMode.None;
    //    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    //    options.Events.OnValidatePrincipal = async context =>
    //    {
    //        var a = 1;
    //    };
    //});

    builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddSignalR();
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("Default_Front", builder => builder
                .WithOrigins("http://localhost:4200","https://localhost:4200")
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()
            );
    });
}


var app = builder.Build();
{

    app.UseExceptionHandler("/error");

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();

        //using (var scope = app.Services.CreateScope())
        //{
        //    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        //    db.Database.Migrate();
        //}
    }

    app.UseMiddleware<CookieMiddleware>();
    app.UseMiddleware<UserGuidMiddleware>();

    app.UseCors("Default_Front");

    app.UseHttpsRedirection();

    app.UseAuthentication();

    app.UseAuthorization();

    app.MapControllers();

    app.MapHub<NewPublicationsHub>("/new-pubs-hub");

    app.Run();
}
