using System;
using System.Web.Mvc;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace STAR.Originations.MegaRunbook.Website.AppCode
{
    /// <summary>
    /// Intended to provide JsonResults with ISO formatted dates instead of UTC dates.
    /// </summary>
    /// <remarks>
    /// One of the sore points of JSON is the lack of a date/time literal. Many people are surprised and disappointed to learn this when 
    /// they first encounter JSON. The simple explanation (consoling or not) for the absence of a date/time literal is that JavaScript 
    /// never had one either: The support for date and time values in JavaScript is entirely provided through the Date object. 
    /// 
    /// Most applications using JSON as a data format, generally tend to use either a STRING or a NUMBER to express date and time values. 
    /// 
    ///     1) If a STRING is used, you can generally expect it to be in the ISO 8601 format. 
    /// 
    ///     2) If a number is used, then the value is usually taken to mean the number of milliseconds since epoch in UTC format 
    ///        (Universal Coordinated Time), where epoch is defined as Midnight January 1, 1970. 
    /// 
    /// Again, this is a mere convention and not part of the JSON standard. If you are exchanging data with another application, you will 
    /// need to check its documentation to see how it encodes date and time values within a JSON literal. 
    /// 
    /// For example, Microsoft's ASP.NET AJAX uses neither of the described conventions. 
    /// 
    ///     3) Rather, it encodes .NET DateTime values as a combination of both a STRING and a NUMBER, where the content of the string is...
    /// 
    ///                 /Date(ticks)/ 
    /// 
    ///         ...and where ticks represents milliseconds since epoch (UTC). So November 29, 1989, 4:55:30 AM, in UTC is encoded as... 
    /// 
    ///             \/Date(628318530718)\/
    /// 
    ///     1) STRING  : ISO 8601                                                         : 2016-01-19T06:00:00.000Z
    ///     2) NUMBER  : UTC MILLISECONDS SINCE JAN 1, 1970                               : 628318530718
    ///     3) .NET    : NUMERIC UTC MILLISECONDS SINCE JAN 1, 1970 AS A FORMATTED STRING : /Date(628318530718)
    /// 
    /// By default, dates are sent from the Server to the UI in a UTC format, e.g. /Date(628318530718).
    /// However, when posting back from the UI to the Server, the Server is expecting ISO 8601. So when the Server receives back the 
    /// date it sent in UTC format, e.g. /Date(628318530718), the date conversion fails because it's expecting an ISO format, 
    /// e.g. 2016-01-19T06:00:00.000Z.
    /// 
    /// This meant that the website UI had to account for every date it received and then convert each date to ISO before posting 
    /// it back to the server. For example, 
    /// 
    ///         $scope.reviewLoanDetail.OriginalDropDate = coreServices.toIso($scope.reviewLoanDetail.OriginalDropDate);
    ///         $scope.reviewLoanDetail.DropDate = coreServices.toIso($scope.reviewLoanDetail.DropDate);
    ///         $scope.reviewLoanDetail.FollowUpDate = coreServices.toIso($scope.reviewLoanDetail.FollowUpDate);
    ///         etc...
    /// 
    /// To fix this, we're creating our own JsonResult that SENDS dates in ISO 8601 instead of the default UTC.
    /// 
    /// The 'DefaultDateTimeFormat' for the 'IsoDateTimeConverter' is "yyyy'-'MM'-'dd'T'HH':'mm':'ss.FFFFFFFK"
    /// 
    /// Of particular note is the 'T' designation which will treat the time as a TIME ZONE. This has the unfortunate 
    /// side-affect of causing dates to be off by one day when sent to the UI.
    /// 
    /// Optionally, you can *explicitly* set the 'DateTimeFormat' property of the 'IsoDateTimeConverter' if 
    /// necessary. For example...
    /// 
    ///     var isoConverter = new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" }
    /// 
    /// This will format a date as... 
    /// 
    ///         2013-03-03 00:00:00
    /// 
    /// ... instead of ...
    /// 
    ///         2013-03-03T00:00:00 
    /// 
    /// ... which would, because of the 'T' translate to GMT which would be one day behind and render as...
    /// 
    ///         2013-03-02
    /// 
    /// </remarks>
    public class JsonDateResult : JsonResult
    {
        public override void ExecuteResult(ControllerContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var response = context.HttpContext.Response;

            response.ContentType = !String.IsNullOrEmpty(this.ContentType) ? this.ContentType : "application/json";

            if (this.ContentEncoding != null)
            {
                response.ContentEncoding = ContentEncoding;
            }

            if (this.Data != null)
            {
                // IE requires date format using SLASHES - DASHES break IE.
                var isoConverter = new IsoDateTimeConverter { DateTimeFormat = "MM/dd/yyyy HH:mm:ss" };
                response.Write(JsonConvert.SerializeObject(this.Data, isoConverter));
            }
        }
    }
}