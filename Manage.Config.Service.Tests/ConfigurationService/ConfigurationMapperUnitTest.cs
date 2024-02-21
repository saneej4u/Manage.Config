using AutoFixture;
using FluentAssertions;
using Manage.Config.Service.Models;
using Manage.Config.Service.Services.Mappers;
using Manage.Configuration.Service.Models;

namespace Manage.Config.Service.Tests
{
    public class ConfigurationMapperUnitTest
    {
        private Fixture _fixture;
        private IConfigurationMapper _sut;


        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            _sut = new ConfigurationMapper();
        }

        [Test]
        public void MapToDefaultServerDetailsModel_Returns_DefaultServer()
        {
            // Arrange
            var configText = @";START DEFAULTS
                                SERVER_NAME=MRAPPPOOLPORTL01
                                URL=http://dummy.DOMAIN.COMPANY.COM/Available.html
                                DB=SQL_SERVER
                                IP_ADDRESS=10.200.0.3
                                DOMAIN=MYDOMAIN
                                COOKIE_DOMAIN=dummy.DOMAIN.COMPANY.COM
                                ;END DEFAULTS

                                ;START SRVTST0003
                                SERVER_NAME{SRVTST0003}=MRAPPPOOLPORTL0003
                                IP_ADDRESS{SRVTST0003}=10.200.0.100
                                ;END SRVTST0003";

            var serverDetails = ParseConfiguration(configText);

            // Act
            var result =  _sut.MapToDefaultServerDetailsModel(serverDetails.FirstOrDefault());

            // Assert;
            result.Should().NotBeNull();
            result.ServerName.Key.Should().BeEquivalentTo("SERVER_NAME");
            result.ServerName.Value.Should().BeEquivalentTo("MRAPPPOOLPORTL01");
            result.ServerName.IsDefaultUsed.Should().BeTrue();
            result.ServerName.ServerSuffixName.Should().BeEmpty();
            result.ServerType.Should().Be(ServerTypes.DEFAULT);

            result.URL.Value.Should().BeEquivalentTo("http://dummy.DOMAIN.COMPANY.COM/Available.html");
            result.Database.Value.Should().BeEquivalentTo("SQL_SERVER");
            result.IPAddress.Value.Should().BeEquivalentTo("10.200.0.3");
            result.Domain.Value.Should().BeEquivalentTo("MYDOMAIN");
            result.CookieDomain.Value.Should().BeEquivalentTo("dummy.DOMAIN.COMPANY.COM");
        }

        [Test]
        public void MapToSpecificServerDetailsModel_Returns_DefaultServer()
        {
            // Arrange
            var configText = @";START DEFAULTS
                                SERVER_NAME=MRAPPPOOLPORTL01
                                URL=http://dummy.DOMAIN.COMPANY.COM/Available.html
                                DB=SQL_SERVER
                                IP_ADDRESS=10.200.0.3
                                DOMAIN=MYDOMAIN
                                COOKIE_DOMAIN=dummy.DOMAIN.COMPANY.COM
                                ;END DEFAULTS

                                ;START SRVTST0003
                                SERVER_NAME{SRVTST0003}=MRAPPPOOLPORTL0003
                                IP_ADDRESS{SRVTST0003}=10.200.0.100
                                ;END SRVTST0003";

            var defaultServer = _fixture.Create<ServerDetailsModel>();
            var serverDetails = ParseConfiguration(configText);

            // Act
            var result = serverDetails.Skip(1).Select(serverDetails => _sut.MapToSpecificServerDetailsModel(serverDetails, defaultServer)).ToList();


            // Assert;
            result.Should().NotBeNull();
            result.First().ServerName.Key.Should().BeEquivalentTo("SERVER_NAME");
            result.First().ServerName.Value.Should().BeEquivalentTo("MRAPPPOOLPORTL0003");
            result.First().ServerName.IsDefaultUsed.Should().BeFalse();
            result.First().ServerName.ServerSuffixName.Should().BeEquivalentTo("SRVTST0003");
            result.First().ServerType.Should().Be(ServerTypes.SERVER_SPECIFIC);

            result.First().URL.Value.Should().BeEquivalentTo(defaultServer.URL.Value);
            result.First().Database.Value.Should().BeEquivalentTo(defaultServer.Database.Value);
            result.First().IPAddress.Value.Should().BeEquivalentTo("10.200.0.100");
            result.First().Domain.Value.Should().BeEquivalentTo(defaultServer.Domain.Value);
            result.First().CookieDomain.Value.Should().BeEquivalentTo(defaultServer.CookieDomain.Value);
        }

        private static List<List<string>> ParseConfiguration(string text)
        {
            List<List<string>> groups = new List<List<string>>();
            List<string> currentGroup = new List<string>();

            string[] lines = text.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);

            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    if (currentGroup.Count > 0)
                    {
                        groups.Add(currentGroup);
                        currentGroup = new List<string>();
                    }
                }
                else if (!line.Trim().StartsWith(";"))
                {
                    currentGroup.Add(line.Trim());
                }
            }

            // Add the last group if it's not empty
            if (currentGroup.Count > 0)
            {
                groups.Add(currentGroup);
            }

            return groups;
        }

    }
}
