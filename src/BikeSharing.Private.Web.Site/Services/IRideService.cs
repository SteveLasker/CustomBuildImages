using BikeSharing.Private.Web.Site.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BikeSharing.Private.Web.Site.Services
{
    public interface IRideService
    {
        int TotalItems { get; }
        Task<List<Ride>> GetDataAsync(int? take, int? skip);
    }
}
