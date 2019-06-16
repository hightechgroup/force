using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using Force.Infrastructure;

namespace Force.Ddd
{
    public abstract class ValueObject
    {
        protected abstract IEnumerable<object> GetEqualityComponents();

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (GetType() != obj.GetType())
            {
                throw new ArgumentException(
                    $"Invalid comparison of Value Objects of different types: {GetType()} and {obj.GetType()}");
            }

            var valueObject = (ValueObject)obj;

            return GetEqualityComponents().SequenceEqual(valueObject.GetEqualityComponents());
        }

        public override int GetHashCode()
        {
            return GetEqualityComponents()
                .Aggregate(1, (current, obj) =>
                {
                    unchecked
                    {
                        return current * 23 + (obj?.GetHashCode() ?? 0);
                    }
                });
        }

        public static bool operator == (ValueObject a, ValueObject b)
        {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
                return true;

            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
                return false;

            return a.Equals(b);
        }

        public static bool operator != (ValueObject a, ValueObject b)
        {
            return !(a == b);
        }

        public static bool TryParse(object value, out ValueObject valueObject)
        {
            throw new NotImplementedException();
        }

        
        public static bool TryParse<TValue, TValueObject>(TValue value, out TValueObject valueObject)
            where TValueObject: ValueObject<TValue>
        {
            if (Type<TValueObject>.TryCreateMethod("IsValid", out var method))
            {
                var isValid = (method.DynamicInvoke(value) as bool?) == true;
                valueObject = null;
                if (!isValid)
                {
                    return false;
                }
            }
            
            Console.WriteLine("Create Start");
            valueObject = (TValueObject)
                Activator.CreateInstance(typeof(TValueObject), value);
                //Type<TValueObject>.Create(value);
            return true;
        }
    }

    public abstract class ValueObject<T> : ValueObject
    {
        public readonly T Value;

        public static ValidationResult IsValid(T value)
        {
            if (value == null)
            {
                return new ValidationResult("Value is null");
            }

            if (value.Equals(default(T)))
            {
                return new ValidationResult($"Value is default(\"{typeof(T)}\")");
            }
            
            return ValidationResult.Success;
        }
        
        protected ValueObject(T value)
        {
            var method = GetType().GetMethod("IsValid");

            if (method != null && method.IsStatic 
                               && method.ReturnType == typeof(ValidationResult))
            {
                var vr = (ValidationResult)method.Invoke(null, new object[]{value});
                if (vr != ValidationResult.Success)
                {
                    // ReSharper disable once PossibleNullReferenceException
                    throw new ArgumentException(vr.ErrorMessage, nameof(value));
                }
            }

            Value = value;
        }

        public static implicit operator T(ValueObject<T> value)
            => value.Value;
        
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

    }
}