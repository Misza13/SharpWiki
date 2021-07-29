// ReSharper disable InconsistentNaming
namespace SharpWiki.API.Queries
{
    internal class LinksQueryRequest : QueryRequest<LinksQueryResult>
    {
        public string prop => "links";

        public string titles { get; }

        public string plcontinue { get; private set; }

        public string pllimit => "max";

        public string pldir { get; private set; }

        public string plnamespace { get; set; }

        public LinksQueryRequest(string titles)
        {
            this.titles = titles;
        }

        public LinksQueryRequest WithContinue(string @continue)
        {
            this.plcontinue = @continue;
            return this;
        }

        public LinksQueryRequest WithDirection(string dir)
        {
            this.pldir = dir;
            return this;
        }

        public LinksQueryRequest WithNamespaces(params int[] namespaceIds)
        {
            this.plnamespace = string.Join('|', namespaceIds);
            return this;
        }
    }
}