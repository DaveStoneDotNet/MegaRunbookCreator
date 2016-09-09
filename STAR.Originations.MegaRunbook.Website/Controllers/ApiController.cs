
using System;
using System.Web.Mvc;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Web.Hosting;

using Newtonsoft.Json;

using STAR.Originations.MegaRunbook.Contracts;
using STAR.Originations.MegaRunbook.Website.CustomAttributes;

namespace STAR.Originations.MegaRunbook.Website.Controllers
{
    [JsonRequestBehavior]
    public class ApiController : BaseController
    {
        [System.Web.Http.HttpGet]
        public JsonResult GetUserProfile()
        {
            var userProfile = new UserProfile
            {
                UserId = 21981,
                UserDisplayName = "Dave Stone", 
                HasRole = true,
                IsAdministrator = true, 
                PrimaryRoleName = "Administrator", 
                RoleNames = new []{ "Administrator" }, 
                UserInitials = "LDS",
                IsSuccessful = true
            };

            return this.Json(userProfile);
        }

        [System.Web.Http.HttpGet]
        public async Task<JsonResult> GetRunbookTemplates()
        {
            var runbookTemplates = await this.LoadJson<RunbookTemplate>(@"templates.json");
            runbookTemplates = (from o in runbookTemplates select o).OrderBy(o => o.Name).ToList();

            return this.Json(runbookTemplates);
        }

        [System.Web.Http.HttpGet]
        public async Task<JsonResult> GetRunbookTemplate(int id)
        {
            var runbookTemplates = await this.LoadJson<RunbookTemplate>(@"runbooks.json");
            var runbookTemplate = (from o in runbookTemplates where o.ID == id select o).OrderBy(o => o.Name).FirstOrDefault();

            return this.Json(runbookTemplate);
        }

        public async Task<List<T>> LoadJson<T>(string fileName)
        {
            var path = HostingEnvironment.MapPath(String.Format("~/app/Json/{0}", fileName));
            if (!String.IsNullOrWhiteSpace(path))
            {
                using (var streamReader = new StreamReader(path))
                {
                    var json = await streamReader.ReadToEndAsync();
                    var runbookTemplates = JsonConvert.DeserializeObject<List<T>>(json);
                    return runbookTemplates;
                }
            }
            else
            {
                return null;
            }
        }

        public List<IOrderable> GetSteps()
        {
            var steps = new List<IOrderable>
            {
                new Step { ID = 55, Name = "E", Order = 5 },
                new Step { ID = 89, Name = "H", Order = 8 },
                new Step { ID = 22, Name = "B", Order = 2 },
                new Step { ID = 77, Name = "G", Order = 7 },
                new Step { ID = 10, Name = "J", Order = 10 }, 
                new Step { ID = 44, Name = "D", Order = 4 },
                new Step { ID =  6, Name = "F", Order = 6 },
                new Step { ID = 11, Name = "A", Order = 1 },
                new Step { ID = 33, Name = "C", Order = 3 },
                new Step { ID = 99, Name = "I", Order = 9 }
            };
            return steps;
        }
    }

    public class Step : IOrderable
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
    }

    public interface IOrderable
    {
        int Order { get; set; }
    }

    public static class OrderableExtensions
    {
        public static List<IOrderable> Reorder(this List<IOrderable> orderableList, int originalOrder, int newOrder, bool isCircular = false)
        {
            List<IOrderable> orderedList = null;

            var item = (from s in orderableList where s.Order == originalOrder select s).FirstOrDefault();

            if (item != null)
            {
                orderableList.Remove(item);

                orderedList = orderableList.OrderBy(a => a.Order).ToList();

                newOrder = newOrder - 1;
                if (newOrder < 0)
                {
                    newOrder = isCircular ? orderableList.Count : 0;
                }
                if (newOrder > orderableList.Count)
                {
                    newOrder = isCircular ? 0 : orderableList.Count;
                }

                orderedList.Insert(newOrder, item);

                for (var i = 0; i < orderedList.Count; i++)
                {
                    orderedList[i].Order = i + 1;
                }
            }
            else
            {
                orderedList = orderableList;
            }


            return orderedList;
        }
    }
}