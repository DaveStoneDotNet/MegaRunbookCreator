
using System.Web.Mvc;

namespace STAR.Originations.MegaRunbook.Website.Controllers
{
    public class HomeController : Controller
    {
        #region Index
        public ActionResult Index()
        {
            return this.View();
        }
        #endregion Index
    }
}