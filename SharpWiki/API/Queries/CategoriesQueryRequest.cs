namespace SharpWiki.API.Queries
{
    internal class CategoriesQueryRequest : QueryRequest<CategoriesQueryResult>
    {
        public string prop => "categories";
        
        public string titles { get; }

        public string clcontinue { get; private set; }

        public int cllimit { get; private set; } = 100;

        public CategoriesQueryRequest(string titles, string clcontinue = null)
        {
            this.titles = titles;
            this.clcontinue = clcontinue;
        }
        
        public CategoriesQueryRequest WithContinuation(string clcontinue)
        {
            this.clcontinue = clcontinue;
            return this;
        }

        public CategoriesQueryRequest WithLimit(int limit)
        {
            this.cllimit = limit;
            return this;
        }
    }
}