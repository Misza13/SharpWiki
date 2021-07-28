namespace SharpWiki.API.Queries
{
    internal class CategoryMembersQueryRequest : QueryRequest<CategoryMembersQueryResult>
    {
        public string list => "categorymembers";
        
        public string cmtitle { get; }

        public string cmcontinue { get; private set; }

        public int cmlimit { get; private set; } = 100;
        
        public CategoryMembersQueryRequest(string title, string cmcontinue = null)
        {
            this.cmtitle = title;
            this.cmcontinue = cmcontinue;
        }
        
        public CategoryMembersQueryRequest WithContinuation(string cmcontinue)
        {
            this.cmcontinue = cmcontinue;
            return this;
        }

        public CategoryMembersQueryRequest WithLimit(int limit)
        {
            this.cmlimit = limit;
            return this;
        }
    }
}