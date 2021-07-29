namespace SharpWiki.API.Queries
{
    internal class CategoriesQueryRequest : QueryRequest<CategoriesQueryResult>
    {
        public string prop => "categories";
        
        public string titles { get; }

        public string clcontinue { get; private set; }

        public string cllimit { get; private set; } = "max";

        public string cldir { get; set; } = "ascending";

        public string clshow { get; set; }

        public CategoriesQueryRequest(string titles)
        {
            this.titles = titles;
        }
        
        public CategoriesQueryRequest WithContinue(string @continue)
        {
            this.clcontinue = @continue;
            return this;
        }

        public CategoriesQueryRequest WithLimit(int limit)
        {
            this.cllimit = limit.ToString();
            return this;
        }

        public CategoriesQueryRequest WithDirection(string direction)
        {
            this.cldir = direction;
            return this;
        }

        public CategoriesQueryRequest WithShow(string show)
        {
            this.clshow = show;
            return this;
        }
    }
}