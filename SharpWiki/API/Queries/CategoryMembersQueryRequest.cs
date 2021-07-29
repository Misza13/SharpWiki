namespace SharpWiki.API.Queries
{
    internal class CategoryMembersQueryRequest : QueryRequest<CategoryMembersQueryResult>
    {
        public string list => "categorymembers";
        
        public string cmtitle { get; }

        public string cmcontinue { get; private set; }

        public string cmlimit { get; private set; } = "max";
        
        public CategoryMembersQueryRequest(string title)
        {
            this.cmtitle = title;
        }
        
        public CategoryMembersQueryRequest WithContinue(string @continue)
        {
            this.cmcontinue = @continue;
            return this;
        }

        public CategoryMembersQueryRequest WithLimit(int limit)
        {
            this.cmlimit = limit.ToString();
            return this;
        }
    }
}