using GetDocNumbers.Models;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Serilog;

namespace GetDocNumbers.BusinessLogic {
    public class Requester : IRequester {

        const string baseUrl = "https://pub.fsa.gov.ru/";
        static string jsessionId = "";
        static string access_token;
        static RestClient restClient;
        const string access_token_path = @"Data/access_token.txt";

        static Requester() {
            if (File.Exists(access_token_path))
                access_token = File.ReadAllText(access_token_path);
            restClient = Tools.GetRestClient(baseUrl);
        }


        public async Task<Tuple<int, bool>> CertCounterRequestProcAsync(DateTime dt) {
            RestRequest req;
            int tryCounter = 0;
            bool httpStatusOK = false;
            do {
                req = new RestRequest("/api/v1/rss/common/certificates/get", Method.POST)
                {
                    Timeout = 5000
                };
                req = AddJHeaders(req);
                var postRequest = CreateCertPostRequest(dt);
                req.AddBody(postRequest);
                var resp = restClient.Execute(req);
                httpStatusOK = resp.StatusCode == HttpStatusCode.OK;
                if (httpStatusOK) {
                    // вот это - тот ответ, который нас устраивает!
                    var answer = JsonSerializer.Deserialize<CertAnswer>(resp.Content);
                    return new Tuple<int, bool>(answer.total, true);
                }
                else {
                    // статус - ненормальный. Логгируем и пытаемся залогиниться
                    Log.Error($"Request at /api/v1/rss/common/certificates/get return StatusCode={resp.StatusCode}");
                    TryAuth(ref req);
                }

                tryCounter++;
            } while (!httpStatusOK && tryCounter < 5);

            return new Tuple<int, bool>(0, false);

        }

        RestRequest AddJHeaders(RestRequest request) {
            if (string.IsNullOrEmpty(jsessionId)) jsessionId = Tools.NewJsessionId(32);
            request.AddCookie("JSESSIONID", jsessionId);
            request.AddHeader("Referer", "https://pub.fsa.gov.ru/rss/certificate");
            request.RequestFormat = DataFormat.Json;
            request.AddParameter("Authorization", string.Format("Bearer " + access_token), ParameterType.HttpHeader);
            return request;
        }

        CertPostRequest CreateCertPostRequest(DateTime dt) {
            CertPostRequest postRequest = new CertPostRequest
            {
                page = 0,
                size = 10
            };

            CertColumnsSort ccs = new CertColumnsSort
            {
                column = "date",
                sort = "DESC"
            };
            postRequest.columnsSort = new List<CertColumnsSort>();
            postRequest.columnsSort.Add(ccs);

            CertFilter cf = new CertFilter
            {
                endDate = new CertEndDate(),
                regDate = new CertRegDate
                {
                    minDate = dt.Date.ToString("s"),
                    maxDate = dt.Date.ToString("s")
                }
            };
            postRequest.filter = cf;

            return postRequest;
        }

        void TryAuth(ref RestRequest req) {
            // надо пройти процедуру авторизации

            req = new RestRequest("/login", Method.POST);
            req.Timeout = 5000;
            req = AddJHeaders(req);
            UserAndPassword uip = new UserAndPassword
            {
                username = "anonymous",
                password = "hrgesf7HDR67Bd"
            };
            req.AddBody(uip);

            var resp = restClient.Execute(req);
            if (resp.StatusCode == HttpStatusCode.OK) {
                access_token = Tools.GetAuthToken(resp);
                Log.Information($"TryAuth(): token={access_token}");
                File.WriteAllText(access_token_path, access_token);
            }
            else
                Log.Error($"TryAuth(): StatusCode={resp.StatusCode}");
        }

    }
}
