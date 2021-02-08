using System;
using System.Net.Http;

namespace MMTDigital.Model
{
    public class MMTDigitalClient
    {
        public HttpClient Client { get; set; }

        public MMTDigitalClient(HttpClient client)
        {
            Client = client;
        }
    }
}
