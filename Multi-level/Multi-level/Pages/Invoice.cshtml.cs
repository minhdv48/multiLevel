using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Multi_level.Helper;
using Multi_LevelModels;
using Multi_LevelModels.Interfaces;
using Multi_LevelModels.Models;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Security.Cryptography;

namespace Multi_level.Pages
{
    public class InvoiceModel : PageModel
    {
        private Ibase _ibase;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _environment;
        private readonly IConfiguration _config;
        public List<Invoice> lstInvoice { get; set; }
        public InvoiceModel(Ibase @base, IHttpContextAccessor httpContextAccessor, Microsoft.AspNetCore.Hosting.IHostingEnvironment environment, IConfiguration configuration)
        {
            _ibase = @base;
            _contextAccessor = httpContextAccessor;
            _environment = environment;
            _config = configuration;
        }
        public IActionResult OnGet()
        {
            var userSess = SessionHelpers.GetUser(_contextAccessor);
            if (userSess == null)
            {
                return RedirectToPage("SignIn");
            }
            return Page();
        }
        public JsonResult OnGetInvoiceInfo(DateTime? startDate, DateTime? endDate, int offset, int limit)
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
                if (userSess.ProfileId == 1)
                    id = 0;
                //offset = offset / limit + 1;
                if (startDate == null)
                    startDate = DateTime.Now.AddDays(-30);
                if (endDate == null)
                    endDate = DateTime.Now;
                //Expression<Func<Invoice, bool>> condition = s => s.DateCreated.Value.Date >= startDate.Value.Date && s.DateCreated.Value.Date <= endDate.Value.Date;
                var data = _ibase.invoiceRepo.GetInvoices(id, startDate, endDate, offset, limit, ref total);
                //var data2 = data.Where(x => x.Status == status).ToList();
                return new JsonResult(new { status = true, rows = data, total = total });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { status = false, total = 0 });
            }
        }
        public JsonResult OnGetBenefitList(DateTime? startDate, DateTime? endDate, int offset, int limit)
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
                if (startDate == null)
                    startDate = DateTime.Now.AddDays(-30);
                if (endDate == null)
                    endDate = DateTime.Now;
                var data = _ibase.invoiceRepo.GetListBenefit(id, startDate, endDate, offset, limit, ref total);
                return new JsonResult(new { status = true, rows = data, total = total });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { status = false, total = 0 });
            }
        }

        public JsonResult OnPostBenefit(int profileId, DateTime? startDate, DateTime? endDate)
        {
            try
            {
                var userSess = SessionHelpers.GetUser(_contextAccessor);
                if (userSess == null)
                {
                    return new JsonResult(new { status = false, total = 0, msg = "Vui lòng đăng nhập để xem thông tin." });
                }
                bool retVal = false;
                int id = userSess.ProfileId;
                //offset = offset / limit + 1;
                if (startDate == null)
                    startDate = DateTime.Now.AddDays(-30);
                if (endDate == null)
                    endDate = DateTime.Now;
                Expression<Func<Invoice, bool>> condition = s => s.ProfileId == profileId && s.IsPay == false && s.DateCreated.Value.Date >= startDate.Value.Date && s.DateCreated.Value.Date <= endDate.Value.Date;
                var data = _ibase.invoiceRepo.Get(condition);
                if (data != null)
                {
                    retVal = _ibase.invoiceRepo.UpdatePayment(profileId, startDate, endDate);
                }
                //var data2 = data.Where(x => x.Status == status).ToList();
                return new JsonResult(new { status = retVal, msg = "Tri ân thành công" });
            }
            catch (Exception ex)
            {
                return new JsonResult(new { status = false, msg = ex.Message });
            }
        }
    }
}
