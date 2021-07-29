namespace SharpWiki.Model.Queries
{
    using System.Collections.Generic;
    
    /// <summary>
    /// Represents a collection of categories tht a page belongs to.
    /// Has a fluent interface for refining the results using native API parameters.
    /// </summary>
    public interface ICategoriesQuery : IAsyncEnumerable<Category>
    {
        /// <summary>
        /// Modify the query so that categories are sorted in descending order.
        /// </summary>
        /// <returns>Self, for fluent chaining or iteration</returns>
        ICategoriesQuery Descending();

        /// <summary>
        /// Modify the query so that it returns only hidden categories.
        /// </summary>
        /// <returns>Self, for fluent chaining or iteration</returns>
        ICategoriesQuery OnlyHidden();
        
        /// <summary>
        /// Modify the query so that it returns only non-hidden categories.
        /// </summary>
        /// <returns>Self, for fluent chaining or iteration</returns>
        ICategoriesQuery OnlyNonHidden();
    }
}