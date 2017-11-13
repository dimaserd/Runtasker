namespace Runtasker.Logic.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ArticleClarifications",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Header = c.String(),
                        Text = c.String(),
                        ArticleId = c.String(nullable: false, maxLength: 128),
                        LanguageCode = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Articles", t => t.ArticleId, cascadeDelete: true)
                .Index(t => t.ArticleId);
            
            CreateTable(
                "dbo.Articles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        ToShow = c.Boolean(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        ImageCaptionHtml = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Attachments",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FilePath = c.String(),
                        FileName = c.String(),
                        FileData = c.Binary(),
                        FileMymeType = c.String(),
                        Type = c.Int(nullable: false),
                        MessageId = c.Int(),
                        OrderId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Orders", t => t.OrderId)
                .ForeignKey("dbo.Messages", t => t.MessageId)
                .Index(t => t.MessageId)
                .Index(t => t.OrderId);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Status = c.Int(nullable: false),
                        LastStatus = c.Int(nullable: false),
                        ErrorType = c.Int(nullable: false),
                        WorkType = c.Int(nullable: false),
                        HasCustomerFiles = c.Boolean(nullable: false),
                        UserGuid = c.String(nullable: false, maxLength: 128),
                        PerformerGuid = c.String(nullable: false, maxLength: 128),
                        Subject = c.Int(nullable: false),
                        OtherSubject = c.String(),
                        Description = c.String(nullable: false, maxLength: 500),
                        FinishDate = c.DateTime(nullable: false),
                        PublishDate = c.DateTime(nullable: false),
                        PaidSum = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Sum = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Rating = c.Int(nullable: false),
                        Comment = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserGuid, cascadeDelete: false)
                .ForeignKey("dbo.AspNetUsers", t => t.PerformerGuid, cascadeDelete: false)
                .Index(t => t.UserGuid)
                .Index(t => t.PerformerGuid);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        VkDomain = c.String(),
                        VkId = c.String(),
                        ShouldBeNotifictedInVk = c.Boolean(nullable: false),
                        Specialization = c.String(),
                        Language = c.String(),
                        Name = c.String(),
                        Balance = c.Decimal(nullable: false, precision: 18, scale: 2),
                        RegistrationDate = c.DateTime(),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Coupons",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        Count = c.Int(nullable: false),
                        Type = c.Int(nullable: false),
                        BonusMoney = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Payments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ViaType = c.Int(nullable: false),
                        UserGuid = c.String(maxLength: 128),
                        Date = c.DateTime(nullable: false),
                        Description = c.String(),
                        Hash = c.String(),
                        PaymentServiceId = c.String(),
                        Confirmed = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserGuid)
                .Index(t => t.UserGuid);
            
            CreateTable(
                "dbo.PaymentTransactions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.Int(nullable: false),
                        UserGuid = c.String(maxLength: 128),
                        Sum = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Description = c.String(),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserGuid)
                .Index(t => t.UserGuid);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SenderId = c.String(nullable: false, maxLength: 128),
                        ReceiverId = c.String(nullable: false, maxLength: 128),
                        Status = c.Int(nullable: false),
                        Type = c.Int(nullable: false),
                        Mark = c.String(),
                        Text = c.String(maxLength: 512),
                        Date = c.DateTime(nullable: false),
                        AttachmentId = c.String(),
                        OrderId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Orders", t => t.OrderId)
                .ForeignKey("dbo.AspNetUsers", t => t.ReceiverId, cascadeDelete: false)
                .ForeignKey("dbo.AspNetUsers", t => t.SenderId, cascadeDelete: false)
                .Index(t => t.SenderId)
                .Index(t => t.ReceiverId)
                .Index(t => t.OrderId);
            
            CreateTable(
                "dbo.Clicks",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        PreviousUrl = c.String(),
                        Info = c.String(),
                        ClickDate = c.DateTime(nullable: false),
                        CountingClickLinkId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CountingClickLinks", t => t.CountingClickLinkId)
                .Index(t => t.CountingClickLinkId);
            
            CreateTable(
                "dbo.CountingClickLinks",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        ClickName = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Invitations",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Status = c.Int(nullable: false),
                        SenderGuid = c.String(),
                        ReceiverEmail = c.String(),
                        ReceiverGuid = c.String(),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Notifications",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        UserGuid = c.String(),
                        Type = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        AboutType = c.Int(nullable: false),
                        Title = c.String(),
                        Text = c.String(),
                        CreationDate = c.DateTime(nullable: false),
                        Link = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.QuestionAnswerLangClarifications",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Question = c.String(),
                        Answer = c.String(),
                        IsVisible = c.Boolean(nullable: false),
                        LanguageCode = c.Int(nullable: false),
                        QuestionAnswer_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.QuestionAnswers", t => t.QuestionAnswer_Id)
                .Index(t => t.QuestionAnswer_Id);
            
            CreateTable(
                "dbo.QuestionAnswers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ResourceFileModels",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        ResourcePath = c.String(),
                        CreateDate = c.DateTime(nullable: false),
                        LangCode = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ResourceStrings",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        ResourceKey = c.String(),
                        ResourceValue = c.String(),
                        LastEditedDate = c.DateTime(nullable: false),
                        ResourceFileId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ResourceFileModels", t => t.ResourceFileId)
                .Index(t => t.ResourceFileId);
            
            CreateTable(
                "dbo.ResourceStringTypes",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        ResourceValue = c.String(),
                        LangCode = c.Int(nullable: false),
                        ResourceStringId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ResourceStrings", t => t.ResourceStringId)
                .Index(t => t.ResourceStringId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.VkFoundPosts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PostIdInGroup = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        PublishDate = c.DateTime(nullable: false),
                        VkGroupId = c.Int(nullable: false),
                        Text = c.String(),
                        VkLink = c.String(),
                        PostOwnerId = c.String(),
                        FoundKeyWords = c.String(),
                        Subject = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.VkGroups", t => t.VkGroupId, cascadeDelete: true)
                .Index(t => t.VkGroupId);
            
            CreateTable(
                "dbo.VkGroups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ScreenName = c.String(),
                        Name = c.String(),
                        LastCheckDate = c.DateTime(nullable: false),
                        LastCheckedPostId = c.Int(nullable: false),
                        GroupId = c.Int(nullable: false),
                        IsMember = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.VkGroupMembers",
                c => new
                    {
                        VkManId = c.String(nullable: false, maxLength: 128),
                        VkGroupId = c.String(nullable: false, maxLength: 128),
                        Group_Id = c.Int(),
                    })
                .PrimaryKey(t => new { t.VkManId, t.VkGroupId })
                .ForeignKey("dbo.VkGroups", t => t.Group_Id)
                .ForeignKey("dbo.VkMen", t => t.VkManId, cascadeDelete: false)
                .Index(t => t.VkManId)
                .Index(t => t.Group_Id);
            
            CreateTable(
                "dbo.VkMen",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        VkId = c.Int(nullable: false),
                        VkLink = c.String(),
                        IsInformed = c.Boolean(nullable: false),
                        FirstName = c.String(),
                        LastName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.VkPostLookUps",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Count = c.Int(nullable: false),
                        VkFoundPostId = c.Int(nullable: false),
                        VkPerformerGuid = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.VkFoundPosts", t => t.VkFoundPostId, cascadeDelete: false)
                .Index(t => t.VkFoundPostId);
            
            CreateTable(
                "dbo.VkKeyWords",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MainForm = c.String(),
                        OtherWordForms = c.String(),
                        Subject = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserCoupons",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        CouponId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.CouponId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: false)
                .ForeignKey("dbo.Coupons", t => t.CouponId, cascadeDelete: false)
                .Index(t => t.UserId)
                .Index(t => t.CouponId);
            
            CreateTable(
                "dbo.VkKeyWordFoundPosts",
                c => new
                    {
                        VkKeyWordId = c.Int(nullable: false),
                        VkFoundPostId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.VkKeyWordId, t.VkFoundPostId })
                .ForeignKey("dbo.VkKeyWords", t => t.VkKeyWordId, cascadeDelete: false)
                .ForeignKey("dbo.VkFoundPosts", t => t.VkFoundPostId, cascadeDelete: false)
                .Index(t => t.VkKeyWordId)
                .Index(t => t.VkFoundPostId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.VkKeyWordFoundPosts", "VkFoundPostId", "dbo.VkFoundPosts");
            DropForeignKey("dbo.VkKeyWordFoundPosts", "VkKeyWordId", "dbo.VkKeyWords");
            DropForeignKey("dbo.VkPostLookUps", "VkFoundPostId", "dbo.VkFoundPosts");
            DropForeignKey("dbo.VkFoundPosts", "VkGroupId", "dbo.VkGroups");
            DropForeignKey("dbo.VkGroupMembers", "VkManId", "dbo.VkMen");
            DropForeignKey("dbo.VkGroupMembers", "Group_Id", "dbo.VkGroups");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.ResourceStringTypes", "ResourceStringId", "dbo.ResourceStrings");
            DropForeignKey("dbo.ResourceStrings", "ResourceFileId", "dbo.ResourceFileModels");
            DropForeignKey("dbo.QuestionAnswerLangClarifications", "QuestionAnswer_Id", "dbo.QuestionAnswers");
            DropForeignKey("dbo.Clicks", "CountingClickLinkId", "dbo.CountingClickLinks");
            DropForeignKey("dbo.Attachments", "MessageId", "dbo.Messages");
            DropForeignKey("dbo.Attachments", "OrderId", "dbo.Orders");
            DropForeignKey("dbo.Orders", "PerformerGuid", "dbo.AspNetUsers");
            DropForeignKey("dbo.Messages", "SenderId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Messages", "ReceiverId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Messages", "OrderId", "dbo.Orders");
            DropForeignKey("dbo.Orders", "UserGuid", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.PaymentTransactions", "UserGuid", "dbo.AspNetUsers");
            DropForeignKey("dbo.Payments", "UserGuid", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserCoupons", "CouponId", "dbo.Coupons");
            DropForeignKey("dbo.UserCoupons", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.ArticleClarifications", "ArticleId", "dbo.Articles");
            DropIndex("dbo.VkKeyWordFoundPosts", new[] { "VkFoundPostId" });
            DropIndex("dbo.VkKeyWordFoundPosts", new[] { "VkKeyWordId" });
            DropIndex("dbo.UserCoupons", new[] { "CouponId" });
            DropIndex("dbo.UserCoupons", new[] { "UserId" });
            DropIndex("dbo.VkPostLookUps", new[] { "VkFoundPostId" });
            DropIndex("dbo.VkGroupMembers", new[] { "Group_Id" });
            DropIndex("dbo.VkGroupMembers", new[] { "VkManId" });
            DropIndex("dbo.VkFoundPosts", new[] { "VkGroupId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.ResourceStringTypes", new[] { "ResourceStringId" });
            DropIndex("dbo.ResourceStrings", new[] { "ResourceFileId" });
            DropIndex("dbo.QuestionAnswerLangClarifications", new[] { "QuestionAnswer_Id" });
            DropIndex("dbo.Clicks", new[] { "CountingClickLinkId" });
            DropIndex("dbo.Messages", new[] { "OrderId" });
            DropIndex("dbo.Messages", new[] { "ReceiverId" });
            DropIndex("dbo.Messages", new[] { "SenderId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.PaymentTransactions", new[] { "UserGuid" });
            DropIndex("dbo.Payments", new[] { "UserGuid" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Orders", new[] { "PerformerGuid" });
            DropIndex("dbo.Orders", new[] { "UserGuid" });
            DropIndex("dbo.Attachments", new[] { "OrderId" });
            DropIndex("dbo.Attachments", new[] { "MessageId" });
            DropIndex("dbo.ArticleClarifications", new[] { "ArticleId" });
            DropTable("dbo.VkKeyWordFoundPosts");
            DropTable("dbo.UserCoupons");
            DropTable("dbo.VkKeyWords");
            DropTable("dbo.VkPostLookUps");
            DropTable("dbo.VkMen");
            DropTable("dbo.VkGroupMembers");
            DropTable("dbo.VkGroups");
            DropTable("dbo.VkFoundPosts");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.ResourceStringTypes");
            DropTable("dbo.ResourceStrings");
            DropTable("dbo.ResourceFileModels");
            DropTable("dbo.QuestionAnswers");
            DropTable("dbo.QuestionAnswerLangClarifications");
            DropTable("dbo.Notifications");
            DropTable("dbo.Invitations");
            DropTable("dbo.CountingClickLinks");
            DropTable("dbo.Clicks");
            DropTable("dbo.Messages");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.PaymentTransactions");
            DropTable("dbo.Payments");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.Coupons");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Orders");
            DropTable("dbo.Attachments");
            DropTable("dbo.Articles");
            DropTable("dbo.ArticleClarifications");
        }
    }
}
