using System.ComponentModel.DataAnnotations;

namespace ReservasPruebaUNI.Models
{
    public class User
    {
        public int Id { get; set; }
        public required string Name { get; set; } = string.Empty;
        [EmailAddress(ErrorMessage = "El correo no es valido")]
        public required string Email { get; set; } = string.Empty;
        public required string Password { get; set; } = string.Empty;
    }
}
