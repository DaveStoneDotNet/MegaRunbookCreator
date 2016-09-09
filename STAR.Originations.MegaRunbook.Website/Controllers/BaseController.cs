using System;
using System.Diagnostics;
using System.Web.Mvc;

using STAR.Framework.Utility;

using STAR.Originations.MegaRunbook.Website.AppCode;

namespace STAR.Originations.MegaRunbook.Website.Controllers
{
    /// <summary>
    /// Note that Authorize Attributes may be controlled globally in 'App_Start.FilterConfig.RegisterGlobalFilters'.
    /// For example, 
    /// 
    ///     filters.Add(new RedirectAndLogAuthorizeAttribute());
    /// 
    /// However, this appears to prevent the 'AllowAnonymous' attribute, necessary on the 'ErrorController' from working.
    /// 
    /// </summary>
    public class BaseController : Controller
    {
        #region Methods

        #region OnAuthorization
        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            this.ViewBag.WelcomeMessage = String.Format("Good {0} {1}", Format.DayTime(), "Dave Stone");
            this.ViewBag.TodaysDate = String.Format("Today is {0}", DateTime.Now.ToLongDateString());
            base.OnAuthorization(filterContext);
        }
        #endregion OnAuthorization

        #region OnException
        protected override void OnException(ExceptionContext exceptionContext)
        {
            base.OnException(exceptionContext);
        }
        #endregion OnException

        #region StartStopwatch
        protected Stopwatch StartStopwatch()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            return stopwatch;
        }
        #endregion StartStopwatch

        // ---

        #region JsonDateResult
        /// <summary>
        /// Creates a JsonDateResult object that serializes the specified object to JavaScript Object Notation (JSON) using the 
        /// IsoDateTimeConverter defined in the custom JsonDateResult class.
        /// </summary>
        /// <returns>
        /// The JSON result object that serializes the specified object to JSON format. The result object that is prepared by 
        /// this method is written to the response by the ASP.NET MVC framework when the object is executed.
        /// </returns>
        /// <param name="data">The JavaScript object graph to serialize.</param>
        protected internal JsonDateResult JsonDateResult(object data)
        {
            return new JsonDateResult()
            {
                Data = data,
                ContentType = "application/json",
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        #endregion JsonDateResult

        #endregion Methods
    }
}