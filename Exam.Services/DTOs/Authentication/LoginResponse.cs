namespace Exam.Services.DTOs.Authentication
{
    public class LoginResponse
    {
        public bool IsSuccess { get; set; }
        public int? UserId { get; set; }
        public string? Name { get; set; }
        public string? ErrorMessage { get; set; }
    }
}