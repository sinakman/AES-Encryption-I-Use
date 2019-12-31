using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAL.ORM.Entity;
using Service.Option;
using System.Web.Mvc;
using UICRM.Models;
using System.Web.Security;


namespace UICRM.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        EmployeeService _employeeSevice;
        public AccountController()
        {
            _employeeSevice = new EmployeeService();
        }



        public ActionResult Login()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return Redirect("/CRMarea/Area/Index");
            }
            TempData["class"] = "custom-hide";
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginVM loginVM)
        {
            if (ModelState.IsValid)
            {
                //encrypt password when is complete
                string pass = PassCrypto.base64Encode(loginVM.PasSword);
                if (_employeeSevice.CheckCredentials(loginVM.UserName, pass))
                {
                    Employee currentUser = _employeeSevice.FindByUserName(loginVM.UserName);
                    if (currentUser.Password == pass)
                    {

                    }

                    string cookie = currentUser.UserName;
                    int idm = currentUser.ID;
                    TempData["idm"] = idm;
                    Session["admin"] = currentUser.UserName;
                    FormsAuthentication.SetAuthCookie(cookie, true);
                    TempData["name1"] = currentUser.Name;
                    //TempData["mail"] = currentUser.Email;
                    return Redirect("/CRMarea/Area/Index");

                }
                else
                {
                    ViewData["error"] = "Warning!";
                    return View();
                }
            }
            else
            {
                TempData["class"] = "custom-show";
                return View();
            }
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return Redirect("/Account/Login");
        }


    }
}