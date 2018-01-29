using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Force.Extensions;

namespace Force.Ddd
{
    public class Failure
    {        
        public static implicit operator Failure(Exception e) => new ExceptionFailure(e);
                
        public Failure(params Failure[] failures)
        {
            if (!failures.Any())
            {
                throw new ArgumentException(nameof(failures));
            }
            
            Message = failures.Select(x => x.Message).Join(Environment.NewLine);
            var dict = new Dictionary<string, object>();

            for(var i = 0; i < failures.Length; i++)
            {
                dict[(i + 1).ToString()] = failures[i];
            }
            
            Data = new ReadOnlyDictionary<string, object>(dict);
        }

        
        public Failure(string message)
        {
            Message = message;
        }

        public Failure(string message, IDictionary<string, object> data)
        {
            Message = message;
            Data = new ReadOnlyDictionary<string, object>(data);
        }
        
        public string Message { get; }

        public ReadOnlyDictionary<string, object> Data { get; protected set; }
    }
}