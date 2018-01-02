using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PredictingImages.Api
{
    internal class CustomVisionStarWars
    {
        private readonly Dictionary<string, string> _chars;
        private readonly string _url;
        private readonly string _token;

        public CustomVisionStarWars(string url, string token)
        {
            _chars = new Dictionary<string, string>
            {
                { "darth-vader", "Darth Vader"},
                { "kylo-ren", "Kylo Ren"},
                { "unknown", "Unknown"},

            };
            _url = url;
            _token = token;
        }

        public async Task<string> Detect(string image)
        {
            using (var client = new HttpClient())
            {
                var content = new StringContent(JsonConvert.SerializeObject(new { Url = image }));
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                content.Headers.Add("Prediction-Key", _token);

                var response = await client.PostAsync(_url, content);
                var result = JsonConvert.DeserializeObject<CustomVisionResult>(
                    await response.Content.ReadAsStringAsync());

                var character = result.Predictions
                    .OrderByDescending(p => p.Probability)
                    .FirstOrDefault();
                
                if (character.Probability >= 0.7m)
                    return _chars[character.Tag];
                
                return _chars["unknown"];
            }
        }

        private class Prediction
        {
            public string Tag { get; set; }
            public decimal Probability { get; set; }
        }

        private class CustomVisionResult
        {
            public Prediction[] Predictions { get; set; }
        }
    }
}