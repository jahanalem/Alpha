﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;

namespace Alpha.Infrastructure
{
    public static class Extensions
    {
        public static bool IsValidEmailAddress(this string s)
        {
            Regex regex = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
            return regex.IsMatch(s);
        }

        public static bool IsDate(this string input)
        {
            if (!string.IsNullOrEmpty(input))
            {
                DateTime dt;
                return (DateTime.TryParse(input, out dt));
            }
            else
            {
                return false;
            }
        }

        public static DateTime? ToDateTime(this object value)
        {
            DateTime convertedDate = DateTime.MinValue;
            try
            {
                convertedDate = Convert.ToDateTime(value);
                Console.WriteLine("'{0}' converts to {1}.", value, convertedDate);
            }
            catch (FormatException)
            {
                Console.WriteLine("'{0}' is not in the proper format.", value);
                return DateTime.MinValue;
            }
            catch (InvalidCastException)
            {
                Console.WriteLine("Conversion of the {0} '{1}' is not supported", value.GetType().Name, value);
                return DateTime.MinValue;
            }
            return convertedDate;
        }

        public static int GetMaxLengthOfProperty<T>(this string nameOfProperty)
        {
            int maxLen = 1;
            StringLengthAttribute strLenAttr = typeof(T).GetProperty(nameOfProperty)?.GetCustomAttributes(typeof(StringLengthAttribute), false).Cast<StringLengthAttribute>().SingleOrDefault();
            if (strLenAttr != null)
            {
                maxLen = strLenAttr.MaximumLength;
            }

            return maxLen;
        }

        public static IList<IdentityError> Add(this IList<IdentityError> errorList, IEnumerable<IdentityError> identityErrors)
        {
            foreach (var validEmailError in identityErrors)
            {
                errorList.Add(validEmailError);
            }
            return errorList;
        }
    }
}
