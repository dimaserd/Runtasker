
namespace HtmlExtensions.StaticRenderers
{
    public static class GISigns
    {
        public static string Trash
        {
            get { return "<span class='glyphicon glyphicon-trash'></span>"; }
        }

        public static string MinusSign
        {
            get { return "<span class='glyphicon glyphicon-minus'></span>"; }
        }

        public static string ListAlt
        {
            get { return "<span class='glyphicon glyphicon-list-alt'></span>"; }
        }

        public static string Pencil
        {
            get { return "<span class='glyphicon glyphicon-pencil'></span>"; }
        }

        public static string Send
        {
            get { return "<span class='glyphicon glyphicon-send'></span>"; }
        }

        public static string Gift
        {
            get { return "<span class='glyphicon glyphicon-gift'></span>"; }
        }

        public static string PiggyBank
        {
            get { return "<span class='glyphicon glyphicon-piggy-bank'></span>"; }
        }

        public static string Login
        {
            get { return "<span class='glyphicon glyphicon-log-in'></span>"; }
        }

        public static string Envelope
        {
            get { return "<span class='glyphicon glyphicon-envelope'></span>"; }
        }


        public static string User
        {
            get { return "<span class='glyphicon glyphicon-user'></span>"; }
        }

        public static string Star
        {
            get { return "<span class='glyphicon glyphicon-star'></span>"; }
        }

        public static string Save
        {
            get { return "<span class='glyphicon glyphicon-save'></span>"; }
        }

        public static string Briefcase
        {
            get { return "<span class='glyphicon glyphicon-briefcase'></span>"; }
        }

        public static string PlusSign
        {
            get { return "<span class='glyphicon glyphicon-plus'> </span>"; }
        }

        public static string Count(int count)
        {
            return $"<span class='badge'>{count}</span>";
        }

        public static string Configuration
        {
            get { return $"<span class='glyphicon glyphicon-cog'></span>"; }
        }

        public static string Ok
        {
            get { return $"<span class='glyphicon glyphicon-ok'></span>"; }
        }

        public static string Refresh
        {
            get { return $"<span class='glyphicon glyphicon-refresh'></span>"; }
        }
    }
}
