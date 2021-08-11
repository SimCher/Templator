using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace Templator.Validation
{
    public class CorrectExcelColumnValidationRule : ValidationRule
    {
        private static Regex _regExp = new Regex("\\b([A-Z]+)(\\d+)\\b");
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value != null)
            {
                if (!_regExp.IsMatch((string)value))
                {
                    return new ValidationResult(false, "Некорректное название столбца Excel!");
                }
                return ValidationResult.ValidResult;
            }
            return new ValidationResult(false, "Некорректное название столбца Excel!");
        }
    }
}