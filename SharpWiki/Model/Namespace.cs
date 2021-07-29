namespace SharpWiki.Model
{
    public class Namespace
    {
        /// <summary>
        /// The <see cref="MediaWikiSite"/> that this namespace belongs to.
        /// </summary>
        public MediaWikiSite Site { get; }
        
        /// <summary>
        /// <code>id</code> of the namespace.
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Name of the namespace. Empty string for main namespace (id = 0).
        /// </summary>
        public string Name { get; }

        internal Namespace(MediaWikiSite site, int id, string name)
        {
            this.Site = site;
            this.Id = id;
            this.Name = name;
        }

        /// <summary>
        /// Get a <see cref="Page"/> from this namespace by its title.
        /// </summary>
        /// <param name="title">Title of the page (without namespace)</param>
        /// <returns>The <see cref="Page"/></returns>
        public Page GetPage(string title)
        {
            return new Page(this.Site, this.Id, title);
        }
    }
}