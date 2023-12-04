using System.ComponentModel.DataAnnotations;

namespace Multi_level.Helper
{
    public class UserLogin
    {
        [Required(ErrorMessage ="Vui lòng nhập số điện thoại đã đăng ký")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        public string Password { get; set; }
    }
    public class UserResetPassword
    {
        public int UserId { get; set; }
        public string NewPassword { get; set; }
        public string RePassword { get; set; }
    }
    public class UserSession
    {
        public string SessionId { get; set; }
        public int UserId { get; set; }
        public string FullName { get; set; }
        public int ProfileId { get; set; }
        public string CodeRefer { get; set; }
        public string ImagePath { get; set; }
    }
    public class UserChangePassword
    {
        public int UserId { get; set; }
        public string Password { get; set; }
        public string NewPassword { get; set; }
        public string RePassword { get; set; }
    }
}