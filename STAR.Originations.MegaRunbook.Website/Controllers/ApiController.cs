
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
        #region GetUserProfile
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
        #endregion GetUserProfile

        #region GetRunbookTemplates
        [System.Web.Http.HttpGet]
        public async Task<JsonResult> GetRunbookTemplates()
        {
            var runbookTemplates = await this.LoadJson<RunbookTemplate>(@"templates.json");
            runbookTemplates = (from o in runbookTemplates select o).OrderBy(o => o.Name).ToList();

            return this.Json(runbookTemplates);
        }
        #endregion GetRunbookTemplates

        #region GetRunbookTemplate
        [System.Web.Http.HttpGet]
        public async Task<JsonResult> GetRunbookTemplate(int id)
        {
            var runbookTemplates = await this.LoadJson<RunbookTemplate>(@"runbooks.json");
            var runbookTemplate = (from o in runbookTemplates where o.ID == id select o).FirstOrDefault();

            return this.Json(runbookTemplate);
        }
        #endregion GetRunbookTemplate

        #region GetRunbookSteps
        [System.Web.Http.HttpGet]
        public async Task<JsonResult> GetRunbookSteps()
        {
            var runbookSteps = await this.LoadJson<RunbookStep>(@"runbook-steps.json");
            runbookSteps = (from o in runbookSteps select o).OrderBy(o => o.ID).ToList();

            return this.Json(runbookSteps);
        }
        #endregion GetRunbookSteps

        #region GetRunbookStep
        [System.Web.Http.HttpGet]
        public async Task<JsonResult> GetRunbookStep(int id)
        {
            var runbookSteps = await this.LoadJson<RunbookStep>(@"runbook-steps.json");
            var runbookStep = (from o in runbookSteps where o.ID == id select o).FirstOrDefault();

            return this.Json(runbookStep);
        }
        #endregion GetRunbookStep

        // ---

        #region LoadJson
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
        #endregion LoadJson

        // ---

        #region GetHeroes
        [System.Web.Http.HttpGet]
        public async Task<JsonResult> GetHeroes()
        {
            var heroes = await this.LoadJson<Hero>(@"heroes/heroes.json");

            return this.Json(heroes);
        }
        #endregion GetHeroes


    }
}