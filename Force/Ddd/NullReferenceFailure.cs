using System;

namespace Force.Ddd
{
    public class NullReferenceFailure: Failure
    {
        public Type Type { get; }

        public NullReferenceFailure(Type type):base($"Instance of type \"{type}\" is null")
        {
            Type = type ?? throw new ArgumentNullException(nameof(type));
        }
    }
}