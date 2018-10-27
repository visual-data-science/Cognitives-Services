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
        public IActionResult Index()
        {
            // That variables will load key and url where will be processed from Microsoft
            var apiKey = "<YOUR Key>";
            var apiUrl = "https://api.videoindexer.ai";
            var location = "trial";
            var accountId = "<YOUR AccountId>";
            var video = "https://images.all-free-download.com/footage_preview/mp4/horses_101.mp4";

            // This code predict informations concern image from API Microsoft
            var videoIndexer = new VideoInformation(apiKey, apiUrl, location, accountId);
            videoIndexer.Run(video);
            var url = videoIndexer.PlayerWidgetUrl;

            // Return a view and the object that will be processed
            return View("Index", new VideoInformationViewModel{
                PlayerWidgetUrl = videoIndexer.PlayerWidgetUrl,
                Insights = videoIndexer.Insights
            });
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
