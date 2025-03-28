using System.ComponentModel.DataAnnotations;

namespace PullAt.Models
{
    public class User
    {
        [Key]
        public int Id { get; set;}
        [Required(ErrorMessage = "Username is required.")]
        public string? Username { get; set;}
        [Required(ErrorMessage = "Password is required.")]
        public string? Password { get; set;}
        [Required(ErrorMessage = "Email is required.")]
        public string? Email { get; set;}
    }
}