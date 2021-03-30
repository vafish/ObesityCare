using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ObesityCare.Models;
using System.Web.UI.DataVisualization.Charting;
using System.IO;
using System.Text;
using System.Drawing;
using Newtonsoft.Json;

namespace ObesityCare.Controllers
{

    //public class BasicAuthenticationAttribute : ActionFilterAttribute
    //{
    //    public string BasicRealm { get; set; }
    //    protected string Username { get; set; }
    //    protected string Password { get; set; }

    //    public BasicAuthenticationAttribute(string username, string password)
    //    {
    //        this.Username = username;
    //        this.Password = password;
    //    }

    //    public override void OnActionExecuting(ActionExecutingContext filterContext)
    //    {
    //        var req = filterContext.HttpContext.Request;
    //        var auth = req.Headers["Authorization"];
    //        if (!String.IsNullOrEmpty(auth))
    //        {
    //            var cred = System.Text.ASCIIEncoding.ASCII.GetString(Convert.FromBase64String(auth.Substring(6))).Split(':');
    //            var user = new { Name = cred[0], Pass = cred[1] };
    //            if (user.Name == Username && user.Pass == Password) return;
    //        }
    //        filterContext.HttpContext.Response.AddHeader("WWW-Authenticate", String.Format("Basic realm=\"{0}\"", BasicRealm ?? "Ryadel"));
    //        /// thanks to eismanpat for this line: http://www.ryadel.com/en/http-basic-authentication-asp-net-mvc-using-custom-actionfilter/#comment-2507605761
    //        filterContext.Result = new HttpUnauthorizedResult();
    //    }
    //}

    ////[BasicAuthenticationAttribute("admin@123.com", "Abcd1234`",
    ////BasicRealm = "your-realm")]
    //[Authorize]
    public class HomeController : Controller
    {
        //public ActionResult GetData()
        //{
        //    ObesityCare_BMI_Entities context = new ObesityCare_BMI_Entities();

        //    var query = context.BOY_BMI
        //           .GroupBy(p => p.Age_in_months)
        //           .Select(g => new { name = g.Key, count = g.Sum(w => w.Quantity) }).ToList();
        //    return Json(query, JsonRequestBehavior.AllowGet);
        //}

        //ObesityCare_BMI_Entities db = new ObesityCare_BMI_Entities();


        //public ActionResult ChartFromEF()
        //{
        //    var data = db.BOY_BMI.ToList();
        //    var chart = new Chart();
        //    var area = new ChartArea();
        //    chart.ChartAreas.Add(area);
        //    var series = new Series();
        //    foreach (var item in data)
        //    {
        //        series.Points.AddXY(item.Age_in_months, item.fifth_Percentile);
        //    }
        //    series.Label = "#PERCENT{P0}";
        //    series.Font = new Font("Arial", 8.0f, FontStyle.Bold);
        //    series.ChartType = SeriesChartType.Column;
        //    series["PieLabelStyle"] = "Outside";
        //    chart.Series.Add(series);
        //    var returnStream = new MemoryStream();
        //    chart.ImageType = ChartImageType.Png;
        //    chart.SaveImage(returnStream);
        //    returnStream.Position = 0;
        //    return new FileStreamResult(returnStream, "image/png");
        //}
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
        

        public ActionResult BMI()
        {
            ViewBag.Message = "BMI Calculator";
            ObesityCare_BMI_Entities db = new ObesityCare_BMI_Entities();

            try
            {
                ViewBag.DataPoints = JsonConvert.SerializeObject(db.BOY_BMI.ToList(), _jsonSetting);
                ViewBag.DataPoints2 = JsonConvert.SerializeObject(db.GIRL_BMI.ToList(), _jsonSetting);
                return View();
            }
            catch (System.Data.Entity.Core.EntityException)
            {
                return View("Error");
            }
            catch (System.Data.SqlClient.SqlException)
            {
                return View("Error");
            }
        }
        JsonSerializerSettings _jsonSetting = new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore };
            //return View();
        
        //}

        

        public ActionResult Activities()
        {
            ViewBag.Message = "Recommended Activities";

            return View();
        }
    }
}