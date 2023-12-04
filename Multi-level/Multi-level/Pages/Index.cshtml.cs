using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Multi_level.Helper;
using Multi_level.Models;
using Multi_LevelModels;
using Multi_LevelModels.Models;
using System.Net.WebSockets;

namespace Multi_level.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private Ibase _ibase;
        private readonly IConfiguration _config;
        private readonly IHttpContextAccessor _contextAccessor;
        public IndexModel(Ibase @base, ILogger<IndexModel> logger, IConfiguration configuration, IHttpContextAccessor contextAccessor)
        {
            _ibase = @base;
            _config = configuration;
            _logger = logger;
            _contextAccessor = contextAccessor;
        }
        [BindProperty]
        public RegisModel model { get; set; }
        [ViewData]
        public string message { get; set; }
        public IActionResult OnGet()
        {
            var userSess = SessionHelpers.GetUser(_contextAccessor);
            if (userSess != null)
            {
                return RedirectToPage("/Profile");
            }
            CaptchaImage();
            return Page();
        }
        [ValidateAntiForgeryToken]
        public IActionResult OnPostRegister()
        {
            try
            {
                int maximumMember = int.Parse(_config.GetSection("MaximumMember").Value);
                int maximumLevel = int.Parse(_config.GetSection("MaximumLevelSub").Value);
                if (!Captcha.ValidateCaptchaCode(model.CaptchaCode, _contextAccessor.HttpContext))
                {
                    CaptchaImage();
                    message = "Capchat không hợp lệ";
                    return Page();
                }
                CaptchaImage();

                if (string.IsNullOrEmpty(model.CodeRefer) || string.IsNullOrEmpty(model.Phone) || string.IsNullOrEmpty(model.AmwayCode))
                {
                    message = "Vui lòng nhập mã người giới thiệu, số điện thoại, Mã Amway để đăng ký thành viên.";
                    return Page();
                }
                var checkPhone = _ibase.profileRepo.Read(x => x.Phone.Equals(model.Phone) || x.Idcard.Equals(model.Idcard));
                if (checkPhone != null)
                {
                    message = "Số điện thoại hoặc số CCCD đã được đăng ký. Vui lòng kiểm tra lại";
                    return Page();
                }
                //Get parent with code refer
                var _levelParent = _ibase.profileRepo.Read(x => x.CodeRefer.ToLower().Equals(model.CodeRefer.ToLower()));
                if (_levelParent == null)
                {
                    message = "Mã giới thiệu không hợp lệ.";
                    return Page();
                }
                //Kiểm tra tầng số 0 đã đủ member hay chưa
                //Lấy parentId trong bảng profile để xác định
                int count = _ibase.profileRepo.Get(x => x.ParentId == _levelParent.Id).Count();
                Profile retProfileRefer = new Profile();
                if (count >= maximumMember)
                {
                    //Kiểm tra tuần tự các thành viên cấp dưới đã đủ member chưa. Nếu chưa append cho đủ.
                    retProfileRefer = recursiveLevel(_levelParent.Id);
                }

                var profile = new Profile
                {
                    FullName = model.FullName,
                    Address = model.Address,
                    Email = model.Email,
                    Phone = model.Phone,
                    Idcard = model.Idcard,
                    CardVerifyBy = model.CardVerifyBy,
                    CardVerifyDate = model.CardVerifyDate,
                    DateJoin = DateTime.Now,
                    CodeRefer = CodeReferGenerate(),
                    ParentId = _levelParent.Id,
                    ReferBy = model.CodeRefer,
                    ReferId = _levelParent.Id,
                    Levels = _levelParent.ParentId == 0 ? 1 : _levelParent.Levels + 1,
                    AmwayCode = model.AmwayCode,
                    BankAccount = model.BankAccount,
                    BankName = model.BankName, 
                    AccountName = model.AccountName,
                    Branch  = model.Branch,
                    IsPay = true
                };

                if (retProfileRefer != null && retProfileRefer.Id > 0)
                {
                    //profile.ReferBy = retProfileRefer.CodeRefer;
                    profile.ParentId = retProfileRefer.Id;
                    profile.Levels = retProfileRefer.Levels + 1;
                }
                var account = new Account
                {
                    UserName = model.Phone,
                    Password = Utility.Sha256Hash(model.Password),
                };
                _ibase.profileRepo.Insert(profile);
                _ibase.Commit();
                //Account
                account.ProfileId = profile.Id;
                _ibase.accountRepo.Insert(account);
                _levelParent.IsPay = false;
                _ibase.profileRepo.Update(_levelParent);
                _ibase.Commit();

                message = "Đăng ký thành công. Chào mừng tới SPDAICAT.";
                return RedirectToPage("/ResultRegister", new { id = profile.Id });
            }
            catch (Exception e)
            {
                message = "Có lỗi xảy ra, trong quá trình đăng ký. Vui lòng liên hệ với kỹ thuật";
                return Page();
            }
        }
        private string CodeReferGenerate()
        {
            var code = Utility.GenerateCode();
            var _levelParent = _ibase.profileRepo.Read(x => x.CodeRefer.ToLower().Equals(code.ToLower()));
            if (_levelParent == null)
            {
                return code;
            }
            else
            {
                return CodeReferGenerate();
            }
        }

        private Profile recursiveLevel(int parentId)
        {
            var parent = _ibase.profileRepo.Get(x => x.ParentId == parentId).ToList();
            if (parent == null)
            {
                return null;
            }
            var allProfile = _ibase.profileRepo.Get().ToList();
            foreach (var level in parent)
            {
                var _count = allProfile.Where(x => x.ParentId == level.Id).Count();
                if (_count < 5)
                {
                    return level;
                }
            }
            return null;
        }
        public string CaptchaImage(string captname = null)
        {
            int width = 100;
            int height = 36;
            var captchaCode = Captcha.GenerateCaptchaCode();
            var path = Directory.GetCurrentDirectory() + "/wwwroot/captcha.png";
            System.IO.File.Delete(path);
            if (!string.IsNullOrEmpty(captname))
            {
                captname = "captcha.png";
                path = Directory.GetCurrentDirectory() + "/wwwroot/" + captname;
            }
            var result = Captcha.GenerateCaptchaImage(width, height, captchaCode);
            _contextAccessor.HttpContext.Session.SetString("CaptchaCode", result.CaptchaCode);
            Stream s = new MemoryStream(result.CaptchaByteData);
            ExtendMethod.StreamToFile(s, path);
            return captname;
        }
        public JsonResult OnPostResetCaptcha(string captname)
        {
            var path = Directory.GetCurrentDirectory() + "/wwwroot" + captname;
            System.IO.File.Delete(path);
            var name = CaptchaImage(captname);
            return new JsonResult(new { name = name });
        }
    }
}