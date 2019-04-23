namespace Social.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ArticleImages",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        ArticleId = c.Int(nullable: false),
                        ImageId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Articles",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Heading = c.String(),
                        Details = c.String(),
                        Time = c.DateTime(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.BlogPosts",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Details = c.String(),
                        Time = c.DateTime(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CommentReads",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        CommentId = c.Int(nullable: false),
                        IsRead = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        Type = c.String(),
                        Content = c.String(),
                        Time = c.DateTime(nullable: false),
                        ElementId = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ConfirmAccounts",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Paramters = c.String(),
                        User_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Users", t => t.User_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        FirstName = c.String(),
                        OtherNames = c.String(),
                        Email = c.String(),
                        Gender = c.String(),
                        Dob = c.DateTime(nullable: false),
                        Country = c.String(),
                        Password = c.String(),
                        Time = c.DateTime(nullable: false),
                        Type = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ConnectionRequests",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        SenderId = c.Int(nullable: false),
                        ReceiverId = c.Int(nullable: false),
                        Time = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CoverPictures",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        ImageId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        _time = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Images",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        Url = c.String(),
                        Caption = c.String(),
                        Time = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Inboxes",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        MessageId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        SourceId = c.Int(nullable: false),
                        DestinationId = c.Int(nullable: false),
                        Details = c.String(),
                        Time = c.DateTime(nullable: false),
                        IsRead = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Networks",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        InitiatorId = c.Int(nullable: false),
                        AcceptorId = c.Int(nullable: false),
                        Time = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Outboxes",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        MessageId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SpeakUps",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        Details = c.String(),
                        Time = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Testimonies",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        _details = c.String(),
                        Time = c.DateTime(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserDisableds",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserImages",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        ImageId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserProfiles",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        Mobile = c.String(),
                        Interest = c.String(),
                        Occupation = c.String(),
                        About = c.String(),
                        Language = c.String(),
                        Address = c.String(),
                        Education = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Videos",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        Caption = c.String(),
                        Time = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Votes",
                c => new
                    {
                        Id = c.Int(nullable: false),
                        ElementId = c.String(),
                        Type = c.String(),
                        Voting = c.String(),
                        CommentId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        Time = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ConfirmAccounts", "User_Id", "dbo.Users");
            DropIndex("dbo.ConfirmAccounts", new[] { "User_Id" });
            DropTable("dbo.Votes");
            DropTable("dbo.Videos");
            DropTable("dbo.UserProfiles");
            DropTable("dbo.UserImages");
            DropTable("dbo.UserDisableds");
            DropTable("dbo.Testimonies");
            DropTable("dbo.SpeakUps");
            DropTable("dbo.Outboxes");
            DropTable("dbo.Networks");
            DropTable("dbo.Messages");
            DropTable("dbo.Inboxes");
            DropTable("dbo.Images");
            DropTable("dbo.CoverPictures");
            DropTable("dbo.ConnectionRequests");
            DropTable("dbo.Users");
            DropTable("dbo.ConfirmAccounts");
            DropTable("dbo.Comments");
            DropTable("dbo.CommentReads");
            DropTable("dbo.BlogPosts");
            DropTable("dbo.Articles");
            DropTable("dbo.ArticleImages");
        }
    }
}
