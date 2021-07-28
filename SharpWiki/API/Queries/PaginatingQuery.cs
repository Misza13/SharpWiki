namespace SharpWiki.API.Queries
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

    public abstract class PaginatingQuery<TMod> : IAsyncEnumerable<TMod>
    {
        protected async IAsyncEnumerable<TMod> Execute<TReq, TRes, TItem>(
            IApiWrapper apiWrapper,
            Func<TReq> requestBuilder,
            Action<TReq, string> continuationSetter,
            Func<TRes, IEnumerable<TItem>> itemListGetter,
            Func<TItem, TMod> resultProducer,
            Func<TRes, string> continuationGetter)
        {
            string continuation = null;
            
            while (true)
            {
                var request = requestBuilder();

                if (!string.IsNullOrEmpty(continuation))
                {
                    continuationSetter(request, continuation);
                }
                
                var result = await apiWrapper.Get<TRes>((ApiRequest<TRes>)(object)request);
                
                var items = itemListGetter(result);
            
                if (items != null)
                {
                    foreach (var item in items)
                    {
                        yield return resultProducer(item);
                    }
                }

                var next = continuationGetter(result);

                if (string.IsNullOrEmpty(next))
                {
                    yield break;
                }

                continuation = next;
            }
        }

        public abstract IAsyncEnumerator<TMod> GetAsyncEnumerator(
            CancellationToken cancellationToken = new CancellationToken());
    }
}