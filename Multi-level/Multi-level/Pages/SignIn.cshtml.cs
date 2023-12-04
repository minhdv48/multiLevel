using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Multi_level.Helper;
using Multi_level.Models;
using Multi_LevelModels;
using Newtonsoft.Json;

namespace Multi_level.Pages
{
    public class SignInModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private Ibase _ibase;
        private readonly IHttpContextAccessor _contextAccessor;
        public SignInModel(Ibase @base, ILogger<IndexModel> logger, IHttpContextAccessor httpContextAccessor)
        {
            _ibase = @base;
            _logger = logger;
            _contextAccessor = httpContextAccessor;
        }
        [BindProperty]
        public UserLogin model { get; set; }
        [ViewData]
        public string message { get; set; }
        public void OnGet()
        {
        }
        public IActionResult OnPost()
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string pass = Utility.Sha256Hash(model.Password);
                    var checkLogin = _ibase.accountRepo.Read(x => x.UserName == model.Username && x.Password == pass);
                   // var checkLogin = _ibase.accountRepo.Read(x => x.UserName == model.Username);
                    if (checkLogin == null)
                    {
                        message = "Số điện thoại hoặc mật khẩu không đúng. Vui lòng thử lại.";
                        return Page();
                    }

                    var profile = _ibase.profileRepo.Read(x => x.Id == checkLogin.ProfileId);
                    var userSess = new UserSession
                    {
                        SessionId = HttpContext.Session.Id,
                        UserId = checkLogin.Id,
                        FullName = !string.IsNullOrEmpty(profile.FullName) ? profile.FullName : checkLogin.UserName,
                        ProfileId = checkLogin.ProfileId.Value,
                        CodeRefer = profile.CodeRefer,
                    };
                    Response.Cookies.Delete("AspNetCore.Security");
                    Response.Cookies.Delete("AspNetCore.Security.Out");
                    //luu cookie username
                    string userData = JsonConvert.SerializeObject(userSess);
                    SessionHelpers.Set(_contextAccessor, userData, 10 * 365);
                    return RedirectToPage("/Profile");
                }
                message = "Số điện thoại hoặc mật khẩu không đúng. Vui lòng thử lại.";
                return Page();
            }
            catch (Exception ex)
            {
                message = "Lỗi:" + ex.Message;
                return Page();
            }
        }
        public IActionResult OnPostLogout()
        {
            SessionHelpers.Remove(_contextAccessor);
            return RedirectToPage("/SignIn");
        }
    }
}
