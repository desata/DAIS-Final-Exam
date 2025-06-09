using System.ComponentModel.DataAnnotations;

namespace Exam.Models
{
    public class User
    {

        public int UserId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 100 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Username is required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 50 characters")]
        public string Username { get; set; }


        [Required(ErrorMessage = "Password is required")]
        [StringLength(256)]
        public string Password { get; set; }
    }
}
