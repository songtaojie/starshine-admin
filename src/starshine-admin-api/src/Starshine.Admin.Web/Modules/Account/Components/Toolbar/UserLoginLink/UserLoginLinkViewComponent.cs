﻿using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace Starshine.Admin.Web.Modules.Account.Components.Toolbar.UserLoginLink;

public class UserLoginLinkViewComponent : AbpViewComponent
{
    public virtual IViewComponentResult Invoke()
    {
        return View("~/Modules/Account/Components/Toolbar/UserLoginLink/Default.cshtml");
    }
}
