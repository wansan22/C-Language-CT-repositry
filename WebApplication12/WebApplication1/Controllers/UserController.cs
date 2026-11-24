using System.Security.Claims;
using Application.Behaviour.Authorization;
using Application.Behaviour.Course;
using Domain.Entities;
using Domain.Models;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Dto.Course;


namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("/api/auth")]
    public class UserController(IMediator mediator) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<UserEntity> Register([FromBody] RegisterDto dto)
        {
            UserEntity result = await mediator.Send(dto.Adapt<RegisterCommand>());
            return result;
        }

        [HttpPost("login")]
        public async Task<ActionResult<AccessPair>> Login([FromBody] LoginDto dto)
        {
            var command = dto.Adapt<LoginCommand>();
            command.UserAgent = Request.Headers.UserAgent.ToString();

            AccessPair? result = await mediator.Send(command);
            if (result is null)
                return Unauthorized();

            SetAuthCookies(result);
            return Ok(result);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] TokenRequest? request, CancellationToken cancellationToken)
        {
            try
            {
                var userAgent = Request.Headers.UserAgent.ToString();
                string? refreshToken = !string.IsNullOrWhiteSpace(request?.RefreshToken)
                    ? request.RefreshToken
                    : Request.Cookies["refreshToken"];

                if (string.IsNullOrWhiteSpace(refreshToken))
                    return Unauthorized();

                var pair = await mediator.Send(new RefreshCommand
                {
                    RefreshToken = refreshToken,
                    UserAgent = userAgent
                }, cancellationToken);

                if (pair is null)
                    return Unauthorized();

                SetAuthCookies(pair);
                return Ok(pair);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout(CancellationToken cancellationToken)
        {
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier)
                              ?? User.FindFirstValue("sub")
                              ?? User.FindFirstValue("id");

            if (!Guid.TryParse(userIdClaim, out Guid userId))
                return Unauthorized();

            var revoked = await mediator.Send(new Application.Behaviour.Authorization.LogoutCommand
            {
                UserId = userId
            }, cancellationToken);

            if (!revoked)
                return Unauthorized();

            Response.Cookies.Delete("accessToken");
            Response.Cookies.Delete("refreshToken");

            return NoContent();
        }

        [Authorize(Roles = "User,Admin")]
        [HttpGet]
        public bool GetMyRole()
        {
            return HttpContext.User.IsInRole("Admin");
        }

        private void SetAuthCookies(AccessPair pair)
        {
            CookieOptions accessOptions = new()
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddMinutes(15),
                Path = "/"
            };

            CookieOptions refreshOptions = new()
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddDays(28),
                Path = "/"
            };

            Response.Cookies.Append("accessToken", pair.AccessToken, accessOptions);
            Response.Cookies.Append("refreshToken", pair.RefreshToken, refreshOptions);
        }
    }

    public record TokenRequest(string RefreshToken);

    public class RegisterDto
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public DateTime Birthday { get; set; }
    }

    public class LoginDto
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }
}