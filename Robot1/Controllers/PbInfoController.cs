﻿using Microsoft.AspNetCore.Mvc;
using RestSharp;
using Serilog;
using System;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Globalization;
using System.Threading;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace Pb.Controllers {
    [ApiController]
    [Route("[controller]")]

    public class PbInfoController : ControllerBase {


        string baseUrl = "https://pb.nalog.ru/";
        RestClient restClient;
        string jsessionId = null;


        [HttpGet]
        public string Get() {
            Log.Information($"PbInfoController.Get() called");

            var request = HttpContext.Request;
            var query = request.Query;
            string searchQuery = "";
            foreach (var item in query) { // item is key-value pair
                if (item.Key == "query") {
                    searchQuery = item.Value[0];
                    break;
                }
            }
            Log.Information($"query={searchQuery}");

            if (string.IsNullOrEmpty(searchQuery)) return "";

            restClient = Tools.GetRestClient(baseUrl);

            //RestRequest req = new RestRequest("/compare-proc.json", Method.POST);
            RestRequest req = new RestRequest("/search-proc.json", Method.POST);
            
            req.Timeout = 5000;
            req = AddJHeaders(req);
            req.AddCookie("pb-compare-inn-list", searchQuery);

            req.AddParameter("page", "1");
            req.AddParameter("pageSize", "10");
            req.AddParameter("pbCaptchaToken", "");
            req.AddParameter("token", "");
            req.AddParameter("mode", "search-all");
            req.AddParameter("queryAll", searchQuery);
            req.AddParameter("queryUl", "");
            req.AddParameter("okvedUl", "");
            req.AddParameter("statusUl", "");
            req.AddParameter("regionUl", "");
            req.AddParameter("isMspUl", "");
            req.AddParameter("mspUl1", "1");
            req.AddParameter("mspUl2", "2");
            req.AddParameter("mspUl3", "3");
            req.AddParameter("queryIp", "");
            req.AddParameter("okvedIp", "");
            req.AddParameter("statusIp", "");
            req.AddParameter("regionIp", "");
            req.AddParameter("isMspIp", "");
            req.AddParameter("mspIp1", "1");
            req.AddParameter("mspIp2", "2");
            req.AddParameter("mspIp3", "3");
            req.AddParameter("queryUpr", "");
            req.AddParameter("uprType1", "1");
            req.AddParameter("uprType0", "1");
            req.AddParameter("queryRdl", "");
            req.AddParameter("dateRdl", "");
            req.AddParameter("queryAddr", "");
            req.AddParameter("regionAddr", "");
            req.AddParameter("queryOgr", "");
            req.AddParameter("ogrFl", "1");
            req.AddParameter("ogrUl", "1");
            req.AddParameter("npTypeDoc", "1");
            req.AddParameter("ogrnUlDoc", "");
            req.AddParameter("ogrnIpDoc", "");
            req.AddParameter("nameUlDoc", "");
            req.AddParameter("nameIpDoc", "");
            req.AddParameter("formUlDoc", "");
            req.AddParameter("formIpDoc", "");
            req.AddParameter("ifnsDoc", "");
            req.AddParameter("dateFromDoc", "");
            req.AddParameter("dateToDoc", "");
            
            var resp = restClient.Execute(req);
            if (resp.StatusCode != HttpStatusCode.OK) return "";

            Thread.Sleep(1000);

            string rawJson = resp.Content;
            bool flagIp = false;
            CompanyInfo k = new CompanyInfo();
            //System.IO.File.WriteAllText("answer.json", rawJson);

            JObject j = JObject.Parse(rawJson);
            JToken jitem = j.SelectToken("$.ul.data[0].token");
            if (jitem == null) {
                // возможно, это - ИП?
                JToken jitemIp = j.SelectToken("$.ip");
                if (jitemIp != null) {
                    flagIp = true;
                    JToken jitemIpData = j.SelectToken("$.ip.data[0]");
                    jitem = j.SelectToken("$.ip.data[0].token");
                    k.isIp = true;
                    k.ShortName = k.LongName = (string)jitemIpData.SelectToken("$.namec");
                }
                else return "";
            }
            if (jitem == null) return "";
            string token = (string)jitem;

            RestRequest req2 = new RestRequest("/company-proc.json", Method.POST);
            req2.Timeout = 5000;
            req2.AddCookie("JSESSIONID", jsessionId);
            req2.AddHeader("Content-Type", "application/x-www-form-urlencoded; charset=UTF-8");
            req2.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/93.0.4577.63 Safari/537.36");
            req2.AddHeader("Referer", "https://pb.nalog.ru/company.html?token=" + token);
            req2.AddParameter("token", token);
            req2.AddParameter("method", "get-request");

            var resp2 = restClient.Execute(req2);
            if (resp2.StatusCode != HttpStatusCode.OK) return "";

            

            string rawJson2 = resp2.Content;

            j = JObject.Parse(rawJson2);
            JToken jitem_id = j.SelectToken("$.id");
            JToken jitem_token = j.SelectToken("$.token");
            JToken jitem_capure = j.SelectToken("$.captchaRequired");
            if (jitem_id == null || jitem_token == null || jitem_capure == null) return "";

            string id = (string)jitem_id;
            token = (string)jitem_token;
            bool captchaRequired = (bool)jitem_capure;

            if (captchaRequired) {
                Log.Error("captcha required!");
                //return "";
            }

            int reqCnt = 0;
            string rawJson3 = null;
            do {
                RestRequest req3 = new RestRequest("/company-proc.json", Method.POST);
                req3.Timeout = 5000;
                req3.AddCookie("JSESSIONID", jsessionId);
                req3.AddHeader("Content-Type", "application/x-www-form-urlencoded; charset=UTF-8");
                req3.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/93.0.4577.63 Safari/537.36");
                req3.AddHeader("Referer", "https://pb.nalog.ru/company.html?token=" + token);
                req3.AddParameter("token", token);
                req3.AddParameter("id", id);
                req3.AddParameter("method", "get-response");
                var resp3 = restClient.Execute(req3);
                if (resp3.StatusCode == HttpStatusCode.OK) {
                    rawJson3 = resp3.Content;
                    Log.Information($"begin of resp3.Content={resp3.Content.Substring(0, Math.Min(resp3.Content.Length, 50))}");
                }
                else
                    Log.Information($"req3.StatusCode={resp3.StatusCode.ToString()}");
                Thread.Sleep(2000);
            } while (reqCnt++ < 50 && (string.IsNullOrEmpty(rawJson3) || rawJson3 == "null"));
            if (string.IsNullOrEmpty(rawJson3) || rawJson3 == "null") {
                string tmpFN = ToLegalFn( $"{searchQuery}_{DateTime.Now.ToShortDateString()}_{DateTime.Now.ToShortTimeString()}.json" );
                Log.Information($"Answer on searchQuery={searchQuery} saved to {tmpFN}");
                System.IO.File.WriteAllText(tmpFN, rawJson3);
            }
            else {
                Log.Error("Response from server not received within fixed time");
                return ("ERROR: response from server not received within fixed time");
            }

            j = JObject.Parse(rawJson3);
            JToken v = j.SelectToken("$.vyp");
            if (flagIp) {
                string t_regDate = (string)v.SelectToken("$.ДатаПостУч");
                bool v1 = false, v2 = false;
                DateTime t1 = DateTime.Now, t2 = DateTime.Now; ;
                if (!string.IsNullOrEmpty(t_regDate))
                    v1 = DateTime.TryParseExact(t_regDate, "yyyy-MM-dd", new CultureInfo("en-US"), DateTimeStyles.None, out t1);
                if (v1)
                    k.RegDate = t1;
                string t_postUchetDate = (string)v.SelectToken("$.ДатаОГРНИП");
                if (!string.IsNullOrEmpty(t_postUchetDate))
                    v2 = DateTime.TryParseExact(t_postUchetDate, "yyyy-MM-dd", new CultureInfo("en-US"), DateTimeStyles.None, out t2);
                if (v2)
                    k.PostUchetDate = t2;
                k.Ogrn = (string)v.SelectToken("$.ОГРНИП");
                k.Inn = (string)v.SelectToken("$.ИННФЛ");
                k.liquidated = (bool)j.SelectToken("$.liquidated");
                string tmpT = (string)j.SelectToken("$.token");
                k.TempCompanyUrl = $"https://pb.nalog.ru/company.html?token={tmpT}";
                k.PdfTempUrl = (string)j.SelectToken("$.rsmppdf"); 
            }
            else {
                k.isIp = false;
                k.ShortName = (string)v.SelectToken("$.НаимЮЛСокр");
                k.LongName = (string)v.SelectToken("$.НаимЮЛПолн");
                string t_regDate = (string)v.SelectToken("$.ДатаРег");
                bool v1 = false, v2 = false;
                DateTime t1 = DateTime.Now, t2 = DateTime.Now; ;
                if (!string.IsNullOrEmpty(t_regDate))
                    v1 = DateTime.TryParseExact(t_regDate, "yyyy-MM-dd", new CultureInfo("en-US"), DateTimeStyles.None, out t1);
                if (v1)
                    k.RegDate = t1;
                string t_postUchetDate = (string)v.SelectToken("$.ДатаПостУч");
                if (!string.IsNullOrEmpty(t_postUchetDate))
                    v2 = DateTime.TryParseExact(t_postUchetDate, "yyyy-MM-dd", new CultureInfo("en-US"), DateTimeStyles.None, out t2);
                if (v2)
                    k.PostUchetDate = t2;
                k.Ogrn = (string)v.SelectToken("$.ОГРН");
                k.Inn = (string)v.SelectToken("$.ИНН");
                k.Kpp = (string)v.SelectToken("$.КПП");
                k.LegalAddress = (string)v.SelectToken("$.Адрес");
                k.liquidated = (bool)j.SelectToken("$.liquidated");
            }

            string answ = JsonConvert.SerializeObject(k);

            return answ;
        }

        private string ToLegalFn(string fn) => Regex.Replace(fn, @"[^\w\.-]", "-");





        RestRequest AddJHeaders(RestRequest request) {
            if ( string.IsNullOrEmpty(jsessionId) ) jsessionId = Tools.NewJsessionId(32);
            request.AddCookie("JSESSIONID", jsessionId);
            request.AddHeader("Referer", "https://pb.nalog.ru/");
            request.RequestFormat = DataFormat.Json;
            //request.AddParameter("Authorization", string.Format("Bearer " + _settings.Access_token), ParameterType.HttpHeader);
            return request;
        }
    }

    public class CompanyInfo {
        public string ShortName { get; set; }
        public string LongName { get; set; }
        public DateTime? RegDate { get; set; }
        public DateTime? PostUchetDate { get; set; }
        public string Ogrn { get; set; }
        public string Inn { get; set; }
        public string Kpp { get; set; }
        public string LegalAddress { get; set; }
        public bool isIp { get; set; }
        public bool liquidated {get;set;}
        public string TempCompanyUrl { get; set; }
        public string PdfTempUrl { get; set; }

    }
}
