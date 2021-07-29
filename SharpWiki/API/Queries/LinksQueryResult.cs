// ReSharper disable InconsistentNaming
namespace SharpWiki.API.Queries
{
    using System.Collections.Generic;

    internal class LinksQueryResult
    {
        public ContinueSection @continue { get; set; }
        
        public string batchcomplete { get; set; }

        public QuerySection query { get; set; }

        public class ContinueSection
        {
            public string plcontinue { get; set; }

            public string @continue { get; set; }
        }

        public class QuerySection
        {
            public Dictionary<string, PageLinkInfo> pages { get; set; }
        }

        public class PageLinkInfo
        {
            public int pageid { get; set; }

            public int ns { get; set; }

            public string title { get; set; }

            public List<LinkInfo> links { get; set; }
        }

        public class LinkInfo
        {
            public int ns { get; set; }

            public string title { get; set; }
        }
    }
}