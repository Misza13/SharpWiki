namespace SharpWiki.Model.Queries
{
    using System.Collections.Generic;

    public interface ILinksQuery : IAsyncEnumerable<Page>
    {
        ILinksQuery Descending();

        ILinksQuery OnlyToNamespaces(params int[] namespaceIds);

        ILinksQuery OnlyToNamespaces(params string[] namespaceNames);

        ILinksQuery OnlyToNamespaces(params Namespace[] namespaces);
    }
}