using Extensions.String;
using Runtasker.Logic.Entities;
using System;
using System.ComponentModel;
using System.Globalization;
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
            if(message.SenderId == userGuid)
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
            AttachmentId = message.AttachmentId;
            Status = message.Status;
            
        }

        DestinationType Destination { get; set; }
        MessageStatus Status { get; set; }

        Message message { get; set; }
        
        #region Properties For a good life
        string NickName { get; set; }


        DateTime Date { get; set; }
        string Text { get; set; }
        string AttachmentId { get; set; }
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
            return Date.ToString("G", new CultureInfo("ru-RU"));
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
            return $"<img src='/File/GetAvatar?userGuid={message.SenderId}' alt='User Avatar' class='img-circle img-50'/>";
        }

        

        public override string ToString()
        {
            //active
            StringBuilder sb = new StringBuilder();
            sb.Append(GetStarted())
            .Append(GetImage())
            .Append("</span><div class='chat-body clearfix'><div class='header'>")
            .Append($"{GetNickandTimeSpan()}</div>");
            
            string textAlignClass = (Destination == DestinationType.To)? "text-left" : "text-right";
            
            sb.Append(Text.WrapToHtmlTag("p", new { @class = textAlignClass })
                        .WrapToHtmlTag("div", new { @class = "row", style = "margin-right:2px; margin-left:2px;" }));

            if (!string.IsNullOrEmpty(AttachmentId))
            {
                sb.Append("[Скачать]".WrapToHtmlTag("a", new { href = $"/File/Download/{AttachmentId}" })
                    .WrapToHtmlTag("p", new { @class = textAlignClass })
                    .WrapToHtmlTag("div", new { @class = "row", style = "margin-right:2px; margin-left:2px;" }));
            }



            sb.Append("</div></li>");
            return sb.ToString();
        }

        
    }


}