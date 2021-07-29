namespace SharpWiki.Model.Queries
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents a collection of pages belonging to a category.
    /// Has a fluent interface for refining the results using native API parameters.
    /// </summary>

    public interface ICategoryMembersQuery : IAsyncEnumerable<Page>
    {
        /// <summary>
        /// Modify the query so that members are sorted in descending order.
        /// </summary>
        /// <returns>Self, for fluent chaining or iteration</returns>
        ICategoryMembersQuery Descending();

        /// <summary>
        /// Modify the query so that members are sorted by timestamp.
        /// </summary>
        /// <returns>Self, for fluent chaining or iteration</returns>
        ICategoryMembersQuery ByTimestamp();

        /// <summary>
        /// Modify the query so that only pages are returned.
        /// Not compatible with <see cref="ByTimestamp"/>, use <see cref="OnlyToNamespaces(int[])"/> instead.
        /// </summary>
        /// <returns>Self, for fluent chaining or iteration</returns>
        ICategoryMembersQuery OnlyPages();
        
        /// <summary>
        /// Modify the query so that only subcategories are returned.
        /// Not compatible with <see cref="ByTimestamp"/>, use <see cref="OnlyToNamespaces(int[])"/> instead.
        /// </summary>
        /// <returns>Self, for fluent chaining or iteration</returns>
        ICategoryMembersQuery OnlySubcategories();
        
        /// <summary>
        /// Modify the query so that only files are returned.
        /// Not compatible with <see cref="ByTimestamp"/>, use <see cref="OnlyToNamespaces(int[])"/> instead.
        /// </summary>
        /// <returns>Self, for fluent chaining or iteration</returns>
        ICategoryMembersQuery OnlyFiles();
        
        /// <summary>
        /// Modify the query so that only pages from given namespaces are returned.
        /// </summary>
        /// <param name="namespaceIds"><code>id</code>s of namespaces to filter on</param>
        /// <returns>Self, for fluent chaining or iteration</returns>
        ICategoryMembersQuery OnlyToNamespaces(params int[] namespaceIds);

        /// <summary>
        /// Modify the query so that only pages from given namespaces are returned.
        /// </summary>
        /// <param name="namespaceNames">names of namespaces to filter on</param>
        /// <returns>Self, for fluent chaining or iteration</returns>
        ICategoryMembersQuery OnlyToNamespaces(params string[] namespaceNames);

        /// <summary>
        /// Modify the query so that only pages from given namespaces are returned.
        /// </summary>
        /// <param name="namespaces"><see cref="Namespace"/> objects to filter on</param>
        /// <returns>Self, for fluent chaining or iteration</returns>
        ICategoryMembersQuery OnlyToNamespaces(params Namespace[] namespaces);
    }
}