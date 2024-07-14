using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace jim.wiki.back.model.Models.ObjectsValue;

 public record Email
{
    private static readonly string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
    public string Value { get; set; }
    public Email(string email)
    {
        if (!IsValidEmail(email)) throw new ArgumentException("The email not is valid");
        Value = email;
    }
    public static bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return false;
        }

        return Regex.IsMatch(email, emailPattern);
    }
}

