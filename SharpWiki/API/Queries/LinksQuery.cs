namespace SharpWiki.API.Queries
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using Model;

    public class LinksQuery : PaginatingQuery<Page>
    {
        private readonly MediaWikiSite site;
        private readonly Page page;

        private readonly List<Func<LinksQueryRequest, LinksQueryRequest>> requestMods =
            new List<Func<LinksQueryRequest, LinksQueryRequest>>();
        
        internal LinksQuery(MediaWikiSite site, Page page)
        {
            this.site = site;
            this.page = page;
        }
        
        public override IAsyncEnumerator<Page> GetAsyncEnumerator(CancellationToken cancellationToken = new CancellationToken())
        {
            return this.Execute<LinksQueryRequest, LinksQueryResult, LinksQueryResult.LinkInfo>(
                this.site.ApiWrapper,
                this.BuildRequest,
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
            this.requestMods.Add(req => req.WithDirection("descending"));
            return this;
        }

        public LinksQuery OnlyToNamespaces(params int[] namespaceIds)
        {
            this.requestMods.Add(req => req.WithNamespaces(namespaceIds));
            return this;
        }

        public LinksQuery OnlyToNamespaces(params string[] namespaceNames)
        {
            var namespaceIds = namespaceNames.Select(ns => this.site.NamespacesByName[ns].Id).ToArray();
            this.requestMods.Add(req => req.WithNamespaces(namespaceIds));
            return this;
        }

        public LinksQuery OnlyToNamespaces(params Namespace[] namespaces)
        {
            var namespaceIds = namespaces.Select(ns => ns.Id).ToArray();
            this.requestMods.Add(req => req.WithNamespaces(namespaceIds));
            return this;
        }

        private LinksQueryRequest BuildRequest()
        {
            var req = new LinksQueryRequest(this.page.CanonicalTitle);

            foreach (var requestMod in this.requestMods)
            {
                req = requestMod(req);
            }

            return req;
        }
    }
}