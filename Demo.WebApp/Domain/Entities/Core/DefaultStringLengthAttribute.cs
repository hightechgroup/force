using System.ComponentModel.DataAnnotations;

namespace Demo.WebApp.Domain.Entities.Core
{
    public class DefaultStringLengthAttribute: StringLengthAttribute
    {
        public DefaultStringLengthAttribute() : base(255)
        {
        }
    }
}