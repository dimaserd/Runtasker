
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Runtasker.Logic.Entities
{
    public enum NotificationAboutType
    {
        Ordinary, Balance
    }

    public enum NotificationType
    {
        [Description("alert-success")]
        Success,
        [Description("alert-info")]
        Info,
        [Description("alert-warning")]
        Warning,
        [Description("alert-danger")]
        Danger
    }

    public enum NotificationStatus
    {
        Unseen, Seen
    }

    public static class NotificationEnumExtensions
    {
        public static string ToDescriptionString(this NotificationType val)
        {
            DescriptionAttribute[] attributes = (DescriptionAttribute[])val.GetType().GetField(val.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : string.Empty;
        }
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
