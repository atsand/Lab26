using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Net;
using Lab26.Models;

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

        public ActionResult SearchByLocation()
        {
            ViewBag.Message = "Weather Search Page.";

            return View();
        }

        //Trying to allow returning to search results in top bar
        //public ActionResult WeatherByLocationDefault()
        //{
        //    if (ViewBag.NotInUS=="inUS")
        //    {
        //        return View("WeatherByLocation");
        //    }
        //    else
        //    {
        //        return View("SearchByLocation");
        //    }
        //}

        public ActionResult WeatherByLocation(Location loc)
        {
            try
            {
                HttpWebRequest request = WebRequest.CreateHttp(String.Format("http://forecast.weather.gov/MapClick.php?lat={0}&lon={1}&FcstType=json", loc.Lat.ToString(), loc.Lon.ToString()));

                request.UserAgent = @"User-Agent: Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/53.0.2785.116 Safari/537.36";

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                StreamReader rd = new StreamReader(response.GetResponseStream());

                String data = rd.ReadToEnd();

                JObject s = JObject.Parse(data);

                List<string> tempInfo = new List<string>();

                for (int i = 0; i < s["data"]["text"].Count(); i++)
                {
                    string input = (s["time"]["startPeriodName"][i].ToString() + " " + s["data"]["text"][i].ToString());

                    tempInfo.Add(input);
                }

                ViewBag.AllSearchTemps = tempInfo;
                ViewBag.SearchArea = s["location"]["areaDescription"];
                ViewBag.NotInUS = "inUS";

                return View();
            }
            catch (Exception)
            {
                ViewBag.NotInUS = "Location is not in the US.  Please try again.";
                return View("SearchByLocation");
            }

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
            ViewBag.Area = o["location"]["areaDescription"];

            return View("Weather");
        }

        public ActionResult Weather()
        {
            return GetWeather();
        }
    }
}