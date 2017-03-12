using System;

namespace Extensions.Decimal
{
    public static class DecimalExtensions
    {
        public static string ToMoney(this decimal Number)
        {
            int trancResult = int.Parse(Math.Truncate(Number).ToString());
            int roundResult = int.Parse(Math.Round(Number).ToString());
            
            if(roundResult == trancResult)
            {
                return trancResult.ToString();
            }

            return Number.ToString();
        }
    }
}
