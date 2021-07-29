namespace SharpWiki.IntegrationTests.Queries
{
    using System;
    using System.Threading.Tasks;
    using API.Queries;
    using FluentAssertions;
    using Infrastructure;
    using NUnit.Framework;

    public class CategoriesQueryTests
    {
        private RestSharpApiWrapper wrapper;

        [SetUp]
        public void Setup()
        {
            var apiUrl = new Uri("https://en.wikipedia.org/w/api.php");
            this.wrapper = new RestSharpApiWrapper(apiUrl);
        }

        [Test]
        public async Task ShouldGetOnePage()
        {
            // Arrange
            var queryParameters = new CategoriesQueryRequest("QBQ");

            // Act
            var result = await this.wrapper.Get(queryParameters);

            // Assert
            result.batchcomplete.Should().NotBeNull();
            result.batchcomplete.Should().BeEmpty();

            result.@continue.Should().BeNull();
        }

        [Test]
        public async Task ShouldGetResultWithNextPage()
        {
            // Arrange
            var queryParameters = new CategoriesQueryRequest("Albert Einstein")
                .WithLimit(10);
            
            // Act
            var result = await this.wrapper.Get(queryParameters);

            // Assert
            result.batchcomplete.Should().BeNull();
            
            result.@continue.Should().NotBeNull();
            
            result.@continue.clcontinue.Should().NotBeNullOrEmpty();
            result.@continue.@continue.Should().NotBeNullOrEmpty();

            var pages = result.query.pages;
            pages.Should().ContainKey("736");

            var page = pages["736"];
            page.pageid.Should().Be(736);
            page.ns.Should().Be(0);
            page.title.Should().Be("Albert Einstein");
            page.categories.Should().NotBeEmpty();
        }

        [Test]
        public async Task ShouldContinue()
        {
            // Arrange
            var prepQueryParameters = new CategoriesQueryRequest("Albert Einstein")
                .WithLimit(10);
            
            var prepResult = await this.wrapper.Get(prepQueryParameters);

            var firstLink = prepResult.query.pages["736"].categories[0].title;
            
            var queryParameters = new CategoriesQueryRequest("Albert Einstein")
                .WithContinue(prepResult.@continue.clcontinue)
                .WithLimit(10);

            // Act
            var result = await this.wrapper.Get(queryParameters);

            // Assert
            result.@continue.Should().NotBeNull();
            
            result.@continue.clcontinue.Should().NotBeNullOrEmpty();
            result.@continue.@continue.Should().NotBeNullOrEmpty();

            var pages = result.query.pages;
            pages.Should().ContainKey("736");

            var page = pages["736"];
            page.pageid.Should().Be(736);
            page.ns.Should().Be(0);
            page.title.Should().Be("Albert Einstein");
            page.categories.Should().NotBeEmpty();
            page.categories.Should().NotContain(l => l.title == firstLink);
        }
    }
}