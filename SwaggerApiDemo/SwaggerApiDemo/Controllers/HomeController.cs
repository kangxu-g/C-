using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SwaggerApiDemo.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// 重定向到webapi页面
        /// </summary>
        public void Index()
        {
            Response.Redirect("/swagger");
        }

    }
}