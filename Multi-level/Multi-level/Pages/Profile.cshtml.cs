using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Multi_level.Helper;
using Multi_level.Models;
using Multi_LevelModels;
using Multi_LevelModels.Models;
using System.Security.Cryptography;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace Multi_level.Pages
{
    public class ProfileModel : PageModel
    {
        private Ibase _ibase;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _environment;
        private readonly IConfiguration _config;
        public ProfileModel(Ibase @base, ILogger<IndexModel> logger, IHttpContextAccessor httpContextAccessor, Microsoft.AspNetCore.Hosting.IHostingEnvironment environment, IConfiguration config)
        {
            _ibase = @base;
            _contextAccessor = httpContextAccessor;
            _environment = environment;
            _config = config;
            profitLevel1 = decimal.Parse(_config.GetSection("profitLevel1").Value);
            bonus = decimal.Parse(_config.GetSection("bonus").Value);
            _config = config;
        }
        [BindProperty]
        public Profile model { get; set; }
        [BindProperty]
        public IFormFile Upload { get; set; }
        //[BindProperty]
        //public IFormFile Invoice { get; set; }
        [BindProperty]
        public InvoicesModel modelInvoice { get; set; }
        [BindProperty]
        public UserChangePassword modelChangePass { get; set; }
        [BindProperty]
        public List<Profile> profiles { get; set; }
        [BindProperty]
        public PaymentModel PaymentModel { get; set; }
        [ViewData]
        public string message { get; set; }
        [ViewData]
        public string messageInvoice { get; set; }
        [BindProperty]
        public Profile referBy { get; set; }
        [ViewData]
        public int ParentId { get; set; }
        [BindProperty]
        public int PId { get; set; }
        [ViewData]
        public decimal TotalProfit { get; set; }
        [ViewData]
        public decimal TotalBonus { get; set; }
        [ViewData]
        public int TotalMember { get; set; }
        [ViewData]
        public bool IsUpdate { get; set; }
        [ViewData]
        public DateTime Month { get; set; }
        public decimal profitLevel1 = 0;
        public decimal bonus = 0;
        public IActionResult OnGet(int? id, DateTime? month)
        {
            try
            {
                IsUpdate = true;
                var userSess = SessionHelpers.GetUser(_contextAccessor);
                if (userSess != null)
                {
                    if (id == null)
                    {
                        id = userSess.ProfileId;
                    }
                    else
                    {
                        IsUpdate = false;
                    }
                    defaultData(id.Value, month);
                    return Page();
                }
                else
                {
                    return RedirectToPage("SignIn");
                }
            }
            catch (Exception)
            {
                return RedirectToPage("SignIn");
            }
        }
        private void recursiveProfile(List<Profile> lstParent)
        {
            foreach (var profile in lstParent)
            {
                var _lst = _ibase.profileRepo.Get(x => x.ParentId == profile.Id).ToList();
                if (_lst != null && _lst.Count > 0)
                {
                    profiles.AddRange(_lst);
                    recursiveProfile(_lst);
                }
            }
        }
        private void defaultData(int userId, DateTime? month = null)
        {
            ParentId = userId;
            PId = userId;
            model = _ibase.profileRepo.Read(x => x.Id == userId);
            referBy = _ibase.profileRepo.Read(x => x.Id == model.ReferId);
            List<Profile> lstParent = new List<Profile>();
            if (month == null || month == DateTime.MinValue)
            {
                lstParent = _ibase.profileRepo.Get(x => x.ParentId == userId).ToList();
            }
            else
            {
                Month = month.Value;
                lstParent = _ibase.profileRepo.Get(x => x.ParentId == userId && (x.DateJoin.Value.Month == month.Value.Month && x.DateJoin.Value.Year == month.Value.Year)).ToList();
            }
            profiles = new List<Profile>();
            if (lstParent != null && lstParent.Count > 0)
            {
                profiles.AddRange(lstParent);
                foreach (var item in lstParent)
                {
                    if (item.ReferId == userId)
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
                TotalMember = lstParent.Count;
                if (month == null || month == DateTime.MinValue)
                {
                    recursiveProfile(lstParent);
                }
                else
                {
                    recursivePay(lstParent, userId, month.Value);
                }

            }
        }
        public async Task<IActionResult> OnPostUpdateProfile()
        {
            List<Profile> lstParent = new List<Profile>();
            if (model.Id > 0)
            {
                var checkExists = _ibase.profileRepo.Get(x => x.Id != model.Id && (x.AmwayCode == model.AmwayCode || x.Idcard == model.Idcard));
                if (checkExists != null && checkExists.Count() > 0)
                {
                    message = "Mã Amway hoặc thông tin CCCD đã tồn tại. Vui lòng kiểm tra lại";
                    defaultData(model.Id);
                    //lstParent = _ibase.profileRepo.Get(x => x.ParentId == model.Id).ToList();
                    //profiles = new List<Profile>();
                    //if (lstParent != null && lstParent.Count > 0)
                    //{
                    //    profiles.AddRange(lstParent);
                    //    recursiveProfile(lstParent);
                    //}
                    return Page();
                }
                var info = _ibase.profileRepo.Read(x => x.Id == model.Id);
                if (info != null)
                {
                    if (Upload != null)
                    {
                        var ext = Path.GetExtension(Upload.FileName);
                        if (ext == ".png" || ext == ".jpg" || ext == ".gif" || ext == ".jpeg" || ext == ".webp")
                        {
                            string rootPath = _environment.ContentRootPath;
                            var newPath = Path.Combine(rootPath + "/wwwroot/", "uploads");
                            if (!Directory.Exists(newPath))
                            {
                                Directory.CreateDirectory(newPath);
                            }
                            var file = Path.Combine(newPath, Upload.FileName);
                            using (var fileStream = new FileStream(file, FileMode.Create))
                            {
                                await Upload.CopyToAsync(fileStream);
                            }
                            info.PathInvoice = Path.Combine("/uploads/", Upload.FileName);
                        }
                    }
                    info.FullName = model.FullName;
                    info.Email = model.Email;
                    info.Idcard = model.Idcard;
                    info.CardVerifyBy = model.CardVerifyBy;
                    info.CardVerifyDate = model.CardVerifyDate;
                    info.Address = model.Address;
                    info.AmwayCode = model.AmwayCode;
                    info.Branch = model.Branch;
                    info.BankAccount = model.BankAccount;
                    info.AccountName = model.AccountName;
                    info.BankName = model.BankName;
                    _ibase.profileRepo.Update(info);
                    _ibase.Commit();
                    message = "Cập nhật thông tin thành công";
                    model = info;
                    defaultData(model.Id);
                    return RedirectToPage("/Profile");
                }
            }
            message = "Lỗi dữ liệu. Không thể lấy được thông tin tài khoản. Vui lòng liên hệ tới kỹ thuật";
            defaultData(model.Id);

            return Page();
        }
        public JsonResult OnGetProfileInfo(int id, DateTime? month)
        {
            DateTime Search = new DateTime();
            if (month == null)
            {
                Search = DateTime.Now;
            }
            else
            {
                Search = month.Value;
            }
            var info = _ibase.profileRepo.Read(x => x.Id == id);
            if (info == null)
            {
                return new JsonResult(new { status = false });
            }
            var lstLevel1 = _ibase.profileRepo.Get(x => x.ParentId == id && (x.DateJoin.Value.Month == Search.Month && x.DateJoin.Value.Year == Search.Year)).ToList();
            if (lstLevel1 != null && lstLevel1.Count > 0)
            {
                foreach (var item in lstLevel1)
                {
                    if (item.ParentId == id)
                    {
                        TotalProfit = TotalProfit + profitLevel1;
                    }
                    else
                    {
                        TotalBonus = TotalBonus + bonus;
                    }
                }
                TotalMember = lstLevel1.Count;
                recursivePay(lstLevel1, id, Search);
            }

            return new JsonResult(new { status = true, data = info, subCount = TotalMember, TotalProfit = TotalProfit });
        }
        public JsonResult OnPostProfileInfoVerify(int id)
        {
            var info = _ibase.profileRepo.Read(x => x.Id == id);
            if (info == null)
            {
                return new JsonResult(new { status = false, msg = "Có lỗi xảy ra. Không tồn tại thông tin tài khoản này." });
            }
            info.DateVerify = DateTime.Now;
            _ibase.profileRepo.Update(info);
            _ibase.Commit();
            return new JsonResult(new { status = true, msg = "Xác nhận thành công" });
        }
        public async Task<IActionResult> OnPostUpdateInvoice()
        {
            if (PId > 0)
            {
                defaultData(PId);
                var info = _ibase.profileRepo.Read(x => x.Id == PId);
                string paths = "";
                if (info != null)
                {
                    var existsInvoiceCode = _ibase.invoiceRepo.Read(x => x.InvoiceCode.ToLower().Trim().Equals(modelInvoice.InvoiceCode.ToLower().Trim()));
                    if (existsInvoiceCode != null)
                    {
                        message = "Mã hóa đơn này đã tồn tại";
                        return Page();
                    }
                    if (modelInvoice.Invoices != null)
                    {
                        foreach (var item in modelInvoice.Invoices)
                        {
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
                                paths = paths + ";" + Path.Combine("/uploads/", item.FileName);
                            }
                        }
                        var invoiceInfo = new Invoice();
                        invoiceInfo.Path = paths;
                        invoiceInfo.ProfileId = info.Id;
                        invoiceInfo.DateCreated = DateTime.Now;
                        invoiceInfo.DateInvoice = modelInvoice.DateInvoice;
                        invoiceInfo.InvoiceCode = modelInvoice.InvoiceCode;
                        invoiceInfo.IsPay = false;
                        _ibase.invoiceRepo.Insert(invoiceInfo);
                        _ibase.Commit();
                        message = "Cập nhật hoá đơn thành công";
                    }
                }
                else
                {
                    message = "Lỗi dữ liệu. Không thể lấy được thông tin tài khoản. Vui lòng liên hệ tới kỹ thuật";
                }
            }
            return Page();
        }
        private void recursivePay(List<Profile> lstParent, int rootParent, DateTime Search)
        {
            foreach (var profile in lstParent)
            {
                //if(profile.ReferId == rootParent)
                //{
                //    TotalProfit = TotalProfit + profitLevel1;
                //}
                var _lst = _ibase.profileRepo.Get(x => x.ParentId == profile.Id && (x.DateJoin.Value.Month == Search.Month && x.DateJoin.Value.Year == Search.Year)).ToList();
                if (_lst != null && _lst.Count > 0)
                {
                    foreach (var item in _lst)
                    {
                        if (item.ReferId == rootParent)
                        {
                            TotalProfit = TotalProfit + profitLevel1;
                            item.benefit = profitLevel1;
                        }
                        else
                        {
                            TotalProfit = TotalProfit + bonus;
                            item.benefit = bonus;
                        }
                    }
                    TotalMember = TotalMember + _lst.Count;
                    profiles.AddRange(_lst);
                    recursivePay(_lst, rootParent, Search);
                }
            }
        }
    }
}
