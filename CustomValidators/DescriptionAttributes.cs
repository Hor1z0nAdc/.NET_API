using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace FirstProject.CustomValidators
{
    public class DescriptionAttribute : ValidationAttribute
    {
        private readonly string startingString;
        public string DefaultErrorMessage { get; set; } = "The description is incorrect";

        public DescriptionAttribute(string startingString) { 
            this.startingString = startingString;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {

            if (value is string)
            {
                var output = GetFirstWord(value.ToString());
                if(String.Equals(output, startingString, StringComparison.OrdinalIgnoreCase))
                {
                    return ValidationResult.Success;
                }
            }

            return new ValidationResult(string.Format(ErrorMessage ?? DefaultErrorMessage));
        }

        private string GetFirstWord(string text)
        {
            var candidate = text.Trim();
            if (!candidate.Any(Char.IsWhiteSpace))
                return text;

            return candidate.Split(' ').FirstOrDefault();
        }
    }


    public class DescriptionCrossAttribute : ValidationAttribute
    {
        private readonly string startingString;
        public string TitlePropertyName { get; }
        public string DefaultErrorMessage { get; set; } = "The description is incorrect";

        public DescriptionCrossAttribute(string titlePropertyName, string startingString)
        {
            this.startingString = startingString;
            TitlePropertyName = titlePropertyName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var titleProperty = validationContext.ObjectType.GetProperty(TitlePropertyName);

            if (titleProperty != null) {
                var title = (string)titleProperty.GetValue(validationContext.ObjectInstance);
           
                if(GetFirstWord(title) != "title:")
                {
                    return new ValidationResult(string.Format(ErrorMessage ?? DefaultErrorMessage));
                }
            }

            if (value is string)
            {
                var output = GetFirstWord(value.ToString());
                if (output == startingString)
                {
                    return ValidationResult.Success;
                }
            }

            return new ValidationResult(string.Format(ErrorMessage ?? DefaultErrorMessage));
        }

        private string GetFirstWord(string text)
        {
            var candidate = text.Trim();
            if (!candidate.Any(Char.IsWhiteSpace))
                return text;

            return candidate.Split(' ').FirstOrDefault();
        }
    }
}
