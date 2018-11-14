using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Force.Ddd;
using Force.Extensions;

namespace Demo.WebApp.Domain
{
    public class User: EntityBase, IAuditable, IHasDomainEvents
    {     
        private static DefaultStringLengthAttribute _defaultStringLength = new DefaultStringLengthAttribute();
        
        private DomainEventStore _domainEventStore = new DomainEventStore();
        
        private Email _email { get; set; } // проверить
        
        public static readonly Expression<Func<User, string>> FullNameExpression = x => $"{x.FirstName} {x.LastName}";
        
        protected User()
        {}
        
        public User(Email email, string firstName, string lastName)
        {
            Created = DateTime.UtcNow;
            Email = email ?? throw new ArgumentNullException(nameof(email));
            ChangeName(firstName, lastName);
        }

        public DateTime Created { get; protected set; }
        
        public DateTime? LastUpdated { get; protected set; }

        public Email Email
        {
            get => _email;
            set
            {
                if (value != _email)
                {
                    _domainEventStore.Raise(new UserEmailChanged(this, _email, value));
                }

                _email = value;
            }
        }

        [DefaultStringLength]
        public string FirstName { get; protected set; }
        
        [DefaultStringLength]
        public string LastName { get; protected set; }

        public void ChangeName(string firstName, string lastName)
        {
            if (!_defaultStringLength.IsValid(firstName))
            {
                throw new ArgumentException(_defaultStringLength.ErrorMessage, nameof(firstName));
            }

            if (!_defaultStringLength.IsValid(lastName))
            {
                throw new ArgumentException(_defaultStringLength.ErrorMessage, nameof(lastName));
            }
            
            FirstName = firstName;
            LastName = lastName;
        }

        public string FullName => FullNameExpression.AsFunc()(this);

        public IEnumerable<IDomainEvent> GetDomainEvents()
            => _domainEventStore;
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