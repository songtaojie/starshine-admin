using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hx.Admin.Web.Entry.Controllers
{
    /// <summary>
    /// 首页控制器
    /// </summary>
    //[AllowAnonymous]
    public class HomeController : Controller
    {
        //private readonly ISystemService _systemService;

        //public HomeController(ISystemService systemService)
        //{
        //    _systemService = systemService;
        //}
        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            //ViewBag.Description = _systemService.GetDescription();

            return View();
        }
    }
}