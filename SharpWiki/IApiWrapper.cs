namespace SharpWiki
{
    using System.Threading.Tasks;
    using API;

    /// <summary>
    /// Represents a wrapper around the MediaWiki API.
    /// Is responsible for issuing web requests.
    /// </summary>
    public interface IApiWrapper
    {
        /// <summary>
        /// Issue a GET request to the API and return a typed response.
        /// </summary>
        /// <param name="parameters">An object representing the request parameters</param>
        /// <typeparam name="TRes">Type of the result returned by the API</typeparam>
        /// <returns>Asynchronous web result</returns>
        Task<TRes> Get<TRes>(ApiRequest<TRes> parameters);
    }
}