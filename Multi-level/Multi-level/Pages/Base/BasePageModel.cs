using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Multi_level.Helper;
using Multi_LevelModels;

namespace Multi_level.Pages.Base
{
    public class BasePageModel<T>:PageModel where T : BasePageModel<T>
    {
        protected Ibase _base;
        protected readonly IHttpContextAccessor _httpContextAccessor;
        protected static IConfiguration _configuration;
        protected readonly ILogger<T> _logger;
        public BasePageModel(Ibase @base, IHttpContextAccessor httpContextAccessor, IConfiguration configuration, ILogger<T> logger)
        {
            _base = @base;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _logger = logger;
        }
        protected JsonResult Success(string message)
        {
            return new JsonResult(new
            {
                status = true,
                message = message
            });
        }
        protected UserSession GetUser()
        {
            return SessionHelpers.GetUser(_httpContextAccessor);
        }
        protected void SetUser(string value, int expiredTime)
        {
            SessionHelpers.Set(_httpContextAccessor, value, expiredTime);
        }
        protected JsonResult Data(object data)
        {
            return new JsonResult(data);
        }
        protected JsonResult Success(object data, string message = "")
        {
            return new JsonResult(new
            {
                status = true,
                data = data,
                message = message
            });
        }
        protected JsonResult Error(string message)
        {
            return new JsonResult(new
            {
                status = false,
                message = message
            });
        }
        protected JsonResult Error(object data, string message = "")
        {
            return new JsonResult(new
            {
                status = false,
                data = data,
                message = message
            });
        }
        public override void OnPageHandlerExecuting(PageHandlerExecutingContext context)
        {
            if (HttpContext.Request.Cookies != null)
            {
                if (GetUser() == null)
                {
                    context.Result = new RedirectResult("/Login/Index");
                }
                else
                {
                    var sessionCurrent = GetUser();
                    //var dataCurrent = _base.userRespository.Read(x => x.EmployeeId == sessionCurrent.EmployeeId);
                    //if (dataCurrent != null)
                    //{
                    //    if (dataCurrent.RoleGroupId != sessionCurrent.RoleGroupId)
                    //    {
                    //        sessionCurrent.RoleGroupId = dataCurrent.RoleGroupId;
                    //        SessionHelpers.Set(_httpContextAccessor, JsonConvert.SerializeObject(sessionCurrent), 10 * 365);
                    //    }
                    //}
                }
            }
            else
            {
                context.Result = new RedirectResult("/Login/Index");

            }
        }
    }
}
