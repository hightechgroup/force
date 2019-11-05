using System.ComponentModel.DataAnnotations;
using Force.Ddd;

namespace WebApplication.Features.Account
{
    public class User : EntityBase
    {
        public Contact Contact { get; protected set; }
        
        public UserName? UserName { get; protected set; }

        protected User()
        {
        }
        
        public User(Contact contact, UserName? userName)
        {
            Contact = contact;
            UserName = userName;
        }
    }


    public interface IHasFullName
    {
        string FirstName { get; }
        
        string LastName { get; }
    }

    public class FullNameFormatter<T> : Formatter<T>
        where T : IHasFullName
    {
        public FullNameFormatter() : base(x => $"{x.FirstName} {x.LastName}")
        {
        }
    }
}