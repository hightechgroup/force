using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using Castle.Core.Logging;
using Demo.WebApp.Domain;
using Force.Ddd;
using Microsoft.AspNetCore.Http;

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

    // ReSharper disable UnassignedGetOnlyAutoProperty
    // ReSharper disable UnusedMember.Global

    public abstract class Document : EntityBase
    {        
        public UploadedFile File { get; }

        public abstract void Update(object o);
    }

    public abstract class DriverLicense : Document
    {        
        public DriverLicenseNumber Number { get; }
    }

    public abstract class Cv : Document
    {
        public PhoneNumber PhoneNumber { get; }     
        public string Summary { get; }
    }
    
    public class UploadedFile
    {
    }

    public class PhoneNumber
    {
    }

    public class DriverLicenseNumber
    {
    }

    public class OE
    {
        public void Update<T>(IHasFile<T> entity, IFormFile file, T data)
        {
            FileStorage fileStorage = null;
            ILogger logger = null;
            UploadedFile uploaded = fileStorage.Upload(file);
            
            try
            {
                entity.Update(uploaded, data);
            }
            catch (SqlException se)
            {
                try
                {
                    fileStorage.Delete(uploaded);
                }
                catch (FileStorageException fe)
                {
                    logger.Fatal(fe.Message);
                    throw;
                }
                throw;
            }
        }
        
        public IEnumerable<Document> Fetch(IQueryable<Document> documents)
        {
            return documents
                .Take(10)
                .ToList(); 
        }
        public bool TryCancel(State<Order> orderState, out State<Order> newState)
        {
            IQueryable<Document> documents;

            
            switch (orderState)
            {
                case ICancellableOrder o:
                    newState = o.Cancel();
                    return true;
                default:
                    newState = null;
                    return false;
            }
        }
        
        
        public State<Order> Process(State<Order> orderState, User user)
        {            
            switch (orderState)
            {
                case UnvalidatedOrder o: return o.Validate();
                case ValidatedOrder o:
                    var priceCalculator = new PriceCalculator(user);
                    var price = priceCalculator.Calculate(o);
                    return o.SetPrice(price);
                case PricedOrder o: 
                    var deliveryManager = new DeliveryManager(user);
                    var trackingUrl = deliveryManager.Ship(o);
                    return o.Shipped(trackingUrl);
                default: return orderState;
            }
        }
    }

    public class FileStorage
    {
        public UploadedFile Upload(IFormFile file)
        {
            throw new NotImplementedException();
        }

        public void Delete(UploadedFile uploaded)
        {
            throw new NotImplementedException();
        }
    }

    public class FileStorageException : Exception
    {
    }

    public interface IHasFile<in T>
    {
        void Update(UploadedFile file, T data);
    }

    public interface ICancellableOrder
    {
        State<Order> Cancel();
    }

    public class DeliveryManager
    {
        public DeliveryManager(User user)
        {
            throw new NotImplementedException();
        }

        public State<Order> Ship(PricedOrder pricedOrder)
        {
            throw new NotImplementedException();
        }
    }

    public class PriceCalculator
    {
        public PriceCalculator(User user)
        {
            throw new NotImplementedException();
        }

        public decimal Calculate(ValidatedOrder validatedOrder)
        {
            throw new NotImplementedException();
        }
    }

    public class UnvalidatedOrder: State<Order>
    {
        public ValidatedOrder Validate()
        {
            return new ValidatedOrder(Entity, DateTime.UtcNow);
        }
        
        public UnvalidatedOrder(Order entity) : base(entity)
        {
            var a = SoDemo.GetStateName(null);
        }
    }
    
    public class ValidatedOrder: State<Order>
    {
        internal ValidatedOrder(Order entity, DateTime dateTime) : base(entity)
        {
        }

        public State<Order> SetPrice(decimal price)
        {
            throw new NotImplementedException();
        }
    }

    public class PricedOrder: State<Order>
    {
        public PricedOrder(Order entity) : base(entity)
        {
        }

        public State<Order> Shipped(State<Order> trackingUrl)
        {
            throw new NotImplementedException();
        }
    }

    public class ShippedOrder: State<Order>
    {
        public ShippedOrder(Order entity) : base(entity)
        {
        }
    }
    
    public class CancelledOrder: State<Order>
    {
        public ShippedOrder(Order entity) : base(entity)
        {
        }
    }
    
    public class Order
    {
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