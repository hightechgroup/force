using System.ComponentModel.DataAnnotations;

namespace Demo.WebApp.Domain
{
    public class DefaultStringLengthAttribute: StringLengthAttribute
    {
        public DefaultStringLengthAttribute() : base(255)
        {
        }
    }
}