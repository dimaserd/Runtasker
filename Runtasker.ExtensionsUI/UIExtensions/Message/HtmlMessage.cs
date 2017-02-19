using Extensions.String;
using Runtasker.Logic.Entities;
using System;
using System.ComponentModel;
using System.Text;

namespace Runtasker.ExtensionsUI.UIExtensions
{
    public enum DestinationType
    {   
        [Description("left")]
        To, 
        [Description("right")]
        From
    }

    public static class DestinationExtensions
    {
        public static string ToDescriptionString(this DestinationType val)
        {
            DescriptionAttribute[] attributes = (DescriptionAttribute[])val.GetType().GetField(val.ToString()).GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : string.Empty;
        }
    }

    public class HtmlMessage
    {
        

        public HtmlMessage(Message message, string userGuid, string senderName, string receiverName)
        {
            if(message.SenderGuid == userGuid)
            {
                Destination = DestinationType.From;
                NickName = senderName;
            }
            else
            {
                Destination = DestinationType.To;
                NickName = receiverName;
            }

            this.message = message;

            Id = message.Id;
            Date = message.Date;
            Text = message.Text;
            FilesUrl = message.AttachmentId;
            Status = message.Status;
            
        }

        DestinationType Destination { get; set; }
        MessageStatus Status { get; set; }

        Message message { get; set; }
        
        #region Properties For a good life
        string NickName { get; set; }


        DateTime Date { get; set; }
        string Text { get; set; }
        string FilesUrl { get; set; }
        int Id { get; set; }
        #endregion
        //возможные косяки
        private string GetStarted()
        {
            
            return $"<li id={Id} date={(long)(Date - new DateTime(1970, 1, 1)).TotalMilliseconds} class='mes {((Status == MessageStatus.New && Destination == DestinationType.To)? "unread" : null )} {Destination.ToDescriptionString()} clearfix'>"
            + $"<span class='chat-img pull-{Destination.ToDescriptionString()}'>";
        }

        private string GetTime()
        {
            string time = "";
            if ((DateTime.Now - Date).TotalMinutes < 2)
            {
                time = "Just now";
            }
            else if ((DateTime.Now - Date).TotalHours < 1)
            {
                time = $"{Math.Round((DateTime.Now - Date).TotalMinutes)} mins ago";
            }
            else if (Date.Day != DateTime.Now.Day)
            {
                time = $"Yesterday {Date.Hour}:{Date.Minute}";
            }
            else if ((DateTime.Now - Date).TotalHours < 24)
            {
                time = $"{Math.Round((DateTime.Now - Date).TotalHours)} hours ago";
            }
            else
            {
                time = $"{Date.Day}/{Date.Month}/{Date.Year}";
            }


            return time;
        }

        private string GetNickandTimeSpan()
        {
            string time = GetTime();

            if(Destination == DestinationType.To) //left
            {
                return $"<strong class='primary-font'>{NickName}</strong>"
                +"<small class='pull-right text-muted timeSpan'>"
             + $"<span class='glyphicon glyphicon-time'></span>{time}</small>";
            }
            else
            {
                return "<small class='text-muted timeSpan'>"
             + $"<span class='glyphicon glyphicon-time'></span>{time}</small>"
             + $"<strong class='pull-{Destination.ToDescriptionString()} primary-font'>{NickName}</strong>";
            }
        }

        private string GetImage()
        {
            return $"<img src='/File/GetAvatar?userGuid={message.SenderGuid}' alt='User Avatar' class='img-circle img-50'/>";
        }

        private string GetAttachmentsLink()
        {
            if (!string.IsNullOrEmpty(FilesUrl))
            {
                if(Destination == DestinationType.From)
                {
                    return $"<p class='pull-right'><a href='{FilesUrl}'>Download files<span class='glyphicon glyphicon-cloud-download'></span></a></p>";
                }
                else
                {
                    return $"<p class='pull-left'><a href='{FilesUrl}'>Download files<span class='glyphicon glyphicon-cloud-download'></span></a></p>";
                }
            }
            else
            {
                return null;
            }
        }

        public override string ToString()
        {
            //active
            StringBuilder sb = new StringBuilder();
            sb.Append(GetStarted())
            .Append(GetImage())
            .Append("</span><div class='chat-body clearfix'><div class='header'>")
            .Append($"{GetNickandTimeSpan()}</div>")
            .Append($"<p>{Text}</p>")
            .Append(GetAttachmentsLink())
            .Append("</div></li>");
            return sb.ToString();
        }

        
    }


}