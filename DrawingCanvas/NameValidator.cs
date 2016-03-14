using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DrawingCanvas
{
    public class NameValidator : ValidationRule
    {
        public Regex InvalidCharsRegex { get; } = new Regex($"[{Regex.Escape(new string(Path.GetInvalidFileNameChars()))}]");
        public Regex customExcludingRegex { get; set; }

        public string valueName { get; set; } = "file name";

        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            string casted = (string)value;
            if (string.IsNullOrWhiteSpace(casted?.ToString()))
            {
                return new ValidationResult(false, $"{valueName} cannot be empty.");
            }

            var matches = InvalidCharsRegex.Matches(casted);

            if (matches?.Count > 0)
            {
                return new ValidationResult(false, $"{valueName} cannot contain {string.Join(",", matches)}");
            }

            matches = customExcludingRegex?.Matches(casted);

            if (matches?.Count > 0)
            {
                return new ValidationResult(false, $"{valueName} cannot contain {string.Join(",", matches)}");
            }
            return ValidationResult.ValidResult;
        }
    }
}
