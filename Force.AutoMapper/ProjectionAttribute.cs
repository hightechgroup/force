using System;

namespace AutoMapper.Extensions
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ProjectionAttribute : Attribute
    {
        public Type Type { get; }

        public ProjectionAttribute(Type type)
        {
            Type = type;
        }
    }
}