using System;
using System.ComponentModel.DataAnnotations;
using Demo.WebApp.Infrastructure;
using Force.Ddd;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Demo.WebApp.Domain.Entities.Account
{
    [JsonConverter(typeof(ValueTypeConverter))]
    [ModelBinder(typeof(EmailModelBinder))]
    public class Email: ValueObject<string>
    {
        private static readonly EmailAddressAttribute Attr = new EmailAddressAttribute();
        
        public Email(string email): base(email)
        {
            if (!Attr.IsValid(email))
            {
                throw new ArgumentException(nameof(email), $"\"{email}\" is not valid email");    
            }
        }

        public static bool TryParse(string value, out Email email)
        {
            if (Attr.IsValid(value))
            {
                email = new Email(value);
                return true;
            }

            email = null;
            return false;
        }
    }
}