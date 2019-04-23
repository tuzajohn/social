using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Social.Helpers;
using Social.Interfaces;
using Social.Models;

namespace Social.Services
{
    public class TimeLineService : ITimeLine
    {
        private YeauplatoContext context;
        public TimeLineService()
        {
            context = new YeauplatoContext();
        }
        public List<string> GetTimeLines(int page, int count, User user)
        {
            var articles = context.Articles
                .Where(x => x.UserId == user.Id).ToList()
                .OrderByDescending(ar => ar.Time)
                .Skip(page * count)
                .Take(count);
            var timelines = new List<string>();
            foreach (var article in articles)
            {
                var d = ShowTimeLinePost(article);
                timelines.Add(d);
            }

            return timelines;
        }

        public string ShowTimeLinePost(Article article)
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
                    <h4><a href='../home/post/{Support.Encode(article.Id)}'>{article?.Heading}</a></h4>
                    <ul class='blog-post-info list-inline'>
                        <li>
                            <a href='../home/post/{Support.Encode(article.Id)}'>
                                <i class='fa fa-clock-o'></i>
                                <span class='font-lato'>{Support.GetRelativetime(article.Time)}</span>
                            </a>
                        </li>
                        <li>
                            <a href='../home/post/{Support.Encode(article.Id)}'>
                                <i class='fa fa-comment-o'></i>
                                <span class='font-lato'>0 Comments</span>
                            </a>
                        </li>
                        <li>
                            <a href='../home/post/{Support.Encode(article.Id)}'>
                                <i class='fa fa-thumbs-o-up'></i>
                                <span class='font-lato'>1</span>
                            </a>
                        </li>
                        <li>
                            <a href='../home/post/{Support.Encode(article.Id)}'>
                                <i class='fa fa-thumbs-o-down'></i>
                                <span class='font-lato'>0</span>
                            </a>
                        </li>
                    </ul>

                    {image}
                    <p>
                        {(article.Details.Length > 200 ? article.Details.Remove(199) + " ..." : article.Details)}
                    </p>
                    <hr/>
                    <div style='display: none; padding-top:10px;' id='art12'>
                        <div class='room'></div>
                    </div>
                    <div class='input-group'>
                        <input name='post_{Support.Encode(article.Id)}' type='text' class='form-control required' placeholder='comment'>
                        <span class='input-group-btn'>
                            <input type='button' name='' value='SEND' id='post_{Support.Encode(article.Id)}' class='btn btn-success' disabled='disabled'>
                        </span>
                    </div>
                </div>
            </div>";
            return d;
        }
    }
}