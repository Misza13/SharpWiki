// ReSharper disable InconsistentNaming
namespace SharpWiki.API.Queries
{
    using System.Collections.Generic;

    internal class CategoriesQueryResult
    {
        public ContinueSection @continue { get; set; }
        
        public string batchcomplete { get; set; }

        public QuerySection query { get; set; }
        
        internal class ContinueSection
        {
            public string clcontinue { get; set; }

            public string @continue { get; set; }
        }
        
        internal class QuerySection
        {
            public Dictionary<string, PageCategoryInfo> pages { get; set; }
        }
        
        internal class PageCategoryInfo
        {
            public int pageid { get; set; }

            public int ns { get; set; }

            public string title { get; set; }

            public List<CategoryInfo> categories { get; set; }
        }
        
        internal class CategoryInfo
        {
            public int ns { get; set; }

            public string title { get; set; }
        }
    }
}