// ReSharper disable InconsistentNaming
namespace SharpWiki.API.Queries
{
    using System.Collections.Generic;

    internal class SiteInfoQueryResult
    {
        public QuerySection query { get; set; }
        
        internal class QuerySection
        {
            public Dictionary<int, NamespaceInfo> namespaces { get; set; }
        }
     
        internal class NamespaceInfo
        {
            public int id { get; set; }

            public string canonical { get; set; }
        }
    }
}