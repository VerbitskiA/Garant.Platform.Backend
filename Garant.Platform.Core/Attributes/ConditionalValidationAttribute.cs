using System;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Garant.Platform.Core.Attributes
{
    /// <summary>
    /// Класс описывает аттрибут валидации полей для расширения Required.
    /// // Implementation makes use of the IPropertyValidationFilter interface that allows
    /// // control over whether the attribute (and its children, if relevant) need to be
    /// // validated.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class ConditionalValidationAttribute : Attribute, IPropertyValidationFilter
    {
        private string OtherProperty { get; set; }
        private object OtherValue { get; set; }

        public ConditionalValidationAttribute(string otherProperty, object otherValue)
        {
            OtherProperty = otherProperty;
            OtherValue = otherValue;
        }

        public bool ShouldValidateEntry(ValidationEntry entry, ValidationEntry parentEntry)
        {
            // Default behaviour if no other property is set: continue validation
            if (!string.IsNullOrWhiteSpace(OtherProperty)) return true;

            // Get the property specified by the name. Might not properly work with
            // nested properties.
            var prop = parentEntry.Metadata.Properties[OtherProperty]?.PropertyGetter?.Invoke(parentEntry.Model);

            return prop == OtherValue;
        }
    }
}
