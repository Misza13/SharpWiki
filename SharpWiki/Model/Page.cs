namespace SharpWiki.Model
{
    using System.Collections.Generic;
    using System.Linq;
    using API.Queries;
    using SharpWiki.Model.Queries;

    public class Page
    {
        /// <summary>
        /// The <see cref="MediaWikiSite"/> this page belongs to.
        /// </summary>
        public MediaWikiSite Site { get; }

        /// <summary>
        /// <code>id</code> of namespace that this page belongs to.
        /// </summary>
        public int NamespaceId { get; }

        /// <summary>
        /// <see cref="Namespace"/> that this page belongs to.
        /// </summary>
        public Namespace Namespace => this.Site.Namespaces[this.NamespaceId];

        /// <summary>
        /// Title of page without namespace.
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// Full title of page (includes namespace).
        /// </summary>
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

        /// <summary>
        /// Get all pages that this page links to.
        /// </summary>
        /// <returns>A query listing the <see cref="Page"/>s,
        /// can be refined with a fluent interface, see <see cref="ILinksQuery"/>.</returns>
        public ILinksQuery GetLinks()
        {
            return new LinksQuery(this.Site, this);
        }

        /// <summary>
        /// Get all categories that this page belongs to.
        /// </summary>
        /// <returns></returns>
        public IAsyncEnumerable<Category> GetCategories()
        {
            return this.Site.ApiWrapper.RunPaginatingQuery
                <CategoriesQueryRequest, CategoriesQueryResult, CategoriesQueryResult.CategoryInfo, Category>(
                    () => new CategoriesQueryRequest(this.CanonicalTitle),
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