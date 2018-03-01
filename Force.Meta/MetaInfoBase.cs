namespace Force.Meta
{
    public abstract class MetaInfoBase<T>
        where T:ItemMetaInfoBase 
    {
        protected MetaInfoBase(params T[] items)
        {
            Items = items;
        }

        public string Title { get; set; }
        
        public T[] Items { get; }
    }
}