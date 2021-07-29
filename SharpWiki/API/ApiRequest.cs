// ReSharper disable InconsistentNaming
namespace SharpWiki.API
{
    // ReSharper disable once UnusedTypeParameter
    public abstract class ApiRequest<TResult>
    {
        public string action { get; }

        internal ApiRequest(string action)
        {
            this.action = action;
        }
    }
}