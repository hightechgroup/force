using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Force.Ddd;

namespace Force.Benchmarks
{
    public class UserName
    {
        public UserName(string firstName, string lastName, string middleName)
        {
            FirstName = firstName;
            LastName = lastName;
            MiddleName = middleName;
        }

        [Required, StringLength(255)] 
        public string FirstName { get; }

        [Required, StringLength(255)] 
        public string LastName { get; }


        [StringLength(1)] 
        public string MiddleName { get; }
        
    }
}