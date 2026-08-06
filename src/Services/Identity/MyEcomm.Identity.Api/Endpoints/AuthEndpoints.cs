using MyEcomm.Contracts.Common;
using MyEcomm.Contracts.Identity;
using MyEcomm.Identity.Api.Models;
using MyEcomm.Identity.Api.Repositories;
using MyEcomm.Identity.Api.Services;

namespace MyEcomm.Identity.Api.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/identity");

        group.MapPost("/login", (LoginRequest request, IUserRepository repo, JwtTokenService tokenService) =>
        {
            var user = repo.GetByUsername(request.Username);
            if (user is null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                return Results.Json(new ErrorResponse { Message = "Invalid username or password." }, statusCode: StatusCodes.Status401Unauthorized);
            }

            var (token, expiresAt) = tokenService.GenerateToken(user);
            return Results.Ok(new LoginResponse
            {
                Token = token,
                ExpiresAt = expiresAt,
                UserId = user.Id.ToString(),
                Username = user.Username,
            });
        });

        group.MapPost("/register", (RegisterRequest request, IUserRepository repo, JwtTokenService tokenService) =>
        {
            if (repo.UsernameExists(request.Username))
            {
                return Results.Conflict(new ErrorResponse { Message = "Username is already taken." });
            }

            var user = repo.Add(new User
            {
                Username = request.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            });

            var (token, expiresAt) = tokenService.GenerateToken(user);
            return Results.Ok(new LoginResponse
            {
                Token = token,
                ExpiresAt = expiresAt,
                UserId = user.Id.ToString(),
                Username = user.Username,
            });
        });
    }
}
