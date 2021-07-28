namespace SharpWiki
{
    using System.Threading.Tasks;
    using API;

    public interface IApiWrapper
    {
        Task<TRes> Get<TRes>(ApiRequest<TRes> parameters);
    }
}