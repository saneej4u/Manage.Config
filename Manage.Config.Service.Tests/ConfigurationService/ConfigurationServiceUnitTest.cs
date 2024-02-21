using AutoFixture;
using FluentAssertions;
using Manage.Config.Service.Models;
using Manage.Config.Service.Services.FileManager;
using Manage.Config.Service.Services.Mappers;
using Manage.Config.Services.Configuration;
using Manage.Configuration.Services.Configuration;
using NSubstitute;

namespace Manage.Config.Service.Tests
{
    public class ConfigurationServiceUnitTest
    {
        private Fixture _fixture;
        private IConfigurationService _sut;
        private IConfigurationMapper _configurationMapperStub;
        private IFileManagerService _fileManagerServiceStub;

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            _configurationMapperStub = Substitute.For<IConfigurationMapper>();
            _fileManagerServiceStub = Substitute.For<IFileManagerService>();
            _sut = new ConfigurationService(_configurationMapperStub, _fileManagerServiceStub);
        }

        [Test]
        public async Task GetAllServerConfigurationAsync_With_NoFilePath_Returns_FilePath_Not_Exist()
        {
            // Arrange
            string filePath = string.Empty;

            // Act
            var result = await _sut.GetAllServerConfigurationAsync(filePath);

            // Assert;
            result.Should().NotBeNull();
            result.Data.Should().BeNull();

            result.HasError.Should().BeTrue();
            result.ErrorMessage.Should().Be("File path not exist.");

            result.IsNotFound.Should().BeFalse();
            result.IsClientError.Should().BeTrue();
        }
        [Test]
        public async Task GetAllServerConfigurationAsync_With_FilePath_Returns_ServerConfig_Missing()
        {
            // Arrange
            string filePath = Path.GetFullPath(@"TestFiles\nodataconfig.txt");

            var lines = new List<List<string>> { };
            _fileManagerServiceStub.ReadFileAndGroupByEmptyLineAsync(default).ReturnsForAnyArgs(lines);

            // Act
            var result = await _sut.GetAllServerConfigurationAsync(filePath);

            // Assert;
            result.Should().NotBeNull();
            result.Data.Should().BeNull();

            result.HasError.Should().BeTrue();
            result.ErrorMessage.Should().Be("Server configuration is missing.");

            result.IsNotFound.Should().BeTrue();
            result.IsClientError.Should().BeFalse();
        }

        [Test]
        public async Task GetAllServerConfigurationAsync_Returns_DefaultServerConfig_Missing()
        {
            // Arrange
            string filePath = Path.GetFullPath(@"TestFiles\nodefaultserverconfig.txt");

            var lines = _fixture.Create<List<List<string>>>();
            _fileManagerServiceStub.ReadFileAndGroupByEmptyLineAsync(default).ReturnsForAnyArgs(lines);

            var defaultServer = _fixture.Create<ServerDetailsModel>();
            defaultServer.ServerType = Configuration.Service.Models.ServerTypes.SERVER_SPECIFIC;
            _configurationMapperStub.MapToDefaultServerDetailsModel(default).ReturnsForAnyArgs(defaultServer);

            // Act
            var result = await _sut.GetAllServerConfigurationAsync(filePath);

            // Assert;
            result.Should().NotBeNull();
            result.Data.Should().BeNull();

            result.HasError.Should().BeTrue();
            result.ErrorMessage.Should().Be("Default server not exist.");

            result.IsNotFound.Should().BeTrue();
            result.IsClientError.Should().BeFalse();
        }

        [Test]
        public async Task GetAllServerConfigurationAsync_Returns_SpecificServerConfig_Missing()
        {
            // Arrange
            string filePath = Path.GetFullPath(@"TestFiles\nospecificserverconfig.txt");

            var defaultServer = _fixture.Create<ServerDetailsModel>();
            defaultServer.ServerType = Configuration.Service.Models.ServerTypes.DEFAULT;
            _configurationMapperStub.MapToDefaultServerDetailsModel(default).ReturnsForAnyArgs(defaultServer);

            var specificServer = _fixture.Create<ServerDetailsModel>();
            specificServer.ServerType = Configuration.Service.Models.ServerTypes.DEFAULT;
            _configurationMapperStub.MapToSpecificServerDetailsModel(default, specificServer).ReturnsForAnyArgs(specificServer);

            var lines = _fixture.Create<List<List<string>>>();
            _fileManagerServiceStub.ReadFileAndGroupByEmptyLineAsync(default).ReturnsForAnyArgs(lines);

            // Act
            var result = await _sut.GetAllServerConfigurationAsync(filePath);

            // Assert;
            result.Should().NotBeNull();
            result.Data.Should().BeNull();

            result.HasError.Should().BeTrue();
            result.ErrorMessage.Should().Be("Specific servers not exist.");

            result.IsNotFound.Should().BeTrue();
            result.IsClientError.Should().BeFalse();
        }

        [Test]
        public async Task GetAllServerConfigurationAsync_Returns_ServerConfigs_Succesfully()
        {
            // Arrange
            string filePath = Path.GetFullPath(@"TestFiles\correctserverconfig.txt");

            var defaultServer = _fixture.Create<ServerDetailsModel>();
            defaultServer.ServerType = Configuration.Service.Models.ServerTypes.DEFAULT;
            _configurationMapperStub.MapToDefaultServerDetailsModel(default).ReturnsForAnyArgs(defaultServer);

            var specificServer = _fixture.Create<ServerDetailsModel>();
            specificServer.ServerType = Configuration.Service.Models.ServerTypes.SERVER_SPECIFIC;
            _configurationMapperStub.MapToSpecificServerDetailsModel(default, specificServer).ReturnsForAnyArgs(specificServer);

            var lines = _fixture.Create<List<List<string>>>();
            _fileManagerServiceStub.ReadFileAndGroupByEmptyLineAsync(default).ReturnsForAnyArgs(lines);
            // Act
            var result = await _sut.GetAllServerConfigurationAsync(filePath);

            // Assert;
            result.Should().NotBeNull();
            result.Data.Should().NotBeNull();

            result.HasError.Should().BeFalse();
            result.ErrorMessage.Should().BeNull();

            result.IsNotFound.Should().BeFalse();
            result.IsClientError.Should().BeFalse();
        }
    }
}
