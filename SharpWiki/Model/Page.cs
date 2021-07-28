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

        public override string ToString()
        {
            return $"Page({this.CanonicalTitle})";
        }
    }
}