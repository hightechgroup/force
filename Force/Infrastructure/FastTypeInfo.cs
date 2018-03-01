using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Force.Extensions;

namespace Force.Infrastructure
{
    public delegate T ObjectActivator<T>(params object[] args);
    
    public static class FastTypeInfo<T>
    {
        private static Attribute[] _attributes;

        private static PropertyInfo[] _properties;

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

            _constructors = typeof(T).GetConstructors();
            _activators = new ConcurrentDictionary<string, ObjectActivator<T>>();
        }

        public static PropertyInfo[] PublicProperties => _properties;

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
        
        #endregion
    }
}