using System;
using System.Collections.Generic;
using Force;

namespace Demo.WebApp.Infrastructure
{
    public class DomainEventDispatcher: IHandler<object>
    {
        private Dictionary<Type, IEnumerable<dynamic>> _handlers
            = new Dictionary<Type, IEnumerable<dynamic>>();
        
        public void Handle(object input)
        {
            var type = input.GetType();
            if (!_handlers.ContainsKey(type)) return;
            
            foreach (var handler in _handlers[type])
            {
                handler.Invoke(input);
            }
        }
    }
}