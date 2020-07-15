using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RestSharp;
using RestSharp.Authenticators;

namespace AzureCalculatorAPI.Base
{
    public class RequestHelper
    {
        public IRestResponse ExecuteJson(string host, string path, Method method,
            bool https = false,
            IDictionary<string, string> headers = null,
            IDictionary<string, string> parameters = null,
            object jsonBody = null)
        {
            var client = new RestClient
            {
                BaseUrl = https ? new Uri($"https://{host}") : new Uri(host)
            };
            var request = new RestRequest(path, method)
            {
                RequestFormat = DataFormat.Json
            };
            request.AddHeader("Accept-Encoding", "gzip, deflate, br");
            request.AddHeader("Accept-Language", "en-US,en;q=0.9");
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/83.0.4103.116 Safari/537.36");
            if (headers != null)
            {
                foreach (var header in headers)
                {
                    request.AddHeader(header.Key, header.Value);
                }
            }
            if (parameters != null)
            {
                foreach (var param in parameters)
                {
                    request.AddParameter(param.Key, param.Value);
                }
            }
            if (jsonBody != null)
            {
                request.AddJsonBody(jsonBody);
            }
            return client.Execute(request);
        }
    }
}
