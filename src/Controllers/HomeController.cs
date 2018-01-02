using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PredictingImages.Api;
using PredictingImages.Models;
using PredictingImages.ViewModel;

namespace PredictingImages.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() => View(new ResultViewModel());

        [HttpPost]
        public async Task<IActionResult> Index(string image)
        {
            var url = "<Your url>";
            var token = "<Your token>";
            var customVision = new CustomVisionStarWars(url, token);
            var result = await customVision.Detect(image);
            return View("Index", new ResultViewModel{
                Label = result,
                Url = image
            });
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
