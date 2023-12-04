using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Multi_level.Helper;
using Multi_LevelModels.Models;
using Multi_LevelModels;
using System.Linq.Expressions;
using System.ComponentModel.DataAnnotations;
using Multi_level.Models;
using System.Security.Cryptography;

namespace Multi_level.Pages
{
    public class ReportModel : PageModel
    {
        private Ibase _ibase;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _environment;
        private readonly IConfiguration _config;
        public ReportModel(Ibase @base, ILogger<IndexModel> logger, IHttpContextAccessor httpContextAccessor, Microsoft.AspNetCore.Hosting.IHostingEnvironment environment, IConfiguration configuration)
        {
            _ibase = @base;
            _contextAccessor = httpContextAccessor;
            _environment = environment;
            _config = configuration;
            profitLevel1 = decimal.Parse(_config.GetSection("profitLevel1").Value);
            bonus = decimal.Parse(_config.GetSection("bonus").Value);
        }
        public decimal profitLevel1 = 0;
        public decimal bonus = 0;
        [BindProperty]
        public Profile model { get; set; }
        [BindProperty]
        public List<Profile> profiles { get; set; }
        [BindProperty]
        public PaymentModel PaymentModel { get; set; }
        [ViewData]
        public int ParentId { get; set; }
        [ViewData]
        public decimal TotalProfit { get; set; }
        [ViewData]
        public decimal TotalBonus { get; set; }
        [ViewData]
        public int TotalMember { get; set; }
        [ViewData]
        public int TotalLevel { get; set; }
        [ViewData]
        public DateTime Search { get; set; }
        [ViewData]
        public string message { get; set; }
        protected List<int> intsparent = new List<int>();

