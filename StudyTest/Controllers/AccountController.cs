using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Study.Services;
using Study.Models.Models;
using Study.DataAccess;

namespace StudyTEST.Controllers
{
    public class AccountController : Controller
    {
        SqlRepository _sqlRepository = new SqlRepository();
        AccountServices _accountServices = new AccountServices();
        // GET: Account
        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult RegisterAdd(string Email,string Psw)
        {
            List<member> m = new List<member>();
            m.Add(new member { email = Email, password = Psw });
            _sqlRepository.insertMember(m);
            return View("Register");
        }
        public ActionResult Forgotpassword()
        {
            return View();
        }
        
    }
}