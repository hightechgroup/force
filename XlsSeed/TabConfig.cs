namespace XlsSeed
{
    public class TabConfig
    {
        public bool SkipIdentityInsert { get; set; }
        
        public string[] Keys { get; set; }
        
        public string MatchSql { get; set; }
    }
}