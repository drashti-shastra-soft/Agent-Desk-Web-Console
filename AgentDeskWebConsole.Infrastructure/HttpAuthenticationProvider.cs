using System;
using System.Linq;
using System.Net.Http;

namespace AgentDeskWebConsole.Infrastructure
{
    public class HttpAuthenticationProvider
    {
        private static readonly HttpClient _client = new HttpClient();

        public static HttpClient Client => _client;
    }
}
