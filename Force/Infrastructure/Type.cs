using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Force.Extensions;

namespace Force.Infrastructure
{
    public delegate T ObjectActivator<out T>(params object[] args);

    public static class FastTypeInfo<T>
    {
        private static Attribute[] _attributes;

        private static PropertyInfo[] _properties;

        private static MethodInfo[] _methods;
        
        private static ConstructorInfo[] _constructors;

        private static ConcurrentDictionary<string, ObjectActivator<T>> _activators;
        
        static FastTypeInfo()
        {
            var type = typeof(T);
            _attributes = type.GetCustomAttributes().ToArray();
            
            _properties = type
                .GetProperties()
                .Where(x => x.CanRead && x.CanWrite)
                .ToArray();

            _methods = type.GetMethods()
                .Where(x => x.IsPublic && !x.IsAbstract)
                .ToArray();
            
            _constructors = typeof(T).GetConstructors();
            _activators = new ConcurrentDictionary<string, ObjectActivator<T>>();
        }

        public static PropertyInfo[] PublicProperties => _properties;

        public static MethodInfo[] PublicMethods => _methods;

        public static Attribute[] Attributes => _attributes;

        public static bool HasAttribute<TAttr>()
            where TAttr : Attribute
            => Attributes.Any(x => x.GetType() == typeof(TAttr));
            
        public static TAttr GetCustomAttribute<TAttr>() 
            where TAttr: Attribute
            => (TAttr)_attributes.FirstOrDefault(x => x.GetType() == typeof(TAttr));

        #region Create

        public static T Create(params object[] args)
            => _activators.GetOrAdd(
                GetSignature(args),
                GetActivator(GetConstructorInfo(args)))
                    .Invoke(args);

        private static string GetSignature(object[] args)
            => args
                .Select(x => x.GetType().ToString())
                .Join(",");
        
        private static ConstructorInfo GetConstructorInfo(object[] args)
        {
            for (var i = 0; i < _constructors.Length; i++)
            {
                var consturctor = _constructors[i];
                var ctrParams = consturctor.GetParameters();
                if (ctrParams.Length != args.Length)
                {
                    continue;
                }

                var flag = true;
                for (var j = 0; j < args.Length; i++)
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

                return consturctor;
            }

            var signature = GetSignature(args);
            
            throw new InvalidOperationException(
                $"Constructor ({signature}) is not found for {typeof(T)}");
        }
        
        private static ObjectActivator<T> GetActivator(ConstructorInfo ctor)
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
        
        public static Func<TObject, TProperty> PropertyGetter<TObject, TProperty>(string propertyName)
        {
            var paramExpression = Expression.Parameter(typeof(TObject), "value");

            var propertyGetterExpression = Expression.Property(paramExpression, propertyName);

            var result = Expression.Lambda<Func<TObject, TProperty>>(propertyGetterExpression, paramExpression)
                .Compile();

            return result;
        }

        public static Action<TObject, TProperty> PropertySetter<TObject, TProperty>(string propertyName)
        {            
            var paramExpression = Expression.Parameter(typeof(TObject));
            var paramExpression2 = Expression.Parameter(typeof(TProperty), propertyName);
            var propertyGetterExpression = Expression.Property(paramExpression, propertyName);
            var result = Expression.Lambda<Action<TObject, TProperty>>
            (
                Expression.Assign(propertyGetterExpression, paramExpression2), paramExpression, paramExpression2
            ).Compile();
            
            return result;
        }        
    }
}