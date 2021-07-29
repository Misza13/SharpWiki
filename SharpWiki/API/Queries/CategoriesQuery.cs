namespace SharpWiki.API.Queries
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using SharpWiki.Model;
    using SharpWiki.Model.Queries;

    internal class CategoriesQuery : PaginatingQuery<CategoriesQueryRequest, CategoriesQueryResult, Category>, ICategoriesQuery
    {
        private readonly Page page;

        public CategoriesQuery(Page page)
        {
            this.page = page;
        }

        public override IAsyncEnumerator<Category> GetAsyncEnumerator(CancellationToken cancellationToken = new CancellationToken())
        {
            var site = this.page.Site;
            
            return this.Execute(
                    site.ApiWrapper,
                    () => new CategoriesQueryRequest(this.page.CanonicalTitle),
                    (request, c) => request.WithContinue(c),
                    result => result.query.pages.First().Value.categories,
                    item =>
                    {
                        var ns = item.ns;
                        var title = item.title;
                        if (ns != 0)
                        {
                            title = title.Split(':', 2)[1];
                        }
                    
                        return site.GetCategory(ns, title);
                    },
                    result => result?.@continue?.clcontinue)
                .GetAsyncEnumerator(cancellationToken);
        }

        public ICategoriesQuery Descending()
        {
            this.RequestMods.Add(request => 
                request.WithDirection("descending"));
            return this;
        }

        public ICategoriesQuery OnlyHidden()
        {
            this.RequestMods.Add(request => 
                request.WithShow("hidden"));
            return this;
        }

        public ICategoriesQuery OnlyNonHidden()
        {
            this.RequestMods.Add(request => 
                request.WithShow("!hidden"));
            return this;
        }
    }
}