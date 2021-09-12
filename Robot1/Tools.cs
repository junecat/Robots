using RestSharp;
using System.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pb {
    public static class Tools {

        static readonly CookieContainer _cookieContainer = new CookieContainer();
        public static RestClient GetRestClient(string url) {
            var restClient = new RestClient(url);
            restClient.DefaultParameters.Clear();
            restClient.AddDefaultHeader("Accept", "application/json, text/javascript, */*; q=0.01");
            restClient.AddDefaultHeader("Accept-Language", "ru-RU,ru;q=0.8,en-US;q=0.6,en;q=0.4,ar;q=0.2");
            restClient.UserAgent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/93.0.4577.63 Safari/537.36";
            restClient.CookieContainer = _cookieContainer;
            return restClient;
        }

        const string HexSymbols = "0123456789ABCDEF";
        public static string NewJsessionId(int num) {
            if (num <= 0) return null;
            string pswd = string.Empty;
            Random rnd = new Random();
            for (int i = 0; i < num; i++)
                pswd += HexSymbols[rnd.Next(HexSymbols.Length)];
            return pswd;
        }

    }
}
