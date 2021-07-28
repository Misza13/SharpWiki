namespace SharpWiki.API.Queries
{
    internal abstract class QueryRequest<TResult> : ApiRequest<TResult>
    {
        internal QueryRequest() : base("query")
        {
        }
    }
}