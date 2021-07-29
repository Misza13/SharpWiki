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

        public LinksQueryRequest(string titles, string plcontinue = null)
        {
            this.titles = titles;
            this.plcontinue = plcontinue;
        }

        public LinksQueryRequest WithContinuation(string plcontinue)
        {
            this.plcontinue = plcontinue;
            return this;
        }

        public LinksQueryRequest WithDirection(string pldir)
        {
            this.pldir = pldir;
            return this;
        }

        public LinksQueryRequest WithNamespaces(params int[] namespaceIds)
        {
            this.plnamespace = string.Join('|', namespaceIds);
            return this;
        }
    }
}