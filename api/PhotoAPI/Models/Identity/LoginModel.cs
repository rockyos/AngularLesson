using System.ComponentModel.DataAnnotations;


namespace PhotoAPI.Models.Identity
{
    public class LoginModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

       // public bool RememberMe { get; set; }
    }
}
