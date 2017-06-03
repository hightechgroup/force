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

        protected Paging()
        {
            Page = DefaultStartPage;
            Take = DefaultTake;
        }

        private int _page;
        private int _take;

        // ReSharper disable once StaticMemberInGenericType
        public static int DefaultStartPage = 1;

        // ReSharper disable once StaticMemberInGenericType
        public static int DefaultTake = 30;

        public int Page
        {
            get => _page;
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
            get => _take;
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("take must be > 0", nameof(value));
                }

                _take = value;
            }
        }

    }

    public abstract class Paging<T> : Paging, IQueryablePaging<T>
        where T : class
    {
        protected Paging(int page, int take) : base(page, take)
        {
        }

        protected Paging()
        {
        }

        public abstract IOrderedQueryable<T> Order(IQueryable<T> queryable);
    }
}
