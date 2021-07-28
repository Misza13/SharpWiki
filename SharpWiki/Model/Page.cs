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

        public async IAsyncEnumerable<Page> GetLinks()
        {
            string plcontinue = null;

            while (true)
            {
                var request = new LinksQueryRequest(this.CanonicalTitle);

                if (!string.IsNullOrEmpty(plcontinue))
                {
                    request = request.WithContinuation(plcontinue);
                }
                
                var result = await this.Site.ApiWrapper.Get(request);
                
                var links = result.query.pages.First().Value.links;
            
                foreach (var linkInfo in links)
                {
                    yield return this.Site.GetPage(linkInfo.ns, linkInfo.title);
                }

                if (result.batchcomplete == "")
                {
                    yield break;
                }

                plcontinue = result.@continue.plcontinue;
            }
        }

        public async IAsyncEnumerable<Category> GetCategories()
        {
            string clcontinue = null;
            
            while (true)
            {
                var request = new CategoriesQueryRequest(this.CanonicalTitle);

                if (!string.IsNullOrEmpty(clcontinue))
                {
                    request = request.WithContinuation(clcontinue);
                }
                
                var result = await this.Site.ApiWrapper.Get(request);
                
                var categories = result.query.pages.First().Value.categories;
            
                foreach (var categoryInfo in categories)
                {
                    var ns = categoryInfo.ns;
                    var title = categoryInfo.title;
                    if (ns != 0)
                    {
                        title = title.Split(':', 2)[1];
                    }
                    
                    yield return this.Site.GetCategory(ns, title);
                }

                if (result.batchcomplete == "")
                {
                    yield break;
                }

                clcontinue = result.@continue.clcontinue;
            }
        }

        public override string ToString()
        {
            return $"Page({this.CanonicalTitle})";
        }
    }
}