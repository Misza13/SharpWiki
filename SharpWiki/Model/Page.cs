namespace SharpWiki.Model
{
    using System.Collections.Generic;
    using System.Linq;
    using API.Queries;

    public class Page
    {
        public MediaWikiSite Site { get; }

        public int NamespaceId { get; }

        public string Title { get; }

        public string CanonicalTitle
        {
            get
            {
                var namespaceName = this.Site.Namespaces[this.NamespaceId].Name;
                return string.IsNullOrEmpty(namespaceName)
                    ? this.Title
                    : $"{this.Site.Namespaces[this.NamespaceId].Name}:{this.Title}";
            }
        }

        internal Page(MediaWikiSite site, int namespaceId, string title)
        {
            this.Site = site;
            this.NamespaceId = namespaceId;
            this.Title = title;
        }

        public IAsyncEnumerable<Page> GetLinks()
        {
            return this.Site.ApiWrapper.RunPaginatingQuery
                <LinksQueryRequest, LinksQueryResult, LinksQueryResult.LinkInfo, Page>(
                    () => new LinksQueryRequest(this.CanonicalTitle),
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
                    
                        return this.Site.GetCategory(ns, title);
                    },
                    result => result?.@continue?.plcontinue);
        }

        public IAsyncEnumerable<Category> GetCategories()
        {
            return this.Site.ApiWrapper.RunPaginatingQuery
                <CategoriesQueryRequest, CategoriesQueryResult, CategoriesQueryResult.CategoryInfo, Category>(
                    () => new CategoriesQueryRequest(this.CanonicalTitle),
                    (request, c) => request.WithContinuation(c),
                    result => result.query.pages.First().Value.categories,
                    item =>
                    {
                        var ns = item.ns;
                        var title = item.title;
                        if (ns != 0)
                        {
                            title = title.Split(':', 2)[1];
                        }
                        
                        return this.Site.GetCategory(ns, title);
                    },
                    result => result?.@continue?.clcontinue);
        }

        public override string ToString()
        {
            return $"Page({this.CanonicalTitle})";
        }
    }
}