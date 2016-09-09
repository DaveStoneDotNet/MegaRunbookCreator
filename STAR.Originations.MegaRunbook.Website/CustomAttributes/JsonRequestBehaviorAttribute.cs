
using System.Web.Mvc;

namespace STAR.Originations.MegaRunbook.Website.CustomAttributes
{
    /// <summary>
    /// Adds 'AllowGet' JsonRequestBehaviors by default to all actions in a controller decorated with this attribute.
    /// like this:
    /// </summary>
    /// <remarks>
    /// As of MVC 2 Json for GET requests are blocked by default. 
    /// This means that instead of returning a JsonResult like this...
    /// 
    ///         return this.Json(data)
    /// 
    /// ... calls must be made like this...
    /// 
    ///         return this.Json(data, JsonRequestBehavior.AllowGet)
    /// 
    /// This attribute will set the Behavior to JsonRequestBehavior.AllowGet by default instead of having to 
    /// explicitly set the behavior in every action.
    /// 
    ///     See: http://haacked.com/archive/2009/06/25/json-hijacking.aspx/
    /// 
    /// </remarks>
    public class JsonRequestBehaviorAttribute : ActionFilterAttribute
    {
        private JsonRequestBehavior Behavior { get; set; }

        public JsonRequestBehaviorAttribute()
        {
            this.Behavior = JsonRequestBehavior.AllowGet;
        }

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            var result = filterContext.Result as JsonResult;

            if (result != null)
            {
                result.JsonRequestBehavior = this.Behavior;
            }
        }
    }
}