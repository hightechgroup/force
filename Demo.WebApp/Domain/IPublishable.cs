using System;
using Force.Ddd;

namespace Demo.WebApp.Domain
{
    public interface ICanBePublished
    {
        DateTime? Published { get; }
    }

    public interface IPublishable : ICanBePublished
    {
        void Publish();
    }

    public static class CanBePublishedSpec<T>
        where T: ICanBePublished
    {
        public static Spec<T> Published = new Spec<T>(x => x.Published != null);
        
        public static Spec<T> NotPublished = new Spec<T>(x => x.Published == null);
    }
}