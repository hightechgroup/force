namespace Demo.WebApp.Domain
{
    public class User: EntityBase
    {                
        protected User()
        {}
        
        public User(Email email, string firstName, string lastName)
        {
            Email = email;
            FirstName = firstName;
            LastName = lastName;
        }

        public Email Email { get; set; }
        
        public string FirstName { get; set; }
        
        public string LastName { get; set; }
    }
}