using Social.Helpers;
using Social.Interfaces;
using Social.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace Social.Services
{
    public class PostService : IPost
    {
        private YeauplatoContext context;

        public PostService(YeauplatoContext context)
        {
            this.context = context;
        }

        public int CountPost(User user)
        {
            var articles = context.Articles.Where(art => art.UserId == user.Id);
            return articles?.Count() ?? 0;
        }

        public Article GetPost(int articleid)
        {
            var article = context.Articles.FirstOrDefault(a => a.Id == articleid);

            return article;
        }
        public List<string> GetPost()
        {
            var articles = context.Articles.OrderByDescending(ar => ar.Time).Take(2);
            var postList = new List<string>();
            foreach (var article in articles)
            {
                var title = !string.IsNullOrWhiteSpace(article.Heading) ? $"<h2><a href='blog-single-default.html'>{article.Heading}</a></h2>" : "";
                var image = "";

                var artImage = context.Images.FirstOrDefault(img => img.Id == context.ArticleImages.FirstOrDefault(art => art.ArticleId == article.Id).ImageId);
                if (artImage != null)
                {
                    image = $@"
                    <!-- IMAGE -->
                    <figure class='margin-bottom-20'>
	                    <img class='img-responsive' src='{artImage.Url}' alt=''>
                    </figure>";
                }
                var author = context.Users.FirstOrDefault(a => a.Id == article.UserId);
                var comments = context.Comments.Where(cmt => cmt.ElementId == article.Id).ToList();
                var d = "<div class='panel panel-success'><div class='panel-body'><div class='blog-post-item'>";
                d += $@"
                {image}{title}
                <ul class='blog-post-info list-inline'>
		                <li>
			                <a href='#'>
				                <i class='fa fa-clock-o'></i> 
				                <span class='font-lato'>{Support.GetRelativetime(article.Time)}</span>
			                </a>
		                </li>
		                <li>
			                <a href='#'>
				                <i class='fa fa-comment-o'></i> 
				                <span class='font-lato'>{comments.Count} Comment(s)</span>
			                </a>
		                </li>
		                <li>
			                <a href='#'>
				                <i class='fa fa-user'></i> 
				                <span class='font-lato'>{author.FirstName} {author.OtherNames}</span>
			                </a>
		                </li>
	                </ul>";

                d += $"<p>{(article.Details.Length > 150 ? article.Details.Remove(149) + "...": article.Details)}</p>";
                //if (article.Details.Length > 150)
                {
                    d += $@"
	                <a href='home/post/{article.Id}' class='btn btn-reveal btn-default'>
		                <i class='fa fa-plus'></i>
		                <span>Read More</span>
	                </a>";
                }
                d += "</div></div></div>";

                postList.Add(d);
            }

            return postList;
        }

        public List<string> GetPosts(int page, int count, User user)
        {
            var myNetwork = context.Networks.Where(n => n.InitiatorId == user.Id || n.AcceptorId == user.Id);

            var articles = context.Articles
                .Where(x => myNetwork.Any(n=>n.AcceptorId == x.UserId || n.InitiatorId == x.UserId)).ToList()
                .OrderByDescending(ar => ar.Time)                    
                .Skip(page * count)
                .Take(count);
            var postList = new List<string>();
            foreach (var article in articles)
            {
                var d = ShowPost(article);
                postList.Add(d);
            }

            return postList;
        }
        public string SavePost(string post, User user)
        {
            if (string.IsNullOrEmpty(post)) { return ""; }
            var article = new Article
            {
                Id = Support.GetID(),
                UserId = user.Id,
                Details = post,
                Heading = null,
                Time = DateTime.UtcNow
            };

            context.Articles.Add(article);
            context.SaveChanges();
            return Support.ShowMessage("Saved!", Support.MessageType.SUCCESS);
        }

        public string SavePost(Posting post, User user)
        {
            try
            {
                var _post = new Article
                {
                    Details = post.Body,
                    Heading = post.Heading,
                    UserId = user.Id,
                    Id = Support.GetID(),
                    Time = DateTime.UtcNow
                };
                if (post.File != null)
                {
                    var filename = Path.GetFileNameWithoutExtension(post.File.FileName);
                    var extension = Path.GetExtension(post.File.FileName);
                    filename = $"{Support.GetMD5(Support.Encode(Support.GetID()))}{extension}";
                    var pathfilename = Path.Combine(HostingEnvironment.MapPath($"~/assets/resources/post/"), filename);
                    post.File.SaveAs(pathfilename);
                    var image = new Image
                    {
                        Caption = "",
                        Id = Support.GetID(),
                        Url = "../assets/resources/post/" + filename,
                        Time = DateTime.UtcNow
                    };
                    var postImage = new ArticleImage
                    {
                        ArticleId = _post.Id,
                        Id = Support.GetID(),
                        ImageId = image.Id
                    };
                    context.Images.Add(image);
                    context.ArticleImages.Add(postImage);
                }
                context.Articles.Add(_post);
                context.SaveChanges();
                return Support.ShowMessage("Post saved successfully", Support.MessageType.SUCCESS);
            }
            catch (Exception ex)
            {
                return Support.ShowMessage("Could not save this post!", Support.MessageType.ERROR);
            }
        }

        public string ShowPost(Article article)
        {
            var user = context.Users.FirstOrDefault(u => u.Id == article.UserId);
            var comments = context.Comments.Where(cmt => cmt.ElementId == article.Id);
            var image = "";

            var artImage = context.Images.FirstOrDefault(img => img.Id == context.ArticleImages.FirstOrDefault(art => art.ArticleId == article.Id).ImageId);
            if (artImage != null)
            {
                image = $@"<img class='pull-left img-responsive width-200' src='{artImage.Url}' alt='{article?.Heading}'>";
            }

            var d = $@"
            <div class='panel panel-default'>
                <div class='panel-body'>
                    <p class='text-primary'><i class='fa fa-user'></i> {user.FirstName} {user.OtherNames} posted</p>
                    <h4><a href='home/post/{Support.Encode(article.Id)}'>{article?.Heading}</a></h4>
                    <ul class='blog-post-info list-inline'>
                        <li>
                            <a href='home/post/{Support.Encode(article.Id)}'>
                                <i class='fa fa-clock-o'></i>
                                <span class='font-lato'>{Support.GetRelativetime(article.Time)}</span>
                            </a>
                        </li>
                        <li>
                            <a href='home/post/{Support.Encode(article.Id)}'>
                                <i class='fa fa-comment-o'></i>
                                <span class='font-lato'><span id='comment_counter_{Support.Encode(article.Id)}'>{comments.Count()}</span> Comments</span>
                            </a>
                        </li>
                        <li>
                            <a href='home/post/{Support.Encode(article.Id)}'>
                                <i class='fa fa-thumbs-o-up'></i>
                                <span class='font-lato'>1</span>
                            </a>
                        </li>
                        <li>
                            <a href='home/post/{Support.Encode(article.Id)}'>
                                <i class='fa fa-thumbs-o-down'></i>
                                <span class='font-lato'>0</span>
                            </a>
                        </li>
                    </ul>

                    {image}
                    <p>
                        {(article.Details.Length > 200 ? article.Details.Remove(199)+" ...": article.Details)}
                    </p>
                    <hr/>
                    {ChatBox(user, comments.OrderBy(cmt=>cmt.Time).ToList(), article.Id)}
                    <div class='input-group'>
                        <input name='post_{Support.Encode(article.Id)}' type='text' id='commentbox_{Support.Encode(article.Id)}' class='form-control comment required' placeholder='comment'>
                        <span class='input-group-btn'>
                            <input type='button' name='' value='SEND' id='post_{Support.Encode(article.Id)}' class='btn btn-success' disabled='disabled'>
                        </span>
                    </div>
                </div>
            </div>";
            return d;
        }
        string ChatBox(User user, List<Comment> comments, int artID)
        {
            var chats = "";
            var image = "";
            foreach (var comment in comments)
            {
                if(comment.Content is null) { continue; }
                var side = comment.UserId == user.Id ? "right" : "left";
                var profileImage = context.Images.FirstOrDefault(img => img.Id == context.UserImages.FirstOrDefault(umg=>umg.UserId == comment.UserId).ImageId);
                if (profileImage != null)
                {
                    image = $@"<img class='pull-{side} img-responsive' width='60' height='60' src='{profileImage.Url}' alt='{user.FirstName} {user.OtherNames}'>";
                }
                else
                {
                    image = $"<img class='thumbnail pull-{side}' src='assets/images/demo/people/300x300/b-min.jpg' width='60' height='60' alt='' />";
                }
                chats += $@"
                <div class='clearfix margin-bottom-10'>
                    <!-- post item -->
                    {image}
                    <h4 class='size-13 nomargin noborder nopadding'><a href='../post/{Support.Encode(comment.ElementId)}'>{(comment?.Content.Length > 100? comment?.Content.Remove(99)+" ...": comment?.Content ?? "")}</a></h4>
                    <span class='size-11 text-muted'>{Support.GetRelativetime(comment.Time)}</span>
                </div><!-- /post item -->";
            }
            var d = $@"
            <div class='panel panel-default'>
                <div class='panel-body'>
                    <div class='box-inner' style='{(chats == ""? "display:none;":"")}'>
                        <div class='height-150 slimscroll' data-always-visible='true' data-size='10px' data-position='right' data-opacity='0.4' disable-body-scroll='true' id='comment_box_{Support.Encode(artID)}'>{chats}
                        </div>
                    </div>
                </div>
            </div>";
            return d;
        }
    }
}