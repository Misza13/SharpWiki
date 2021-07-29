namespace SharpWiki.Model
{
    using API.Queries;
    using SharpWiki.Model.Queries;

    public class Category : Page
    {
        internal Category(MediaWikiSite site, int namespaceId, string title) : base(site, namespaceId, title)
        {
        }

        /// <summary>
        /// Get all pages that are contained in this category.
        /// </summary>
        /// <returns>A query listing the <see cref="Page"/>s,
        /// can be refined with a fluent interface, see <see cref="ILinksQuery"/>.</returns>
        public ICategoryMembersQuery GetMembers()
        {
            return new CategoryMembersQuery(this);
        }
    }
}