namespace SharpWiki.Model
{
    using System.Collections.Generic;
    using API.Queries;

    public class Category : Page
    {
        internal Category(MediaWikiSite site, int namespaceId, string title) : base(site, namespaceId, title)
        {
        }

        public IAsyncEnumerable<Page> GetMembers()
        {
            return this.Site.ApiWrapper.RunPaginatingQuery
                <CategoryMembersQueryRequest, CategoryMembersQueryResult, CategoryMembersQueryResult.PageInfo, Page>(
                    () => new CategoryMembersQueryRequest(this.CanonicalTitle),
                    (request, c) => request.WithContinuation(c),
                    result => result.query.categorymembers,
                    item => this.Site.GetPage(item.ns, item.title),
                    result => result.@continue?.cmcontinue);
        }
    }
}