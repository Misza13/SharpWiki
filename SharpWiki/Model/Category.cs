namespace SharpWiki.Model
{
    public class Category : Page
    {
        internal Category(MediaWikiSite site, int namespaceId, string title) : base(site, namespaceId, title)
        {
        }
    }
}