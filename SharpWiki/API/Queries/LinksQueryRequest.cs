// ReSharper disable InconsistentNaming
namespace SharpWiki.API.Queries
{
    internal class LinksQueryRequest : QueryRequest<LinksQueryResult>
    {
        public string prop => "links";

        public string titles { get; }

        public string plcontinue { get; private set; }

        public int pllimit { get; private set; } = 100;

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

        public LinksQueryRequest WithLimit(int limit)
        {
            this.pllimit = limit;
            return this;
        }
    }
}