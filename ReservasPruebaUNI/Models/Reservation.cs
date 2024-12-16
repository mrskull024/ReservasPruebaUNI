namespace ReservasPruebaUNI.Models
{
    public class Reservation
    {
        public long Id { get; set; }
        public long RoomId { get; set; }
        public long UserId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
