namespace SharpWiki.API.Queries
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using SharpWiki.Model;
    using SharpWiki.Model.Queries;

    internal class CategoryMembersQuery : PaginatingQuery<CategoryMembersQueryRequest, CategoryMembersQueryResult, Page>, ICategoryMembersQuery
    {
        private readonly Category category;

        public CategoryMembersQuery(Category category)
        {
            this.category = category;
        }
        
        public override IAsyncEnumerator<Page> GetAsyncEnumerator(CancellationToken cancellationToken = new CancellationToken())
        {
            var site = this.category.Site;
            
            return this.Execute(
                    site.ApiWrapper,
                    () => new CategoryMembersQueryRequest(this.category.CanonicalTitle),
                    (request, c) => request.WithContinue(c),
                    result => result.query.categorymembers,
                    item =>
                    {
                        var ns = item.ns;
                        var title = item.title;
                        if (ns != 0)
                        {
                            title = title.Split(':', 2)[1];
                        }
                    
                        return site.GetPage(ns, title);
                    },
                    result => result?.@continue?.cmcontinue)
                .GetAsyncEnumerator(cancellationToken);
        }

        public ICategoryMembersQuery Descending()
        {
            this.RequestMods.Add(request =>
                request.WithDirection("descending"));
            return this;
        }

        public ICategoryMembersQuery ByTimestamp()
        {
            this.RequestMods.Add(request =>
                request.WithSort("timestamp"));
            return this;
        }

        public ICategoryMembersQuery OnlyPages()
        {
            this.RequestMods.Add(request =>
                request.WithType("page"));
            return this;
        }

        public ICategoryMembersQuery OnlySubcategories()
        {
            this.RequestMods.Add(request =>
                request.WithType("subcat"));
            return this;
        }

        public ICategoryMembersQuery OnlyToNamespaces(params int[] namespaceIds)
        {
            this.RequestMods.Add(request =>
                request.WithNamespaces(namespaceIds));
            return this;
        }

        public ICategoryMembersQuery OnlyToNamespaces(params string[] namespaceNames)
        {
            var namespaceIds = namespaceNames.Select(
                ns => this.category.Site.NamespacesByName[ns].Id).ToArray();
            this.RequestMods.Add(request =>
                request.WithNamespaces(namespaceIds));
            return this;
        }

        public ICategoryMembersQuery OnlyToNamespaces(params Namespace[] namespaces)
        {
            var namespaceIds = namespaces.Select(ns => ns.Id).ToArray();
            this.RequestMods.Add(request =>
                request.WithNamespaces(namespaceIds));
            return this;
        }

        public ICategoryMembersQuery OnlyFiles()
        {
            this.RequestMods.Add(request =>
                request.WithType("file"));
            return this;
        }
    }
}