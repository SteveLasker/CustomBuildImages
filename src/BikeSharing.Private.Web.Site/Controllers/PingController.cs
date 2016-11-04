using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;

namespace BikeSharing.Private.Web.Site.Controllers
{
    [Route("api/[controller]")]
    public class PingController : Controller
    {
        private AppSettings _settings;
        public PingController(IOptions<AppSettings> options)
        {
            _settings = options.Value;
        }

        [HttpGet]
        public dynamic GetProfilesMicroserviceUrl()
        {
            return new
            {
                Data = "Ok " + DateTime.Now
            };
        }
        
    }
}
