using Dapper;
using Microsoft.IdentityModel.Tokens;
using ReservasPruebaUNI.Models;
using ReservasPruebaUNI.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace ReservasPruebaUNI.Endpoints
{
    public static class UserEndpoints
    {
        public static void MapUserEndpoints(this IEndpointRouteBuilder builder)
        {
            builder.MapGet("User/List", async (DbFactory dbFactory) =>
            {
                using var connection = dbFactory.Create();

                const string sql = "SELECT Id, Name, Email, Password FROM [User]";

                var users = await connection.QueryAsync<User>(sql);

                return Results.Ok(users);
            });

            builder.MapPost("User/Create", async (User request, DbFactory dbFactory) =>
            {
                using var connection = dbFactory.Create();

                const string sqlValidaUsuario = "SELECT Id, Name, Email, Password FROM [User] WHERE Email = @Email";

                var validaUsuario = await connection.QuerySingleOrDefaultAsync<User>(sqlValidaUsuario, new { Email = request.Email });

                if (validaUsuario is not null)
                    return Results.BadRequest("Ya existe un Usuario con el correo especificado");

                const string sql = "INSERT INTO [User] (Name, Email, Password)" +
                " VALUES (@Name, @Email, @Password)";

                var customer = await connection.ExecuteAsync(sql, request);

                return Results.NoContent();
            });

            builder.MapPost("User/Login", async (LoginRequest request, DbFactory dbFactory) =>
            {
                using var connection = dbFactory.Create();

                const string sql = "SELECT Id, Name, Email, Password FROM [User] WHERE Email = @Email AND Password = @Password";

                var userLog = await connection.QuerySingleOrDefaultAsync<User>(sql, new { Email = request.Email, Password = request.Password });

                if (userLog is null)
                    return Results.Unauthorized();

                var token = GenerateJwtToken(userLog);

                return Results.Ok(token);

            });
        }

        private static string GenerateJwtToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Abs12**66988775365asdasdas****JWFG"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "ReservasSalasUni",
                audience: "ReservasSalasUni",
                claims: null,
                expires: DateTime.Now.AddMinutes(1),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
