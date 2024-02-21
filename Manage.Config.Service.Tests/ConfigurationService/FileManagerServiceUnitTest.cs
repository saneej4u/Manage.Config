using AutoFixture;
using FluentAssertions;
using Manage.Config.Service.Services.FileManager;

namespace Manage.Config.Service.Tests
{
    public class FileManagerServiceUnitTest
    {
        private Fixture _fixture;
        private IFileManagerService _sut;


        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            _sut = new FileManagerService();
        }

        [Test]
        public async Task ReadFileAndGroupByEmptyLineAsync_EmptyFile_Returns_NoData()
        {
            // Arrange

            string filePath = Path.GetFullPath(@"TestFiles\nodataconfig.txt");
            // Act
            var result = await _sut.ReadFileAndGroupByEmptyLineAsync(filePath);

            // Assert;
            result.Count.Should().Be(0);
        }

        [Test]
        public async Task ReadFileAndGroupByEmptyLineAsync_CorrectFile_Returns_FullConfig()
        {
            // Arrange
            string filePath = Path.GetFullPath(@"TestFiles\correctserverconfig.txt");
            // Act
            var result = await _sut.ReadFileAndGroupByEmptyLineAsync(filePath);

            // Assert;
            result.Should().NotBeNull();
            result.Count.Should().Be(7);

            var defaultServer = result.First();

            defaultServer[0].Should().Contain("SERVER_NAME");
            defaultServer[1].Should().Contain("URL");
            defaultServer[2].Should().Contain("DB");
            defaultServer[3].Should().Contain("ADDRESS");
            defaultServer[4].Should().Contain("DOMAIN");
            defaultServer[5].Should().Contain("COOKIE");
        }
    }
}
