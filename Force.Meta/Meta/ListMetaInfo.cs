namespace Force.Meta
{
    public class ListMetaInfo: MetaInfoBase<ListItemMetaInfo>
    {
        public ListMetaInfo(params ListItemMetaInfo[] items) : base(items)
        {
        }
    }
}