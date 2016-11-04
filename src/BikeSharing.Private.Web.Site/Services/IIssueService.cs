using BikeSharing.Private.Web.Site.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikeSharing.Private.Web.Site.Services
{
    public interface IIssueService
    {
        int TotalItems { get; }
        Task<List<Issue>> GetDataAsync(int? take, int? skip);
    }
}
