using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Social.Helpers;
using Social.Interfaces;
using Social.Models;
using Social.Services;
using Social.ViewModels;

namespace Social.Controllers
{
    public class ChatController : Controller
    {
        private IUser _userService;
        private INetwork _myNetworkService;
        private IChat _chatService;
        public ChatController()
        {
            _userService = new UserService();
            _myNetworkService = new MyNetworkService();
            _chatService = new ChatService();
        }
        public ActionResult Index(string id = "")
        {
            if (Session["user"] is null) { return RedirectToAction("Index", "Account"); }
            var user = Session["user"].ToString().FromJson<User>();
            if (id != "")
            {
                try
                {
                    var _id = Support.Decode(id);
                    var isInNetowrk = _myNetworkService.IsInMyNetwork(_id);
                    if (isInNetowrk)
                    {
                        var _user = _userService.GetUser(Support.Decode(id));
                        ViewBag.User = _user;
                    }
                }
                catch (Exception) { }
            }
            return View();
        }
        public ActionResult SendMessage(ChatViewModel model)
        {
            if (Session["user"] is null) { return RedirectToAction("Index", "Account"); }
            var user = Session["user"].ToString().FromJson<User>();
            int id = 0;
            try { id = Support.Decode(model.Id); }
            catch (Exception) { return Json(new { check = false, message = "" }); }

            var response = _chatService.SendMessage(user, id, model.Content);
            if (!response) { return Json(new { check = false, message = "" }); }

            var newMsg = _chatService.LaodLatesMessage(user, id);

            return Json(new { check = true, el = newMsg });
        }
        public ActionResult LoadMessages(ChatViewModel model)
        {
            if (Session["user"] is null) { return RedirectToAction("Index", "Account"); }
            var user = Session["user"].ToString().FromJson<User>();
            int id = 0;
            try { id = Support.Decode(model.Id); }
            catch (Exception) { return Json(new { check = false, message = "" }); }
            var _messages = _chatService.LaodMessages(user, id);
            return Json(new { check = true, el = _messages });
        }
        public ActionResult CountAllMessages()
        {
            if (Session["user"] is null) { return RedirectToAction("Index", "Account"); }
            var user = Session["user"].ToString().FromJson<User>();
            var count = _chatService.CountNewMessages(user);
            return Json(new { count });
        }
        public ActionResult Notifications()
        {
            if (Session["user"] is null) { return RedirectToAction("Index", "Account"); }
            var user = Session["user"].ToString().FromJson<User>();
            var response = _chatService.Notification(user);
            return Json(new { el = response, check = !response.Any() });
        }
    }
}