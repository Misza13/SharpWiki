namespace SharpWiki.API.Queries
{
    using System.Collections.Generic;

    internal class CategoryMembersQueryResult
    {
        public ContinueSection @continue { get; set; }
        
        public string batchcomplete { get; set; }

        public QuerySection query { get; set; }
        
        internal class ContinueSection
        {
            public string cmcontinue { get; set; }

            public string @continue { get; set; }
        }
        
        internal class QuerySection
        {
            public List<PageInfo> categorymembers { get; set; }
        }
        
        internal class PageInfo
        {
            public int pageid { get; set; }

            public int ns { get; set; }

            public string title { get; set; }
        }
    }
}