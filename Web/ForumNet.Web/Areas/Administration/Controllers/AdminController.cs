namespace ForumNet.Web.Areas.Administration.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Authorization;

    using Common;

    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    [Area(GlobalConstants.AdministratorAreaName)]
    public class AdminController : Controller
    {
    }
}