using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BikeSharing.Services.Feedback.Api.Queries;
using BikeSharing.Services.Core.Controllers;
using BikeSharing.Services.Core.Commands;
using BikeSharing.Services.Feedback.Api.StdHost.Commands;
using BikeSharing.Services.Feedback.Api.StdHost.Requests;
using System.Linq;
using BikeSharing.Services.Feedback.Api.StdHost.Models;
using BikeSharing.Services.Feedback.Api.Models;
using System;
using BikeSharing.Services.Feedback.Api.StdHost.Responses;
using System.Collections.Generic;
using Microsoft.Extensions.Primitives;

namespace BikeSharing.Services.Feedback.Api.Controllers
{
    [Route("api/[controller]")]
    public class LastRidesIssuesController : BaseApiController
    {
        private readonly IIssuesQueries _issuesQueries;
        private readonly IRidesQueries _ridesQueries;
        public LastRidesIssuesController(ICommandBus bus, IIssuesQueries issuesQueries, IRidesQueries ridesQueries) : base(bus)
        {
            _ridesQueries = ridesQueries;
            _issuesQueries = issuesQueries;
        }

        [HttpGet()]
        public async Task<IActionResult> Get(int from = 0, int size = 20)
        { 
            // BREAKPOINT
            // get collections of issues, users, requests and merge the collections together 
            var issues = (await _issuesQueries.GetTopLastIssues(from, size)).ToList();
            var userIds = issues.Select(i => i.UserId).Distinct();
            var ridesRequest = issues.Select(i => new RidesByUserBike { UserId = i.UserId, BikeId = i.BikeId }).Distinct();
            var rides = await _ridesQueries.GetRidesByUserAndBike(ridesRequest);
            var result = MergeIssuesAndRides(issues, rides);
            SetCountPaginationHeader(result.Count());
            var response = result;
            return Ok(response);
        }

        private IEnumerable<DetailedIssue> MergeIssuesAndRides(IEnumerable<ReportedIssue> issues, IEnumerable<RidesResponse> rides)
        {
            var finalIssues = new List<DetailedIssue>();
            foreach (var issue in issues)
            {
                var ride = rides.FirstOrDefault(r => r.UserId == issue.UserId && r.Bike.Id == issue.BikeId);
                finalIssues.Add(MapIssue(issue, ride));
            }

            return finalIssues;
        }

        private void SetCountPaginationHeader(int count)
        {
            var countHeaderValue = new StringValues(count.ToString());
            var header = new KeyValuePair<string, StringValues>("total", countHeaderValue);

            Response.Headers.Add(header);
        }

        private DetailedIssue MapIssue(ReportedIssue issue, RidesResponse ride)
        {
            return new DetailedIssue
            {
                IssueId = issue.Id,
                IssueDate = issue.UtcTime,
                IssueTitle = issue.Title,
                IssueDescription = issue.Description,
                IssueSolved = issue.Solved,
                IssueType = issue.IssueType.ToString(),
                UserId = issue.UserId,
                BikeId = issue.BikeId,
                BikeSerialNumber = ride?.Bike?.SerialNumber,
                RideDuration = ride?.Duration ?? 0,
                RideFrom = ride?.From,
                RideTo = ride?.To,
                RideStart = ride?.Start ?? DateTime.UtcNow,
                RideStop = ride?.Stop ?? DateTime.UtcNow,
                RideType = ride?.RideType
            };
        }
    }
}
