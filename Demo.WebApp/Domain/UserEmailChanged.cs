using System;
using Force.Ddd;
using Force.Ddd.DomainEvents;

namespace Demo.WebApp.Domain
{
    public class UserEmailChanged : IDomainEvent
    {
        protected UserEmailChanged()
        {}
        
        public UserEmailChanged(User user, Email from, Email to)
        {
            User = user ?? throw new ArgumentNullException(nameof(user));
            From = @from ?? throw new ArgumentNullException(nameof(@from));
            To = to ?? throw new ArgumentNullException(nameof(to));
        }

        public User User { get; protected set;}
        
        public Email From { get; protected set; }
        
        public Email To { get; protected set; }

        public DateTime Happened { get; protected set; } = DateTime.UtcNow;
    }
}