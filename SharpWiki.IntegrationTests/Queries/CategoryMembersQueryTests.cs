namespace SharpWiki.IntegrationTests.Queries
{
    using System;
    using System.Threading.Tasks;
    using API.Queries;
    using FluentAssertions;
    using Infrastructure;
    using NUnit.Framework;

    [TestFixture]
    public class CategoryMembersQueryTests
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
            var queryParameters = new CategoryMembersQueryRequest("Category:Physics");

            // Act
            var result = await this.wrapper.Get(queryParameters);

            // Assert
            result.batchcomplete.Should().NotBeNull();
            result.batchcomplete.Should().BeEmpty();

            result.@continue.Should().BeNull();

            result.query.categorymembers.Should().NotBeEmpty();
        }
    }
}