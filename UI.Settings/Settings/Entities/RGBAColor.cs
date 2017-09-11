namespace UI.Settings.Entities
{
    public class RGBAColor
    {
        public byte R { get; set; }

        public byte G { get; set; }

        public byte B { get; set; }

        public string A { get; set; }

        public void Reset()
        {
            A = A.Replace(",", ".");
        }
    }
}