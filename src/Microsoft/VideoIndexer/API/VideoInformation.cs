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
    internal class VideoInformation
    {
        private readonly string _url;
        private readonly string _token;

        public VideoInformation(string url, string token)
        {
            _url = url;
            _token = token;
        }

        
    }
}