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
        public IActionResult Index() => View();

        [HttpPost]
        public IActionResult Video()
        {
            // That variables will load key and url where will be processed from Microsoft
            var apiKey = "<key>";
            var apiUrl = "https://api.videoindexer.ai";
            var location = "<location>";
            var accountId = "<key>";
            var video = "https://images.all-free-download.com/footage_preview/mp4/horses_101.mp4";

            // This code predict informations concern image from API Microsoft
            var videoIndexer = new VideoInformation(apiKey, apiUrl, location, accountId);
            videoIndexer.Run(new MetaInformation{
                Name = "Horse",
                Description = "Description",
                VideoUrl = video,
                Privacy = Privacy.Public.ToString().ToLower()
            });

            // Return a view and the object that will be processed
            return View("Index", new VideoInformationViewModel{
                PlayerWidgetUrl = videoIndexer.PlayerWidgetUrl,
                Embed = videoIndexer.Embed
            });
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
