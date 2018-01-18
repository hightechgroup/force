using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Force.Ddd;

namespace Force.Demo.Domain
{
    public class Category : HasNameBase
    {
        public static class Specs
        {
            //public static Spec<Category> IsActive => new Spec<Category>(x => x.IsActive);
        }

        public Category()
        {
        }

        public Category(string name, string url) : base(name)
        {
            Url = url;
        }

        public bool IsActive { get; set; }

        [Required]
        public string Url { get; protected set; }
    }
}