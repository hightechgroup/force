namespace Force.Meta
{
    public class FormItemMetaInfo: ItemMetaInfoBase
    {
        public bool Hidden { get; set; }
        
        public bool Required { get; set; }
        
        public bool ReadOnly { get; set; }
        
        public int MaxLength { get; set; }
        
        public string Pattern { get; set; }
        
        public string Mask { get; set; }
        
    }
}