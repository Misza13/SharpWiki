namespace SharpWiki.Model
{
    public class Namespace
    {
        public MediaWikiSite Site { get; }
        
        public int Id { get; }

        public string Name { get; }

        internal Namespace(MediaWikiSite site, int id, string name)
        {
            this.Site = site;
            this.Id = id;
            this.Name = name;
        }

        public Page GetPage(string title)
        {
            return new Page(this.Site, this.Id, title);
        }
    }
}