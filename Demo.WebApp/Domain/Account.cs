using Demo.WebApp.Infrastructure;
using Newtonsoft.Json;

namespace Demo.WebApp.Domain
{
    public class Account
    {
        public long Id { get; set; }
        
        public Email Email { get; set; }
    }
}