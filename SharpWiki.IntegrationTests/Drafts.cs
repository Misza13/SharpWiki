using NUnit.Framework;

namespace SharpWiki.IntegrationTests
{
    using System;
    using System.Threading.Tasks;
    using Infrastructure;

    [TestFixture]
    public class Drafts
    {
        private MediaWikiSite site;

        [SetUp]
        public async Task Setup()
        {
            var actionWrapper = new RestSharpApiWrapper(
                new Uri("https://en.wikipedia.org/w/api.php"));
            this.site = new MediaWikiSite(actionWrapper);

            await this.site.LoadMetadata();
        }

        [Test]
        public async Task Blah()
        {
            var page = this.site
                .GetNamespace("")
                .GetPage("Albert Einstein");

            await foreach (var link in page.GetLinks())
            {
                Console.WriteLine(link);
            }
        }
        
        [Test]
        public async Task Misza13()
        {
            var page = this.site
                .GetNamespace("User")
                .GetPage("Misza13");

            await foreach (var link in page.GetLinks())
            {
                Console.WriteLine(link);
            }
        }
    }
}