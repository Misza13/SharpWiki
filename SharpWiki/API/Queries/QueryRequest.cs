namespace SharpWiki.API.Queries
{
    public abstract class QueryRequest<TResult> : ApiRequest<TResult>
    {
        internal QueryRequest() : base("query")
        {
        }
    }
}