        public IActionResult OnGet(DateTime? month)
        {
            var userSess = SessionHelpers.GetUser(_contextAccessor);
            if (userSess != null)
            {
                ParentId = userSess.ProfileId;
                model = _ibase.profileRepo.Read(x => x.Id == ParentId);
                if (month == null)
                {
                    Search = DateTime.Now;
                }
                else
                {
                    Search = month.Value;
                }
                List<Profile> lstParent = _ibase.profileRepo.Get(x => (x.ParentId == ParentId) && (x.DateJoin.Value.Month == Search.Month && x.DateJoin.Value.Year == Search.Year)).ToList();
                profiles = new List<Profile>();
                if (ParentId == 1)
                {
                    TotalLevel = _ibase.profileRepo.Get().Where(x => x.Levels > 0).GroupBy(x => x.Levels).Count();
                }
                else
                {
                    TotalLevel = lstParent.Where(x => x.Levels > 0).GroupBy(x => x.Levels).Count();
                }
                TotalMember = lstParent.Count();
                if (lstParent != null && lstParent.Count > 0)
                {
                    foreach (var item in lstParent)
                    {
                        if (item.ReferId == ParentId)
                        {
                            TotalProfit = TotalProfit + profitLevel1;
                            item.benefit = profitLevel1;
                        }
                        else
                        {
                            TotalBonus = TotalBonus + bonus;
                            item.benefit = bonus;
                        }
                        profiles.Add(item);
                    }
                    recursiveProfile(lstParent, ParentId);
                }
                return Page();
            }
            else
            {
                return RedirectToPage("SignIn");
            }
        }
        private void recursiveProfile(List<Profile> lstParent, int rootParent = 0)
        {
            foreach (var profile in lstParent)
            {
                var _lst = _ibase.profileRepo.Get(x => x.ParentId == profile.Id).ToList();
                if (_lst != null && _lst.Count > 0)
                {
                    if (rootParent != 1 && rootParent != profile.ParentId)
                    {
                        TotalLevel = TotalLevel + 1;
                    }
                    foreach (var item in _lst)
                    {
                        if(item.ReferId == rootParent)
                        {
                            TotalProfit = TotalProfit + profitLevel1;
                            item.benefit = profitLevel1;
                        }
                        else
                        {
                            TotalBonus = TotalBonus + bonus;
                            item.benefit = bonus;
                        }
                    }
                    profiles.AddRange(_lst);
                    TotalMember = TotalMember + _lst.Count();
                    recursiveProfile(_lst, rootParent);
                }
            }
        }
        public JsonResult OnGetBenefitList(DateTime? month, int offset, int limit)
        {
            try
            {
                var userSess = SessionHelpers.GetUser(_contextAccessor);
                if (userSess == null)
                {
                    return new JsonResult(new { status = false, total = 0, msg = "Vui long đăng nhập để xem thông tin hoá đơn" });
                }
                var total = 0;
                int id = userSess.ProfileId;
                //offset = offset / limit + 1;
                if (month == null)
                    month = DateTime.Now;
                Expression<Func<HistoryPay, bool>> condition = x => x.PayDate.Month == month.Value.Month && x.PayDate.Year == month.Value.Year;
                var data = _ibase.historyPayRepo.ReadAllLimit(offset, limit, null, condition);
                //var data2 = data.Where(x => x.Status == status).ToList();
                return new JsonResult(new { status = true, rows = data.Item1, total = data.Item2 });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { status = false, total = 0 });
            }
        }
        public JsonResult OnPostBenefit(int profileId)
        {
            try
            {
                var userSess = SessionHelpers.GetUser(_contextAccessor);
                if (userSess == null)
                {
                    return new JsonResult(new { status = false, total = 0, msg = "Vui lòng đăng nhập để xem thông tin." });
                }
                int id = userSess.ProfileId;
                //offset = offset / limit + 1;
                var data = _ibase.profileRepo.Read(x => x.Id == profileId);
                if (data != null)
                {
                    data.IsPay = true;
                    _ibase.profileRepo.Update(data);
                    _ibase.Commit();
                }
                //var data2 = data.Where(x => x.Status == status).ToList();
                return new JsonResult(new { status = true, msg = "Tri ân thành công" });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { status = false, msg = ex.Message });
            }
        }
        public JsonResult OnGetProfileBonus(int id, DateTime month)
        {
            var info = _ibase.profileRepo.Read(x => x.Id == id);
            var subCount = _ibase.profileRepo.Get(x => x.ReferId == id && x.DateJoin.Value.Month == month.Month && x.DateJoin.Value.Year == month.Year).Count();
            if (info == null)
            {
                return new JsonResult(new { status = false });
            }
            return new JsonResult(new { status = true, data = info, subCount = subCount });
        }
        public async Task<IActionResult> OnPostUpdatePayment()
        {
            var userSess = SessionHelpers.GetUser(_contextAccessor);
            if (userSess == null)
            {
                return RedirectToPage("SignIn");
            }
            if (PaymentModel.ProfileId > 0)
            {
                var info = _ibase.profileRepo.Read(x => x.Id == PaymentModel.ProfileId);
                string paths = "";
                if (PaymentModel.img != null)
                {
                    var item = PaymentModel.img;
                    var ext = Path.GetExtension(item.FileName);
                    if (ext != ".png" && ext != ".jpg" && ext != ".gif" && ext != ".jpeg" && ext != ".webp")
                    {
                        message = "File không đúng định dạng. Chỉ được nhập file ảnh png, jpg";
                        return Page();
                    }
                    else
                    {
                        string rootPath = _environment.ContentRootPath;
                        var newPath = Path.Combine(rootPath + "/wwwroot/", "uploads");
                        if (!Directory.Exists(newPath))
                        {
                            Directory.CreateDirectory(newPath);
                        }
                        var file = Path.Combine(newPath, item.FileName);
                        using (var fileStream = new FileStream(file, FileMode.Create))
                        {
                            await item.CopyToAsync(fileStream);
                        }
                        paths = Path.Combine("/uploads/", item.FileName);
                    }
                    info.IsPay = true;
                    _ibase.profileRepo.Update(info);
                    var historyPay = new HistoryPay
                    {
                        ProfileId = PaymentModel.ProfileId,
                        ReferPay = userSess.ProfileId,
                        ImgPath = paths,
                        PayDate = DateTime.Now,
                        FullName = info.FullName
                    };
                    _ibase.historyPayRepo.Insert(historyPay);
                    _ibase.Commit();
                    message = "Cập nhật hoá đơn thành công";
                }
            }
            else
            {
                message = "Lỗi dữ liệu. Không thể lấy được thông tin tài khoản. Vui lòng liên hệ tới kỹ thuật";
            }
            return RedirectToPage("Report");
        }
    }
}
