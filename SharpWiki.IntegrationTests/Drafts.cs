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
        public async Task LinksFromArticleToArticlesDescending()
        {
            var page = this.site
                .GetNamespace("")
                .GetPage("Albert Einstein");

            var list = page.GetLinks()
                .Descending()
                .OnlyToNamespaces(0);

            await foreach (var link in list)
            {
                Console.WriteLine(link);
            }
        }
        
        [Test]
        public async Task LinksToNamespaceByName()
        {
            var page = this.site
                .GetNamespace("")
                .GetPage("Wikipedia:Bots");

            var list = page.GetLinks()
                .OnlyToNamespaces("User");

            await foreach (var link in list)
            {
                Console.WriteLine(link);
            }
        }
        
        [Test]
        public async Task LinksToNamespaceByObject()
        {
            var page = this.site
                .GetNamespace("")
                .GetPage("User:Jimbo Wales");

            var list = page.GetLinks()
                .OnlyToNamespaces(this.site.GetNamespace("Wikipedia"));

            await foreach (var link in list)
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

            await foreach (var category in page.GetCategories().Descending())
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

        [Test]
        public async Task GetSubcategories()
        {
            var cat = this.site.GetCategory(14, "Physics");

            await foreach (var subcat in cat
                .GetMembers().OnlySubcategories().Descending())
            {
                Console.WriteLine(subcat);
            }
        }
        
        [Test]
        public async Task GetMembersByTimestamp()
        {
            var cat = this.site.GetCategory(14, "2021 deaths");

            await foreach (var subcat in cat
                .GetMembers().OnlyPages().ByTimestamp().Descending())
            {
                Console.WriteLine(subcat);
            }
        }

        [Test]
        public async Task GetHiddenCategories()
        {
            var page = this.site.GetArticle("Albert Einstein");

            await foreach (var category in page.GetCategories().OnlyHidden())
            {
                Console.WriteLine(category);
            }
        }

        [Test]
        public async Task GetNonHiddenCategories()
        {
            var page = this.site.GetArticle("Albert Einstein");

            await foreach (var category in page.GetCategories().OnlyNonHidden())
            {
                Console.WriteLine(category);
            }
        }
    }
}