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
        private static Dictionary<string, string> videosUrl = new Dictionary<string, string>
        {
            {"Horse", "https://goo.gl/bwgQin"},
            {"Dog", "https://goo.gl/eaE26k"},
            {"Lion", "https://goo.gl/FhtS2e"}
        };

        public IActionResult Index() => View(videosUrl.Select(k => k.Key).ToList());

        [HttpGet]
        public IActionResult Video(string id)
        {
            // That variables will load key and url where will be processed from Microsoft
            var apiKey = "<Key>";
            var apiUrl = "https://api.videoindexer.ai";
            var location = "trial";
            var accountId = "<Key>";
            var video = videosUrl[id];

            // This code predict informations concern image from API Microsoft
            var videoIndexer = new VideoInformation(apiKey, apiUrl, location, accountId);
            videoIndexer.Run(new MetaInformation{
                Name = id,
                Description = "Description",
                VideoUrl = video,
                Privacy = Privacy.Public.ToString().ToLower()
            });

            // Return a view and the object that will be processed
            return View("Video", new VideoInformationViewModel{
                PlayerWidgetUrl = videoIndexer.PlayerWidgetUrl,
                Embed = videoIndexer.Embed,
                Name = id
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
