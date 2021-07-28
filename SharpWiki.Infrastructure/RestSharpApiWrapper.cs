namespace SharpWiki.Infrastructure
{
    using System;
    using System.Threading.Tasks;
    using API;
    using RestSharp;

    public class RestSharpApiWrapper : IApiWrapper
    {
        private readonly RestClient restClient;

        public RestSharpApiWrapper(Uri apiUrl)
        {
            this.restClient = new RestClient(apiUrl);
        }
        
        public async Task<TRes> Get<TRes>(ApiRequest<TRes> parameters)
        {
            var request = new RestRequest(Method.GET);
            
            foreach (var prop in parameters.GetType().GetProperties())
            {
                var propValue = prop.GetValue(parameters);
                if (propValue == null) continue;

                request.AddQueryParameter(
                    prop.Name,
                    propValue.ToString()!);
            }

            request.AddQueryParameter("format", "json");

            return await this.restClient.GetAsync<TRes>(request);
        }
    }
}