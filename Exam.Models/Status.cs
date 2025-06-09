using System.ComponentModel.DataAnnotations;

namespace Exam.Models
{
    public class Status
    {
        public int StatusId { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "Description cannot exceed 20 characters")]
        public string? Description { get; set; }
    }
}
