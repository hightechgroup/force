using System;
using System.Collections.Generic;
using System.Linq;

namespace Force.Ddd.Pagination
{
    public abstract class Paging<TEntity, TOrderKey> : IPaging<TEntity, TOrderKey>
        where TEntity : class
    {
        // ReSharper disable once StaticMemberInGenericType
        public static int DefaultStartPage = 1;

        // ReSharper disable once StaticMemberInGenericType
        public static int DefaultTake = 30;

        private readonly IEnumerable<OrderBy<TEntity, TOrderKey>> _orderBy;

        private int _page;

        private int _take;

        protected Paging(int page, int take, params OrderBy<TEntity, TOrderKey>[] orderBy)
        {
            Page = page;
            Take = take;
            if (orderBy == null)
            {
                throw new ArgumentException("OrderBy can't be null", nameof(orderBy));
            }

            _orderBy = orderBy;
        }

        protected Paging()
        {
            Page = DefaultStartPage;
            Take = DefaultTake;
            // ReSharper disable once VirtualMemberCallInConstructor
            _orderBy = BuildDefaultSorting();
            
            if (_orderBy == null || !_orderBy.Any())
            {
                throw new ArgumentException("OrderBy can't be null or empty", nameof(_orderBy));
            }
        }

        protected abstract IEnumerable<OrderBy<TEntity, TOrderKey>> BuildDefaultSorting();

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

        public IEnumerable<OrderBy<TEntity, TOrderKey>> OrderBy => _orderBy;
    }
}
