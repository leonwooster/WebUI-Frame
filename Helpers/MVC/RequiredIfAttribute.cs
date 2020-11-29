using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace AspCoreFrame.WebUI.Helpers.MVC
{
    [AttributeUsage(AttributeTargets.Property)]
    public class RequiredIfAttribute : ValidationAttribute, IClientModelValidator
    {
        private string PropertyName { get; set; }
        public string PropertyValue { get; set; }        

        public RequiredIfAttribute(string propertyName, string propertyVal)
        {
            PropertyName = propertyName;
            PropertyValue = propertyVal;            

            ErrorMessage = "The {0} field is required."; //used if error message is not set on attribute itself
        }

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            object instance = context.ObjectInstance;
            Type type = instance.GetType();

            string v = type.GetProperty(PropertyName).GetValue(instance)?.ToString();

            if(v != PropertyValue)
            {
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }

        public void AddValidation(ClientModelValidationContext context)
        {
            MergeAttribute(context.Attributes, "data-val", "true"); //to instruct client un-obtrusive validation.
            var errorMessage = FormatErrorMessage(context.ModelMetadata.GetDisplayName());
            MergeAttribute(context.Attributes, "data-val-requiredif", errorMessage);
            MergeAttribute(context.Attributes, "data-val-requiredif-id", PropertyName);
            MergeAttribute(context.Attributes, "data-val-requiredif-value", PropertyValue);            
        }

        private bool MergeAttribute(IDictionary<string, string> attributes, string key, string value)
        {
            if (attributes.ContainsKey(key))
            {
                return false;
            }
            attributes.Add(key, value);
            return true;
        }
    }
}
