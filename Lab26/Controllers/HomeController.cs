﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Net;

namespace Lab26.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult GetWeather()
        {
            HttpWebRequest request = WebRequest.CreateHttp("http://forecast.weather.gov/MapClick.php?lat=38.4247341&lon=-86.9624086&FcstType=json");

            request.UserAgent = @"User-Agent: Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/53.0.2785.116 Safari/537.36";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            
            StreamReader rd = new StreamReader(response.GetResponseStream());

            String data = rd.ReadToEnd();

            JObject o = JObject.Parse(data);

            List<string> tempInfo = new List<string>();

            for (int i = 0; i < o["data"]["text"].Count(); i++)
            {
                string input = (o["time"]["startPeriodName"][i].ToString() + " " + o["data"]["text"][i].ToString());

                tempInfo.Add(input);
            }

            ViewBag.AllTemps = tempInfo;

            return View("Weather");
        }

        public ActionResult Weather()
        {
            return GetWeather();
        }
    }
}