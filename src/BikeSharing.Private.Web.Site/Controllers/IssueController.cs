using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BikeSharing.Private.Web.Site.Services;
using BikeSharing.Private.Web.Site.Models.IssueViewModels;
using BikeSharing.Private.Web.Site.Services.Pagination;
using Microsoft.Extensions.Options;
using BikeSharing.Private.Web.Site.Configuration;
using Microsoft.AspNetCore.Authorization;

namespace BikeSharing.Private.Web.Site.Controllers
{
    public class IssueController : Controller
    {


        [HttpGet]
        public IActionResult Index(int page)
        {
            var vm = new IndexViewModel()
            {
                PaginationInfo = new PaginationInfo()
                {
                    ActualPage = page,
                    ItemsPerPage = 20
                }
            };

            return View(vm);
        }
    }
}