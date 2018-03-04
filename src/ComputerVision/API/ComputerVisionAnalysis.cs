using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ComputerVision.Api
{
    internal class ComputerVisionAnalysis
    {
        private readonly string _uriBase;
        private readonly string _key;

        /// <summary>
        /// Initialize the ComputerVisionAnalysis object.
        /// </summary>
        /// <param name="uriBase">Uri used to make the analysis.</param>
        /// <param name="key">Key necessary to validate your subscription .</param>
        public ComputerVisionAnalysis(string uriBase, string key)
        {
            _uriBase = uriBase;
            _key = key;
        }

        /// <summary>
        /// Returns the contents analized.
        /// </summary>
        /// <param name="imageUrl">The image url to read.</param>
        /// <returns>The content analyzed of the image data.</returns>
        public async Task<string> MakeAnalysis(string imageUrl)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _key);
                var content = new StringContent(JsonConvert.SerializeObject(new { Url = imageUrl }));
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                // Request parameters. A third optional parameter is "details".
                var requestParameters = "visualFeatures=Categories,Description,Color,Faces&language=en";

                // Assemble the URI for the REST API Call.
                var uri = _uriBase + "?" + requestParameters;

                // Execute the REST API call.
                var response = await client.PostAsync(uri, content);
                var contentString = await response.Content.ReadAsStringAsync();
                System.Diagnostics.Debug.WriteLine(contentString);
                return contentString;
            }
        }
    }
}