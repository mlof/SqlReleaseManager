using Moq;
using NUnit.Framework;
using SqlReleaseManager.Core.Repositories;
using System;
using System.Reflection;
using System.Threading.Tasks;
using FluentAssertions;
using SqlReleaseManager.Core.Models;

namespace SqlReleaseManager.Core.Tests.Repositories
{
    [TestFixture]
    public class DacpacRepositoryTests
    {
        private MockRepository mockRepository;
        private string testPath;

        private string ValidDacpacPath = Path.Join(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
            "CsSqlProj.dacpac");

        [SetUp]
        public void SetUp()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            // ensure dacpac folder is empty


            this.testPath = Path.Combine(Path.GetTempPath(), "SqlReleaseManager", "Dacpacs");

            if (Directory.Exists(testPath))
            {
                Directory.Delete(testPath, true);
            }
        }

        private DacpacRepository CreateDacpacRepository()
        {
            return new DacpacRepository(testPath);
        }

        [Test]
        public async Task Create()
        {
            // Arrange
            var dacpacRepository = this.CreateDacpacRepository();

            var file = File.OpenRead(ValidDacpacPath);


            CreateOrUpdateDacpac dacpac = new CreateOrUpdateDacpac("Test", file);


            // Act
            await dacpacRepository.Create(
                dacpac);
            // Assert

            File.Exists(Path.Join(testPath, "Test", "Test.dacpac")).Should().BeTrue();
        }

        [Test]
        public async Task Create_DacpacAlreadyExists()
        {
            // Arrange
            var dacpacRepository = this.CreateDacpacRepository();
            var file = File.OpenRead(ValidDacpacPath);
            CreateOrUpdateDacpac dacpac = new CreateOrUpdateDacpac("Test", file);

            await dacpacRepository.Create(
                dacpac);

            // Act

            Func<Task> act = async () => await dacpacRepository.Create(
                dacpac);

            // Assert

            await act.Should().ThrowAsync<InvalidOperationException>();
        }

        [Test]
        public async Task List()
        {
            // Arrange
            var dacpacRepository = this.CreateDacpacRepository();
            var file = File.OpenRead(ValidDacpacPath);
            CreateOrUpdateDacpac dacpac = new CreateOrUpdateDacpac("Test", file);
            await dacpacRepository.Create(
                dacpac);
            // Act

            var result = await dacpacRepository.List();
            // Assert
            result.Should().HaveCount(1);
            result.Should().Contain(x => x.Name == "Test");
        }

        [Test]

        public async Task List_NoDacpacs()
        {
            // Arrange
            var dacpacRepository = this.CreateDacpacRepository();
            // Act
            var result = await dacpacRepository.List();
            // Assert
            result.Should().BeEmpty();
        }

        [Test]

        public async Task Delete()
        {
            // Arrange
            var dacpacRepository = this.CreateDacpacRepository();
            var file = File.OpenRead(ValidDacpacPath);
            CreateOrUpdateDacpac dacpac = new CreateOrUpdateDacpac("Test", file);
            await dacpacRepository.Create(
                               dacpac);
            // Act
            await dacpacRepository.Delete("Test");
            // Assert

            File.Exists(Path.Join(testPath, "Test", "Test.dacpac")).Should().BeFalse();
        }

        [Test]

        public async Task Delete_DacpacDoesNotExist()
        {
            // Arrange
            var dacpacRepository = this.CreateDacpacRepository();
            // Act
            Func<Task> act = async () => await dacpacRepository.Delete("Test");
            // Assert
            await act.Should().ThrowAsync<InvalidOperationException>();
        }

        [Test]

        public async Task Get()
        {
        }

      
    }
}