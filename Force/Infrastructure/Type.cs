using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Force.Extensions;

namespace Force.Infrastructure
{
    public delegate T ObjectActivator<out T>(params object[] args);

    public static class Type<T>
    {
        private static Attribute[] _attributes;

        private static Dictionary<string, PropertyInfo> _properties;

        private static MethodInfo[] _methods;
        
        private static ConstructorInfo[] _constructors;

        private static ConcurrentDictionary<long, ObjectActivator<T>> _activators;
        
        static Type()
        {
            var type = typeof(T);
            _attributes = type.GetCustomAttributes().ToArray();
            
            _properties = type
                .GetProperties()
                .Where(x => x.CanRead && x.CanWrite)
                .ToDictionary(x => x.Name, x => x);

            _methods = type.GetMethods()
                .Where(x => x.IsPublic && !x.IsAbstract)
                .ToArray();
            
            _constructors = typeof(T).GetConstructors();
            _activators = new ConcurrentDictionary<long, ObjectActivator<T>>();
        }

        public static Dictionary<string, PropertyInfo> PublicProperties => _properties;

        public static MethodInfo[] PublicMethods => _methods;

        public static Attribute[] Attributes => _attributes;

        public static bool HasAttribute<TAttr>()
            where TAttr : Attribute
            => Attributes.Any(x => x.GetType() == typeof(TAttr));
            
        public static TAttr GetCustomAttribute<TAttr>() 
            where TAttr: Attribute
            => (TAttr)_attributes.FirstOrDefault(x => x.GetType() == typeof(TAttr));

        #region Create

        public static T CreateInstance(params object[] args)
            => _activators.GetOrAdd(
                GetSignature(args),
                x => GetActivator(GetConstructorInfo(args)))
                    .Invoke(args);

        public static long GetSignature(object[] args)
        {
            long hc = 0;
            unchecked
            {
                // ReSharper disable once LoopCanBeConvertedToQuery
                foreach (var arg in args)
                {
                    hc = hc* 23 + (arg?.GetHashCode() ?? 0);
                }
            }

            return hc;
        }
        
        public static ConstructorInfo GetConstructorInfo(object[] args)
        {
            for (var i = 0; i < _constructors.Length; i++)
            {
                var constructor = _constructors[i];
                var ctrParams = constructor.GetParameters();
                if (ctrParams.Length != args.Length)
                {
                    continue;
                }

                var flag = true;
                for (var j = 0; j < args.Length; j++)
                {
                    if (ctrParams[j].ParameterType != args[j].GetType())
                    {
                        flag = false;
                        break;
                    }
                }

                if (!flag)
                {
                    continue;
                }

                return constructor;
            }

            var signature = GetSignature(args);
            
            throw new InvalidOperationException(
                $"Constructor ({signature}) is not found for {typeof(T)}");
        }
        
        public static ObjectActivator<T> GetActivator(ConstructorInfo ctor)
        {
            var type = ctor.DeclaringType;
            var paramsInfo = ctor.GetParameters();                  

            //create a single param of type object[]
            var param = Expression.Parameter(typeof(object[]), "args");
 
            var argsExp = new Expression[paramsInfo.Length];            

            //pick each arg from the params array 
            //and create a typed expression of them
            for (var i = 0; i < paramsInfo.Length; i++)
            {
                var index = Expression.Constant(i);
                var paramType = paramsInfo[i].ParameterType;              

                Expression paramAccessorExp = Expression.ArrayIndex(param, index);              
                Expression paramCastExp = Expression.Convert (paramAccessorExp, paramType);              

                argsExp[i] = paramCastExp;
            }                  

            //make a NewExpression that calls the
            //ctor with the args we just created
            var newExp = Expression.New(ctor,argsExp);                  

            //create a lambda with the New
            //Expression as body and our param object[] as arg
            var lambda = Expression.Lambda(typeof(ObjectActivator<T>), newExp, param);              

            //compile it
            var compiled = (ObjectActivator<T>)lambda.Compile();
            return compiled;
        }
        
        public static Delegate CreateMethod(MethodInfo method)
        {
            if (method == null)
            {
                throw new ArgumentNullException(nameof(method));
            }

            if (!method.IsStatic)
            {
                throw new ArgumentException("The provided method must be static.", nameof(method));
            }

            if (method.IsGenericMethod)
            {
                throw new ArgumentException("The provided method must not be generic.", nameof(method));
            }

            var parameters = method.GetParameters()
                .Select(p => Expression.Parameter(p.ParameterType, p.Name))
                .ToArray();
            
            var call = Expression.Call(null, method, parameters);
            return Expression.Lambda(call, parameters).Compile();
        }
        
        #endregion
        
        public static Expression<Func<T, TProperty>> PropertyGetter<TProperty>(string propertyName)
        {
            var paramExpression = Expression.Parameter(typeof(T), "value");
            var propertyGetterExpression = Expression.Property(paramExpression, propertyName);
            return Expression
                .Lambda<Func<T, TProperty>>(propertyGetterExpression, paramExpression);
        }

        public static Expression<Action<T, TProperty>> PropertySetter<TProperty>(string propertyName)
        {            
            var paramExpression = Expression.Parameter(typeof(T));
            var paramExpression2 = Expression.Parameter(typeof(TProperty), propertyName);
            var propertyGetterExpression = Expression.Property(paramExpression, propertyName);
            return Expression.Lambda<Action<T, TProperty>>
            (
                Expression.Assign(propertyGetterExpression, paramExpression2),
                paramExpression, paramExpression2
            );
        }        
    }
}