using System;
using System.ComponentModel.DataAnnotations;
using Demo.WebApp.Infrastructure;
using Newtonsoft.Json;

namespace Demo.WebApp.Domain
{
    [JsonConverter(typeof(ValueTypeConverter))]
    public class Email
    {
        private static readonly EmailAddressAttribute Attr = new EmailAddressAttribute();
        
        private readonly string _email;

        public Email(string email)
        {
            if (!Attr.IsValid(email))
            {
                throw new ArgumentException(nameof(email), $"\"{email}\" is not valid email");    
            }
            
            _email = email;
        }

        public bool StartsWith(string value)
        {
            return _email.StartsWith(value);
        }

        public static implicit operator string (Email email)
            => email.ToString();
        
        public static implicit operator Email (string email)
            => new Email(email);     
        
        public override string ToString()
            => _email;
    }
}