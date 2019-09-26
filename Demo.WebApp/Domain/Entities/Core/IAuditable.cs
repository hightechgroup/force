using System;

namespace Demo.WebApp.Domain.Entities.Core
{
    public interface IAuditable
    {
        DateTime Created { get; }
        
        DateTime? LastUpdated { get; }
    }
}