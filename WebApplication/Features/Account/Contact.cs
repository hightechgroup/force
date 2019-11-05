using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;

namespace WebApplication.Features.Account
{
    public class Contact: EntityBase
    {
        const string PhonePattern = @"\+?\d";
        
        [EmailAddress]
        public string? Email { get; protected set; }
        
        [RegularExpression(PhonePattern)]
        public string? Phone { get; protected set; }

        protected Contact()
        {
            
        }
        
        protected Contact(string? email, string? phone)
        {
            Email = email;
            Phone = phone;
        }

        private static readonly Regex PhoneRegex = new Regex(PhonePattern);
        public static bool TryParsePhone(string phone, out Contact c)
        {
            if (PhoneRegex.IsMatch(phone))
            {
                c = new Contact(null, phone);
                return true;
            }

            c = null;
            return false;
        }
        
        private static readonly EmailAddressAttribute EmailAttribute = new EmailAddressAttribute();
        public static bool TryParseEmail(string email, out Contact c)
        {
            if (EmailAttribute.IsValid(email))
            {
                c = new Contact(email, null);
                return true;
            }

            c = null;
            return false;
        }

    }
}