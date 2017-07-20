
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Runtasker.Logic.Entities
{
    public enum NotificationAboutType
    {
        Ordinary, Balance
    }

    public enum NotificationType
    {
        [Display(Name = "alert-success")]
        Success,
        [Display(Name = "alert-info")]
        Info,
        [Display(Name = "alert-warning")]
        Warning,
        [Display(Name = "alert-danger")]
        Danger
    }

    public enum NotificationStatus
    {
        Unseen, Seen
    }

    


    public class Notification
    {
        #region Constructors

        public Notification()
        {
            DefaultSettings();
        }
        
        void DefaultSettings()
        {
            Id = Guid.NewGuid().ToString();
            Status = NotificationStatus.Unseen;
        }

        #endregion

        #region Properties
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Id { get; set; }

        //[ForeignKey("User")]
        public string UserGuid { get; set; }
        //public virtual ApplicationUser User { get; set; }

        public NotificationType Type { get; set; }

        public NotificationStatus Status { get; set; }

        public NotificationAboutType AboutType { get; set; }

        public string Title { get; set; }

        public string Text { get; set; }

        //Prepared HtmlLink
        public string Link { get; set; }
        #endregion
    }
}
