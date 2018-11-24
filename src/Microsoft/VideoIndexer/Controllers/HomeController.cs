using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VideoIndexer.Api;
using VideoIndexer.Models;
using VideoIndexer.ViewModel;

namespace VideoIndexer.Controllers
{
    public class HomeController : Controller
    {
        private readonly string apiKey = "<Key>";
        private readonly string apiUrl = "https://api.videoindexer.ai";
        private readonly string location = "trial";
        private readonly string accountId = "<Key";
        private readonly string description = "Description";
        private readonly Dictionary<string, string> videosUrl = new Dictionary<string, string>
        {
            {"Horse", "https://goo.gl/bwgQin"},
            {"Dog", "https://goo.gl/eaE26k"},
            {"Lion", "https://goo.gl/FhtS2e"}
        };

        public IActionResult Index() 
        { 
            return View(videosUrl.Select(k => new HomeInformationViewModel{
                Name = k.Key,
                Image = $"images/{k.Key}.jpg"
            }).ToList()); 
        }

        [HttpGet]
        public IActionResult Video(string id)
        {
            // This code predict informations concern image from API Microsoft
            var videoIndexer = new VideoInformation(apiKey, apiUrl, location, accountId);
            videoIndexer.Run(new MetaInformation{
                Name = id,
                Description = description,
                VideoUrl = videosUrl[id],
                Privacy = Privacy.Public.ToString().ToLower()
            });

            // Return a view and the object that will be processed
            return View("Video", new VideoInformationViewModel{
                Name = id,
                UrlVideo = videosUrl[id],
                Embed = videoIndexer.Embed,
                Insights = videoIndexer
                    .Insights
                    .Select(k => k.Name)
                    .Aggregate((current, next) => $"{current}, {next}")
            });
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel 
            { 
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier 
            });
        }
    }
}
