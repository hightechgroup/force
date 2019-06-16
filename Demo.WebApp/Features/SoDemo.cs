using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.OleDb;
using Force.Ddd;

namespace Demo.WebApp.Features
{
    public class SoDemo
    {
        public static string CacheKey = ";";

        public static List<HelperPost> All()
            => Current
                .GlobalCache
                .GetSet<List<HelperPost>>(
                    CacheKey,
                    (old, ctx) =>
                    {
                        using (var sitesDb = new SitesDbContext())
                        {
                            return sitesDb.Query<List<HelperPost>>(
                                "SELECT p.Id, p.Title from Posts");
                        }
                    },
                    24 * 60 * 60, 24 * 60 * 60
                );

    }

    public class UserName: ValueObject
    {
        [StringLength(255)]
        public string FirstName { get; }
        
        [StringLength(255)]
        public string LastName { get; }

        private string _middleName;
        
        [StringLength(1)]
        public string MiddleName 
        {
            get => _middleName;
            set
            {
                _middleName = value ?? throw new ArgumentNullException(nameof(value));
                if (value.Length != 1)
                {
                    throw new ArgumentException("Middle name length must be 1 char length", nameof(value));
                }
            } 
        }
        
        public UserName(string firstName, string lastName)
        {
            FirstName = firstName ?? throw new ArgumentNullException(nameof(firstName));
            LastName = lastName ?? throw new ArgumentNullException(nameof(lastName));

            if (FirstName.Length > 255)
            {
                throw new ArgumentException("First name length must be < 255 char length", nameof(firstName));
            }
            
            if (LastName.Length > 255)
            {
                throw new ArgumentException("First name length must be < 255 char length", nameof(firstName));
            }
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return FirstName;
            yield return LastName;
            yield return MiddleName;
        }
    }
    
    
    public class SitesDbContext : IDisposable
    {
        public void Dispose()
        {
            var a = SoDemo.All();
            var b = new UserName("1", "2") {MiddleName = "3"};
            var c = b.MiddleName;
        }

        public T Query<T>(string selectPIdPTitleFromPosts)
        {
            throw new NotImplementedException();
        }
    }

    public static class Current
    {
        public static GlobalCache GlobalCache { get; set; }
    }

    public class GlobalCache
    {
        public List<HelperPost> GetSet<T>(string s, Func<T, Db, T> f, int a, int b)
        {
            return null;
        }

    }

    public class Db
    {
    }


    public class HelperPost
    {
    }
}