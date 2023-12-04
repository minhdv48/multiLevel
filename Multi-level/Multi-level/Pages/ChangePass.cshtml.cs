using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Multi_level.Helper;
using Multi_level.Models;
using Multi_LevelModels;
using Newtonsoft.Json;

namespace Multi_level.Pages
{
    public class PagePassModel : PageModel
    {
        private readonly ILogger<PagePassModel> _logger;
        private Ibase _ibase;
        private readonly IHttpContextAccessor _contextAccessor;
        public PagePassModel(Ibase @base, ILogger<PagePassModel> logger, IHttpContextAccessor httpContextAccessor)
        {
            _ibase = @base;
            _logger = logger;
            _contextAccessor = httpContextAccessor;
        }
        [BindProperty]
        public string oldPass { get; set; }
        [BindProperty]
        public string newPass { get; set; }
        [BindProperty]
        public string RePass { get; set; }
        [ViewData]
        public string message { get; set; }
        public void OnGet()
        {
        }
        public IActionResult OnPost()
        {
            try
            {
                var userSess = SessionHelpers.GetUser(_contextAccessor);
                if (userSess != null)
                {
                    string pass = Utility.Sha256Hash(oldPass);
                    var checkLogin = _ibase.accountRepo.Read(x => x.ProfileId == userSess.ProfileId && x.Password == pass);
                    if (checkLogin == null)
                    {
                        message = "mật khẩu cũ không đúng. Vui lòng thử lại.";
                        return Page();
                    }

                    checkLogin.Password = Utility.Sha256Hash(newPass);
                     _ibase.accountRepo.Update(checkLogin);
                    _ibase.Commit();
                    SessionHelpers.Remove(_contextAccessor);
                    Response.Cookies.Delete("AspNetCore.Security");
                    Response.Cookies.Delete("AspNetCore.Security.Out");
                    message = "Đổi mật khẩu thành công. Vui lòng đăng nhập lại.";
                    return Page();
                }
                message = "Mật khẩu cũ không đúng. Vui lòng thử lại.";
                return Page();
            }
            catch (Exception ex)
            {
                message = "Lỗi:" + ex.Message;
                return Page();
            }
        }
    }
}
