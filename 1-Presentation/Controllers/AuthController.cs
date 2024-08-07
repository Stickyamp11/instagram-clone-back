using Microsoft.AspNetCore.Mvc;
using Instagram_Api.Application.Auth.Models;
using FluentResults;
using MediatR;
using Instagram_Api.Application.Auth.Commands;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Azure.Core;
using Domain.Entities;
using Application;
using Microsoft.AspNetCore.Authentication.Cookies;
using Azure;
using Microsoft.AspNetCore.Http;
using Infrastructure;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Instagram_Api.Application.Auth.Queries;
using Instagram_Api.Contracts.Auth;

namespace Instagram_Api.Presentation.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : Controller
    {
        private readonly ISender _mediator;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IConfiguration _configuration;

        public AuthController(ISender mediator, IJwtTokenGenerator jwtTokenGenerator, IConfiguration configuration)
        {
            _mediator = mediator;
            _jwtTokenGenerator = jwtTokenGenerator;
            this._configuration = configuration;
        }

        [Route("register")]
        [HttpPost]
        public async Task<IActionResult> Register(Contracts.Auth.RegisterRequest request)
        {
            var requestAplication = new Application.Auth.Models.RegisterRequest(request.FullName, request.Email, request.Password);

            var command = new RegisterCommand(requestAplication);
            Result<AuthResult> response = await _mediator.Send(command);

            if (response.IsSuccess)
            {
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddDays(1)
                };

                Response.Cookies.Append("IG_LOGIN", response.Value.Token, cookieOptions);

                return Ok(response.Value);
            }

            var firstError = response.Errors[0];
            return Problem(statusCode: StatusCodes.Status400BadRequest, detail: firstError.Message);

        }

        [Route("login")]
        [HttpPost]
        public async Task<IActionResult> Login(Contracts.Auth.LoginRequest request)
        {
            var requestAplication = new Application.Auth.Models.LoginRequest(request.Email, request.Password);

            var command = new LoginCommand(requestAplication);
            Result<AuthResult> response = await _mediator.Send(command);

            if (response.IsSuccess)
            {
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,  // Keep this true if using HTTPS
                    SameSite = SameSiteMode.None,  // Change this from Lax to None
                    Expires = DateTime.UtcNow.AddDays(1)
                };

                Response.Cookies.Append("IG_LOGIN", response.Value.Token, cookieOptions);

                LoginResponse loginResponse = new LoginResponse(response.IsSuccess);
                return Ok(loginResponse);
            }

            var firstError = response.Errors[0];
            return Problem(statusCode: StatusCodes.Status400BadRequest, detail: firstError.Message);
        }

        [Route("IsAuth")]
        [HttpGet]
        public IActionResult IsAuth()
        {
            if(HttpContext.User.Identity == null)
            {
                return Ok(new { IsAuthenticated = false });
            }

            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return Ok(new { IsAuthenticated = true });
            }
            
            return Ok(new { IsAuthenticated = false });   
        }

        [Route("google")]
        [HttpGet]
        public async Task LoginGoogle()
        {
            await HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme, new AuthenticationProperties
            {
                RedirectUri = Url.Action("PostGoogle")
            });

            //return Problem(statusCode: StatusCodes.Status400BadRequest, detail: "");
        }

        [Route("googlePost")]
        [HttpGet]
        public async Task<IActionResult> PostGoogle()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            if (!result.Succeeded || result.Principal == null)
            {
                return Problem(statusCode: StatusCodes.Status401Unauthorized, detail: "User is not authenticated.");
            }
            var emailClaim = result.Principal.Claims.Where(c => c.Type.Contains("emailaddress")).FirstOrDefault().ToString();

            var requestAplication = new Application.Auth.Models.RegisterRequest("", emailClaim, "noPassword");
            var command = new RegisterCommand(requestAplication);
            Result<AuthResult> response = await _mediator.Send(command);
            Guid userGuid = Guid.Empty;

            if(response.IsSuccess)
            {
                userGuid = response.Value.UserGuid;

            }
            else
            {
                var queryCommand = new UserByUserEmailQuery(emailClaim);
                Result<User> queryResponse = await _mediator.Send(queryCommand);
                if (!queryResponse.IsSuccess)
                {
                    var error = response.Errors[0];
                    return Problem(statusCode: StatusCodes.Status400BadRequest, detail: error.Message);
                }

                userGuid = queryResponse.Value.Guid.Value;    
            }

            var token = _jwtTokenGenerator.GenerateToken(userGuid, emailClaim);

            Response.Headers.Add("Authorization", "Bearer " + token);

            return Ok(new { Token = token });
        }

    }
}
