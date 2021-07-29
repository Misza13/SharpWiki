namespace SharpWiki.API.Queries
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using Model;
    using SharpWiki.Model.Queries;

    internal class LinksQuery : PaginatingQuery<LinksQueryRequest, LinksQueryResult, Page>, ILinksQuery
    {
        private readonly Page page;

        public LinksQuery(MediaWikiSite site, Page page) : base(site)
        {
            this.page = page;
        }
        
        public override IAsyncEnumerator<Page> GetAsyncEnumerator(CancellationToken cancellationToken = new CancellationToken())
        {
            return this.Execute(
                () => new LinksQueryRequest(this.page.CanonicalTitle),
                (request, c) => request.WithContinue(c),
                result => result.query.pages.First().Value.links,
                item =>
                {
                    var ns = item.ns;
                    var title = item.title;
                    if (ns != 0)
                    {
                        title = title.Split(':', 2)[1];
                    }
                    
                    return this.Site.GetPage(ns, title);
                },
                result => result?.@continue?.plcontinue)
                .GetAsyncEnumerator(cancellationToken);
        }

        public ILinksQuery Descending()
        {
            this.RequestMods.Add(request =>
                request.WithDirection("descending"));
            return this;
        }

        public ILinksQuery OnlyToNamespaces(params int[] namespaceIds)
        {
            this.RequestMods.Add(request =>
                request.WithNamespaces(namespaceIds));
            return this;
        }

        public ILinksQuery OnlyToNamespaces(params string[] namespaceNames)
        {
            var namespaceIds = namespaceNames.Select(
                ns => this.Site.NamespacesByName[ns].Id).ToArray();
            this.RequestMods.Add(request =>
                request.WithNamespaces(namespaceIds));
            return this;
        }

        public ILinksQuery OnlyToNamespaces(params Namespace[] namespaces)
        {
            var namespaceIds = namespaces.Select(ns => ns.Id).ToArray();
            this.RequestMods.Add(request =>
                request.WithNamespaces(namespaceIds));
            return this;
        }
    }
}