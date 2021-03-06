namespace SharpWiki
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using API.Queries;
    using Model;

    /// <summary>
    /// Represents a whole MediaWiki site.
    /// Provides an entry point to retrieving all data such as articles, users etc.
    /// </summary>
    public class MediaWikiSite
    {
        internal IApiWrapper ApiWrapper { get; }

        internal Dictionary<int, Namespace> Namespaces { get; } =
            new Dictionary<int, Namespace>();

        internal Dictionary<string, Namespace> NamespacesByName { get; } =
            new Dictionary<string, Namespace>();

        /// <summary>
        /// Create a new instance of the site wrapper, pointing to a specific
        /// MediaWiki installation.
        ///
        /// Before making any requests, you <b>MUST</b> invoke <see cref="LoadMetadata"/> first.
        /// </summary>
        /// <param name="apiWrapper">
        /// An instance of an <see cref="IApiWrapper"/> implementation that
        /// performs the actual requests to the API endpoint.
        /// </param>
        public MediaWikiSite(IApiWrapper apiWrapper)
        {
            this.ApiWrapper = apiWrapper;
        }

        /// <summary>
        /// Loads the site metadata such as namespace definitions and stores it
        /// internally. Required to be called once to ensure the model functions
        /// properly.
        /// </summary>
        public async Task LoadMetadata()
        {
            var result = await this.ApiWrapper.Get(
                new SiteInfoQueryRequest());

            this.Namespaces.Clear();
            this.NamespacesByName.Clear();
            
            foreach (var (_, ns) in result.query.namespaces)
            {
                var nsName = ns.name ?? ns.canonical ?? "";
                
                var @namespace = new Namespace(
                    this,
                    ns.id,
                    nsName);

                this.Namespaces[ns.id] = @namespace;
                this.NamespacesByName[nsName] = @namespace;
            }
        }

        /// <summary>
        /// Get a namespace by its id.
        /// </summary>
        /// <param name="id">id of the namespace</param>
        /// <returns>An instance of <see cref="Namespace"/></returns>
        /// <exception cref="KeyNotFoundException">
        /// If a namespace with the given id does not exist or if this instance
        /// was not initialized using <see cref="LoadMetadata"/>.
        /// </exception>
        public Namespace GetNamespace(int id) => this.Namespaces[id];

        /// <summary>
        /// Get a namespace by its canonical name.
        /// </summary>
        /// <param name="name">Canonical name of the namespace</param>
        /// <returns>An instance of <see cref="Namespace"/></returns>
        /// <exception cref="KeyNotFoundException">
        /// If a namespace with the given name does not exist or if this instance
        /// was not initialized using <see cref="LoadMetadata"/>.
        /// </exception>
        public Namespace GetNamespace(string name) => this.NamespacesByName[name];

        /// <summary>
        /// Get an article (that is, page from namespace with <c>id = 0</c>) by its title.
        /// </summary>
        /// <param name="title">Title of the article</param>
        /// <returns>The article</returns>
        public Page GetArticle(string title) => this.GetPage(0, title);
        
        /// <summary>
        /// Get a page on the site.
        /// </summary>
        /// <param name="namespaceId"><c>id</c> of the namespace</param>
        /// <param name="title">Title of the page</param>
        /// <returns>The <see cref="Page"/></returns>
        public Page GetPage(int namespaceId, string title)
        {
            if (namespaceId != 0 && this.Namespaces.ContainsKey(namespaceId))
            {
                var nsPrefix = this.Namespaces[namespaceId].Name + ":";
                
                if (title.StartsWith(nsPrefix))
                {
                    title = title[nsPrefix.Length..];
                }
            }
            
            return new Page(this, namespaceId, title);
        }

        /// <summary>
        /// Get a page on the site.
        /// </summary>
        /// <param name="namespaceName">Name of the namespace</param>
        /// <param name="title">Title of the page</param>
        /// <returns>The <see cref="Page"/></returns>
        public Page GetPage(string namespaceName, string title)
        {
            return this.GetNamespace(namespaceName).GetPage(title);
        }

        /// <summary>
        /// Get a category on the site.
        /// </summary>
        /// <param name="namespaceId"><c>id</c> of the namespace</param>
        /// <param name="title">Title of the category</param>
        /// <returns>The <see cref="Page"/></returns>
        public Category GetCategory(int namespaceId, string title)
        {
            // TODO: Is the namespaceId always fixed?
            return new Category(this, namespaceId, title);
        }
    }
}