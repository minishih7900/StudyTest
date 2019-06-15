using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Study.Models;
using System.IO;
using System.Web.Hosting;
using Microsoft.Win32;
using System.Diagnostics;

namespace WebApplication1.Controllers
{
    public class RouteTestController : Controller
    {
        // GET: RouteTest
        public ActionResult Index()
        {
            var result = Study.Models.RouteTest.TempProducts.getAllProducts();
            return View(result);
        }
        public ActionResult Index2(string id)
        {
            return Content(string.Format("這是{0}",id));
        }
        public ActionResult Index3(string id ,string page)
        {
            return Content(string.Format("這是{0},這是{1}頁", id,page));
        }
        public ActionResult UploadZip()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                var path = Server.MapPath("~/freezip");
                //若該資料夾不存在，則新增一個
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                path = Path.Combine(path, fileName);
                file.SaveAs(path);
            }
            return RedirectToAction("UploadZip","RouteTest");
        }
        public ActionResult unzip()
        {
            String the_rar;
            RegistryKey the_Reg;
            Object the_Obj;
            String the_Info;
            ProcessStartInfo the_StartInfo;
            Process the_Process;
            string strzipPath = Path.Combine(HostingEnvironment.MapPath("~/freezip/JTI.zip"));
            string strtxtPath = Path.Combine(HostingEnvironment.MapPath("/JTI"));
            try
            {
                the_Reg = Registry.ClassesRoot.OpenSubKey("WinRAR\\shell\\open\\command");
                the_Obj = the_Reg.GetValue("");
                the_rar = the_Obj.ToString();
                the_rar = the_Obj.ToString();
                the_rar = the_rar.Substring(1, the_rar.Length - 7);
                the_Info = " X " + strzipPath + " " + strtxtPath;
                the_StartInfo = new ProcessStartInfo();
                the_StartInfo.FileName = the_rar;
                the_StartInfo.Arguments = the_Info;
                the_StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                the_Process = new Process();
                the_Process.StartInfo = the_StartInfo;
                the_Process.Start();
                return Content("<script>alert('解壓縮成功');  window.location.href ='UploadZip' </script>");

            }
            catch (Exception EX)
            {
                return Content("<script>alert('" + EX + "');</script>");
            }
        }
    }
}