using Newtonsoft.Json;
using Social.Helpers;
using Social.Interfaces;
using Social.Models;
using Social.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace Social.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        private IPost _postService;
        private IComment _commentService;
        private INetwork _myNetworkService;
        private ITimeLine _timeLine;
        private YeauplatoContext context = new YeauplatoContext();

        public HomeController()
        {
            _postService = new PostService(context);
            _commentService = new CommentService(context);
            _myNetworkService = new MyNetworkService();
            _timeLine = new TimeLineService();
        }
        #region ActionResult

        public ActionResult Index()
        {
            if (Session["user"] is null) { return RedirectToAction("Index", "Account"); }
            var user = Session["user"].ToString().FromJson<User>();

            if (Session["new"] != null)
            {
                var _user = (Register)Session["new"];
                var _initiator = context.Users.FirstOrDefault(u => u.Id == _user.SenderId);
                _myNetworkService.SendRequest(_initiator, _user.Email);
                Session["new"] = null;
            }
            return View();
        }
        public ActionResult Post(string id)
        {
            if (Session["user"] is null) { return RedirectToAction("Index", "Account"); }
            if (!string.IsNullOrWhiteSpace(id))
            {
                var post = _postService.GetPost(Support.Decode(id));
                ViewBag.Post = post;
                return View();
            }
            return PartialView("PostPartial");
        }
        public ActionResult CommentPartial(string id)
        {
            var comments = _commentService.GetComments(Support.Decode(id));
            return PartialView("CommentPartial", comments);
        }

        [ValidateInput(false)]
        public ActionResult SaveComment(string id, string comment_data)
        {
            var user = JsonConvert.DeserializeObject<User>(Session["user"].ToString());
            _commentService.SaveComment(comment_data, Support.Decode(id), user);
            return RedirectToAction($"Post/{id}", "Home");
        }
        [System.Web.Http.HttpPost]
        public ActionResult SavePostComment([FromBody]post post)
        {
            if (string.IsNullOrWhiteSpace(post.Data))
            {
                return Json(new { check = false });
            }
            var user = JsonConvert.DeserializeObject<User>(Session["user"].ToString());
            _commentService.SaveComment(post.Data, Support.Decode(post.Id), user);
            var count = _commentService.CounComment(Support.Decode(post.Id));
            return Json(new { check = true, count });
        }
        public ActionResult LoadLatest(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return Json(new { check = false });
            }
            var user = JsonConvert.DeserializeObject<User>(Session["user"].ToString());
            var el = _commentService.LatestComment(Support.Decode(id), user);
            return Json(new { check = true, el });
        }


        [System.Web.Http.HttpPost]
        public ActionResult PostData(Posting posting)
        {
            var user = JsonConvert.DeserializeObject<User>(Session["user"].ToString());

            var _response = _postService.SavePost(posting, user);
            var msg =new Helpers.Message
            {
                _Message = _response
            };

            return PartialView("PostPartial", msg);
        }
        public ActionResult Network()
        {
            return View();
        }

        public ActionResult Timeline()
        {

            return View();
        }



        #endregion

        #region JsonResult
        public JsonResult SavePost(string post)
        {
            var user = JsonConvert.DeserializeObject<User>(Session["user"].ToString());
            var response = _postService.SavePost(post, user);
            var check = false;
            if (response.Contains("Saved")) { check = true; }
            return Json(new { check });
        }
        public JsonResult LoadPosts(int page, int count)
        {
            var user = JsonConvert.DeserializeObject<User>(Session["user"].ToString());
            var posts = _postService.GetPosts(page, count, user);
            return Json(new { element = posts });
        }
        public JsonResult LoadTimeLine(int page, int count)
        {
            var user = JsonConvert.DeserializeObject<User>(Session["user"].ToString());
            var timelines = _timeLine.GetTimeLines(page, count, user);
            return Json(new { element = timelines });
        }
        public JsonResult SendRequest(string email)
        {
            var user = JsonConvert.DeserializeObject<User>(Session["user"].ToString());
            var (check, message) = _myNetworkService.SendRequest(user, email);
            return Json(new { check, message });
        }
        public JsonResult SendInvite(string email)
        {
            var user = JsonConvert.DeserializeObject<User>(Session["user"].ToString());
            return Json(new { });
        }
        public JsonResult CountNetwork()
        {
            var user = JsonConvert.DeserializeObject<User>(Session["user"].ToString());
            var count = _myNetworkService.CountMyNetwork(user);
            return Json(new { count });
        }
        public JsonResult CountPost()
        {
            var user = JsonConvert.DeserializeObject<User>(Session["user"].ToString());
            var count = _postService.CountPost(user);
            return Json(new { count });
        }
        public JsonResult CountNewRequest()
        {
            var user = JsonConvert.DeserializeObject<User>(Session["user"].ToString());
            var count = _myNetworkService.CountNewRequest(user);
            return Json(new { count });
        }
        public JsonResult LoadNetwork(int page, int count)
        {
            var user = JsonConvert.DeserializeObject<User>(Session["user"].ToString());
            var networkList = _myNetworkService.GetMyNetwork(user, page, count);
            return Json(new { element = networkList });
        }
        public JsonResult LoadAllRequest()
        {
            var user = JsonConvert.DeserializeObject<User>(Session["user"].ToString());
            var _reqestList = _myNetworkService.GetNewRequest(user);
            return Json(new { element = _reqestList });
        }

        public JsonResult AcceptRequest(string id)
        {
            var user = JsonConvert.DeserializeObject<User>(Session["user"].ToString());
            var check = _myNetworkService.AcceptRequest(user, id);
            return Json(new { check });
        }
        public JsonResult DeleteRequest(string id)
        {
            var user = JsonConvert.DeserializeObject<User>(Session["user"].ToString());
            var check = _myNetworkService.DeleteRequest(user, id);
            return Json(new { check });
        }
        #endregion
    }
    public class post
    {
        public string Id { get; set; }
        public string Data { get; set; }
    }
    
}