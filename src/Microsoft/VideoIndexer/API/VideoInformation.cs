using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VideoIndexer.Api
{
    public class VideoInformation
    {
        private readonly int _timeWaiting = 10000;
        private readonly string _apiUrl;
        private readonly string _apiKey;
        private readonly string _location;
        private readonly string _accountId;
        private string _accountAccessToken;
        private string _videoAccessToken;
        private string _videoId;

        public string Embed 
        { 
            get
            { 
                return $@"https://www.videoindexer.ai/embed/insights/{_accountId}/{_videoId}/?version=2";
            }
        } 

        public string Search
        {
            get
            {
                try
                {
                    var handler = new HttpClientHandler();
                    handler.AllowAutoRedirect = false;
                    using (var client = new HttpClient(handler))
                    {
                        var searchRequestResult = client.GetAsync($"{_apiUrl}/{_location}/Accounts/{_accountId}/Videos/Search?accessToken={_accountAccessToken}&id={_videoId}").Result;
                        return searchRequestResult.Content.ReadAsStringAsync().Result;
                    }
                }
                catch(Exception ex)
                {
                    return ex.Message.ToString();
                }
            }
        }

        public string Insights
        {
            get
            {
                try
                {
                    var handler = new HttpClientHandler();
                    handler.AllowAutoRedirect = false;
                    using (var client = new HttpClient(handler))
                    {
                        var insightsWidgetRequestResult = client.GetAsync($"{_apiUrl}/{_location}/Accounts/{_accountId}/Videos/{_videoId}/InsightsWidget?accessToken={_videoAccessToken}&widgetType=Keywords&allowEdit=true").Result;
                        return insightsWidgetRequestResult.Headers.Location.ToString();
                    }
                }
                catch(Exception ex)
                {
                    return ex.Message.ToString();
                }
            }
        }
        
        public string PlayerWidgetUrl
        {
            get
            {
                try
                {
                    var handler = new HttpClientHandler();
                    handler.AllowAutoRedirect = false;
                    using (var client = new HttpClient(handler))
                    {
                        var playerWidgetRequestResult = client.GetAsync($"{_apiUrl}/{_location}/Accounts/{_accountId}/Videos/{_videoId}/PlayerWidget?accessToken={_videoAccessToken}").Result;
                        return playerWidgetRequestResult.Headers.Location.ToString();
                    }
                }
                catch(Exception ex)
                {
                    return ex.Message.ToString();
                }
            }
        }

        public VideoInformation(string apiKey, string apiUrl, string location, string accountId)
        {
            _apiKey = apiKey;
            _apiUrl = apiUrl;
            _location = location;
            _accountId = accountId;
        }

        public void Run(MetaInformation info)
        {
            if (string.IsNullOrWhiteSpace(info.VideoUrl))
                throw new ArgumentNullException(nameof(info.VideoUrl));

            var handler = new HttpClientHandler(); 
            handler.AllowAutoRedirect = false; 
            using (var client = new HttpClient(handler))
            {
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _apiKey); 

                // obtain account access token
                var accountAccessTokenRequestResult = client.GetAsync($"{_apiUrl}/auth/{_location}/Accounts/{_accountId}/AccessToken?allowEdit=true").Result;
                _accountAccessToken = accountAccessTokenRequestResult.Content.ReadAsStringAsync().Result.Replace("\"", "");
                client.DefaultRequestHeaders.Remove("Ocp-Apim-Subscription-Key");

                // upload a video
                var content = new MultipartFormDataContent();
                var uploadRequestResult = client.PostAsync($"{_apiUrl}/{_location}/Accounts/{_accountId}/Videos?accessToken={_accountAccessToken}&name={info.Name}&description={info.Description}&privacy={info.Privacy}&partition=some_partition&videoUrl={info.VideoUrl}", content).Result;
                var uploadResult = uploadRequestResult.Content.ReadAsStringAsync().Result;

                // get the video id from the upload result
                var returnInfos = JsonConvert.DeserializeObject<dynamic>(uploadResult);
                var returnInfosString = returnInfos.ToString();

                _videoId = returnInfos["id"].ToString(); 
                WaitingProcess();
            }
        }

        private void WaitingProcess()
        {
            while (true)
            {
                Thread.Sleep(_timeWaiting);
                var processingState = StatusProcess();
                if (processingState != "Uploaded" && processingState != "Processing")
                    break;
            }
        }

        private string StatusProcess()
        {
            var handler = new HttpClientHandler(); 
            handler.AllowAutoRedirect = false; 
            using (var client = new HttpClient(handler))
            {
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _apiKey);
                var videoTokenRequestResult = client.GetAsync($"{_apiUrl}/auth/{_location}/Accounts/{_accountId}/Videos/{_videoId}/AccessToken?allowEdit=true").Result;
                _videoAccessToken = videoTokenRequestResult.Content.ReadAsStringAsync().Result.Replace("\"", "");

                client.DefaultRequestHeaders.Remove("Ocp-Apim-Subscription-Key");

                var videoGetIndexRequestResult = client.GetAsync($"{_apiUrl}/{_location}/Accounts/{_accountId}/Videos/{_videoId}/Index?accessToken={_videoAccessToken}&language=English").Result;
                var videoGetIndexResult = videoGetIndexRequestResult.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<dynamic>(videoGetIndexResult)["state"].ToString();
            }
        }
    }
}