using Social.Helpers;
using Social.Interfaces;
using Social.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Social.Services
{
    public class CommentService : IComment
    {
        private YeauplatoContext context;
        public CommentService(YeauplatoContext context)
        {
            this.context = context;
        }

        public int CounComment(int id)
        {
            var cmtResponse = context.Comments.Where(cmt => cmt.ElementId == id);
            if (cmtResponse is null) { return 0; }
            return cmtResponse.Count();
        }

        public List<Comment> GetComments(int id)
        {
            var commentList = new List<Comment>();
            var comments = context.Comments.Where(cmt => cmt.ElementId == id).OrderBy(cmt => cmt.Time).ToList();

            if (comments != null) commentList = comments;

            return commentList;
        }

        public string LatestComment(int id, User user)
        {
            var comment = context.Comments.OrderByDescending(cmt=>cmt.Time).FirstOrDefault(cmt => cmt.ElementId == id);

            if (comment is null) { return ""; }
            var image = "";
            var side = comment.UserId == user.Id ? "right" : "left";
            var profileImage = context.Images.FirstOrDefault(img => img.Id == context.UserImages.FirstOrDefault(umg => umg.UserId == comment.UserId).ImageId);
            if (profileImage != null)
            {
                image = $@"<img class='pull-{side} img-responsive' width='60' height='60' src='{profileImage.Url}' alt='{user.FirstName} {user.OtherNames}'>";
            }
            else
            {
                image = $"<img class='thumbnail pull-{side}' src='assets/images/demo/people/300x300/b-min.jpg' width='60' height='60' alt='' />";
            }
            var chats = $@"
                <div class='clearfix margin-bottom-10'>
                    <!-- post item -->
                    {image}
                    <h4 class='size-13 nomargin noborder nopadding'><a href='../post/{Support.Encode(comment.ElementId)}'>{(comment?.Content.Length > 100 ? comment?.Content.Remove(99) + " ..." : comment?.Content ?? "")}</a></h4>
                    <span class='size-11 text-muted'>{Support.GetRelativetime(comment.Time)}</span>
                </div><!-- /post item -->";


            return chats;
        }

        public bool SaveComment(string content, int elementId, User user)
        {
            var check = false;
            var comment = new Comment
            {
                Content = content,
                ElementId = elementId,
                Id = Support.GetID(),
                Time = DateTime.UtcNow,
                Type = "post",
                UserId = user.Id
            };
            try
            {
                context.Comments.Add(comment);
                context.SaveChanges();
                check = true;
            }
            catch (Exception) { }
            return check;
        }
    }
}