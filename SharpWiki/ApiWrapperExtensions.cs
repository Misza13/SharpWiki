namespace SharpWiki
{
    using System;
    using System.Collections.Generic;
    using API.Queries;

    internal static class ApiWrapperExtensions
    {
        public static async IAsyncEnumerable<TMod> RunPaginatingQuery<TReq, TRes, TItem, TMod>(
            this IApiWrapper apiWrapper,
            Func<TReq> requestBuilder,
            Action<TReq, string> continuationSetter,
            Func<TRes, IEnumerable<TItem>> itemListGetter,
            Func<TItem, TMod> resultProducer,
            Func<TRes, string> continuationGetter)
        where TReq : QueryRequest<TRes>
        {
            string continuation = null;
            
            while (true)
            {
                var request = requestBuilder();

                if (!string.IsNullOrEmpty(continuation))
                {
                    continuationSetter(request, continuation);
                }
                
                var result = await apiWrapper.Get(request);
                
                var items = itemListGetter(result);
            
                foreach (var item in items)
                {
                    yield return resultProducer(item);
                }

                var next = continuationGetter(result);

                if (string.IsNullOrEmpty(next))
                {
                    yield break;
                }

                continuation = next;
            }
        }
    }
}