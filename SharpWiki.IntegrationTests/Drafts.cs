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
        public async Task LinksFromArticle()
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
        public async Task LinksFromUserPage()
        {
            var page = this.site
                .GetNamespace("User")
                .GetPage("Misza13");

            await foreach (var link in page.GetLinks())
            {
                Console.WriteLine(link);
            }
        }

        [Test]
        public async Task ArticleCategories()
        {
            var page = this.site
                .GetArticle("Albert Einstein");

            await foreach (var category in page.GetCategories())
            {
                Console.WriteLine(category);
            }
        }
        
        [Test]
        public async Task UserPageCategories()
        {
            var page = this.site
                .GetNamespace("User")
                .GetPage("Misza13");

            await foreach (var category in page.GetCategories())
            {
                Console.WriteLine(category);
            }
        }

        [TestCase("Physics")]
        [TestCase("German Nobel laureates")]
        [TestCase("Wikipedia administrators")]
        public async Task GetCategoryMembers(string categoryName)
        {
            var cat = this.site.GetCategory(14, categoryName);

            await foreach (var page in cat.GetMembers())
            {
                Console.WriteLine(page);
            }
        }
    }
}