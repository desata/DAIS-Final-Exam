namespace Exam.Web.Models.ViewModels
{
    public class UserListViewModel
    {
        public int TotalCount { get; set; }
        public List<UserViewModel> Users { get; set; }
    }

    public class UserViewModel
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
    }
}
