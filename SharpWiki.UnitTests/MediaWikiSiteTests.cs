namespace SharpWiki.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using API.Queries;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;

    public class MediaWikiSiteTests
    {
        private MediaWikiSite site;

        private Mock<IApiWrapper> apiWrapper;
        
        [SetUp]
        public void Setup()
        {
            this.apiWrapper = new Mock<IApiWrapper>();

            this.site = new MediaWikiSite(this.apiWrapper.Object);
        }

        [TestCase(2, "User", TestName = "User")]
        [TestCase(11, "Template talk", TestName = "Template talk")]
        public async Task LoadMetadataShouldInitializeNamespaceInfo(
            int namespaceId,
            string namespaceName)
        {
            // Arrange
            this.apiWrapper.Setup(aw =>
                    aw.Get(It.IsAny<SiteInfoQueryRequest>()))
                .Returns(Task.FromResult(new SiteInfoQueryResult
                {
                    query = new SiteInfoQueryResult.QuerySection
                    {
                        namespaces = new Dictionary<int, SiteInfoQueryResult.NamespaceInfo>
                        {
                            {2, new SiteInfoQueryResult.NamespaceInfo
                            {
                                id = 2, canonical = "User"
                            }},
                            {11, new SiteInfoQueryResult.NamespaceInfo
                            {
                                id = 11, canonical = "Template talk"
                            }}
                        }
                    }
                }));

            // Act
            await this.site.LoadMetadata();
            var namespaceById = this.site.GetNamespace(namespaceId);
            var namespaceByName = this.site.GetNamespace(namespaceName);

            // Assert
            namespaceById.Should().NotBeNull();
            namespaceById.Id.Should().Be(namespaceId);
            namespaceById.Name.Should().Be(namespaceName);
            namespaceById.Site.Should().Be(this.site);

            namespaceByName.Should().NotBeNull();
            namespaceByName.Id.Should().Be(namespaceId);
            namespaceByName.Name.Should().Be(namespaceName);
            namespaceByName.Site.Should().Be(this.site);
        }

        [Test]
        public void GetNamespaceById_ShouldThrow_IfInstanceWasNotInitialized()
        {
            // Arrange
            Action cmd = () => this.site.GetNamespace(0);

            // Act & Assert
            cmd.Should().Throw<KeyNotFoundException>();
        }

        [Test]
        public void GetNamespaceByName_ShouldThrow_IfInstanceWasNotInitialized()
        {
            // Arrange
            Action cmd = () => this.site.GetNamespace("Talk");

            // Act & Assert
            cmd.Should().Throw<KeyNotFoundException>();
        }

        [Test]
        public void GetPageByNamespaceId_ShouldReturnProperlyInitializedPage()
        {
            // Act
            var page = this.site.GetPage(1, "Albert Einstein");

            // Assert
            page.Should().NotBeNull();
            page.NamespaceId.Should().Be(1);
            page.Title.Should().Be("Albert Einstein");
        }
        
        [Test]
        public async Task GetPageByNamespaceName_ShouldReturnProperlyInitializedPage()
        {
            // Arrange
            this.apiWrapper.Setup(aw =>
                    aw.Get(It.IsAny<SiteInfoQueryRequest>()))
                .Returns(Task.FromResult(new SiteInfoQueryResult
                {
                    query = new SiteInfoQueryResult.QuerySection
                    {
                        namespaces = new Dictionary<int, SiteInfoQueryResult.NamespaceInfo>
                        {
                            { 2, new SiteInfoQueryResult.NamespaceInfo
                            {
                                id = 2, canonical = "User"
                            }}
                        }
                    }
                }));

            await this.site.LoadMetadata();
            
            // Act
            var page = this.site.GetPage(
                "User",
                "Jimbo Wales");

            // Assert
            page.Should().NotBeNull();
            page.NamespaceId.Should().Be(2);
            page.Title.Should().Be("Jimbo Wales");
        }

        [Test]
        public void GetArticle_ShouldReturnAnArticlePage()
        {
            // Act
            var page = this.site.GetArticle("Albert Einstein");

            // Assert
            page.Should().NotBeNull();
            page.NamespaceId.Should().Be(0);
            page.Title.Should().Be("Albert Einstein");
        }
    }
}