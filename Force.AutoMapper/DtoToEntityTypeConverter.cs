using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AutoMapper;
using Force.Ddd;
using Force.Extensions;

namespace Force.AutoMapper
{
    public class DtoToEntityTypeConverter<TKey, TDto, TEntity> : ITypeConverter<TDto, TEntity>
        where TKey: IEquatable<TKey>
        where TEntity : class, IHasId<TKey>
    {
        protected readonly IUnitOfWork UnitOfWork;

        public DtoToEntityTypeConverter(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;

            var hasParameterlessCtor = typeof(TEntity)
                .GetTypeInfo()
                .DeclaredConstructors.Any(x => !x.GetParameters().Any());

            if (!hasParameterlessCtor)
                throw new InvalidOperationException(
                    "Generic argument TEntity should refer to class with parameterless constructor (public, protected or private)");
        }

        public virtual TEntity Convert(TDto source, TEntity destination, ResolutionContext context)
        {
            var sourceId = (source as IHasId)?.Id;

            var dest = destination ?? (sourceId != null
                           ? UnitOfWork.Find<TEntity>(sourceId) ??
                             (TEntity) Activator.CreateInstance(typeof(TEntity), true)
                           : (TEntity) Activator.CreateInstance(typeof(TEntity), true));

            var sp = typeof(TDto)
                .GetPublicProperties()
                .Where(x => x.CanRead && x.CanWrite)
                .ToDictionary(x => x.Name.ToUpper(), x => x);

            var dp = typeof(TEntity)
                .GetPublicProperties()
                .Where(x => x.CanRead && x.CanWrite)
                .ToArray();

            foreach (var propertyInfo in dp)
            {
                var key = typeof(IHasId).GetTypeInfo().IsAssignableFrom(propertyInfo.PropertyType)
                    ? propertyInfo.Name.ToUpper() + "ID"
                    : propertyInfo.Name.ToUpper();

                if (!sp.ContainsKey(key)) continue;

                if (key.EndsWith("ID", StringComparison.CurrentCultureIgnoreCase)
                    && typeof(IHasId).GetTypeInfo().IsAssignableFrom(propertyInfo.PropertyType))
                {
                    propertyInfo.SetValue(dest, UnitOfWork.Find(propertyInfo.PropertyType, sp[key].GetValue(source)));
                }
                else
                {
                    if (propertyInfo.PropertyType != sp[key].PropertyType)
                    {
                        var et = IsEntityGenericColections(sp[key].PropertyType, propertyInfo.PropertyType);
                        if (et == null) continue;

                        var collection = propertyInfo.GetValue(dest);
                        var add = collection.GetType().GetTypeInfo().GetMethod("Add");
                        if (add != null)
                        {
                            var ids = (IEnumerable) sp[key].GetValue(source);
                            if (ids == null) continue;

                            foreach (var id in ids)
                            {
                                add.Invoke(collection, new object[] {UnitOfWork.Find(et, id)});
                            }
                        }
                        else
                        {
                            throw new InvalidOperationException(
                                $"Can't map Property {propertyInfo.Name} because of type mismatch:" +
                                $"{sp[key].PropertyType.Name} -> {propertyInfo.PropertyType.Name}");
                        }
                    }
                    else
                    {
                        propertyInfo.SetValue(dest, sp[key].GetValue(source));
                    }
                }
            }

            return dest;
        }

        private static Type IsEntityGenericColections(Type src, Type dest)
        {
            if (!dest.GetTypeInfo().IsGenericType) return null;
            if (dest.GetTypeInfo().GetGenericArguments().Length > 1) return null;

            if (!typeof(IEnumerable).GetTypeInfo().IsAssignableFrom(src) ||
                typeof(ICollection<>) != dest.GetGenericTypeDefinition()
                && !dest.GetTypeInfo()
                    .GetInterfaces()
                    .Any(x => x.GetTypeInfo().IsGenericType
                              && x.GetTypeInfo().GetGenericTypeDefinition() == typeof(ICollection<>)))
            {
                return null;
            }

            return dest.GetTypeInfo().GetGenericArguments().First();
        }
    }
}