using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace DxMDB.Models
{
    public class NameFormatValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return new ValidationResult("Name is required");
            string name = value.ToString();
            string format = @"^([A-Za-z]+[\.\s]?)+$";
            var occurrence = Regex.Matches(name, format);
            if (Regex.IsMatch(name, format)) 
                return ValidationResult.Success;
            return new ValidationResult("Invalid name format");
        }
    }
}