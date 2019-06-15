using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace StudyTest
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RouteConfig.RegisterRoutes(RouteTable.Routes);

            //準備一個HandleErrorAttribute
            var handleError = new HandleErrorAttribute();
            // 指定HandleError導向的頁面，要指定值  但會吃config的頁面
            handleError.View = "/Home";
            // 設定HandleError啟用
            handleError.Order = 0;
            //加到全域的filter中
            GlobalFilters.Filters.Add(handleError);
        }
    }
}
