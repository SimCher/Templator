using System.Globalization;
using System.Windows.Controls;

namespace Templator.Validation
{
    public class TextBoxNotNullValidationRule : ValidationRule
    {
        public string Message { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string str = value as string;
            if (str != null)
            {
                if (str.Length > 0)
                {
                    return ValidationResult.ValidResult;
                }
            }

            return new ValidationResult(false, Message);
        }
    }
}