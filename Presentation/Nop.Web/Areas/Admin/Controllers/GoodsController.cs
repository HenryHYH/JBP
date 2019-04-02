using Microsoft.AspNetCore.Mvc;

namespace Nop.Web.Areas.Admin.Controllers
{
    public partial class GoodsController : BaseAdminController
    {
        public virtual IActionResult Index()
        {
            return RedirectToAction("List");
        }

        public virtual IActionResult List()
        {
            return View();
        }
    }
}