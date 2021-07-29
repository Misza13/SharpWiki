namespace SharpWiki.Model.Queries
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents a collection of links leading out from a given page.
    /// Has a fluent interface for refining the results using native API parameters.
    /// </summary>
    public interface ILinksQuery : IAsyncEnumerable<Page>
    {
        /// <summary>
        /// Modify the query so that links are sorted in descending order.
        /// </summary>
        /// <returns>Self, for fluent chaining or iteration</returns>
        ILinksQuery Descending();

        /// <summary>
        /// Modify the query so that only links to given namespaces are returned.
        /// </summary>
        /// <param name="namespaceIds"><c>id</c>s of namespaces to filter on</param>
        /// <returns>Self, for fluent chaining or iteration</returns>
        ILinksQuery OnlyToNamespaces(params int[] namespaceIds);

        /// <summary>
        /// Modify the query so that only links to given namespaces are returned.
        /// </summary>
        /// <param name="namespaceNames">names of namespaces to filter on</param>
        /// <returns>Self, for fluent chaining or iteration</returns>
        ILinksQuery OnlyToNamespaces(params string[] namespaceNames);

        /// <summary>
        /// Modify the query so that only links to given namespaces are returned.
        /// </summary>
        /// <param name="namespaces"><see cref="Namespace"/> objects to filter on</param>
        /// <returns>Self, for fluent chaining or iteration</returns>
        ILinksQuery OnlyToNamespaces(params Namespace[] namespaces);
    }
}