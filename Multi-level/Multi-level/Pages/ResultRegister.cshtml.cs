using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Multi_level.Helper;
using Multi_level.Models;
using Multi_LevelModels;
using Multi_LevelModels.Models;

namespace Multi_level.Pages
{
    public class ResultRegisterModel : PageModel
    {
        private readonly ILogger<ResultRegisterModel> _logger;
        private Ibase _ibase;
        private readonly IHttpContextAccessor _contextAccessor;
        public ResultRegisterModel(Ibase @base, ILogger<ResultRegisterModel> logger, IHttpContextAccessor httpContextAccessor)
        {
            _ibase = @base;
            _logger = logger;
            _contextAccessor = httpContextAccessor;
        }
        [BindProperty]
        public Profile model { get; set; }
        [ViewData]
        public string message { get; set; }
        [ViewData]
        public string FullName { get; set; }
        [ViewData]
        public string CodeRefer { get; set; }
        public IActionResult OnGet(int id)
        {
            model = _ibase.profileRepo.Read(x => x.Id == id);
            if(model == null)
            {
                return RedirectToPage("/Error");
            }
            if(model.ParentId > 0 && !string.IsNullOrEmpty(model.ReferBy))
            {
                var parent = _ibase.profileRepo.Read(x => x.Id == model.ParentId);
                FullName = parent.FullName;
                CodeRefer = parent.ReferBy;
            }
            return Page();
        }
    }
}
