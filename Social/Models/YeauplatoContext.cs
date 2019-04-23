using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Social.Models
{
    public class YeauplatoContext : DbContext
    {
        public YeauplatoContext()
            : base("name=YeauplatoConnection")
        {
            //Database.SetInitializer<YeauplatoContext>(new CreateDatabaseIfNotExists<YeauplatoContext>());
            Database.SetInitializer<YeauplatoContext>(new DropCreateDatabaseIfModelChanges<YeauplatoContext>());
        }

        public virtual DbSet<Article> Articles { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<ArticleImage> ArticleImages { get; set; }
        public virtual DbSet<BlogPost> BlogPosts { get; set; }
        public virtual DbSet<CommentRead> CommentReads { get; set; }
        public virtual DbSet<ConfirmAccount> ConfirmAccounts { get; set; }
        public virtual DbSet<ConnectionRequest> ConnectionRequests { get; set; }
        public virtual DbSet<CoverPicture> CoverPictures { get; set; }
        public virtual DbSet<Image> Images { get; set; }
        public virtual DbSet<Inbox> Inboxes { get; set; }
        public virtual DbSet<Message> Messages { get; set; }
        public virtual DbSet<Network> Networks { get; set; }
        public virtual DbSet<Notification> Notifications { get; set; }
        public virtual DbSet<Outbox> Outboxes { get; set; }
        public virtual DbSet<SpeakUp> SpeakUps { get; set; }
        public virtual DbSet<Testimony> Testimonies { get; set; }
        public virtual DbSet<UserDisabled> UserDisableds { get; set; }
        public virtual DbSet<UserImage> UserImages { get; set; }
        public virtual DbSet<UserProfile> UserProfiles { get; set; }
        public virtual DbSet<Video> Videos { get; set; }
        public virtual DbSet<Vote> Votes { get; set; }
    }
}