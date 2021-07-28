// ReSharper disable InconsistentNaming
namespace SharpWiki.API
{
    public abstract class ApiRequest<TResult>
    {
        public string action { get; }

        internal ApiRequest(string action)
        {
            this.action = action;
        }
    }
}