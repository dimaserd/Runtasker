using System.ComponentModel.DataAnnotations;

namespace Runtasker.Logic.Attributes
{
    public class RatingSizeAttribute : ValidationAttribute
    {
        private readonly int _maxSize;

        public RatingSizeAttribute(int maxSize = 5)
        {
            _maxSize = maxSize;
        }

        public override bool IsValid(object value)
        {
            if (value == null) return false;

            int? rating = value as int?;

            return (rating <= _maxSize && rating >= 1);
        }

        public override string FormatErrorMessage(string name)
        {
            return string.Format($"The rating value must be between 1 and {_maxSize}");
        }
    }
}
