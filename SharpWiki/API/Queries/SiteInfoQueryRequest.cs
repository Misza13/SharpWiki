// ReSharper disable InconsistentNaming
namespace SharpWiki.API.Queries
{
    internal class SiteInfoQueryRequest : QueryRequest<SiteInfoQueryResult>
    {
        public string meta => "siteinfo";

        public string siprop => "general|namespaces";
    }
}