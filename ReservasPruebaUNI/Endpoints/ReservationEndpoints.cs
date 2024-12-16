using Dapper;
using ReservasPruebaUNI.Models;
using ReservasPruebaUNI.Services;

namespace ReservasPruebaUNI.Endpoints
{
    public static class ReservationEndpoints
    {
        public static void MapReservattionEndpoints(this IEndpointRouteBuilder builder)
        {
            builder.MapGet("Reservations", async (DbFactory dbFactory) =>
            {
                using var connection = dbFactory.Create();

                const string sql = "SELECT Id, RoomId, UserId, StartDate, EndDate FROM Reservation";

                var reservation = await connection.QueryAsync<Reservation>(sql);

                return Results.Ok(reservation);
            });

            builder.MapGet("Reservation/{id}", async (long id, DbFactory dbFactory) =>
            {
                using var connection = dbFactory.Create();

                const string sql = "SELECT Id, RoomId, UserId, StartDate, EndDate FROM Reservation WHERE Id = @ReservationId";

                var reservation = await connection.QuerySingleOrDefaultAsync<Reservation>(sql, new { ReservationId = id });

                return reservation is not null ? Results.Ok(reservation) : Results.NotFound();
            });

            builder.MapPost("Reservation", async (Reservation request, DbFactory dbFactory) =>
            {
                using var connection = dbFactory.Create();

                const string sqlSala = "SELECT Id, Name, Capacity FROM Room WHERE Id = @RoomId";

                var room = await connection.QuerySingleOrDefaultAsync<Room>(sqlSala, new { RoomId = request.RoomId });

                if(room is null)
                    return Results.NotFound("Sala No Valida");

                const string sqlUser = "SELECT Id, Name, Email, Password FROM [User] WHERE Id = @UserId";

                var user = await connection.QuerySingleOrDefaultAsync<User>(sqlUser, new { UserId = request.UserId });

                if (user is null)
                    return Results.NotFound("Usuario No Valido");

                const string sql = "INSERT INTO Reservation (RoomId, UserId, StartDate, EndDate)" +
                " VALUES (@RoomId, @UserId, @StartDate, @EndDate)";

                var reservation = await connection.ExecuteAsync(sql, request);

                return Results.NoContent();
            });

            builder.MapPut("Reservation/{id}", async (long id, Reservation request, DbFactory dbFactory) =>
            {
                using var connection = dbFactory.Create();

                const string sqlSala = "SELECT Id, Name, Capacity FROM Room WHERE Id = @RoomId";

                var room = await connection.QuerySingleOrDefaultAsync<Room>(sqlSala, new { RoomId = request.RoomId });

                if (room is null)
                    return Results.NotFound("Sala No Valida");

                const string sqlUser = "SELECT Id, Name, Email, Password FROM [User] WHERE Id = @UserId";

                var user = await connection.QuerySingleOrDefaultAsync<User>(sqlUser, new { UserId = request.UserId });

                if (user is null)
                    return Results.NotFound("Usuario No Valido");

                const string sql = "UPDATE Reservation SET RoomId = @RoomId, UserId = @UserId, StartDate = @StartDate, " +
                " EndDate = @EndDate WHERE Id = @Id";
                request.Id = id;

                var reservation = await connection.ExecuteAsync(sql, request);

                return Results.NoContent();
            });

            builder.MapDelete("Reservation/Cancel/{id}", async (long id, DbFactory dbFactory) =>
            {
                using var connection = dbFactory.Create();

                const string sql = "DELETE FROM Reservation WHERE Id = @ReservationId";

                var reservation = await connection.ExecuteAsync(sql, new { ReservationId = id });

                return Results.NoContent();
            });
        }
    }
}
