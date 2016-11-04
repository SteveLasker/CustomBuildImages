using BikeSharing.Private.Web.Site.Services.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikeSharing.Private.Web.Site.Models.IssueViewModels
{
    public class IndexViewModel
    {
        public PaginationInfo PaginationInfo { get; set; }
        public List<Issue> Issues { get; set; }
    }
}
