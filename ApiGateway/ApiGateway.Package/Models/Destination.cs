﻿using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Threading.Tasks;

namespace ApiGateway.Package.Models
{
    public class Destination
    {
        public int ServiceId{ get; set; }
        public string Uri { get; set; }
        public bool RequiresAuthentication { get; set; }
        static HttpClient client = new HttpClient();

        public Destination(string uri, bool requiresAuthentication)
        {
            Uri = uri;
            RequiresAuthentication = requiresAuthentication;
        }

        public Destination(string uri)
            : this(uri, false)
        {
        }

        private Destination()
        {
            Uri = "/";
            RequiresAuthentication = false;
        }


        private string CreateDestinationUri(HttpRequest request)
        {
            string requestPath = request.Path.ToString();
            string queryString = request.QueryString.ToString();

            string endpoint = "";
            string[] endpointSplit = requestPath.Substring(1).Split('/');

            if (endpointSplit.Length > 1)
                endpoint = endpointSplit[1];

            return Uri + endpoint + queryString;
        }


        public async Task<HttpResponseMessage> SendRequest(HttpRequest request)
        {
            var newRequest = new HttpRequestMessage(new HttpMethod(request.Method), CreateDestinationUri(request));
            return await client.SendAsync(newRequest);
        }
    }
}