using System;
using System.Net;
using System.Net.Http;

namespace khi.Catalog
{
    public class HttpClientFactory
    {
        public static HttpClient Build()
        {
            var client = new BetterHttpClient.HttpClient();

            if (Configuration.Proxy.Enabled)
            {
                return new HttpClient();
            }

            return client;
        }


        public static HttpClientHandler BuildHttpClientHandler()
        {
            var httpClientHandler = new HttpClientHandler();

            if (!Configuration.Proxy.Enabled) return httpClientHandler;

            var proxy = new WebProxy
            {
                Address = new Uri($"{Configuration.Proxy.Address}:{Configuration.Proxy.Port}"),
                BypassProxyOnLocal = false,
                UseDefaultCredentials = false,
            };

            if (!string.IsNullOrEmpty(Configuration.Proxy.User))
            {
                proxy.Credentials = new NetworkCredential
                {
                    UserName = Configuration.Proxy.User,
                    Password = Configuration.Proxy.Pass
                };
            }

            

            var handler = new HttpClientHandler
            {
                Proxy = proxy,
                UseProxy = true
            };

            if (!string.IsNullOrEmpty(Configuration.Proxy.User) && Configuration.Proxy.PreAuthenticate)
            {
                handler.PreAuthenticate = true;
                handler.UseDefaultCredentials = false;
                handler.Credentials = new NetworkCredential
                {
                    UserName = Configuration.Proxy.User,
                    Password = Configuration.Proxy.Pass
                };
            }

            return httpClientHandler;
        }
    }
}