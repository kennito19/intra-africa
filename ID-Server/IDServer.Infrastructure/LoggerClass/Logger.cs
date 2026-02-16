using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;

namespace IDServer.Infrastructure.LoggerClass
{
    public class Logger
    {
        public int RowNumber { get; set; }
        public int PageCount { get; set; }
        public int RecordCount { get; set; }
        public int? Id { get; set; }
        public string UserId { get; set; }
        public string UserType { get; set; }
        public string URL { get; set; }
        public string Action { get; set; }
        public string LogTitle { get; set; }
        public string LogDescprition { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? Searchtext { get; set; }




        public Logger(string uid, string uType, string url, string action,string logtitle,string logDesc)
        {
            UserId = uid;
            UserType = uType;
            URL = url;
            Action = action;
            LogTitle = logtitle;
            LogDescprition = logDesc;
            CreatedAt = DateTime.Now;
        }

        public void apicall(Logger log)
        {
            string url = "http://65.0.144.207:8011/api/ActivityLog";
            HttpClient client = new HttpClient();
            var json = JsonConvert.SerializeObject(log);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync(url, data).Result;


        } 

    }
}
