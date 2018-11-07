using System;
using System.ComponentModel.DataAnnotations;

namespace Force.Ddd.Pagination
{
    public class Paging : IPaging
    {
        public Paging(int page, int take)
        {
            Page = page;
            Take = take;
        }

        public Paging()
        {
            Page = DefaultStartPage;
            Take = DefaultTake;
        }

        private int _page;
        private int _take;

        public const int MaxTake = 1000;
        
        public static int DefaultStartPage = 1;

        public static int DefaultTake = 30;
        
        public int Page
        {
            get => _page;
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException("Page must be >= 0", nameof(value));
                }

                _page = value;
            }
        }

        [Range(1, MaxTake)]
        public virtual int Take
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
}
