using System;
using System.Net;
using RestSharp;

namespace GetDocNumbers.BusinessLogic {
    static class Tools {
        const string HexSymbols = "0123456789ABCDEF";
        public static string NewJsessionId(int num) {
            if (num <= 0) return null;
            string pswd = string.Empty;
            Random rnd = new Random();
            for (int i = 0; i < num; i++)
                pswd += HexSymbols[rnd.Next(HexSymbols.Length)];
            return pswd;
        }
        static readonly CookieContainer _cookieContainer = new CookieContainer();
        public static RestClient GetRestClient(string url) {
            var restClient = new RestClient(url);
            restClient.DefaultParameters.Clear();
            restClient.AddDefaultHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
            restClient.AddDefaultHeader("Accept-Language", "ru-RU,ru;q=0.8,en-US;q=0.6,en;q=0.4,ar;q=0.2");
            restClient.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/45.0.2454.101 Safari/537.36";
            restClient.CookieContainer = _cookieContainer;
            return restClient;
        }

        public static string GetAuthToken(RestSharp.IRestResponse response) {
            foreach (var h in response.Headers)
                if (h.Name == "Authorization" && ((string)h.Value).Contains("Bearer "))
                    return ((string)(h.Value)).Substring("Bearer ".Length);
            return null;
        }
    }
}
