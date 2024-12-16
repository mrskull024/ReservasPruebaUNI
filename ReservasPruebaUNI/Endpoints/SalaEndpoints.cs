using Dapper;
using ReservasPruebaUNI.Models;
using ReservasPruebaUNI.Services;

namespace ReservasPruebaUNI.Endpoints
{
    public static class SalaEndpoints
    {
        public static void MapRoomEndpoints(this IEndpointRouteBuilder builder)
        {
            builder.MapGet("Rooms", async (DbFactory dbFactory) =>
            {
                using var connection = dbFactory.Create();

                const string sql = "SELECT Id, Name, Capacity FROM Room";

                var rooms = await connection.QueryAsync<Room>(sql);

                return Results.Ok(rooms);
            });

            builder.MapGet("Room/{id}", async (long id, DbFactory dbFactory) =>
            {
                using var connection = dbFactory.Create();

                const string sql = "SELECT Id, Name, Capacity FROM Room WHERE Id = @RoomId";

                var room = await connection.QuerySingleOrDefaultAsync<Room>(sql, new { RoomId = id });

                return room is not null ? Results.Ok(room) : Results.NotFound();
            });

            builder.MapPost("Room", async (Room request, DbFactory dbFactory) =>
            {
                using var connection = dbFactory.Create();

                const string sqlValidaSala = "SELECT Id, Name, Capacity FROM Room WHERE Name = @RoomName";

                var validarSala = await connection.QuerySingleOrDefaultAsync<Room>(sqlValidaSala, new { RoomName = request.Name });

                if (validarSala is not null)
                    return Results.BadRequest("Ya existe una sala con el nombre especificado");

                const string sql = "INSERT INTO Room (Name, Capacity)" +
                " VALUES (@Name, @Capacity)";

                var room = await connection.ExecuteAsync(sql, request);

                return Results.NoContent();
            });

            builder.MapPut("Room/{id}", async (long id, Room request, DbFactory dbFactory) =>
            {
                using var connection = dbFactory.Create();

                const string sqlValidaSala = "SELECT Id, Name, Capacity FROM Room WHERE Name = @RoomName AND Id <> @RoomId";

                var validarSala = await connection.QuerySingleOrDefaultAsync<Room>(sqlValidaSala,
                    new
                    {
                        RoomName = request.Name,
                        RoomId = request.Id
                    });

                if (validarSala is not null)
                    return Results.BadRequest("Ya existe otra sala con el nombre especificado");

                const string sql = "UPDATE Room SET Name = @Name, Capacity = @Capacity WHERE Id = @Id";
                request.Id = id;

                var room = await connection.ExecuteAsync(sql, request);

                return Results.NoContent();
            });

            builder.MapDelete("Room/{id}", async (long id, DbFactory dbFactory) =>
            {
                using var connection = dbFactory.Create();

                const string sql = "DELETE FROM Room WHERE Id = @RoomId";

                var room = await connection.ExecuteAsync(sql, new { RoomId = id });

                return Results.NoContent();
            });
        }
    }
}
