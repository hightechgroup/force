using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Force.Ddd;

namespace Demo.WebApp.Domain.AccountManagement
{
    public class UserName : ValueObject
    {
        public UserName(string firstName, string lastName, string middleName)
        {
            FirstName = firstName;
            LastName = lastName;
            MiddleName = middleName;
        }

        [Required, StringLength(255)] public string FirstName { get; }

        [Required, StringLength(255)] public string LastName { get; }


        [StringLength(1)] public string MiddleName { get; }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return FirstName;
            yield return LastName;
            yield return MiddleName;
        }
    }
}