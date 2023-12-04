using Newtonsoft.Json;

namespace Multi_level.Helper
{
    public static class SessionHelpers
    {
        public static string keyName = "userSession";
        #region Cookies
        //public static void GetUser(IHttpContextAccessor httpContextAccessor)
        //{
        //    string result = string.Empty;
        //    var data = httpContextAccessor.HttpContext.Request.Cookies.TryGetValue("", out result);
        //    var user = JsonConvert.DeserializeObject<UserSession>(result);
        //    userSession = user;
        //}
        //public static string Get(string key = "")
        //{
        //    if (string.IsNullOrEmpty(key))
        //        key = keyName;
        //    return _httpContextAccessor.HttpContext.Request.Cookies[key];
        //}
        public static UserSession GetUser(IHttpContextAccessor _httpContextAccessor)
        {
            var user = new UserSession();
            if (_httpContextAccessor.HttpContext.Request.Cookies[keyName] != null)
            {
                user = JsonConvert.DeserializeObject<UserSession>(_httpContextAccessor.HttpContext.Request.Cookies[keyName]);
            }
            else
            {
                user = null;
            }
            return user;
        }
        public static string GetByKeyName(IHttpContextAccessor _httpContextAccessor, string keyName)
        {
            string data = string.Empty;
            if (_httpContextAccessor.HttpContext.Request.Cookies[keyName] != null)
            {
                data = _httpContextAccessor.HttpContext.Request.Cookies[keyName].ToString();
            }
            else
            {
                data = string.Empty;
            }
            return data;
        }
        public static void Set(IHttpContextAccessor _httpContextAccessor, string value, int? expireTime)
        {
            CookieOptions option = new CookieOptions();
            if (expireTime.HasValue)
                option.Expires = DateTime.Now.AddDays(expireTime.Value);
            else
                option.Expires = DateTime.Now.AddMilliseconds(10);
            _httpContextAccessor.HttpContext.Response.Cookies.Append(keyName, value, option);
        }
        public static void SetByKeyName(IHttpContextAccessor _httpContextAccessor, string keyName, string value, int? expireTime)
        {
            CookieOptions option = new CookieOptions();
            if (expireTime.HasValue)
                option.Expires = DateTime.Now.AddDays(expireTime.Value);
            else
                option.Expires = DateTime.Now.AddMilliseconds(10);
            _httpContextAccessor.HttpContext.Response.Cookies.Append(keyName, value, option);
        }
        public static void Remove(IHttpContextAccessor _httpContextAccessor)
        {
            CookieOptions option = new CookieOptions();
            option.Expires = DateTime.Now.AddDays(-10);
            _httpContextAccessor.HttpContext.Response.Cookies.Delete(keyName);
        }
        public static void RemoveByKeyName(IHttpContextAccessor _httpContextAccessor, string keyName)
        {
            _httpContextAccessor.HttpContext.Response.Cookies.Delete(keyName);
        }
        #endregion
    }
}
