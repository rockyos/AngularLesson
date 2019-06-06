using System.ComponentModel.DataAnnotations;


namespace PhotoAPI.Models.Identity
{
    public class ForgotPasswordModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
