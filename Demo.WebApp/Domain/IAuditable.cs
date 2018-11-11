using System;

namespace Demo.WebApp.Domain
{
    public interface IAuditable
    {
        DateTime Created { get; }
        
        DateTime? LastUpdated { get; }
    }
}