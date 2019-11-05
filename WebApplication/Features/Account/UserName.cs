using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace WebApplication.Features.Account
{
    public class UserName: EntityBase
    {
        public UserName(string firstName, string lastName, 
            string? middleName)
        {
            FirstName = firstName;
            LastName = lastName;
            MiddleName = middleName;
            Validator.ValidateObject(this, new ValidationContext(this));
        }

        [Required, StringLength(255)]
        public string FirstName { get; protected set; }
  
        [Required, StringLength(255)]
        public string LastName { get; protected set; }
  
        [StringLength(1)]
        public string? MiddleName { get; protected set; }
    }
}