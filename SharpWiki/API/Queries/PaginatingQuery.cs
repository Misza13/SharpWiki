namespace SharpWiki.API.Queries
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

    internal abstract class PaginatingQuery<TReq, TRes, TMod> : IAsyncEnumerable<TMod>
        where TReq : ApiRequest<TRes>
    {
        protected readonly MediaWikiSite Site;
        
        protected readonly List<Func<TReq, TReq>> RequestMods = new List<Func<TReq, TReq>>();

        protected PaginatingQuery(MediaWikiSite site)
        {
            this.Site = site;
        }
        
        protected async IAsyncEnumerable<TMod> Execute<TItem>(
            Func<TReq> requestBuilder,
            Action<TReq, string> continuationSetter,
            Func<TRes, IEnumerable<TItem>> itemListGetter,
            Func<TItem, TMod> resultProducer,
            Func<TRes, string> continuationGetter)
        {
            var apiWrapper = this.Site.ApiWrapper;
            
            string continuation = null;
            
            while (true)
            {
                var request = requestBuilder();
                
                foreach (var requestMod in this.RequestMods)
                {
                    request = requestMod(request);
                }

                if (!string.IsNullOrEmpty(continuation))
                {
                    continuationSetter(request, continuation);
                }
                
                var result = await apiWrapper.Get(request);
                
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