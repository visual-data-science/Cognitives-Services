using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ComputerVision.Models;
using ComputerVision.ViewModel;
using ComputerVision.Api;
using Newtonsoft.Json;

namespace ComputerVision.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() => View();

        [HttpPost]
        public async Task<IActionResult> Index(string image)
        {
            // That variables will load key and url where will be processed from Microsoft
            var key = "<KEY>";
            var uriBase = "https://westcentralus.api.cognitive.microsoft.com/vision/v1.0/analyze";

            // This code predict informations concern image from API Microsoft
            var customVision = new ComputerVisionAnalysis(uriBase, key);
            var result = JsonConvert.DeserializeObject<AnalysisViewModel>(
                await customVision.MakeAnalysis(image));
            
            // Set an image url in object
            result.UrlImage = image;

            // Return a view and the object that will be processed
            return View("Index", result);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
