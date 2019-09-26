using Demo.WebApp.Domain.Entities.Core;

namespace Demo.WebApp.Domain.Entities.Account
{
    public class Account: EntityBase<int>
    {
        public Email Email { get; set; }
    }
}