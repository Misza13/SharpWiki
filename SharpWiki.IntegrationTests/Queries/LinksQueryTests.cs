namespace SharpWiki.IntegrationTests.Queries
{
    using System;
    using System.Threading.Tasks;
    using API.Queries;
    using FluentAssertions;
    using Infrastructure;
    using NUnit.Framework;

    public class LinksQueryTests
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
            var queryParameters = new LinksQueryRequest("QBQ");

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
            var queryParameters = new LinksQueryRequest("Albert Einstein");
            
            // Act
            var result = await this.wrapper.Get(queryParameters);

            // Assert
            result.batchcomplete.Should().BeNull();
            
            result.@continue.Should().NotBeNull();
            
            result.@continue.plcontinue.Should().NotBeNullOrEmpty();
            result.@continue.@continue.Should().NotBeNullOrEmpty();

            var pages = result.query.pages;
            pages.Should().ContainKey("736");

            var page = pages["736"];
            page.pageid.Should().Be(736);
            page.ns.Should().Be(0);
            page.title.Should().Be("Albert Einstein");
            page.links.Should().NotBeEmpty();
        }

        [Test]
        public async Task ShouldContinue()
        {
            // Arrange
            var prepQueryParameters = new LinksQueryRequest("Albert Einstein");
            
            var prepResult = await this.wrapper.Get(prepQueryParameters);

            var firstLink = prepResult.query.pages["736"].links[0].title;
            
            var queryParameters = new LinksQueryRequest("Albert Einstein")
                .WithContinuation(prepResult.@continue.plcontinue);

            // Act
            var result = await this.wrapper.Get(queryParameters);

            // Assert
            result.@continue.Should().NotBeNull();
            
            result.@continue.plcontinue.Should().NotBeNullOrEmpty();
            result.@continue.@continue.Should().NotBeNullOrEmpty();

            var pages = result.query.pages;
            pages.Should().ContainKey("736");

            var page = pages["736"];
            page.pageid.Should().Be(736);
            page.ns.Should().Be(0);
            page.title.Should().Be("Albert Einstein");
            page.links.Should().NotBeEmpty();
            page.links.Should().NotContain(l => l.title == firstLink);
        }
    }
}