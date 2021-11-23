using Microsoft.AspNetCore.Authorization;

namespace Library.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using static AdminConstants;

    [Area(AdminAreaName)]
    [Authorize(Roles = AdminRoleName)]

    public class AdminController : Controller
    {
        
    }
}
