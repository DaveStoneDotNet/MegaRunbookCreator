
using System;
using System.Web.Mvc;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Web.Hosting;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;

using STAR.Originations.MRC.DataAccess;

using STAR.Originations.MegaRunbook.Contracts;
using STAR.Originations.MegaRunbook.Contracts.PagingModels;
using STAR.Originations.MegaRunbook.Website.CustomAttributes;

using contracts = STAR.Originations.MegaRunbook.Contracts.Data;

namespace STAR.Originations.MegaRunbook.Website.Controllers
{
    [JsonRequestBehavior]
    public class ApiController : BaseController
    {
        private MrcDataAccess mrcDataAccess;
        public MrcDataAccess MrcDataAccess => this.mrcDataAccess ?? (this.mrcDataAccess = new MrcDataAccess());

        #region GetUserProfile
        [System.Web.Http.HttpGet]
        public JsonResult GetUserProfile()
        {
            var userProfile = new contracts::UserProfile
            {
                UserId = 21981,
                UserDisplayName = "Dave Stone", 
                HasRole = true,
                IsAdministrator = true, 
                PrimaryRoleName = "Administrator", 
                RoleNames = new []{ "Administrator" }, 
                UserInitials = "LDS",
                IsSuccessful = true, 
                IsAuthenticated = true
            };

            return this.Json(userProfile);
        }
        #endregion GetUserProfile

        #region GetRunbookTemplates
        [System.Web.Http.HttpGet]
        public async Task<JsonResult> GetRunbookTemplates(contracts::RunbookTemplate runbookTemplate)
        {
            runbookTemplate.Paging = runbookTemplate.Paging ?? new Paging { PageSize = 1000, PageNumber = 0, SortInfo = new List<SortInfo> { new SortInfo { PropertyName = "Name", Order = SortOrder.Ascending } } };
            runbookTemplate.Paging.SortInfo = runbookTemplate.Paging.SortInfo ?? new List<SortInfo> { new SortInfo { PropertyName = "Name", Order = SortOrder.Ascending } };
            
            var runbookTemplates = await this.MrcDataAccess.GetRunbookTemplatesAsync(runbookTemplate);

            //var runbookTemplates = await this.LoadJson<contracts::RunbookTemplate>(@"templates.json");
            //runbookTemplates = (from o in runbookTemplates select o).OrderBy(o => o.Name).ToList();

            return this.Json(runbookTemplates);
        }
        #endregion GetRunbookTemplates

        #region GetRunbookTemplate
        [System.Web.Http.HttpGet]
        public async Task<JsonResult> GetRunbookTemplate(int id)
        {
            //var runbookTemplates = await this.LoadJson<contracts::RunbookTemplate>(@"runbooks.json");
            //var runbookTemplate = (from o in runbookTemplates where o.ID == id select o).FirstOrDefault();
            
            var runbookTemplate = await this.MrcDataAccess.GetRunbookTemplateAsync(new contracts.RunbookTemplate { ID = id });

            return this.JsonDateResult(runbookTemplate);
        }
        #endregion GetRunbookTemplate

        #region GetRunbookSteps
        [System.Web.Http.HttpGet]
        public async Task<JsonResult> GetRunbookSteps()
        {
            var runbookSteps = await this.LoadJson<contracts::RunbookStep>(@"runbook-steps.json");
            runbookSteps = (from o in runbookSteps select o).OrderBy(o => o.ID).ToList();

            return this.Json(runbookSteps);
        }
        #endregion GetRunbookSteps

        #region GetRunbookStep
        [System.Web.Http.HttpGet]
        public async Task<JsonResult> GetRunbookStep(int id)
        {
            var runbookSteps = await this.LoadJson<contracts::RunbookStep>(@"runbook-steps.json");
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
                    var list = JsonConvert.DeserializeObject<List<T>>(json);
                    return list;
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

        #region GetHero
        [System.Web.Http.HttpGet]
        public async Task<JsonResult> GetHero(int id)
        {
            var heroes = await this.LoadJson<Hero>(@"heroes/heroes.json");
            var hero = (from o in heroes where o.Id == id select o).FirstOrDefault();
            return this.Json(hero);
        }
        #endregion GetHero

        #region GetCrisises
        [System.Web.Http.HttpGet]
        public async Task<JsonResult> GetCrisises()
        {
            var crisises = await this.LoadJson<Hero>(@"heroes/crisises.json");
            return this.Json(crisises);
        }
        #endregion GetCrisises

        #region GetCrisis
        [System.Web.Http.HttpGet]
        public async Task<JsonResult> GetCrisis(int id)
        {
            var crisises = await this.LoadJson<Hero>(@"heroes/crisises.json");
            var crisis = (from o in crisises where o.Id == id select o).FirstOrDefault();
            return this.Json(crisis);
        }
        #endregion GetCrisis
    }
}