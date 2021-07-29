namespace SharpWiki.API.Queries
{
    internal class CategoryMembersQueryRequest : QueryRequest<CategoryMembersQueryResult>
    {
        public string list => "categorymembers";
        
        public string cmtitle { get; }

        public string cmcontinue { get; private set; }

        public string cmlimit { get; private set; } = "max";

        public string cmsort { get; private set; } = "sortkey";
        
        public string cmdir { get; private set; } = "ascending";

        public string cmtype { get; private set; }

        public string cmnamespace { get; private set; }
        
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

        public CategoryMembersQueryRequest WithSort(string sort)
        {
            this.cmsort = sort;
            return this;
        }

        public CategoryMembersQueryRequest WithDirection(string direction)
        {
            this.cmdir = direction;
            return this;
        }

        public CategoryMembersQueryRequest WithType(string type)
        {
            this.cmtype = type;
            return this;
        }
        
        public CategoryMembersQueryRequest WithNamespaces(params int[] namespaceIds)
        {
            this.cmnamespace = string.Join('|', namespaceIds);
            return this;
        }
    }
}