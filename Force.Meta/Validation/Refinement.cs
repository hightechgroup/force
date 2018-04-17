using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using Force.Ddd;
using Force.Extensions;
using Force.Infrastructure;

namespace Force.Meta.Validation
{
    public interface IHasErrorMessage
    {
        string ErrorMessage { get; }
    }

    public class Refinement<T>
        : IValidator<object>
        , IHasErrorMessage
    {
        public Expression<Func<T, bool>> Expression { get; protected set; }
                
        public string ErrorMessage { get; protected set; }
        
        public Refinement(Expression<Func<T, bool>> expression, string errorMessage)
        {
            Expression = expression ?? throw new ArgumentNullException(nameof(expression));
            ErrorMessage = errorMessage;
        }

        public static explicit operator Expression<Func<T, bool>>(Refinement<T> refinement) 
            => refinement.Expression;
        
        public static Refinement<T> operator & (Refinement<T> r1, Refinement<T> r2)
            => new Refinement<T>(r1.Expression.And(r2.Expression),
                $"{r1.ErrorMessage}{Environment.NewLine}{r2.ErrorMessage}");

        private IEnumerable<ValidationResult> Validate(T obj)
            => Expression.AsFunc()(obj)
                ? null
                : new[] {new ValidationResult(ErrorMessage)};

        public IEnumerable<ValidationResult> Validate(object obj)
            => !(obj is T)
                ? new[] {new ValidationResult("Type mismatch")}
                : Validate((T) obj);

        public override string ToString()
        {
            var visitor = new EsExpressionVisitor();
            visitor.Visit(Expression.Body);
            return $"{Expression.Parameters.First().Name} => {visitor}";
        }
        
        public static bool operator false(Refinement<T> refinement)
        {
            return false; // no-op. & and && do exactly the same thing.
        }

        public static bool operator true(Refinement<T> refinement)
        {
            return false; // no-op. & and && do exactly the same thing.
        }

        public static Refinement<T> operator !(Refinement<T> refinement)
        {
            return new Refinement<T>(refinement.Expression.Not(), refinement.ErrorMessage);
        }
    }
}