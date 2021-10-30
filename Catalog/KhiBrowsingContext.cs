using System.Net;
using System.Net.Http;
using AngleSharp;

namespace khi.Catalog
{
    public class KhiBrowsingContext
    {
        private KhiBrowsingContext()
        {
        }

        public static IBrowsingContext Build()
        {
            return BrowsingContext.New(BuildConfig());
        }

        private static IConfiguration BuildConfig()
        {
            var config = AngleSharp.Configuration.Default
                .WithDefaultLoader();
            
            return config;
        }
    }
}