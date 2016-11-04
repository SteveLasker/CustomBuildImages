using BikeSharing.Private.Web.Site.Configuration;
using BikeSharing.Private.Web.Site.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace BikeSharing.Private.Web.Site.Services
{
    public class RideService : IRideService
    {
        private List<Ride> _rides;
        private HttpClient _apiClient;
        private readonly IOptions<ApiSettings> _apiSettings;
        private int _totalItems;

        public int TotalItems {
            get { return _totalItems; }
        }

        public RideService(IOptions<ApiSettings> apiSettings)
        {
            _apiSettings = apiSettings;
        }

        public async Task<List<Ride>> GetDataAsync(int? take, int? skip)
        {
            using (_apiClient = new HttpClient())
            {
                var uri = (skip.HasValue) ? string.Format("{0}?from={1},to={2}", _apiSettings.Value.RidesApiUrl, skip.Value, skip.Value + take.Value) : _apiSettings.Value.RidesApiUrl;

                _apiClient.DefaultRequestHeaders.Accept.Clear();
                var response = await _apiClient.GetAsync(uri);
                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync();
                    _rides = JsonConvert.DeserializeObject<List<Ride>>(responseJson);
                    IEnumerable<string> values;
                    if (response.Headers.TryGetValues("total", out values))
                        _totalItems = int.Parse(values.First());
                }
            }
            return _rides;
        }
    }
}
