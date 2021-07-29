namespace SharpWiki.API.Queries
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using Model;

    public class LinksQuery : PaginatingQuery<LinksQueryRequest, LinksQueryResult, Page>
    {
        private readonly MediaWikiSite site;
        private readonly Page page;

        internal LinksQuery(MediaWikiSite site, Page page)
        {
            this.site = site;
            this.page = page;
        }
        
        public override IAsyncEnumerator<Page> GetAsyncEnumerator(CancellationToken cancellationToken = new CancellationToken())
        {
            return this.Execute(
                this.site.ApiWrapper,
                () => new LinksQueryRequest(this.page.CanonicalTitle),
                (request, c) => request.WithContinuation(c),
                result => result.query.pages.First().Value.links,
                item =>
                {
                    var ns = item.ns;
                    var title = item.title;
                    if (ns != 0)
                    {
                        title = title.Split(':', 2)[1];
                    }
                    
                    return this.site.GetCategory(ns, title);
                },
                result => result?.@continue?.plcontinue)
                .GetAsyncEnumerator(cancellationToken);
        }

        public LinksQuery Descending()
        {
            this.RequestMods.Add(req => req.WithDirection("descending"));
            return this;
        }

        public LinksQuery OnlyToNamespaces(params int[] namespaceIds)
        {
            this.RequestMods.Add(req => req.WithNamespaces(namespaceIds));
            return this;
        }

        public LinksQuery OnlyToNamespaces(params string[] namespaceNames)
        {
            var namespaceIds = namespaceNames.Select(ns => this.site.NamespacesByName[ns].Id).ToArray();
            this.RequestMods.Add(req => req.WithNamespaces(namespaceIds));
            return this;
        }

        public LinksQuery OnlyToNamespaces(params Namespace[] namespaces)
        {
            var namespaceIds = namespaces.Select(ns => ns.Id).ToArray();
            this.RequestMods.Add(req => req.WithNamespaces(namespaceIds));
            return this;
        }
    }
}