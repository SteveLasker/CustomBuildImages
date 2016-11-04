using BikeSharing.Private.Web.Site.Models.IssueViewModels;
using BikeSharing.Private.Web.Site.Services;
using BikeSharing.Private.Web.Site.Services.Pagination;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BikeSharing.Private.Web.Site.Views.Shared.Components
{
    public class IssueList:ViewComponent
    {
        private readonly IIssueService _svc;

        public IssueList(IIssueService svc)
        {
            _svc = svc;
        }

        public async Task<IViewComponentResult> InvokeAsync(IndexViewModel vm)
        {
            var take = vm.PaginationInfo.ItemsPerPage;
            var skip = take * vm.PaginationInfo.ActualPage;
 
            var data = await _svc.GetDataAsync(take, skip);
            vm.Issues = data ?? new List<Models.Issue>();
            vm.PaginationInfo.TotalItems = _svc.TotalItems;
            decimal d = (_svc.TotalItems / vm.PaginationInfo.ItemsPerPage);
            vm.PaginationInfo.TotalPages = int.Parse(Math.Round(d, MidpointRounding.AwayFromZero).ToString());
            vm.PaginationInfo.Next = (vm.PaginationInfo.ActualPage == vm.PaginationInfo.TotalPages - 1) ? "is-disabled" : "";
            vm.PaginationInfo.Previous = (vm.PaginationInfo.ActualPage == 0) ? "is-disabled" : "";

            return View(vm);
        }
    }
}
