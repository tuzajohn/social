using Newtonsoft.Json;
using Social.Helpers;
using Social.Models;
using Social.Services;
using Social.ViewModels;
using System.Linq;
using System.Web.Mvc;

namespace Social.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        private RegisterService _registerService;
        private LoginService _loginService;
        private YeauplatoContext context = new YeauplatoContext();

        public AccountController()
        {
            _registerService = new RegisterService(context);
            _loginService = new LoginService(context);
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login(UserViewModel user)
        {
            var (check, message) = _loginService.CheckUser(user, out User _user);
            if (!check)
            {
                return Json(new { check, message });
            }

            Session["user"] = JsonConvert.SerializeObject(_user);
            var url = Request.Url.AbsoluteUri.Remove(Request.Url.AbsoluteUri.ToLower().IndexOf("account"));
            return Json(new { check = true, url});
        }
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }
        public ActionResult NewUser(UserViewModel user)
        {
            var (check, message) = _registerService.CheckUser(user);
            if (!check)
            {
                return Json(new { check, message });
            }
            message = _registerService.AddNewUser(user);
            return Json(new { check = true, message });
        }
        public ActionResult Register(string d)
        {
            if (!string.IsNullOrWhiteSpace(d))
            {
                var _user = d.ToDecryptedJsonObj<Register>(Support.Key);
                var isAlreadyIn = context.Users.FirstOrDefault(u => u.Email == _user.Email);
                if (isAlreadyIn != null)
                {
                    return View("Index");
                }
                Session["new"] = _user;
                return View(_user);
            }
            return View();
        }
        private void RegisterClaims(User user)
        {
            
        }
    }
    
}