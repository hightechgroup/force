using System;
using System.Linq;

namespace Force.Ddd.Pagination
{
    public abstract class Paging : IPaging
    {
        protected Paging(int page, int take)
        {
            Page = page;
            Take = take;
        }

        public int Page { get; }

        public int Take { get; }

    }

    public abstract class Paging<T> : IQueryablePaging<T>
        where T : class
    {
        // ReSharper disable once StaticMemberInGenericType
        public static int DefaultStartPage = 1;

        // ReSharper disable once StaticMemberInGenericType
        public static int DefaultTake = 30;

        private int _page;

        private int _take;

        protected Paging(int page, int take)
        {
            Page = page;
            Take = take;
        }

        protected Paging()
        {
            Page = DefaultStartPage;
            Take = DefaultTake;
            // ReSharper disable once VirtualMemberCallInConstructor
        }


        public int Page
        {
            get { return _page; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("page must be >= 0", nameof(value));
                }

                _page = value;
            }
        }

        public int Take
        {
            get { return _take; }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("take must be > 0", nameof(value));
                }

                _take = value;
            }
        }

        public abstract IOrderedQueryable<T> Apply(IQueryable<T> queryable);
    }
}
